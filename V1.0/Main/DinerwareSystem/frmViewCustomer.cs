using bLoyal.Utilities;
using DinerwareSystem.Helpers;
using DinerwareSystem.Models;
using DinerwareSystem.Provider;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DinerwareSystem
{
    public partial class frmViewCustomer : Form
    {
        #region Properties

        private int _currentUserId = 1;
        private int _currentOpenTicketId = 0;
        private string _cartExternalId;
        private string _deviceSessionKey;
        bool _isSourceExternalId = true;
        string _urlString;
        string _snippetOriginalUrl;

        DinerwareHelper _dinerwareHelper = new DinerwareHelper();
        LoyaltyEngineServices _services = new LoyaltyEngineServices();
        LoggerHelper _logger = LoggerHelper.Instance;
        ConfigurationHelper _conFigHelper = ConfigurationHelper.Instance;

        #endregion

        #region Public Methods

        public frmViewCustomer()
        {
            InitializeComponent();
        }

        public frmViewCustomer(string sessionKey)
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
        /// ViewCustomer Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmViewCustomer_Load(object sender, EventArgs e)
        {
            try
            {
                // Close snippets automatically
                Timer frmCloseTimer = new Timer();
                frmCloseTimer.Interval = 300000;
                frmCloseTimer.Start();
                frmCloseTimer.Tick += new EventHandler(CloseForm);

                int.TryParse(bLoyalViewCustomerExtension.currentUserId, out _currentUserId);
                int.TryParse(bLoyalViewCustomerExtension.currentOpenTicketId, out _currentOpenTicketId);
                _cartExternalId = bLoyalViewCustomerExtension.currentOpenTicketId;

                if (string.IsNullOrWhiteSpace(_cartExternalId))
                {
                    _cartExternalId = AsyncHelper.RunSync(() => _services.CreateCartForNewTicket());
                    _isSourceExternalId = false;
                }

                if (string.IsNullOrWhiteSpace(_deviceSessionKey))
                    _deviceSessionKey = AsyncHelper.RunSync(() => _services.GetSessionAsync());

                _urlString = string.Format("{0}{1}{2}{3}{4}{5}{6}", _conFigHelper.SNIPPET_URL, DinerwareUrls.VIEWCUSTOMER, _deviceSessionKey, Constants.LOGINDOMAIN, _conFigHelper.LOGIN_DOMAIN, _isSourceExternalId ? Constants.CARTSOURCEEXTERNALID : Constants.CARTUID, _cartExternalId);
                viewCustomerWebBrowser.Navigate(_urlString);
                viewCustomerWebBrowser.AllowNavigation = true;
                viewCustomerWebBrowser.Navigating += WebBrowser_Navigating;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "frmViewCustomer_Load");
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
                string urlString = string.Format("{0}{1}", conFigHelper.SNIPPET_URL, DinerwareUrls.VIEWCUSTOMER);
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

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        #endregion
    }
}
