using DinerwareSystem.Helpers;
using DinerwareSystem.Provider;
using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace DinerwareSystem
{
    public partial class frmAlerts : Form
    {

        #region Properties

        private int _wbReadyStateCount = 0;       
        private string _deviceSessionKey = string.Empty;     
        private string _alertSnippetURL = string.Empty;

        Stopwatch _stopWatch = new Stopwatch();
        DinerwareHelper _dinerwareHelper = new DinerwareHelper();
        LoyaltyEngineServices _services = new LoyaltyEngineServices();
        LoggerHelper _logger = LoggerHelper.Instance;

        #endregion


        public frmAlerts()
        {
            InitializeComponent();
        }

        public frmAlerts(string sourceExternalId, string uId, string snippetURL)
        {          
            _alertSnippetURL = snippetURL;
            InitializeComponent();
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Alerts_Load(object sender, EventArgs e)
        {
            try
            {
                // Close snippets automatically
                Timer frmCloseTimer = new Timer();
                frmCloseTimer.Interval = 300000;
                frmCloseTimer.Start();
                frmCloseTimer.Tick += new EventHandler(CloseWindow);
               
                alertsWebBrowser.Navigate(_alertSnippetURL);

                this.FormClosing += frmAlerts_FormClosing;

                alertsWebBrowser.StatusTextChanged += GetWebBrowserReadyState;

                _stopWatch.Start();
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "Alerts_Load");
            }
        }

        private void GetWebBrowserReadyState(object source, EventArgs e)
        {
            try
            {
                if (alertsWebBrowser.ReadyState == WebBrowserReadyState.Uninitialized)
                {
                    TimeSpan ts = _stopWatch.Elapsed;
                    _wbReadyStateCount++;
                    if (_wbReadyStateCount > 0)
                    {
                        _wbReadyStateCount = 0;
                        int elapsedTime = ts.Seconds;
                        if (1 < elapsedTime)
                        {                                                  
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "Alerts GetWebBrowserReadyState");
            }
        }

        private void frmAlerts_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {               
                _alertSnippetURL = string.Empty;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "Alerts frmAlerts_FormClosing");
            }
        }

        private void applyCouponWebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }
    }
}
