using bLoyal.Connectors;
using bLoyal.Connectors.Grid;
using ConfigApp.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using System.Xml;

namespace ConfigApp
{
    public partial class frmConfiguration : Form
    {
        #region Properties

        private Label lblVirtualClientURL;
        private TextBox txtVirtualClientURL;
        private Label lblLoyaltyengine;
        private TextBox txtLoginDomain;
        private Label lblDataSource;
        private Button button1;
        private GroupBox gbSnippetURLs;
        private GroupBox groupBox_Dinerware_DataCon;
        private GroupBox gbVirtualClient;
        private GroupBox groupBox_bLoyal_Cred;
        private Label label1;
        private TextBox txtApiKey;
        private Label lblCustomSnippetURL;
        private Label lblAccessKeyError;
        private TextBox txtDBDataSource;
        private TextBox txtPassword;
        private Label label3;
        private TextBox txtUserId;
        private Label label2;
        private Label label4;
        private Button btnAddConfiguration;
        private GroupBox dataSyncServicePanel;
        private Button serviceStopbtn;
        private Button serviceStartbtn;
        LoggerHelper logger = new LoggerHelper();
        private TextBox domainUrltxt;
        private Label domainlbl;
        ConfigurationHelper _conFigHelper = new ConfigurationHelper();
        private Label domainUrlErrorMsg;
        private Label lblGiftCardCode;
        private Label label6;
        private TextBox databaseNametxt;
        private Label lblNextSyncTime;
        private Label lblNextSyncTimeVal;
        bLoyal.Connectors.ServiceUrls _serviceUrls = null;
        private BackgroundWorker worker;
        private string _bLoyalAccessApiKey = string.Empty;
        private Button testDatabaseConBtn;
        private Label authenticationlbl;
        private ComboBox authenticationComboBox;
        private Button btnGetBloyalServiceURL;
        private GroupBox groupBox1;
        private Label label11;
        private ListBox loadGiftCardListBox;
        private Button addLoadGiftCardItemBtn;
        private Button removeLoadGiftCardItemBtn;
        private TextBox loadGiftCardItemTxt;
        private Label label12;
        private GroupBox groupBox_bLoyal_Tender;
        private Button btnTestbLoyalConnection;
        private Button lockBtn;
        private Button btnTextClientApi;
        private ComboBox cmbGiftCardTenderCode;
        private ComboBox cmbLoyaltytenderCode;
        private Label lblConnection;
        private Label lblConnectionName;
        private Label lbldatabaseConn;
        private Label lblDatabaseConnName;
        private Label lblVirtualConn;
        private Label lblVirtualConnName;
        private Label label5;
        private Label lblVersion;
        private Button lastSyncResultBtn;
        private string bLoyalApiErrorMsg = string.Empty;

        #endregion

        #region Public Methods

        public frmConfiguration()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmConfiguration_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Test Database Connection 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void testDatabaseConBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = authenticationComboBox.SelectedItem.ToString() == "Windows Authentication" ? 
                    string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True;", txtDBDataSource.Text, databaseNametxt.Text) :
                    string.Format(Constants.DATABASE_CONNECTION_STR_FRM, txtDBDataSource.Text, databaseNametxt.Text, txtUserId.Text, txtPassword.Text);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        GetAllRevenueClasses(connectionString);

                        MessageBox.Show("You have been successfully connected to the database server.");
                        lblDatabaseConnName.Text = "Pass";
                        lblDatabaseConnName.ForeColor = Color.Green;
                    }
                    catch (SqlException ex)
                    {
                        if (ex != null && !string.IsNullOrEmpty(ex.Message))
                            MessageBox.Show(ex.Message);
                        else
                            MessageBox.Show("connection failed.");

                        lblDatabaseConnName.Text = "Fail";
                        lblDatabaseConnName.ForeColor = Color.Red;
                    }
                }
            }
            catch (Exception)
            {
                lblDatabaseConnName.Text = "Fail";
                lblDatabaseConnName.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// Get All RevenueClasses
        /// </summary>
        /// <param name="connectionStr"></param>
        /// <returns></returns>
        public DataSet GetAllRevenueClasses(string connectionStr)
        {
            using (SqlConnection conn = new SqlConnection(connectionStr))
            {
                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("SELECT *FROM RevenueClass", conn))
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(ds);
                    return ds;
                }
            }
        }       

        /// <summary>
        /// Load Configuration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void frmConfiguration_Load_1(object sender, EventArgs e)
        {
            try
            {
                domainUrlErrorMsg.Text = string.Empty;
                lblNextSyncTime.Text = string.Empty;
                lblNextSyncTimeVal.Text = string.Empty;
                GetDataSyncServiceStatus();
                dataSyncServicePanel.Visible = true;
                btnAddConfiguration.Visible = true;

                txtVirtualClientURL.Text = _conFigHelper.URL_VIRTUALCLIENT;
                txtLoginDomain.Text = _conFigHelper.LOGIN_DOMAIN;
                //txtAccessKey.Text = conFigHelper.ACCESS_KEY;
                _bLoyalAccessApiKey = _conFigHelper.ACCESS_KEY;
                domainUrltxt.Text = _conFigHelper.DOMAINURL;

                txtApiKey.Text = _conFigHelper.API_KEY;

                txtDBDataSource.Text = _conFigHelper.DATASOURCE;
                databaseNametxt.Text = _conFigHelper.DATABASENAME;
                txtUserId.Text = _conFigHelper.USERID;
                txtPassword.Text = _conFigHelper.PASSWORD;
                LoadGiftCardItems();

                removeLoadGiftCardItemBtn.BackColor = Color.Gray;
                removeLoadGiftCardItemBtn.Enabled = false;
                txtLoginDomain.TextChanged += new EventHandler(FrmConfiguration_txtLoginDomain);
                txtApiKey.TextChanged += new EventHandler(FrmConfiguration_txtApiKey);
                txtDBDataSource.TextChanged += new EventHandler(FrmConfiguration_txtDBDataSource);
                databaseNametxt.TextChanged += new EventHandler(FrmConfiguration_txtDBName);
                txtUserId.TextChanged += new EventHandler(FrmConfiguration_txtUserId);
                txtPassword.TextChanged += new EventHandler(FrmConfiguration_txtPassword);
                txtVirtualClientURL.TextChanged += new EventHandler(FrmConfiguration_txtVirtualClientURL);
                if (_conFigHelper != null && !string.IsNullOrEmpty(_conFigHelper.ACCESS_KEY))
                    GetServiceStatus();
                BackgroundCheckInWorker();
                authenticationComboBox.SelectedIndex = _conFigHelper.IS_WINDOWS_AUTH ? 0 : 1;
                authenticationComboBox.DropDownStyle = ComboBoxStyle.DropDownList;

                if (_conFigHelper.IS_Test_BLoyal_Connection)
                {
                    lblConnectionName.Text = "Pass";
                    lblConnectionName.ForeColor = Color.Green;
                }
                else
                {
                    lblConnectionName.Text = "Fail";
                    lblConnectionName.ForeColor = Color.Red;
                }

                if (_conFigHelper.IS_Test_Database_Connection)
                {
                    lblDatabaseConnName.Text = "Pass";
                    lblDatabaseConnName.ForeColor = Color.Green;
                }
                else
                {
                    lblDatabaseConnName.Text = "Fail";
                    lblDatabaseConnName.ForeColor = Color.Red;
                }

                if (_conFigHelper.IS_Test_Virtual_Client_Connection)
                {
                    lblVirtualConnName.Text = "Pass";
                    lblVirtualConnName.ForeColor = Color.Green;
                }
                else
                {
                    lblVirtualConnName.Text = "Fail";
                    lblVirtualConnName.ForeColor = Color.Red;
                }

                if (!string.IsNullOrEmpty(txtLoginDomain.Text))
                {
                    string domainURL = !string.IsNullOrEmpty(domainUrltxt.Text) ? domainUrltxt.Text : "https://domain.bloyal.com";
                    var serviceUrls = GetServiceUrls(txtLoginDomain.Text, domainURL);
                    await GetBloyalTenderCode(serviceUrls);
                }
                else
                {
                    if (cmbLoyaltytenderCode.Items.Count > 0)
                        cmbLoyaltytenderCode.SelectedIndex = 0;
                    if (cmbGiftCardTenderCode.Items.Count > 0)
                        cmbGiftCardTenderCode.SelectedIndex = 0;
                    cmbLoyaltytenderCode.DropDownStyle = ComboBoxStyle.DropDownList;
                    cmbGiftCardTenderCode.DropDownStyle = ComboBoxStyle.DropDownList;
                }

                loadGiftCardListBox.SelectionMode = SelectionMode.One;
                loadGiftCardListBox.SelectedIndexChanged += new EventHandler(SelectedItem_LoadGiftCardListBox);

                lblVersion.Text = ConnectorVersion.GetVersion();

                if (string.IsNullOrEmpty(_bLoyalAccessApiKey) || string.IsNullOrEmpty(txtLoginDomain.Text))
                {
                    btnTestbLoyalConnection.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Valid DomainURL
        /// </summary>
        /// <param name="isValid"></param>
        /// <returns></returns>
        private bool ValidDomainURL()
        {
            bool isValid = true;

            if (!string.IsNullOrEmpty(domainUrltxt.Text))
            {
                if (ValidateDomainURL())
                {
                    lblCustomSnippetURL.Text = string.Empty;
                    domainUrlErrorMsg.Visible = false;
                }
                else
                {
                    domainUrlErrorMsg.Visible = true;
                    domainUrlErrorMsg.Text = "Please enter valid domain url";
                    domainUrltxt.Select();
                    domainUrltxt.BackColor = Color.LightCoral;
                    isValid = false;
                }
            }

            return isValid;
        }

        /// <summary>
        /// Get bLoyal Service Urls
        /// </summary>
        /// <param name="loginDomain"></param>
        /// <param name="domainURL"></param>
        /// <returns></returns>
        private static ServiceUrls GetServiceUrls(string loginDomain, string domainURL)
        {
            var serviceUrls = new ServiceUrls();
            var getServiceTask = Task.Run(async () => { serviceUrls = await bLoyalService.GetServiceUrlsAsync(loginDomain, domainURL); });
            getServiceTask.Wait();
            return serviceUrls;
        }

        /// <summary>
        /// Get Connector ContextInfo
        /// </summary>
        /// <param name="serviceUrls"></param>
        /// <returns></returns>
        private ContextInfo GetConnectorContextInfo(ServiceUrls serviceUrls)
        {
            var gridService = new GridService(serviceUrls.GridApiUrl, _bLoyalAccessApiKey);
            ContextInfo contextInfo = null;

            Task.Run(async () =>
            {
                contextInfo = await gridService.GetConnectorContextInfoAsync(); //Get the info about our current key so we can validate it.
            }).Wait();

            return contextInfo;
        }

        /// <summary>
        /// Test bLoyal Connection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTestbLoyalConnection_Click(object sender, EventArgs e)
        {
            try
            {
                bool isValid = ValidDomainURL();
                if (!isValid)
                    return;

                string domainURL = !string.IsNullOrEmpty(domainUrltxt.Text) ? domainUrltxt.Text : "https://domain.bloyal.com";
                var serviceUrls = GetServiceUrls(txtLoginDomain.Text, domainURL);

                if (serviceUrls != null && !string.IsNullOrWhiteSpace(_bLoyalAccessApiKey) && !string.IsNullOrWhiteSpace(txtLoginDomain.Text))
                {
                    ContextInfo contextInfo = GetConnectorContextInfo(serviceUrls);

                    if (contextInfo == null)
                    {
                        MessageBox.Show("Test Connection Failure");
                        lblConnectionName.Text = "Fail";
                        lblConnectionName.ForeColor = Color.Red;
                    }
                    else if (!(contextInfo.KeyType == ContextKeyType.Device))
                    {
                        MessageBox.Show($"API Key type '{contextInfo.KeyType}' not valid. Must be of type Device or Store.");
                        lblConnectionName.Text = "Fail";
                        lblConnectionName.ForeColor = Color.Red;
                    }
                    else if (!contextInfo.LoginDomain.ToLower().Equals(txtLoginDomain.Text.ToLower()))
                    {
                        MessageBox.Show($"Login Domain '{txtLoginDomain.Text}' not valid.");
                        lblConnectionName.Text = "Fail";
                        lblConnectionName.ForeColor = Color.Red;
                    }
                    else
                    {
                        MessageBox.Show("Test Connection Success.");
                        lblConnectionName.Text = "Pass";
                        lblConnectionName.ForeColor = Color.Green;
                    }
                }
                else
                {
                    MessageBox.Show("ServiceUrls Connection Failure.");
                    lblConnectionName.Text = "Fail";
                    lblConnectionName.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                string error = string.IsNullOrEmpty(ex?.InnerException?.Message) ? ex.Message : ex?.InnerException?.Message;
                MessageBox.Show(error);
                lblConnectionName.Text = "Fail";
                lblConnectionName.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// Test Virtual Client API Connection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnTextClientApi_Click(object sender, EventArgs e)
        {
            try
            {
                DinerwareEngineService.VirtualClientClient virtualDinerwareClient;
                var endPointAddress = new System.ServiceModel.EndpointAddress(txtVirtualClientURL.Text);
                var binding = new System.ServiceModel.BasicHttpBinding();
                binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.None;
                virtualDinerwareClient = new DinerwareEngineService.VirtualClientClient(binding, endPointAddress);
                DinerwareEngineService.wsCustomerResult[] allCustomer = virtualDinerwareClient.GetAllCustomers(10000, 10000, "");
                MessageBox.Show("Virtual Client API Connection Success.");
                lblVirtualConnName.Text = "Pass";
                lblVirtualConnName.ForeColor = Color.Green;
            }
            catch (Exception ex)
            {
                string error = string.IsNullOrEmpty(ex?.InnerException?.Message) ? ex.Message : ex?.InnerException?.Message;
                MessageBox.Show(error);
                lblVirtualConnName.Text = "Fail";
                lblVirtualConnName.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// Get last sync result
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lastSyncResultBtn_Click(object sender, EventArgs e)
        {
            try
            {
                string sourceFilePath = Path.Combine(Environment.GetFolderPath(
                   Environment.SpecialFolder.CommonApplicationData),
                   @"bLoyal\DinerwareLogging.txt"
                );

                string result = File.ReadAllText(sourceFilePath);

                frmLastSyncResult lastSyncResult = new frmLastSyncResult(result);
                lastSyncResult.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        /// <summary>
        /// Selected Item Load GiftCard ListBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectedItem_LoadGiftCardListBox(object sender, EventArgs e)
        {
            try
            {
                if (loadGiftCardListBox.SelectedItem != null)
                {
                    removeLoadGiftCardItemBtn.BackColor = Color.Red;
                    removeLoadGiftCardItemBtn.Enabled = true;
                }
                else
                {
                    removeLoadGiftCardItemBtn.BackColor = Color.Gray;
                    removeLoadGiftCardItemBtn.Enabled = false;
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Add Load GiftCard Item 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addLoadGiftCardItemBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(loadGiftCardItemTxt.Text))
                {
                    int index = loadGiftCardListBox.Items.IndexOf(loadGiftCardItemTxt.Text);
                    if (index == -1) // FBPRGRM
                    {
                        loadGiftCardListBox.Items.Add(loadGiftCardItemTxt.Text);
                        loadGiftCardItemTxt.Text = string.Empty;
                    }
                    else
                    {
                        MessageBox.Show("Gift Card Product Name already exist.");
                    }
                }
                else
                {
                    MessageBox.Show("Please enter Dinerware Gift Card Product Name.");
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Remove Load GiftCard Item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeLoadGiftCardItemBtn_Click(object sender, EventArgs e)
        {
            try
            {
                loadGiftCardListBox.Items.Remove(loadGiftCardListBox.SelectedItem);
            }
            catch
            {

            }
        }

        /// <summary>
        /// Load Gift Card Items
        /// </summary>
        private void LoadGiftCardItems()
        {
            try
            {
                string loadgiftCardSku = _conFigHelper.GIFTCARDSKU;
                if (!string.IsNullOrWhiteSpace(loadgiftCardSku))
                {
                    string[] items = loadgiftCardSku.Split(',');
                    foreach (var item in items)
                    {
                        if (!string.IsNullOrWhiteSpace(item))
                            loadGiftCardListBox.Items.Add(item);
                    }
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Virtual ClientURL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmConfiguration_txtVirtualClientURL(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtLoginDomain.Text))
                    txtVirtualClientURL.BackColor = domainUrltxt.BackColor;

            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmConfiguration_txtPassword(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtLoginDomain.Text))
                    txtPassword.BackColor = domainUrltxt.BackColor;

            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmConfiguration_txtUserId(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtLoginDomain.Text))
                    txtUserId.BackColor = domainUrltxt.BackColor;

            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmConfiguration_txtDBName(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtLoginDomain.Text))
                    databaseNametxt.BackColor = domainUrltxt.BackColor;

            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmConfiguration_txtDBDataSource(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtLoginDomain.Text))
                    txtDBDataSource.BackColor = domainUrltxt.BackColor;

            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmConfiguration_txtLoginDomain(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmConfiguration_txtApiKey(object sender, EventArgs e)
        {
            try
            {
            }
            catch
            {

            }
        }

        /// <summary>
        ///  Start background service for Logging
        /// </summary>
        public void BackgroundCheckInWorker()
        {
            try
            {
                worker = new BackgroundWorker();
                worker.DoWork += worker_DoWork;
                System.Timers.Timer timer = new System.Timers.Timer(60000);
                timer.Elapsed += timer_Elapsed;
                timer.Start();
            }
            catch
            {
            }
        }

        /// <summary>
        /// The RunWorkerAsync method submits a request to start the operation running asynchronously.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!worker.IsBusy)
                    worker.RunWorkerAsync();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Call SubmitLogsAsync every 5 minutes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                GetServiceStatus();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Get Service Status
        /// </summary>
        private void GetServiceStatus()
        {
            try
            {
                TextBox.CheckForIllegalCrossThreadCalls = false;
                ServiceController myService = new ServiceController();
                myService.ServiceName = "bLoyal Dinerware Back Office Connector";
                string svcStatus = myService.Status.ToString();
                if (svcStatus == "Running")
                {
                    serviceStopbtn.BackColor = Color.Red;
                    serviceStartbtn.BackColor = Color.Gray;
                    bLoyal.Connectors.Grid.GridService gridService = null;
                    var configuration = new ConfigurationHelper();
                    ServiceUrls serviceUrls = null;
                    var serviceUrlTask = Task.Run(async () => { serviceUrls = await bLoyal.Connectors.bLoyalService.GetServiceUrlsAsync(configuration.LOGIN_DOMAIN, configuration.DOMAIN_URL); });
                    serviceUrlTask.Wait();
                    if (serviceUrls != null)
                        gridService = new bLoyal.Connectors.Grid.GridService(serviceUrls.GridApiUrl, configuration.ACCESS_KEY);

                    if (gridService != null)
                    {
                        ConnectorCheckinResponse checkin = null;
                        var checkinTask = Task.Run(async () => { checkin = await gridService.ConnectorCheckinAsync(); });
                        checkinTask.Wait();
                        if (checkin != null)
                        {
                            if (checkin.NextSyncTime.HasValue)
                            {
                                lblNextSyncTimeVal.Visible = true;
                                lblNextSyncTime.Visible = true;
                                lblNextSyncTime.Text = "Next Sync Time:";
                                //string gmtTime = checkin.NextSyncTime.Value.ToString();
                                lblNextSyncTimeVal.Text = checkin.NextSyncTime.Value.ToLocalTime().ToString();
                            }
                            else
                            {
                                lblNextSyncTime.Text = string.Empty;
                                lblNextSyncTimeVal.Text = string.Empty;
                            }
                        }
                    }
                }
                else if (svcStatus == "Stopped")
                {
                    serviceStartbtn.BackColor = Color.Green;
                    serviceStopbtn.BackColor = Color.Gray;
                    lblNextSyncTime.Text = string.Empty;
                    lblNextSyncTimeVal.Text = string.Empty;
                }
                else if (svcStatus == "StartPending")
                {
                    serviceStopbtn.BackColor = Color.Red;
                    serviceStartbtn.BackColor = Color.Gray;
                    lblNextSyncTimeVal.Visible = true;
                    lblNextSyncTime.Visible = true;
                    lblNextSyncTime.Text = "Next Sync Time:";
                    lblNextSyncTimeVal.Text = "Starting...";
                }
            }
            catch (ApiException)
            {
               
            }
            catch (Exception)
            {
                
            }
        }

        /// <summary>
        /// Get Data Sync Service Status
        /// </summary>
        private void GetDataSyncServiceStatus()
        {
            try
            {
                ServiceController myService = new ServiceController();
                myService.ServiceName = "bLoyal Dinerware Back Office Connector";
                string svcStatus = myService.Status.ToString();
                if (svcStatus == "Running")
                {
                    //serviceStartbtn.BackColor = Color.Green;
                    serviceStopbtn.BackColor = Color.Red;
                }
                else if (svcStatus == "Stopped")
                {
                    //serviceStopbtn.BackColor = Color.Red;
                    serviceStartbtn.BackColor = Color.Green;
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Start service 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serviceStartbtn_Click(object sender, EventArgs e)
        {
            try
            {
                ServiceController myService = new ServiceController();
                myService.ServiceName = "bLoyal Dinerware Back Office Connector";
                string svcStatus = myService.Status.ToString();
                if (svcStatus == "Running")
                {
                    myService.Stop();
                }
                myService.Start();
                serviceStartbtn.BackColor = Color.Gray;
                serviceStopbtn.BackColor = Color.Red;
                GetServiceStatus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Stop Service 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void serviceStopbtn_Click(object sender, EventArgs e)
        {
            try
            {
                ServiceController myService = new ServiceController();
                myService.ServiceName = "bLoyal Dinerware Back Office Connector";
                string svcStatus = myService.Status.ToString();
                if (svcStatus == "Running")
                {
                    myService.Stop();
                }
                lblNextSyncTimeVal.Visible = false;
                lblNextSyncTime.Visible = false;
                serviceStopbtn.BackColor = Color.Gray;
                serviceStartbtn.BackColor = Color.Green;
                GetServiceStatus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Restart Service
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void restartServicebtn_Click(object sender, EventArgs e)
        {
            try
            {

                ServiceController myService = new ServiceController();
                myService.ServiceName = "bLoyal Dinerware Back Office Connector";
                string svcStatus = myService.Status.ToString();
                if (svcStatus == "Running")
                {
                    myService.Stop();
                }
                myService.Start();
                //serviceStartbtn.BackColor = Color.Green;
                //serviceStopbtn.BackColor = Color.Gray;
                serviceStartbtn.BackColor = Color.Gray;
                serviceStopbtn.BackColor = Color.Green;

            }
            catch (Exception ex)
            {

            }
        }

        private void btnGenerateAccessKey_Click(object sender, EventArgs e)
        {
            try
            {
                bool saveFlag = true;
                if (string.IsNullOrEmpty(txtLoginDomain.Text))
                {
                    txtLoginDomain.Select();
                    txtLoginDomain.BackColor = Color.LightCoral;
                    saveFlag = false;
                }
                if (string.IsNullOrEmpty(txtApiKey.Text))
                {
                    txtApiKey.Select();
                    txtApiKey.BackColor = Color.LightCoral;
                    saveFlag = false;
                }
                if (saveFlag)
                {
                    CreatebLoyalAccessKey(true);
                }
            }
            catch (Exception)
            {

            }
        }

        private string CreatebLoyalAccessKey(bool isCreateBtn = false)
        {
            try
            {
                bLoyalApiErrorMsg = string.Empty;
                domainUrlErrorMsg.Text = string.Empty;
                string accessKey = string.Empty;
                var task = Task.Run(async () => { accessKey = await GetAccessKey(); });
                task.Wait();
                if (!string.IsNullOrEmpty(accessKey))
                {
                    _bLoyalAccessApiKey = accessKey;
                    if (isCreateBtn && !string.IsNullOrEmpty(accessKey))
                    {
                        btnAddConfiguration.Enabled = true;
                    }
                    return accessKey;
                }
            }
            catch (ApiException ex)
            {
                if (ex != null && !string.IsNullOrEmpty(ex.Code) && (ex.Code == "Authentication" || ex.Code == "Security" || ex.Code == "Business"))
                {
                    logger.WriteLogError("*** Error in GetAccessKey for Configuration = " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                logger.WriteLogError("*** Error in GetAccessKey for Configuration = " + ex.Message);
            }
            return string.Empty;
        }

        private async Task<string> GetAccessKey()
        {
            bLoyalApiErrorMsg = string.Empty;
            TextBox.CheckForIllegalCrossThreadCalls = false;
            try
            {
                ConfigurationHelper conFigHelper = new ConfigurationHelper();
                string loginDomain = txtLoginDomain.Text;
                _serviceUrls = GetServiceURL(loginDomain);
                if (_serviceUrls != null)
                {
                    var dispenser = new KeyDispenser(loginDomain, _serviceUrls.GridApiUrl);
                    lblAccessKeyError.Text = string.Empty;
                    return await dispenser.GetAccessKeyAsync(conFigHelper.CONNECTOR_KEY, txtApiKey.Text);
                }
                else
                    return string.Empty;
            }
            catch (ApiException ex)
            {
                if (ex != null && !string.IsNullOrEmpty(ex.Code) && (ex.Code == "Authentication" || ex.Code == "Security" || ex.Code == "Business"))
                {
                    bLoyalApiErrorMsg = ex.Message;
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    bLoyalApiErrorMsg = ex.InnerException.Message;
                }
            }
            return string.Empty;
        }

        public bLoyal.Connectors.ServiceUrls GetServiceURL(string loginDomain)
        {
            try
            {
                bLoyalApiErrorMsg = string.Empty;
                if (_serviceUrls == null)
                {
                    XmlDocument xDoc = new XmlDocument();
                    string sourceFilePath = string.Empty;
                    try
                    {
                        //if (!Environment.Is64BitOperatingSystem)
                        //{
                        //    sourceFilePath = string.Format("{0}{1}{2}{3}{4}{5}{6}", Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), System.IO.Path.DirectorySeparatorChar, "bLoyal", System.IO.Path.DirectorySeparatorChar, "Dinerware Integration", System.IO.Path.DirectorySeparatorChar, "DinerwareConfigurationFile.xml");

                        //}
                        //else
                        //{
                        //    sourceFilePath = string.Format("{0}{1}{2}{3}{4}{5}{6}", Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), System.IO.Path.DirectorySeparatorChar, "bLoyal", System.IO.Path.DirectorySeparatorChar, "Dinerware Integration", System.IO.Path.DirectorySeparatorChar, "DinerwareConfigurationFile.xml");
                        //}
                        var configuration = new ConfigurationHelper();
                        sourceFilePath = configuration.GetFilePath();
                        xDoc.Load(@"" + sourceFilePath);
                    }
                    catch
                    {

                    }
                    var getServiceTask = Task.Run(async () => { _serviceUrls = await bLoyal.Connectors.bLoyalService.GetServiceUrlsAsync(loginDomain, !string.IsNullOrEmpty(domainUrltxt.Text) ? domainUrltxt.Text : "https://domain.bloyal.com"); });
                    getServiceTask.Wait();
                }
                return _serviceUrls;
            }
            catch (ApiException ex)
            {
                if (ex != null && !string.IsNullOrEmpty(ex.Code) && (ex.Code == "Authentication" || ex.Code == "Security" || ex.Code == "Business"))
                {
                    bLoyalApiErrorMsg = ex.Message;
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    bLoyalApiErrorMsg = ex.InnerException.Message;

                }
            }
            return null;
        }

        private void btnAddConfiguration_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateTextboxes())
                {
                    if (CreateConnnectorSettingFile())
                    {
                        XmlDocument xDoc = new XmlDocument();

                        var configuration = new ConfigurationHelper();
                        string sourceFilePath = configuration.GetFilePath();
                        xDoc.Load(@"" + sourceFilePath);
                        //string database_Con_Str = "Data Source="+txtDBDataSource.Text+";Initial Catalog=dinerware;User ID="+txtUserId.Text+";Password="+txtPassword.Text;
                        string databaseConnectionStr = authenticationComboBox.SelectedItem.ToString() == "Windows Authentication" ? string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True;", txtDBDataSource.Text, databaseNametxt.Text) : string.Format(Constants.DATABASE_CONNECTION_STR_FRM, txtDBDataSource.Text, databaseNametxt.Text, txtUserId.Text, txtPassword.Text);

                        //String.Format("Data Source={0};Initial Catalog={1};User ID={2};Password={3}", txtDBDataSource.Text, databaseNametxt.Text, txtUserId.Text, txtPassword.Text);
                        xDoc.DocumentElement.SelectSingleNode("DATABASE_CONNECTION_STR").InnerText = CryptoGraphy.EncryptPlainTextToCipherText(databaseConnectionStr);

                        xDoc.DocumentElement.SelectSingleNode("URL_VIRTUALCLIENT").InnerText = txtVirtualClientURL.Text;

                        //if (!string.IsNullOrEmpty(txtSnippetURL.Text))
                        //xDoc.DocumentElement.SelectSingleNode("SNIPPET_URL").InnerText = txtSnippetURL.Text;

                        xDoc.DocumentElement.SelectSingleNode("LOGIN_DOMAIN").InnerText = txtLoginDomain.Text;

                        string encryptedAccessKey = CryptoGraphy.EncryptPlainTextToCipherText(_bLoyalAccessApiKey);
                        xDoc.DocumentElement.SelectSingleNode("ACCESS_KEY").InnerText = encryptedAccessKey;

                        xDoc.DocumentElement.SelectSingleNode("API_KEY").InnerText = txtApiKey.Text;

                        xDoc.DocumentElement.SelectSingleNode("DATASOURCE").InnerText = txtDBDataSource.Text;
                        xDoc.DocumentElement.SelectSingleNode("DATABASENAME").InnerText = databaseNametxt.Text;
                        xDoc.DocumentElement.SelectSingleNode("USERID").InnerText = txtUserId.Text;

                        string encryptedPW = CryptoGraphy.EncryptPlainTextToCipherText(txtPassword.Text);
                        xDoc.DocumentElement.SelectSingleNode("PASSWORD").InnerText = encryptedPW;
                        xDoc.DocumentElement.SelectSingleNode("LoyaltyTenderCode").InnerText = cmbLoyaltytenderCode.SelectedValue != null ? cmbLoyaltytenderCode.SelectedValue.ToString() : string.Empty;
                        xDoc.DocumentElement.SelectSingleNode("GiftCardTenderCode").InnerText = cmbGiftCardTenderCode.SelectedValue != null ? cmbGiftCardTenderCode.SelectedValue.ToString() : string.Empty;

                        string loadGiftCardSKU = string.Empty;
                        if (loadGiftCardListBox.Items != null && loadGiftCardListBox.Items.Count > 0)
                        {
                            foreach (var item in loadGiftCardListBox.Items)
                                loadGiftCardSKU = !string.IsNullOrWhiteSpace(loadGiftCardSKU) ? loadGiftCardSKU + "," + item.ToString() : item.ToString();
                        }
                        xDoc.DocumentElement.SelectSingleNode("GiftCardSKU").InnerText = loadGiftCardSKU;

                        xDoc.DocumentElement.SelectSingleNode("DomainUrl").InnerText = domainUrltxt.Text;

                        xDoc.DocumentElement.SelectSingleNode("IsWindowsAuthentication").InnerText = authenticationComboBox.SelectedItem.ToString() == "Windows Authentication" ? "true" : "false";
                        bool IsTestbLoyalConnection = TestbLoyalConnection();
                        bool IsTestDatabaseConnection = TestDatabaseConnection();
                        bool IsTestVirtualClientConnection = TestVirtualClientConnection();

                        xDoc.DocumentElement.SelectSingleNode("ISTestBLoyalConnection").InnerText = IsTestbLoyalConnection ? "true" : "false";
                        xDoc.DocumentElement.SelectSingleNode("ISTestDatabaseConnection").InnerText = IsTestDatabaseConnection ? "true" : "false";
                        xDoc.DocumentElement.SelectSingleNode("ISTestVirtualClientConnection").InnerText = IsTestVirtualClientConnection ? "true" : "false";


                        //Add Tender
                        DinerwareDBHelper dbHelper = new DinerwareDBHelper();
                        int tenderTypeId = dbHelper.GetTenderTypeId(databaseConnectionStr);
                        if (tenderTypeId == 0)
                            tenderTypeId = dbHelper.AddTenderType(databaseConnectionStr);
                        if (tenderTypeId != 0)
                            xDoc.DocumentElement.SelectSingleNode("DinerwareLoyaltyTenderName").InnerText = Constants.BLOYALLOYALTYTENDER;
                        else
                            xDoc.DocumentElement.SelectSingleNode("DinerwareLoyaltyTenderName").InnerText = string.Empty;

                        int giftCardTenderId = dbHelper.GetbLoyalGiftCardTenderId(databaseConnectionStr);
                        if (giftCardTenderId == 0)
                            giftCardTenderId = dbHelper.AddbLoyalGiftCardTender(databaseConnectionStr);
                        if (giftCardTenderId != 0)
                            xDoc.DocumentElement.SelectSingleNode("DinerwareGiftCardTenderName").InnerText = Constants.BLOYALGIFTCARDTENDER;
                        else
                            xDoc.DocumentElement.SelectSingleNode("DinerwareGiftCardTenderName").InnerText = string.Empty;

                        int bLoyalOrderDiscountId = dbHelper.GetDiscountRuleByName(Constants.ORDERLEVELDISCOUNT, databaseConnectionStr);
                        if (bLoyalOrderDiscountId == 0)
                            bLoyalOrderDiscountId = dbHelper.AddDiscountRule(0, string.Empty, Constants.ORDERLEVELDISCOUNT, string.Empty, Constants.TICKET, databaseConnectionStr);
                        if (bLoyalOrderDiscountId != 0)
                            xDoc.DocumentElement.SelectSingleNode("DinerwareOrderLevelDiscountName").InnerText = Constants.ORDERLEVELDISCOUNT;
                        else
                            xDoc.DocumentElement.SelectSingleNode("DinerwareOrderLevelDiscountName").InnerText = string.Empty;

                        int bLoyalItemDiscountId = dbHelper.GetDiscountRuleByName(Constants.ITEMLEVELDISCOUNT, databaseConnectionStr);
                        if (bLoyalItemDiscountId == 0)
                            bLoyalItemDiscountId = dbHelper.AddDiscountRule(0, string.Empty, Constants.ITEMLEVELDISCOUNT, string.Empty, Constants.ITEMS, databaseConnectionStr);
                        if (bLoyalItemDiscountId != 0)
                            xDoc.DocumentElement.SelectSingleNode("DinerwareItemLevelDiscountName").InnerText = Constants.ITEMLEVELDISCOUNT;
                        else
                            xDoc.DocumentElement.SelectSingleNode("DinerwareItemLevelDiscountName").InnerText = string.Empty;

                        int bLoyalItemPriceId = dbHelper.GetDiscountRuleByName(Constants.ITEMLEVELSALEPRICE, databaseConnectionStr);
                        if (bLoyalItemPriceId == 0)
                            bLoyalItemPriceId = dbHelper.AddDiscountRule(0, string.Empty, Constants.ITEMLEVELSALEPRICE, string.Empty, Constants.ITEMS, databaseConnectionStr);
                        if (bLoyalItemPriceId != 0)
                            xDoc.DocumentElement.SelectSingleNode("DinerwareSalesPriceLevelDiscountName").InnerText = Constants.ITEMLEVELSALEPRICE;
                        else
                            xDoc.DocumentElement.SelectSingleNode("DinerwareSalesPriceLevelDiscountName").InnerText = string.Empty;

                        xDoc.Save(@"" + sourceFilePath);
                        this.Close();
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public bool TestbLoyalConnection()
        {
            try
            {
                bool isValid = ValidDomainURL();
                if (!isValid)
                    return false;

                string domainURL = !string.IsNullOrEmpty(domainUrltxt.Text) ? domainUrltxt.Text : "https://domain.bloyal.com";
                var serviceUrls = GetServiceUrls(txtLoginDomain.Text, domainURL);
                if (serviceUrls != null && !string.IsNullOrWhiteSpace(_bLoyalAccessApiKey) && !string.IsNullOrWhiteSpace(txtLoginDomain.Text))
                {
                    ContextInfo contextInfo = GetConnectorContextInfo(serviceUrls);

                    if (contextInfo == null)
                    {
                        return false;
                    }
                    else if (!(contextInfo.KeyType == ContextKeyType.Device))
                    {
                        return false;
                    }
                    else if (!contextInfo.LoginDomain.ToLower().Equals(txtLoginDomain.Text.ToLower()))
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool TestDatabaseConnection()
        {
            try
            {
                string connectionString = authenticationComboBox.SelectedItem.ToString() == "Windows Authentication" ? string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True;", txtDBDataSource.Text, databaseNametxt.Text) : string.Format(Constants.DATABASE_CONNECTION_STR_FRM, txtDBDataSource.Text, databaseNametxt.Text, txtUserId.Text, txtPassword.Text);
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        GetAllRevenueClasses(connectionString);

                        return true;
                    }
                    catch (SqlException)
                    {
                        return false;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool TestVirtualClientConnection()
        {
            try
            {
                DinerwareEngineService.VirtualClientClient virtualDinerwareClient;
                var endPointAddress = new System.ServiceModel.EndpointAddress(txtVirtualClientURL.Text);
                var binding = new System.ServiceModel.BasicHttpBinding();
                binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.None;
                virtualDinerwareClient = new DinerwareEngineService.VirtualClientClient(binding, endPointAddress);
                var allCustomer = virtualDinerwareClient.GetAllCustomers(10000, 10000, "");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CreateConnnectorSettingFile()
        {
            bool isCreated = false;
            try
            {
                var configuration = new ConfigurationHelper();
                string sourceFilePath = configuration.GetFilePath();

                if (!System.IO.File.Exists(sourceFilePath))
                {
                    System.IO.StringWriter stringwriter = new System.IO.StringWriter();
                    System.Xml.XmlTextWriter xmlTextWriter = new System.Xml.XmlTextWriter(stringwriter);
                    xmlTextWriter.Formatting = System.Xml.Formatting.Indented;
                    xmlTextWriter.WriteStartDocument();
                    xmlTextWriter.WriteStartElement("DinerwareConfiguration");
                    xmlTextWriter.WriteElementString("SNIPPET_URL", string.Empty);
                    xmlTextWriter.WriteElementString("LOGIN_DOMAIN", string.Empty);
                    xmlTextWriter.WriteElementString("ACCESS_KEY", string.Empty);
                    xmlTextWriter.WriteElementString("API_KEY", string.Empty);
                    xmlTextWriter.WriteElementString("URL_VIRTUALCLIENT", string.Empty);
                    xmlTextWriter.WriteElementString("DATABASE_CONNECTION_STR", string.Empty);
                    xmlTextWriter.WriteElementString("DATASOURCE", string.Empty);
                    xmlTextWriter.WriteElementString("DATABASENAME", string.Empty);
                    xmlTextWriter.WriteElementString("USERID", string.Empty);
                    xmlTextWriter.WriteElementString("PASSWORD", string.Empty);
                    xmlTextWriter.WriteElementString("LoyaltyTenderCode", string.Empty);
                    xmlTextWriter.WriteElementString("GiftCardTenderCode", string.Empty);
                    xmlTextWriter.WriteElementString("GiftCardSKU", string.Empty);
                    xmlTextWriter.WriteElementString("DinerwareGiftCardTenderName", string.Empty);
                    xmlTextWriter.WriteElementString("DinerwareLoyaltyTenderName", string.Empty);
                    xmlTextWriter.WriteElementString("EnablebLoyal", "true");
                    xmlTextWriter.WriteElementString("DomainUrl", string.Empty);
                    xmlTextWriter.WriteElementString("ISDISCOUNTSUMMARY", "true");
                    xmlTextWriter.WriteElementString("ISCALCULATEDISCOUNT", "true");
                    xmlTextWriter.WriteElementString("IsWindowsAuthentication", "false");
                    xmlTextWriter.WriteElementString("ISTestBLoyalConnection", "false");
                    xmlTextWriter.WriteElementString("ISTestDatabaseConnection", "false");
                    xmlTextWriter.WriteElementString("ISTestVirtualClientConnection", "false");
                    xmlTextWriter.WriteElementString("DinerwareOrderLevelDiscountName", string.Empty);
                    xmlTextWriter.WriteElementString("DinerwareItemLevelDiscountName", string.Empty);
                    xmlTextWriter.WriteElementString("DinerwareSalesPriceLevelDiscountName", string.Empty);
                    xmlTextWriter.WriteEndElement();
                    xmlTextWriter.WriteEndDocument();
                    System.Xml.XmlDocument docSave = new System.Xml.XmlDocument();
                    docSave.LoadXml(stringwriter.ToString());

                    docSave.Save(@"" + sourceFilePath);

                    return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isCreated;
        }

        private bool ValidateTextboxes()
        {
            bool saveFlag = true;

            ConfigurationHelper conFigHelper = new ConfigurationHelper();

            if (string.IsNullOrEmpty(txtLoginDomain.Text))
            {
                txtLoginDomain.Select();
                txtLoginDomain.BackColor = Color.LightCoral;
                saveFlag = false;
            }
            if (!string.IsNullOrEmpty(domainUrltxt.Text))
            {
                saveFlag = ValidateDomainURL();
                if (saveFlag)
                {
                    domainUrlErrorMsg.Text = string.Empty;
                    domainUrlErrorMsg.Visible = false;
                }
                else
                {
                    domainUrlErrorMsg.Text = "Please enter valid domain url";
                    domainUrlErrorMsg.Visible = true;
                    saveFlag = false;
                }
            }
            else
                domainUrlErrorMsg.Visible = false;


            if (string.IsNullOrEmpty(txtApiKey.Text))
            {
                txtApiKey.Select();
                txtApiKey.BackColor = Color.LightCoral;
                saveFlag = false;
            }
            if (string.IsNullOrEmpty(txtDBDataSource.Text))
            {
                txtDBDataSource.Select();
                txtDBDataSource.BackColor = Color.LightCoral;
                saveFlag = false;
            }
            if (string.IsNullOrEmpty(databaseNametxt.Text))
            {
                databaseNametxt.Select();
                databaseNametxt.BackColor = Color.LightCoral;
                saveFlag = false;
            }
            if (authenticationComboBox.SelectedItem.ToString() != "Windows Authentication")
            {
                if (string.IsNullOrEmpty(txtUserId.Text))
                {
                    txtUserId.Select();
                    txtUserId.BackColor = Color.LightCoral;
                    saveFlag = false;
                }
                if (string.IsNullOrEmpty(txtPassword.Text))
                {
                    txtPassword.Select();
                    txtPassword.BackColor = Color.LightCoral;
                    saveFlag = false;
                }
            }
            if (string.IsNullOrEmpty(txtVirtualClientURL.Text))
            {
                txtVirtualClientURL.Select();
                txtVirtualClientURL.BackColor = Color.LightCoral;
                saveFlag = false;
            }
            return saveFlag;
        }

        private bool ValidateDomainURL()
        {
            try
            {
                if (!string.IsNullOrEmpty(domainUrltxt.Text))
                {
                    var uri = new Uri(domainUrltxt.Text);
                    var host = uri.Host;
                    string scheme = uri.Scheme;
                    int port = uri.Port;
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.WriteLogError("*** Error in GetAccessKey for Configuration = " + ex.Message);
                return false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCreateOrderUrl_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtViewCustomerUrl_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblLoyaltyengine_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblVIEWCUSTOMER_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblAPPID_Click(object sender, EventArgs e)
        {
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void lblFindCustomerUrl_Click(object sender, EventArgs e)
        {

        }

        private void txtAccessKey_TextChanged(object sender, EventArgs e)
        {

        }

        private void Acc_Click(object sender, EventArgs e)
        {

        }

        private void groupBox_bLoyal_Cred_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void gbVirtualClient_Enter(object sender, EventArgs e)
        {

        }

        private void lblVirtualClientURL_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void dataSyncServicePanel_Enter(object sender, EventArgs e)
        {

        }
      
        private void groupBox_Dinerware_DataCon_Enter(object sender, EventArgs e)
        {

        }

        private void authenticationComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (authenticationComboBox.SelectedItem.ToString() == "Windows Authentication")
            {
                txtUserId.ReadOnly = true;
                txtPassword.ReadOnly = true;
            }
            else
            {
                txtUserId.ReadOnly = false;
                txtPassword.ReadOnly = false;
            }
        }

        private void btnGetBloyalServiceURL_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtLoginDomain.Text))
                {
                    ServiceUrls serviceUrls = null;
                    var connectorTask = Task.Run(async () => { serviceUrls = await bLoyal.Connectors.bLoyalService.GetServiceUrlsAsync(txtLoginDomain.Text, !string.IsNullOrWhiteSpace(domainUrltxt.Text) ? domainUrltxt.Text : "https://domain.bloyal.com"); });
                    connectorTask.Wait();
                    if (serviceUrls != null)
                    {
                        var response = JsonConvert.SerializeObject(serviceUrls);
                        List<string> msg = new List<string>();
                        msg.Add("GetServiceUrls Succeeded.  ");
                        msg.Add($"   Director: {serviceUrls.DirectorUrl}");
                        msg.Add("  ");
                        msg.Add($"   LoyaltyEngineApi: {serviceUrls.LoyaltyEngineApiUrl}");
                        msg.Add($"   OrderEngineApi: {serviceUrls.OrderEngineApiUrl}");
                        msg.Add($"   GridApi: {serviceUrls.GridApiUrl}");
                        msg.Add($"   PaymentApi: {serviceUrls.PaymentApiUrl}");
                        msg.Add($"   WebSnippetsApi: {serviceUrls.WebSnippetsApiUrl}");
                        msg.Add($"   EngagementEngineApi: {serviceUrls.EngagementEngineApiUrl}");
                        msg.Add("  ");
                        msg.Add($"   POSSnippetsUrl: {serviceUrls.POSSnippetsUrl}");
                        msg.Add($"   MyMobileLoyaltyUrl: {serviceUrls.MyMobileLoyaltyUrl}");
                        msg.Add($"   WebSnippetsUrl: {serviceUrls.WebSnippetsUrl}");
                        msg.Add($"   LoggingApiUrl: {serviceUrls.LoggingApiUrl}");
                        frmGetbLoyalServiceURLs show = new frmGetbLoyalServiceURLs(msg);
                        show.ShowDialog();
                    }
                }
                else
                {
                    MessageBox.Show("Please enter login domain name.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConfiguration));
            this.btnAddConfiguration = new System.Windows.Forms.Button();
            this.lblVirtualClientURL = new System.Windows.Forms.Label();
            this.txtVirtualClientURL = new System.Windows.Forms.TextBox();
            this.lblLoyaltyengine = new System.Windows.Forms.Label();
            this.txtLoginDomain = new System.Windows.Forms.TextBox();
            this.lblDataSource = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.gbSnippetURLs = new System.Windows.Forms.GroupBox();
            this.lblCustomSnippetURL = new System.Windows.Forms.Label();
            this.domainUrlErrorMsg = new System.Windows.Forms.Label();
            this.domainlbl = new System.Windows.Forms.Label();
            this.domainUrltxt = new System.Windows.Forms.TextBox();
            this.groupBox_Dinerware_DataCon = new System.Windows.Forms.GroupBox();
            this.lblDatabaseConnName = new System.Windows.Forms.Label();
            this.lbldatabaseConn = new System.Windows.Forms.Label();
            this.authenticationComboBox = new System.Windows.Forms.ComboBox();
            this.authenticationlbl = new System.Windows.Forms.Label();
            this.testDatabaseConBtn = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.databaseNametxt = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUserId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDBDataSource = new System.Windows.Forms.TextBox();
            this.gbVirtualClient = new System.Windows.Forms.GroupBox();
            this.lblVirtualConnName = new System.Windows.Forms.Label();
            this.lblVirtualConn = new System.Windows.Forms.Label();
            this.btnTextClientApi = new System.Windows.Forms.Button();
            this.groupBox_bLoyal_Cred = new System.Windows.Forms.GroupBox();
            this.lblConnectionName = new System.Windows.Forms.Label();
            this.btnTestbLoyalConnection = new System.Windows.Forms.Button();
            this.lblConnection = new System.Windows.Forms.Label();
            this.lockBtn = new System.Windows.Forms.Button();
            this.lblAccessKeyError = new System.Windows.Forms.Label();
            this.txtApiKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblGiftCardCode = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dataSyncServicePanel = new System.Windows.Forms.GroupBox();
            this.lastSyncResultBtn = new System.Windows.Forms.Button();
            this.lblNextSyncTimeVal = new System.Windows.Forms.Label();
            this.lblNextSyncTime = new System.Windows.Forms.Label();
            this.serviceStopbtn = new System.Windows.Forms.Button();
            this.serviceStartbtn = new System.Windows.Forms.Button();
            this.btnGetBloyalServiceURL = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.loadGiftCardItemTxt = new System.Windows.Forms.TextBox();
            this.removeLoadGiftCardItemBtn = new System.Windows.Forms.Button();
            this.addLoadGiftCardItemBtn = new System.Windows.Forms.Button();
            this.loadGiftCardListBox = new System.Windows.Forms.ListBox();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox_bLoyal_Tender = new System.Windows.Forms.GroupBox();
            this.cmbGiftCardTenderCode = new System.Windows.Forms.ComboBox();
            this.cmbLoyaltytenderCode = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.gbSnippetURLs.SuspendLayout();
            this.groupBox_Dinerware_DataCon.SuspendLayout();
            this.gbVirtualClient.SuspendLayout();
            this.groupBox_bLoyal_Cred.SuspendLayout();
            this.dataSyncServicePanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox_bLoyal_Tender.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAddConfiguration
            // 
            this.btnAddConfiguration.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.btnAddConfiguration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnAddConfiguration.ForeColor = System.Drawing.Color.Snow;
            this.btnAddConfiguration.Location = new System.Drawing.Point(631, 564);
            this.btnAddConfiguration.Name = "btnAddConfiguration";
            this.btnAddConfiguration.Size = new System.Drawing.Size(179, 53);
            this.btnAddConfiguration.TabIndex = 0;
            this.btnAddConfiguration.Text = "Save";
            this.btnAddConfiguration.UseVisualStyleBackColor = false;
            this.btnAddConfiguration.Click += new System.EventHandler(this.btnAddConfiguration_Click);
            // 
            // lblVirtualClientURL
            // 
            this.lblVirtualClientURL.AutoSize = true;
            this.lblVirtualClientURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVirtualClientURL.Location = new System.Drawing.Point(8, 33);
            this.lblVirtualClientURL.Name = "lblVirtualClientURL";
            this.lblVirtualClientURL.Size = new System.Drawing.Size(44, 17);
            this.lblVirtualClientURL.TabIndex = 1;
            this.lblVirtualClientURL.Text = "URL:";
            this.lblVirtualClientURL.Click += new System.EventHandler(this.lblVirtualClientURL_Click);
            // 
            // txtVirtualClientURL
            // 
            this.txtVirtualClientURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVirtualClientURL.Location = new System.Drawing.Point(11, 57);
            this.txtVirtualClientURL.Name = "txtVirtualClientURL";
            this.txtVirtualClientURL.Size = new System.Drawing.Size(244, 23);
            this.txtVirtualClientURL.TabIndex = 2;
            // 
            // lblLoyaltyengine
            // 
            this.lblLoyaltyengine.AutoSize = true;
            this.lblLoyaltyengine.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoyaltyengine.Location = new System.Drawing.Point(11, 33);
            this.lblLoyaltyengine.Name = "lblLoyaltyengine";
            this.lblLoyaltyengine.Size = new System.Drawing.Size(112, 17);
            this.lblLoyaltyengine.TabIndex = 3;
            this.lblLoyaltyengine.Text = "Login Domain:";
            this.lblLoyaltyengine.Click += new System.EventHandler(this.lblLoyaltyengine_Click);
            // 
            // txtLoginDomain
            // 
            this.txtLoginDomain.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLoginDomain.Location = new System.Drawing.Point(15, 55);
            this.txtLoginDomain.Name = "txtLoginDomain";
            this.txtLoginDomain.Size = new System.Drawing.Size(300, 23);
            this.txtLoginDomain.TabIndex = 4;
            this.txtLoginDomain.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // lblDataSource
            // 
            this.lblDataSource.AutoSize = true;
            this.lblDataSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDataSource.Location = new System.Drawing.Point(6, 33);
            this.lblDataSource.Name = "lblDataSource";
            this.lblDataSource.Size = new System.Drawing.Size(107, 17);
            this.lblDataSource.TabIndex = 13;
            this.lblDataSource.Text = "Server Name:";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Snow;
            this.button1.Location = new System.Drawing.Point(816, 564);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(179, 53);
            this.button1.TabIndex = 36;
            this.button1.Text = "Cancel ";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // gbSnippetURLs
            // 
            this.gbSnippetURLs.Controls.Add(this.lblCustomSnippetURL);
            this.gbSnippetURLs.Controls.Add(this.domainUrlErrorMsg);
            this.gbSnippetURLs.Controls.Add(this.domainlbl);
            this.gbSnippetURLs.Controls.Add(this.domainUrltxt);
            this.gbSnippetURLs.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbSnippetURLs.Location = new System.Drawing.Point(13, 424);
            this.gbSnippetURLs.Name = "gbSnippetURLs";
            this.gbSnippetURLs.Size = new System.Drawing.Size(336, 131);
            this.gbSnippetURLs.TabIndex = 37;
            this.gbSnippetURLs.TabStop = false;
            this.gbSnippetURLs.Text = "bLoyal Custom URLs";
            // 
            // lblCustomSnippetURL
            // 
            this.lblCustomSnippetURL.AutoSize = true;
            this.lblCustomSnippetURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomSnippetURL.ForeColor = System.Drawing.Color.Red;
            this.lblCustomSnippetURL.Location = new System.Drawing.Point(10, 80);
            this.lblCustomSnippetURL.Name = "lblCustomSnippetURL";
            this.lblCustomSnippetURL.Size = new System.Drawing.Size(0, 13);
            this.lblCustomSnippetURL.TabIndex = 32;
            // 
            // domainUrlErrorMsg
            // 
            this.domainUrlErrorMsg.AutoSize = true;
            this.domainUrlErrorMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.domainUrlErrorMsg.ForeColor = System.Drawing.Color.Red;
            this.domainUrlErrorMsg.Location = new System.Drawing.Point(12, 81);
            this.domainUrlErrorMsg.Name = "domainUrlErrorMsg";
            this.domainUrlErrorMsg.Size = new System.Drawing.Size(78, 13);
            this.domainUrlErrorMsg.TabIndex = 30;
            this.domainUrlErrorMsg.Text = "Domain URL";
            // 
            // domainlbl
            // 
            this.domainlbl.AutoSize = true;
            this.domainlbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.domainlbl.Location = new System.Drawing.Point(12, 30);
            this.domainlbl.Name = "domainlbl";
            this.domainlbl.Size = new System.Drawing.Size(103, 17);
            this.domainlbl.TabIndex = 27;
            this.domainlbl.Text = "Domain URL:";
            // 
            // domainUrltxt
            // 
            this.domainUrltxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.domainUrltxt.Location = new System.Drawing.Point(16, 55);
            this.domainUrltxt.Name = "domainUrltxt";
            this.domainUrltxt.Size = new System.Drawing.Size(239, 23);
            this.domainUrltxt.TabIndex = 28;
            // 
            // groupBox_Dinerware_DataCon
            // 
            this.groupBox_Dinerware_DataCon.Controls.Add(this.lblDatabaseConnName);
            this.groupBox_Dinerware_DataCon.Controls.Add(this.lbldatabaseConn);
            this.groupBox_Dinerware_DataCon.Controls.Add(this.authenticationComboBox);
            this.groupBox_Dinerware_DataCon.Controls.Add(this.authenticationlbl);
            this.groupBox_Dinerware_DataCon.Controls.Add(this.testDatabaseConBtn);
            this.groupBox_Dinerware_DataCon.Controls.Add(this.label6);
            this.groupBox_Dinerware_DataCon.Controls.Add(this.databaseNametxt);
            this.groupBox_Dinerware_DataCon.Controls.Add(this.txtPassword);
            this.groupBox_Dinerware_DataCon.Controls.Add(this.label3);
            this.groupBox_Dinerware_DataCon.Controls.Add(this.txtUserId);
            this.groupBox_Dinerware_DataCon.Controls.Add(this.label2);
            this.groupBox_Dinerware_DataCon.Controls.Add(this.lblDataSource);
            this.groupBox_Dinerware_DataCon.Controls.Add(this.txtDBDataSource);
            this.groupBox_Dinerware_DataCon.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_Dinerware_DataCon.Location = new System.Drawing.Point(365, 20);
            this.groupBox_Dinerware_DataCon.Name = "groupBox_Dinerware_DataCon";
            this.groupBox_Dinerware_DataCon.Size = new System.Drawing.Size(275, 363);
            this.groupBox_Dinerware_DataCon.TabIndex = 38;
            this.groupBox_Dinerware_DataCon.TabStop = false;
            this.groupBox_Dinerware_DataCon.Text = "Dinerware Database Connection";
            this.groupBox_Dinerware_DataCon.Enter += new System.EventHandler(this.groupBox_Dinerware_DataCon_Enter);
            // 
            // lblDatabaseConnName
            // 
            this.lblDatabaseConnName.AutoSize = true;
            this.lblDatabaseConnName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDatabaseConnName.Location = new System.Drawing.Point(152, 342);
            this.lblDatabaseConnName.Name = "lblDatabaseConnName";
            this.lblDatabaseConnName.Size = new System.Drawing.Size(34, 17);
            this.lblDatabaseConnName.TabIndex = 55;
            this.lblDatabaseConnName.Text = "Fail";
            // 
            // lbldatabaseConn
            // 
            this.lbldatabaseConn.AutoSize = true;
            this.lbldatabaseConn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbldatabaseConn.Location = new System.Drawing.Point(8, 341);
            this.lbldatabaseConn.Name = "lbldatabaseConn";
            this.lbldatabaseConn.Size = new System.Drawing.Size(150, 17);
            this.lbldatabaseConn.TabIndex = 54;
            this.lbldatabaseConn.Text = "Connection Status :";
            // 
            // authenticationComboBox
            // 
            this.authenticationComboBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.authenticationComboBox.FormattingEnabled = true;
            this.authenticationComboBox.Items.AddRange(new object[] {
            "Windows Authentication",
            "SQL Server Authentication"});
            this.authenticationComboBox.Location = new System.Drawing.Point(11, 157);
            this.authenticationComboBox.Name = "authenticationComboBox";
            this.authenticationComboBox.Size = new System.Drawing.Size(244, 26);
            this.authenticationComboBox.TabIndex = 53;
            this.authenticationComboBox.SelectedIndexChanged += new System.EventHandler(this.authenticationComboBox_SelectedIndexChanged);
            // 
            // authenticationlbl
            // 
            this.authenticationlbl.AutoSize = true;
            this.authenticationlbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.authenticationlbl.Location = new System.Drawing.Point(8, 135);
            this.authenticationlbl.Name = "authenticationlbl";
            this.authenticationlbl.Size = new System.Drawing.Size(117, 17);
            this.authenticationlbl.TabIndex = 52;
            this.authenticationlbl.Text = "Authentication:";
            // 
            // testDatabaseConBtn
            // 
            this.testDatabaseConBtn.BackColor = System.Drawing.Color.Silver;
            this.testDatabaseConBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.testDatabaseConBtn.ForeColor = System.Drawing.Color.Black;
            this.testDatabaseConBtn.Location = new System.Drawing.Point(11, 310);
            this.testDatabaseConBtn.Name = "testDatabaseConBtn";
            this.testDatabaseConBtn.Size = new System.Drawing.Size(167, 29);
            this.testDatabaseConBtn.TabIndex = 51;
            this.testDatabaseConBtn.Text = "Test Database Connection";
            this.testDatabaseConBtn.UseVisualStyleBackColor = false;
            this.testDatabaseConBtn.Click += new System.EventHandler(this.testDatabaseConBtn_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(6, 83);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(128, 17);
            this.label6.TabIndex = 37;
            this.label6.Text = "Database Name:";
            // 
            // databaseNametxt
            // 
            this.databaseNametxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.databaseNametxt.Location = new System.Drawing.Point(11, 105);
            this.databaseNametxt.Name = "databaseNametxt";
            this.databaseNametxt.Size = new System.Drawing.Size(244, 23);
            this.databaseNametxt.TabIndex = 36;
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Location = new System.Drawing.Point(11, 273);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(244, 23);
            this.txtPassword.TabIndex = 33;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 251);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 17);
            this.label3.TabIndex = 32;
            this.label3.Text = "Password:";
            // 
            // txtUserId
            // 
            this.txtUserId.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserId.Location = new System.Drawing.Point(11, 217);
            this.txtUserId.Name = "txtUserId";
            this.txtUserId.Size = new System.Drawing.Size(244, 23);
            this.txtUserId.TabIndex = 31;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 194);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 17);
            this.label2.TabIndex = 30;
            this.label2.Text = "User ID:";
            // 
            // txtDBDataSource
            // 
            this.txtDBDataSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDBDataSource.Location = new System.Drawing.Point(11, 55);
            this.txtDBDataSource.Name = "txtDBDataSource";
            this.txtDBDataSource.Size = new System.Drawing.Size(244, 23);
            this.txtDBDataSource.TabIndex = 29;
            // 
            // gbVirtualClient
            // 
            this.gbVirtualClient.Controls.Add(this.lblVirtualConnName);
            this.gbVirtualClient.Controls.Add(this.lblVirtualConn);
            this.gbVirtualClient.Controls.Add(this.btnTextClientApi);
            this.gbVirtualClient.Controls.Add(this.lblVirtualClientURL);
            this.gbVirtualClient.Controls.Add(this.txtVirtualClientURL);
            this.gbVirtualClient.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbVirtualClient.Location = new System.Drawing.Point(365, 389);
            this.gbVirtualClient.Name = "gbVirtualClient";
            this.gbVirtualClient.Size = new System.Drawing.Size(275, 166);
            this.gbVirtualClient.TabIndex = 39;
            this.gbVirtualClient.TabStop = false;
            this.gbVirtualClient.Text = "Dinerware Virtual Client API";
            this.gbVirtualClient.Enter += new System.EventHandler(this.gbVirtualClient_Enter);
            // 
            // lblVirtualConnName
            // 
            this.lblVirtualConnName.AutoSize = true;
            this.lblVirtualConnName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVirtualConnName.Location = new System.Drawing.Point(152, 133);
            this.lblVirtualConnName.Name = "lblVirtualConnName";
            this.lblVirtualConnName.Size = new System.Drawing.Size(34, 17);
            this.lblVirtualConnName.TabIndex = 56;
            this.lblVirtualConnName.Text = "Fail";
            // 
            // lblVirtualConn
            // 
            this.lblVirtualConn.AutoSize = true;
            this.lblVirtualConn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVirtualConn.Location = new System.Drawing.Point(8, 132);
            this.lblVirtualConn.Name = "lblVirtualConn";
            this.lblVirtualConn.Size = new System.Drawing.Size(150, 17);
            this.lblVirtualConn.TabIndex = 55;
            this.lblVirtualConn.Text = "Connection Status :";
            // 
            // btnTextClientApi
            // 
            this.btnTextClientApi.BackColor = System.Drawing.Color.Silver;
            this.btnTextClientApi.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTextClientApi.ForeColor = System.Drawing.Color.Black;
            this.btnTextClientApi.Location = new System.Drawing.Point(11, 93);
            this.btnTextClientApi.Name = "btnTextClientApi";
            this.btnTextClientApi.Size = new System.Drawing.Size(167, 29);
            this.btnTextClientApi.TabIndex = 52;
            this.btnTextClientApi.Text = "Test Virtual Client API";
            this.btnTextClientApi.UseVisualStyleBackColor = false;
            this.btnTextClientApi.Click += new System.EventHandler(this.btnTextClientApi_Click);
            // 
            // groupBox_bLoyal_Cred
            // 
            this.groupBox_bLoyal_Cred.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(255)))), ((int)(((byte)(223)))));
            this.groupBox_bLoyal_Cred.Controls.Add(this.lblConnectionName);
            this.groupBox_bLoyal_Cred.Controls.Add(this.btnTestbLoyalConnection);
            this.groupBox_bLoyal_Cred.Controls.Add(this.lblConnection);
            this.groupBox_bLoyal_Cred.Controls.Add(this.lockBtn);
            this.groupBox_bLoyal_Cred.Controls.Add(this.lblAccessKeyError);
            this.groupBox_bLoyal_Cred.Controls.Add(this.txtApiKey);
            this.groupBox_bLoyal_Cred.Controls.Add(this.label1);
            this.groupBox_bLoyal_Cred.Controls.Add(this.lblLoyaltyengine);
            this.groupBox_bLoyal_Cred.Controls.Add(this.txtLoginDomain);
            this.groupBox_bLoyal_Cred.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_bLoyal_Cred.Location = new System.Drawing.Point(13, 20);
            this.groupBox_bLoyal_Cred.Name = "groupBox_bLoyal_Cred";
            this.groupBox_bLoyal_Cred.Size = new System.Drawing.Size(336, 219);
            this.groupBox_bLoyal_Cred.TabIndex = 40;
            this.groupBox_bLoyal_Cred.TabStop = false;
            this.groupBox_bLoyal_Cred.Text = "Loyalty Engine Credentials ";
            this.groupBox_bLoyal_Cred.Enter += new System.EventHandler(this.groupBox_bLoyal_Cred_Enter);
            // 
            // lblConnectionName
            // 
            this.lblConnectionName.AutoSize = true;
            this.lblConnectionName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConnectionName.Location = new System.Drawing.Point(160, 195);
            this.lblConnectionName.Name = "lblConnectionName";
            this.lblConnectionName.Size = new System.Drawing.Size(34, 17);
            this.lblConnectionName.TabIndex = 46;
            this.lblConnectionName.Text = "Fail";
            // 
            // btnTestbLoyalConnection
            // 
            this.btnTestbLoyalConnection.BackColor = System.Drawing.Color.Silver;
            this.btnTestbLoyalConnection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnTestbLoyalConnection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTestbLoyalConnection.ForeColor = System.Drawing.Color.Black;
            this.btnTestbLoyalConnection.Location = new System.Drawing.Point(148, 158);
            this.btnTestbLoyalConnection.Name = "btnTestbLoyalConnection";
            this.btnTestbLoyalConnection.Size = new System.Drawing.Size(167, 30);
            this.btnTestbLoyalConnection.TabIndex = 56;
            this.btnTestbLoyalConnection.Text = "Test bLoyal Connection";
            this.btnTestbLoyalConnection.UseVisualStyleBackColor = false;
            this.btnTestbLoyalConnection.Click += new System.EventHandler(this.btnTestbLoyalConnection_Click);
            // 
            // lblConnection
            // 
            this.lblConnection.AutoSize = true;
            this.lblConnection.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConnection.Location = new System.Drawing.Point(16, 194);
            this.lblConnection.Name = "lblConnection";
            this.lblConnection.Size = new System.Drawing.Size(150, 17);
            this.lblConnection.TabIndex = 45;
            this.lblConnection.Text = "Connection Status :";
            // 
            // lockBtn
            // 
            this.lockBtn.BackColor = System.Drawing.Color.Silver;
            this.lockBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.lockBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockBtn.ForeColor = System.Drawing.Color.Black;
            this.lockBtn.Location = new System.Drawing.Point(15, 158);
            this.lockBtn.Name = "lockBtn";
            this.lockBtn.Size = new System.Drawing.Size(88, 30);
            this.lockBtn.TabIndex = 55;
            this.lockBtn.Text = "Lock";
            this.lockBtn.UseVisualStyleBackColor = false;
            this.lockBtn.Click += new System.EventHandler(this.lockBtn_Click);
            // 
            // lblAccessKeyError
            // 
            this.lblAccessKeyError.AutoSize = true;
            this.lblAccessKeyError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccessKeyError.ForeColor = System.Drawing.Color.Red;
            this.lblAccessKeyError.Location = new System.Drawing.Point(10, 192);
            this.lblAccessKeyError.Name = "lblAccessKeyError";
            this.lblAccessKeyError.Size = new System.Drawing.Size(0, 13);
            this.lblAccessKeyError.TabIndex = 24;
            this.lblAccessKeyError.Click += new System.EventHandler(this.label2_Click);
            // 
            // txtApiKey
            // 
            this.txtApiKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtApiKey.Location = new System.Drawing.Point(14, 114);
            this.txtApiKey.Name = "txtApiKey";
            this.txtApiKey.Size = new System.Drawing.Size(301, 23);
            this.txtApiKey.TabIndex = 23;
            this.txtApiKey.TextChanged += new System.EventHandler(this.textBox1_TextChanged_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 89);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 17);
            this.label1.TabIndex = 22;
            this.label1.Text = "API Key:";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // lblGiftCardCode
            // 
            this.lblGiftCardCode.AutoSize = true;
            this.lblGiftCardCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGiftCardCode.Location = new System.Drawing.Point(11, 96);
            this.lblGiftCardCode.Name = "lblGiftCardCode";
            this.lblGiftCardCode.Size = new System.Drawing.Size(230, 17);
            this.lblGiftCardCode.TabIndex = 31;
            this.lblGiftCardCode.Text = "bLoyal Gift Card Tender Code:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(11, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(217, 17);
            this.label4.TabIndex = 25;
            this.label4.Text = "bLoyal Loyalty Tender Code:";
            // 
            // dataSyncServicePanel
            // 
            this.dataSyncServicePanel.Controls.Add(this.lastSyncResultBtn);
            this.dataSyncServicePanel.Controls.Add(this.lblNextSyncTimeVal);
            this.dataSyncServicePanel.Controls.Add(this.lblNextSyncTime);
            this.dataSyncServicePanel.Controls.Add(this.serviceStopbtn);
            this.dataSyncServicePanel.Controls.Add(this.serviceStartbtn);
            this.dataSyncServicePanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataSyncServicePanel.Location = new System.Drawing.Point(661, 312);
            this.dataSyncServicePanel.Name = "dataSyncServicePanel";
            this.dataSyncServicePanel.Size = new System.Drawing.Size(334, 190);
            this.dataSyncServicePanel.TabIndex = 40;
            this.dataSyncServicePanel.TabStop = false;
            this.dataSyncServicePanel.Text = "Backoffice Connector Service";
            this.dataSyncServicePanel.Enter += new System.EventHandler(this.dataSyncServicePanel_Enter);
            // 
            // lastSyncResultBtn
            // 
            this.lastSyncResultBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.lastSyncResultBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.lastSyncResultBtn.ForeColor = System.Drawing.Color.Snow;
            this.lastSyncResultBtn.Location = new System.Drawing.Point(168, 128);
            this.lastSyncResultBtn.Name = "lastSyncResultBtn";
            this.lastSyncResultBtn.Size = new System.Drawing.Size(143, 46);
            this.lastSyncResultBtn.TabIndex = 48;
            this.lastSyncResultBtn.Text = "Last Sync Result";
            this.lastSyncResultBtn.UseVisualStyleBackColor = false;
            this.lastSyncResultBtn.Click += new System.EventHandler(this.lastSyncResultBtn_Click);
            // 
            // lblNextSyncTimeVal
            // 
            this.lblNextSyncTimeVal.AutoSize = true;
            this.lblNextSyncTimeVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNextSyncTimeVal.Location = new System.Drawing.Point(128, 90);
            this.lblNextSyncTimeVal.Name = "lblNextSyncTimeVal";
            this.lblNextSyncTimeVal.Size = new System.Drawing.Size(17, 17);
            this.lblNextSyncTimeVal.TabIndex = 47;
            this.lblNextSyncTimeVal.Text = "0";
            // 
            // lblNextSyncTime
            // 
            this.lblNextSyncTime.AutoSize = true;
            this.lblNextSyncTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNextSyncTime.Location = new System.Drawing.Point(6, 90);
            this.lblNextSyncTime.Name = "lblNextSyncTime";
            this.lblNextSyncTime.Size = new System.Drawing.Size(125, 17);
            this.lblNextSyncTime.TabIndex = 46;
            this.lblNextSyncTime.Text = "Next Sync Time:";
            // 
            // serviceStopbtn
            // 
            this.serviceStopbtn.BackColor = System.Drawing.Color.Gray;
            this.serviceStopbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.serviceStopbtn.ForeColor = System.Drawing.Color.Snow;
            this.serviceStopbtn.Location = new System.Drawing.Point(168, 27);
            this.serviceStopbtn.Name = "serviceStopbtn";
            this.serviceStopbtn.Size = new System.Drawing.Size(143, 46);
            this.serviceStopbtn.TabIndex = 45;
            this.serviceStopbtn.Text = "Stop";
            this.serviceStopbtn.UseVisualStyleBackColor = false;
            this.serviceStopbtn.Click += new System.EventHandler(this.serviceStopbtn_Click);
            // 
            // serviceStartbtn
            // 
            this.serviceStartbtn.BackColor = System.Drawing.Color.Gray;
            this.serviceStartbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.serviceStartbtn.ForeColor = System.Drawing.Color.Snow;
            this.serviceStartbtn.Location = new System.Drawing.Point(6, 25);
            this.serviceStartbtn.Name = "serviceStartbtn";
            this.serviceStartbtn.Size = new System.Drawing.Size(143, 46);
            this.serviceStartbtn.TabIndex = 44;
            this.serviceStartbtn.Text = "Start";
            this.serviceStartbtn.UseVisualStyleBackColor = false;
            this.serviceStartbtn.Click += new System.EventHandler(this.serviceStartbtn_Click);
            // 
            // btnGetBloyalServiceURL
            // 
            this.btnGetBloyalServiceURL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.btnGetBloyalServiceURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnGetBloyalServiceURL.ForeColor = System.Drawing.Color.Snow;
            this.btnGetBloyalServiceURL.Location = new System.Drawing.Point(441, 564);
            this.btnGetBloyalServiceURL.Name = "btnGetBloyalServiceURL";
            this.btnGetBloyalServiceURL.Size = new System.Drawing.Size(179, 53);
            this.btnGetBloyalServiceURL.TabIndex = 42;
            this.btnGetBloyalServiceURL.Text = "Get bLoyal Service URLs";
            this.btnGetBloyalServiceURL.UseVisualStyleBackColor = false;
            this.btnGetBloyalServiceURL.Click += new System.EventHandler(this.btnGetBloyalServiceURL_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.loadGiftCardItemTxt);
            this.groupBox1.Controls.Add(this.removeLoadGiftCardItemBtn);
            this.groupBox1.Controls.Add(this.addLoadGiftCardItemBtn);
            this.groupBox1.Controls.Add(this.loadGiftCardListBox);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(661, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(334, 268);
            this.groupBox1.TabIndex = 38;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Dinerware Gift Card Product Names";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(6, 141);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(206, 13);
            this.label12.TabIndex = 40;
            this.label12.Text = "Dinerware Gift Card Product Name:";
            // 
            // loadGiftCardItemTxt
            // 
            this.loadGiftCardItemTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadGiftCardItemTxt.Location = new System.Drawing.Point(6, 164);
            this.loadGiftCardItemTxt.Name = "loadGiftCardItemTxt";
            this.loadGiftCardItemTxt.Size = new System.Drawing.Size(309, 23);
            this.loadGiftCardItemTxt.TabIndex = 39;
            // 
            // removeLoadGiftCardItemBtn
            // 
            this.removeLoadGiftCardItemBtn.BackColor = System.Drawing.Color.Red;
            this.removeLoadGiftCardItemBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.removeLoadGiftCardItemBtn.ForeColor = System.Drawing.Color.Snow;
            this.removeLoadGiftCardItemBtn.Location = new System.Drawing.Point(211, 202);
            this.removeLoadGiftCardItemBtn.Name = "removeLoadGiftCardItemBtn";
            this.removeLoadGiftCardItemBtn.Size = new System.Drawing.Size(104, 41);
            this.removeLoadGiftCardItemBtn.TabIndex = 35;
            this.removeLoadGiftCardItemBtn.Text = "Remove";
            this.removeLoadGiftCardItemBtn.UseVisualStyleBackColor = false;
            this.removeLoadGiftCardItemBtn.Click += new System.EventHandler(this.removeLoadGiftCardItemBtn_Click);
            // 
            // addLoadGiftCardItemBtn
            // 
            this.addLoadGiftCardItemBtn.BackColor = System.Drawing.Color.Green;
            this.addLoadGiftCardItemBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.addLoadGiftCardItemBtn.ForeColor = System.Drawing.Color.Snow;
            this.addLoadGiftCardItemBtn.Location = new System.Drawing.Point(101, 202);
            this.addLoadGiftCardItemBtn.Name = "addLoadGiftCardItemBtn";
            this.addLoadGiftCardItemBtn.Size = new System.Drawing.Size(104, 41);
            this.addLoadGiftCardItemBtn.TabIndex = 34;
            this.addLoadGiftCardItemBtn.Text = "Add";
            this.addLoadGiftCardItemBtn.UseVisualStyleBackColor = false;
            this.addLoadGiftCardItemBtn.Click += new System.EventHandler(this.addLoadGiftCardItemBtn_Click);
            // 
            // loadGiftCardListBox
            // 
            this.loadGiftCardListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadGiftCardListBox.FormattingEnabled = true;
            this.loadGiftCardListBox.ItemHeight = 17;
            this.loadGiftCardListBox.Location = new System.Drawing.Point(6, 25);
            this.loadGiftCardListBox.Name = "loadGiftCardListBox";
            this.loadGiftCardListBox.Size = new System.Drawing.Size(309, 106);
            this.loadGiftCardListBox.TabIndex = 33;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Red;
            this.label11.Location = new System.Drawing.Point(10, 80);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(0, 13);
            this.label11.TabIndex = 32;
            // 
            // groupBox_bLoyal_Tender
            // 
            this.groupBox_bLoyal_Tender.Controls.Add(this.cmbGiftCardTenderCode);
            this.groupBox_bLoyal_Tender.Controls.Add(this.cmbLoyaltytenderCode);
            this.groupBox_bLoyal_Tender.Controls.Add(this.label4);
            this.groupBox_bLoyal_Tender.Controls.Add(this.lblGiftCardCode);
            this.groupBox_bLoyal_Tender.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_bLoyal_Tender.Location = new System.Drawing.Point(13, 255);
            this.groupBox_bLoyal_Tender.Name = "groupBox_bLoyal_Tender";
            this.groupBox_bLoyal_Tender.Size = new System.Drawing.Size(336, 166);
            this.groupBox_bLoyal_Tender.TabIndex = 44;
            this.groupBox_bLoyal_Tender.TabStop = false;
            this.groupBox_bLoyal_Tender.Text = "bLoyal Tender Code";
            // 
            // cmbGiftCardTenderCode
            // 
            this.cmbGiftCardTenderCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbGiftCardTenderCode.FormattingEnabled = true;
            this.cmbGiftCardTenderCode.Location = new System.Drawing.Point(14, 117);
            this.cmbGiftCardTenderCode.Name = "cmbGiftCardTenderCode";
            this.cmbGiftCardTenderCode.Size = new System.Drawing.Size(301, 26);
            this.cmbGiftCardTenderCode.TabIndex = 55;
            // 
            // cmbLoyaltytenderCode
            // 
            this.cmbLoyaltytenderCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbLoyaltytenderCode.FormattingEnabled = true;
            this.cmbLoyaltytenderCode.Location = new System.Drawing.Point(14, 57);
            this.cmbLoyaltytenderCode.Name = "cmbLoyaltytenderCode";
            this.cmbLoyaltytenderCode.Size = new System.Drawing.Size(301, 26);
            this.cmbLoyaltytenderCode.TabIndex = 54;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(850, 3);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 17);
            this.label5.TabIndex = 62;
            this.label5.Text = "Version :";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.Location = new System.Drawing.Point(918, 3);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(73, 17);
            this.lblVersion.TabIndex = 63;
            this.lblVersion.Text = ".............";
            // 
            // frmConfiguration
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(255)))), ((int)(((byte)(223)))));
            this.ClientSize = new System.Drawing.Size(1007, 627);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.groupBox_bLoyal_Tender);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnGetBloyalServiceURL);
            this.Controls.Add(this.dataSyncServicePanel);
            this.Controls.Add(this.groupBox_bLoyal_Cred);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.gbVirtualClient);
            this.Controls.Add(this.groupBox_Dinerware_DataCon);
            this.Controls.Add(this.gbSnippetURLs);
            this.Controls.Add(this.btnAddConfiguration);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmConfiguration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Master Settings";
            this.Load += new System.EventHandler(this.frmConfiguration_Load_1);
            this.gbSnippetURLs.ResumeLayout(false);
            this.gbSnippetURLs.PerformLayout();
            this.groupBox_Dinerware_DataCon.ResumeLayout(false);
            this.groupBox_Dinerware_DataCon.PerformLayout();
            this.gbVirtualClient.ResumeLayout(false);
            this.gbVirtualClient.PerformLayout();
            this.groupBox_bLoyal_Cred.ResumeLayout(false);
            this.groupBox_bLoyal_Cred.PerformLayout();
            this.dataSyncServicePanel.ResumeLayout(false);
            this.dataSyncServicePanel.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox_bLoyal_Tender.ResumeLayout(false);
            this.groupBox_bLoyal_Tender.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        /// <summary>
        /// lock 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void lockBtn_Click(object sender, EventArgs e)
        {
            btnTestbLoyalConnection.Enabled = false;
            try
            {
                bool isValid = ValidDomainURL();
                if (!isValid)
                    return;

                if (!string.IsNullOrEmpty(_bLoyalAccessApiKey))
                {
                    DialogResult result = MessageBox.Show("Connector is already locked. Are you sure you want to connect to a different device?", "Device Lock Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (!result.Equals(DialogResult.OK))
                        return;
                }

                string domainURL = !string.IsNullOrEmpty(domainUrltxt.Text) ? domainUrltxt.Text : "https://domain.bloyal.com";
                var serviceUrls = GetServiceUrls(txtLoginDomain.Text, domainURL);
                CreatebLoyalAccessKey(true);

                if (serviceUrls != null && !string.IsNullOrWhiteSpace(_bLoyalAccessApiKey) && !string.IsNullOrWhiteSpace(txtLoginDomain.Text))
                {
                    ContextInfo contextInfo = GetConnectorContextInfo(serviceUrls);

                    await GetBloyalTenderCode(serviceUrls);

                    if (contextInfo == null)
                    {
                        MessageBox.Show("Test Connection Failure");
                        lblConnectionName.Text = "Fail";
                        lblConnectionName.ForeColor = Color.Red;
                    }
                    else if (!(contextInfo.KeyType == ContextKeyType.Device))
                    {
                        MessageBox.Show($"API Key type '{contextInfo.KeyType}' not valid. Must be of type Device or Store.");
                        lblConnectionName.Text = "Fail";
                        lblConnectionName.ForeColor = Color.Red;
                    }
                    else if (!contextInfo.LoginDomain.ToLower().Equals(txtLoginDomain.Text.ToLower()))
                    {
                        MessageBox.Show($"Login Domain '{txtLoginDomain.Text}' not valid.");
                        lblConnectionName.Text = "Fail";
                        lblConnectionName.ForeColor = Color.Red;
                    }
                    else
                    {
                        btnAddConfiguration.BackColor = Color.FromArgb(98, 86, 241);
                        btnAddConfiguration.Enabled = true;
                        MessageBox.Show("API key locked successfully.");
                        btnTestbLoyalConnection.Enabled = true;
                        lblConnectionName.Text = "Pass";
                        lblConnectionName.ForeColor = Color.Green;
                    }
                }
                else
                {
                    MessageBox.Show("ServiceUrls Connection Failure.");
                    lblConnectionName.Text = "Fail";
                    lblConnectionName.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                string error = string.IsNullOrEmpty(ex?.InnerException?.Message) ? ex.Message : ex?.InnerException?.Message;
                MessageBox.Show(error);
                lblConnectionName.Text = "Fail";
                lblConnectionName.ForeColor = Color.Red;
            }
        }

        private async Task GetBloyalTenderCode(ServiceUrls serviceUrls)
        {
            var gridService = new GridService(serviceUrls.GridApiUrl, _bLoyalAccessApiKey);
            var listLoyaltyTenderCode = await gridService.CompanyResource.GetAllTendersAsync() as List<Tender>;
            if (listLoyaltyTenderCode != null)
            {

                var listLoyalty = new List<Tender>();

                foreach (var item in listLoyaltyTenderCode)
                {
                    if (item.TenderType.Equals(bLoyal.Connectors.Grid.Common.DM.TenderCodeType.LoyaltyProgram))
                        listLoyalty.Add(item);
                }

                cmbLoyaltytenderCode.DataSource = listLoyalty;
                cmbLoyaltytenderCode.DisplayMember = "Code";
                cmbLoyaltytenderCode.ValueMember = "Code";
                if ((!string.IsNullOrEmpty(_conFigHelper.TENDERCODE)))
                    cmbLoyaltytenderCode.SelectedValue = _conFigHelper.TENDERCODE;
                else
                    cmbLoyaltytenderCode.SelectedValue = 0;

                cmbLoyaltytenderCode.DropDownStyle = ComboBoxStyle.DropDownList;

                var listGift = new List<Tender>();
                foreach (var item in listLoyaltyTenderCode)
                {
                    if (item.TenderType.Equals(bLoyal.Connectors.Grid.Common.DM.TenderCodeType.GiftCard))
                        listGift.Add(item);
                }
                cmbGiftCardTenderCode.DataSource = listGift;
                cmbGiftCardTenderCode.DisplayMember = "Code";
                cmbGiftCardTenderCode.ValueMember = "Code";
                if (!string.IsNullOrEmpty(_conFigHelper.GIFTCARDTENDERCODE))
                    cmbGiftCardTenderCode.SelectedValue = _conFigHelper.GIFTCARDTENDERCODE;
                else
                    cmbGiftCardTenderCode.SelectedValue = 0;

                cmbGiftCardTenderCode.DropDownStyle = ComboBoxStyle.DropDownList;
            }
        }
      
        #endregion
    }
}
