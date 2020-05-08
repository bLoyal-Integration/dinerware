using bLoyal.Connectors.LoyaltyEngine;
using bLoyal.Utilities;
using DinerwareFindCustomer;
using DinerwareSystem.Helpers;
using DinerwareSystem.Models;
using DinerwareSystem.Provider;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DinerwareSystem
{
    public partial class frmFindCustomer : Form
    {
        #region Properties

        private int _currentUserId;
        private string _cartId;
        private int _openTicketId = 0;
        private string _deviceSessionKey;
        bool _isSourceExternalId = true;
        string _urlString;
        string _snippetOriginalUrl;

        LoyaltyEngineServices _services = new LoyaltyEngineServices();
        DinerwareHelper _dinerwareHelper = new DinerwareHelper();
        LoggerHelper _logger = LoggerHelper.Instance;       
        ConfigurationHelper _conFigHelper = ConfigurationHelper.Instance;

        #endregion

        #region Public Methods

        /// <summary>
        /// Initialize FindCustomer
        /// </summary>
        public frmFindCustomer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialize FindCustomer
        /// </summary>
        /// <param name="sessionKey"></param>
        public frmFindCustomer(string sessionKey)
        {
            _deviceSessionKey = sessionKey;
            InitializeComponent();
        }

        /// <summary>
        /// Close automatic window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseForm(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Launch Find Customer POS snippets
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmFindCustomer_Load(object sender, EventArgs e)
        {
            try
            {
                // Close snippets automatically
                Timer frmCloseTimer = new Timer();
                frmCloseTimer.Interval = 300000;
                frmCloseTimer.Start();
                frmCloseTimer.Tick += new EventHandler(CloseForm);

                int.TryParse(FindCustomerExtension.currentUserId, out _currentUserId);
                int.TryParse(FindCustomerExtension.currentOpenTicketId, out _openTicketId);
                _cartId = FindCustomerExtension.currentOpenTicketId;

                if (string.IsNullOrWhiteSpace(_cartId))
                {
                    _cartId = AsyncHelper.RunSync(() => _services.CreateCartForNewTicket());
                    _isSourceExternalId = false;
                }

                if (string.IsNullOrWhiteSpace(_deviceSessionKey))
                    _deviceSessionKey = AsyncHelper.RunSync(() => _services.GetSessionAsync());

                _urlString = string.Format("{0}{1}{2}{3}{4}{5}{6}", _conFigHelper.SNIPPET_URL, DinerwareUrls.FINDCUSTOMER, _deviceSessionKey, Constants.LOGINDOMAIN, _conFigHelper.LOGIN_DOMAIN, _isSourceExternalId ? Constants.CARTSOURCEEXTERNALID : Constants.CARTUID, _cartId);
                findCustomerWebBrowser.Navigate(_urlString);
                this.FormClosing += frmFindCustomer_FormClosing;

                findCustomerWebBrowser.AllowNavigation = true;
                findCustomerWebBrowser.Navigating += WebBrowser_Navigating;

            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "frmFindCustomer_Load");
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
                string urlString = string.Format("{0}{1}", _conFigHelper.SNIPPET_URL, DinerwareUrls.FINDCUSTOMER);
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
        /// FindCustomer FormClosing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmFindCustomer_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                this.Hide();

                AsyncHelper.RunSync(() => GetCustomerAndAssignToDinerwareAsync());

                FindCustomerExtension.currentOpenTicketId = string.Empty;
                _cartId = string.Empty;
                _openTicketId = 0;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "frmFindCustomer_FormClosing");
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

                calculatedCart = _isSourceExternalId ? await _services.GetCartBySourceExternalId(_cartId).ConfigureAwait(true) : await _services.GetCartAsync(new Guid(_cartId)).ConfigureAwait(true);

                if (calculatedCart != null && calculatedCart.Cart != null && calculatedCart.Cart.Customer != null && calculatedCart.Cart.Customer.Uid != Guid.Empty)
                    transactionCustomer = calculatedCart.Cart.Customer;
                else
                    _logger.WriteLogInfo("**** Unable to find transaction Customer using FindCustomer Snippet **** cartUid = " + _cartId);

                FindCustomerExtension.currentOpenTicketId = string.Empty;
                if (transactionCustomer != null)
                {
                    int dwCustomerId;
                    if (_openTicketId > 0)
                    {
                        dwCustomerId = _dinerwareHelper.UpdateTicketWithCustomer(_currentUserId, transactionCustomer, _cartId, _openTicketId);
                        calculatedCart.Cart.Customer.ExternalId = dwCustomerId.ToString();
                    }
                    else
                    {
                        //Add bloyal Customer  to Dinerware                      
                        var response = _dinerwareHelper.AddCustomerToDinerware(_currentUserId, transactionCustomer, _cartId);
                        _openTicketId = response != null ? response.TicketId : 0;
                        if (_openTicketId > 0 && calculatedCart != null && calculatedCart.Cart != null)
                        {
                            calculatedCart.Cart.SourceExternalId = response.TicketId.ToString();
                            calculatedCart.Cart.Customer.ExternalId = response.CustomerId.ToString();
                        }

                        if (calculatedCart != null)
                            await _services.CalculateCartAsync(new CalculateCartCommand { Cart = calculatedCart.Cart }).ConfigureAwait(true);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "frmFindCustomer GetCustomerAndAssignToDinerware");
            }
        }
      

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        #endregion
    }
}
