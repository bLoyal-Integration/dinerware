using bLoyal.Connectors.LoyaltyEngine;
using bLoyal.Utilities;
using DinerwareSystem.Helpers;
using DinerwareSystem.Models;
using DinerwareSystem.Provider;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DinerwareSystem
{
    public partial class frmApplyCoupon : Form
    {
        #region Properties      

        private string _cartExternalId;
        private string _deviceSessionKey;
        string _snippetOriginalUrl;

        DinerwareHelper _dinerwareHelper = new DinerwareHelper();
        LoyaltyEngineServices _services = new LoyaltyEngineServices();
        LoggerHelper _logger = LoggerHelper.Instance;
        ConfigurationHelper _conFigHelper = ConfigurationHelper.Instance;

        #endregion

        #region Public Methods

        public frmApplyCoupon()
        {
            InitializeComponent();
        }

        public frmApplyCoupon(string sessionKey)
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
        /// ApplyCoupon Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmApplyCoupon_Load(object sender, EventArgs e)
        {
            try
            {
                // Close snippets automatically
                Timer frmCloseTimer = new Timer();
                frmCloseTimer.Interval = 300000;
                frmCloseTimer.Start();
                frmCloseTimer.Tick += new EventHandler(CloseForm);

                _cartExternalId = bLoyalApplyCouponExtension.currentOpenTicketId;

                var calculatedCart = AsyncHelper.RunSync(() => _services.GetCartBySourceExternalIdAsync(_cartExternalId));
                if (calculatedCart == null)
                {
                    var cart = new Cart { Customer = new Customer(), Lines = new List<CartLine>(), SourceExternalId = _cartExternalId };
                    var request = new CalculateCartCommand()
                    {
                        Cart = cart,
                        CouponCodes = new List<string>()
                    };
                    calculatedCart = _services.CalculateCart(request);
                }

                bLoyalApplyCouponExtension.currentOpenTicketId = string.Empty;

                applyCouponWebBrowser.Navigate(string.Format("{0}{1}{2}{3}{4}{5}{6}", _conFigHelper.SNIPPET_URL, DinerwareUrls.APPLYCOUPON, _deviceSessionKey, Constants.LOGINDOMAIN, _conFigHelper.LOGIN_DOMAIN, Constants.CARTSOURCEEXTERNALID, _cartExternalId));

                this.FormClosing += frmApplyCoupon_FormClosing;

                applyCouponWebBrowser.AllowNavigation = true;
                applyCouponWebBrowser.Navigating += WebBrowser_Navigating;

            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "frmApplyCoupon_Load");
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
                string urlString = string.Format("{0}{1}", conFigHelper.SNIPPET_URL, DinerwareUrls.APPLYCOUPON);

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
        /// ApplyCoupon FormClosing
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmApplyCoupon_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                bLoyalApplyCouponExtension.currentOpenTicketId = string.Empty;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "frmApplyCoupon_FormClosing");
            }
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
        }

        #endregion
    }
}
