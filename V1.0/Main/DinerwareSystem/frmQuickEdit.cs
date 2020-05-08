using bLoyal.Connectors.LoyaltyEngine;
using bLoyal.Utilities;
using DinerwareSystem.Helpers;
using DinerwareSystem.Models;
using DinerwareSystem.Provider;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DinerwareSystem
{
    public partial class frmQuickEdit : Form
    {
        #region Properties     

        private int _currentOpenTicketId = 0;
        private int _currentUserId = 1;
        private string _cartExternalId;
        private string _deviceSessionKey;
        private bool _isSourceExternalId = true;
        private string _urlString;
        private string _snippetOriginalUrl;

        LoyaltyEngineServices _services = new LoyaltyEngineServices();
        DinerwareHelper _dinerwareHelper = new DinerwareHelper();
        LoggerHelper _logger = LoggerHelper.Instance;
        ConfigurationHelper _conFigHelper = ConfigurationHelper.Instance;

        #endregion

        #region Public Methods

        public frmQuickEdit()
        {
            InitializeComponent();
        }

        public frmQuickEdit(string sessionKey)
        {
            _deviceSessionKey = sessionKey;
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Close automatic window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseForm(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// QuickEdit Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmQuickEdit_Load(object sender, EventArgs e)
        {
            try
            {
                // Close snippets automatically
                Timer frmCloseTimer = new Timer();
                frmCloseTimer.Interval = 300000;
                frmCloseTimer.Start();
                frmCloseTimer.Tick += new EventHandler(CloseForm);
                
                int.TryParse(bLoyalQuickEditExtension.currentUserId, out _currentUserId);
                int.TryParse(bLoyalQuickEditExtension.currentOpenTicketId, out _currentOpenTicketId);
                _cartExternalId = bLoyalQuickEditExtension.currentOpenTicketId;

                if (string.IsNullOrWhiteSpace(_cartExternalId))
                {
                    _cartExternalId = AsyncHelper.RunSync(() =>_services.CreateCartForNewTicket());
                    _isSourceExternalId = false;
                }

                if (string.IsNullOrWhiteSpace(_deviceSessionKey))
                    _deviceSessionKey =  AsyncHelper.RunSync(() => _services.GetSessionAsync());

                _urlString = string.Format("{0}{1}{2}{3}{4}{5}{6}", _conFigHelper.SNIPPET_URL, DinerwareUrls.QUICKEDIT, _deviceSessionKey, Constants.LOGINDOMAIN, _conFigHelper.LOGIN_DOMAIN, _isSourceExternalId ? Constants.CARTSOURCEEXTERNALID : Constants.CARTUID, _cartExternalId);
                QuickEditWebBrowser.Navigate(_urlString);
                this.FormClosing += frmQuickEdit_FormClosing;              

                QuickEditWebBrowser.AllowNavigation = true;
                QuickEditWebBrowser.Navigating += WebBrowser_Navigating;

            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "frmQuickEdit_Load");
            }
        }

        /// <summary>
        /// Get WebBrowser Navigating
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            try
            {
                var conFigHelper = ConfigurationHelper.Instance;
                string urlString = string.Format("{0}{1}", conFigHelper.SNIPPET_URL, DinerwareUrls.QUICKEDIT);
                if (string.IsNullOrWhiteSpace(_snippetOriginalUrl))
                {
                    if (e.Url.OriginalString.ToLower().StartsWith(urlString.ToLower()))
                        this.Close();
                    else
                        _snippetOriginalUrl = e.Url.OriginalString;
                }
                else
                {
                    if (e.Url.OriginalString.ToLower().StartsWith(_snippetOriginalUrl.ToLower()))
                        this.Close();
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "WebBrowser_Navigating");
            }
        }
       

        /// <summary>
        /// QuickEdit FormClosing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmQuickEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.Hide();
                AsyncHelper.RunSync(() => GetCustomerAndAssignToDinerwareAsync());


                bLoyalQuickEditExtension.currentOpenTicketId = string.Empty;
                _cartExternalId = string.Empty;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "frmQuickEdit_FormClosing");
            }
        }

        /// <summary>
        /// Get bLoyal Customer and Assign to Dinerware Ticket
        /// </summary>
        /// <returns></returns>
        private async Task GetCustomerAndAssignToDinerwareAsync()
        {
            try
            {
                Customer transactionCustomer = null;
                CalculatedCart calculatedCart = null;

                calculatedCart = _isSourceExternalId ? await _services.GetCartBySourceExternalId(_cartExternalId).ConfigureAwait(true) : await _services.GetCartAsync(new Guid(_cartExternalId)).ConfigureAwait(true);

                if (calculatedCart != null && calculatedCart.Cart != null && calculatedCart.Cart.Customer != null && calculatedCart.Cart.Customer.Uid != Guid.Empty)
                    transactionCustomer = calculatedCart.Cart.Customer;
                else
                    _logger.WriteLogInfo("**** Unable to find transaction Customer using FindCustomer Snippet **** cartUid = " + _cartExternalId);

                bLoyalQuickEditExtension.currentOpenTicketId = string.Empty;
                if (transactionCustomer != null)
                {
                    int dwCustomerId;
                    if (_currentOpenTicketId != 0)
                    {
                        dwCustomerId = _dinerwareHelper.UpdateTicketWithCustomer(_currentUserId, transactionCustomer, _cartExternalId, _currentOpenTicketId);
                        calculatedCart.Cart.Customer.ExternalId = dwCustomerId.ToString();
                    }                  

                    if (calculatedCart != null)
                        await _services.CalculateCartAsync(new CalculateCartCommand { Cart = calculatedCart.Cart }).ConfigureAwait(true);
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "GetCustomerAndAssignToDinerware");
            }
        }


        #endregion
    }
}
