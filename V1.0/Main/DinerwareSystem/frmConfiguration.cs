using bLoyal.Connectors;
using bLoyal.Connectors.Grid;
using DinerwareSystem.Helpers;
using DinerwareSystem.Models;
using bLoyalLoyaltyEngine = bLoyal.Connectors.LoyaltyEngine;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Linq;
using bLoyal.Utilities;

namespace DinerwareSystem
{
    public partial class frmConfiguration : Form
    {
        #region Properties
        private Label lblLoyaltyengine;
        private Label lblFindCustomerUrl;
        private TextBox txtSnippetURL;
        private Button button1;
        private GroupBox gbSnippetURLs;
        private GroupBox groupBox_bLoyal_Cred;
        private Label label1;
        private TextBox txtApiKey;
        private Label lblCustomSnippetURL;
        private Label lblAccessKeyError;
        private Button btnAddConfiguration;
        private GroupBox DisableEnablePanel;
        LoggerHelper _logger = LoggerHelper.Instance;
        private Label domainUrllbl;
        private TextBox domainUrltxt;
        private CheckBox bypassChkBox;
        ConfigurationHelper _conFigHelper = new ConfigurationHelper(true);
        private GroupBox groupBoxBLoyalLoyaltySummary;
        private CheckBox chkBypassLoyaltySummary;
        private GroupBox groupBoxBloyalCalculateDiscounts;
        private Button btnTestbLoyalConnection;
        private Button lockBtn;
        private CheckBox chkBypassCalculateDiscounts;

        #endregion

        #region Private Member

        bLoyal.Connectors.ServiceUrls _serviceUrls = null;
        private string _bLoyalAccessApiKey = string.Empty;
        private TextBox txtLoginDomain;
        private GroupBox groupBox_bLoyal_Tender;
        private ComboBox cmbGiftCardTenderCode;
        private ComboBox cmbLoyaltytenderCode;
        private Label label4;
        private Label lblGiftCardCode;
        private GroupBox gbVirtualClient;
        private Button btnTextClientApi;
        private Label lblVirtualClientURL;
        private TextBox txtVirtualClientURL;
        private GroupBox groupBox1;
        private Label label12;
        private TextBox loadGiftCardItemTxt;
        private Button removeLoadGiftCardItemBtn;
        private Button addLoadGiftCardItemBtn;
        private ListBox loadGiftCardListBox;
        private Label label11;
        private Label lblConnection;

        private Label label2;
        private Label lblVirtualConnName;
        private Label lblConnectionName;
        private Label label3;
        private Label lblVersion;
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
        /// Load Configuration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void frmConfiguration_Load(object sender, EventArgs e)
        {
            try
            {
                if (_conFigHelper.ENABLE_bLOYAL)
                {
                    bypassChkBox.Checked = false;
                    bypassChkBox.Text = "OFF";
                    bypassChkBox.ForeColor = Color.Green;
                }
                else
                {
                    bypassChkBox.Checked = true;
                    bypassChkBox.Text = "ON";
                    bypassChkBox.ForeColor = Color.Red;
                }

                if (_conFigHelper.IS_CALCULATE_DISCOUNT_WARNING)
                {
                    chkBypassCalculateDiscounts.Checked = false;
                    chkBypassCalculateDiscounts.Text = "OFF";
                    chkBypassCalculateDiscounts.ForeColor = Color.Green;
                }
                else
                {
                    chkBypassCalculateDiscounts.Checked = true;
                    chkBypassCalculateDiscounts.Text = "ON";
                    chkBypassCalculateDiscounts.ForeColor = Color.Red;
                }

                if (_conFigHelper.IS_DISCOUNT_SUMMARY)
                {
                    chkBypassLoyaltySummary.Checked = false;
                    chkBypassLoyaltySummary.Text = "OFF";
                    chkBypassLoyaltySummary.ForeColor = Color.Green;
                }
                else
                {
                    chkBypassLoyaltySummary.Checked = true;
                    chkBypassLoyaltySummary.Text = "ON";
                    chkBypassLoyaltySummary.ForeColor = Color.Red;
                }


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

                txtVirtualClientURL.Text = _conFigHelper.URL_VIRTUALCLIENT;
                txtLoginDomain.Text = _conFigHelper.LOGIN_DOMAIN;
                _bLoyalAccessApiKey = _conFigHelper.ACCESS_KEY;
                //As per michael request on 01/11/2016. Hide Custom snippet url on load.
                txtSnippetURL.Text = _conFigHelper.POS_SNIPPET_URL;
                //txtDBConnectionStr.Text = conFigHelper.DATABASE_CONNECTION_STR;

                txtApiKey.Text = _conFigHelper.API_KEY;

                LoadGiftCardItems();

                domainUrltxt.Text = _conFigHelper.CUSTOM_DOMAIN_URL;
                if (!string.IsNullOrEmpty(txtLoginDomain.Text))
                {
                    string domainURL = !string.IsNullOrEmpty(domainUrltxt.Text) ? domainUrltxt.Text : "https://domain.bloyal.com";
                    var serviceUrls = GetServiceUrls(txtLoginDomain.Text, domainURL);
                    await GetBloyalTenderCode(serviceUrls).ConfigureAwait(true);
                }
                else
                {
                    cmbLoyaltytenderCode.SelectedIndex = 0;
                    cmbGiftCardTenderCode.SelectedIndex = 0;
                    cmbLoyaltytenderCode.DropDownStyle = ComboBoxStyle.DropDownList;
                    cmbGiftCardTenderCode.DropDownStyle = ComboBoxStyle.DropDownList;
                }
                lblVersion.Text = ConnectorVersion.GetVersion();
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "frmConfiguration_Load");
            }
        }

        private void LoadGiftCardItems()
        {
            try
            {
                string loadgiftCardSku = _conFigHelper.GIFTCARD_SKU;
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
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "LoadGiftCardItems");
            }
        }

        private void schedulerServiceDp_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnAddConfiguration_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateTextboxes() && CreateConnnectorSettingFile())
                {
                    XmlDocument xDoc = new XmlDocument();

                    var configuration = new ConfigurationHelper(true);
                    string sourceFilePath = configuration.GetFilePath();
                    xDoc.Load(@"" + sourceFilePath);
                    /*--------------------------------------------------*/
                    //As per discussed with Michael on 09/01/2016, We need to add save button on master setting window for every Workstation to save Snippet URL and enable/disable feature. And all other fields should be read only.   
                    /*--------------------------------------------------*/
                    //string database_Con_Str = "Data Source="+txtDBDataSource.Text+";Initial Catalog=dinerware;User ID="+txtUserId.Text+";Password="+txtPassword.Text;
                    //string database_Con_Str = String.Format("Data Source={0};Initial Catalog=dinerware;User ID={1};Password={2}", txtDBDataSource.Text, txtUserId.Text, txtPassword.Text);
                    //xDoc.DocumentElement.SelectSingleNode("DATABASE_CONNECTION_STR").InnerText = CryptoGraphy.EncryptPlainTextToCipherText(database_Con_Str);
                    xDoc.DocumentElement.SelectSingleNode("URL_VIRTUALCLIENT").InnerText = txtVirtualClientURL.Text;
                    xDoc.DocumentElement.SelectSingleNode("LOGIN_DOMAIN").InnerText = txtLoginDomain.Text;
                    string encryptedAccessKey = CryptoGraphyHelper.EncryptPlainTextToCipherText(_bLoyalAccessApiKey);
                    xDoc.DocumentElement.SelectSingleNode("ACCESS_KEY").InnerText = encryptedAccessKey;
                    xDoc.DocumentElement.SelectSingleNode("API_KEY").InnerText = txtApiKey.Text;
                    //xDoc.DocumentElement.SelectSingleNode("DATASOURCE").InnerText = txtDBDataSource.Text;
                    //xDoc.DocumentElement.SelectSingleNode("USERID").InnerText = txtUserId.Text;
                    //string encryptedPW = CryptoGraphy.EncryptPlainTextToCipherText(txtPassword.Text);
                    //xDoc.DocumentElement.SelectSingleNode("PASSWORD").InnerText = encryptedPW;
                    xDoc.DocumentElement.SelectSingleNode("LoyaltyTenderCode").InnerText = cmbLoyaltytenderCode.SelectedValue != null ? cmbLoyaltytenderCode.SelectedValue.ToString() : string.Empty;
                    xDoc.DocumentElement.SelectSingleNode("GiftCardTenderCode").InnerText = cmbGiftCardTenderCode.SelectedValue != null ? cmbGiftCardTenderCode.SelectedValue.ToString() : string.Empty;
                    //xDoc.DocumentElement.SelectSingleNode("SCHEDULERTIME").InnerText = schedulerServiceDp.Text;

                    //if (!string.IsNullOrWhiteSpace(txtSnippetURL.Text))
                    xDoc.DocumentElement.SelectSingleNode("SNIPPET_URL").InnerText = txtSnippetURL.Text;

                    xDoc.DocumentElement.SelectSingleNode("EnablebLoyal").InnerText = !bypassChkBox.Checked ? "true" : "false";
                    xDoc.DocumentElement.SelectSingleNode("ISDISCOUNTSUMMARY").InnerText = !chkBypassLoyaltySummary.Checked ? "true" : "false";
                    xDoc.DocumentElement.SelectSingleNode("ISCALCULATEDISCOUNT").InnerText = !chkBypassCalculateDiscounts.Checked ? "true" : "false";

                    //if (!string.IsNullOrWhiteSpace(domainUrltxt.Text))
                    string loadGiftCardSKU = string.Empty;
                    if (loadGiftCardListBox.Items != null && loadGiftCardListBox.Items.Count > 0)
                    {
                        foreach (var item in loadGiftCardListBox.Items)
                            loadGiftCardSKU = !string.IsNullOrWhiteSpace(loadGiftCardSKU) ? loadGiftCardSKU + "," + item.ToString() : item.ToString();
                    }
                    xDoc.DocumentElement.SelectSingleNode("GiftCardSKU").InnerText = loadGiftCardSKU;

                    xDoc.DocumentElement.SelectSingleNode("DomainUrl").InnerText = domainUrltxt.Text;

                    bool IsTestbLoyalConnection = AsyncHelper.RunSync(() => TestbLoyalConnectionAsync());
                    bool IsTestVirtualClientConnection = TestVirtualClientConnection();

                    xDoc.DocumentElement.SelectSingleNode("ISTestBLoyalConnection").InnerText = IsTestbLoyalConnection ? "true" : "false";
                    xDoc.DocumentElement.SelectSingleNode("ISTestVirtualClientConnection").InnerText = IsTestVirtualClientConnection ? "true" : "false";

                    //Add Tender

                    DinerwareEngineService.VirtualClientClient virtualDinerwareClient = GetTenderConfiguration(xDoc);
                    if (virtualDinerwareClient != null)
                        virtualDinerwareClient = GetDiscountRuleConfiguration(xDoc);
                    else
                    {
                        xDoc.DocumentElement.SelectSingleNode("DinerwareLoyaltyTenderName").InnerText = string.Empty;
                        xDoc.DocumentElement.SelectSingleNode("DinerwareGiftCardTenderName").InnerText = string.Empty;
                        xDoc.DocumentElement.SelectSingleNode("DinerwareOrderLevelDiscountName").InnerText = string.Empty;
                        xDoc.DocumentElement.SelectSingleNode("DinerwareItemLevelDiscountName").InnerText = string.Empty;
                        xDoc.DocumentElement.SelectSingleNode("DinerwareSalesPriceLevelDiscountName").InnerText = string.Empty;
                    }

                    xDoc.Save(@"" + sourceFilePath);
                    var config = ConfigurationHelper.NewInstance; // Update the Configuration Instance
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "btnAddConfiguration_Click");
                MessageBox.Show($"Failed to save Configuration Settings. Ex:{ex.Message}" );
            }

            this.Close();
        }

        private DinerwareEngineService.VirtualClientClient GetDiscountRuleConfiguration(XmlDocument xDoc)
        {
            try
            {
                DinerwareEngineService.VirtualClientClient virtualDinerwareClient;
                var endPointAddress = new System.ServiceModel.EndpointAddress(txtVirtualClientURL.Text);
                var binding = new System.ServiceModel.BasicHttpBinding();
                binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.None;
                virtualDinerwareClient = new DinerwareEngineService.VirtualClientClient(binding, endPointAddress);
                var allDiscountList = virtualDinerwareClient.GetAllDiscountList();
                if (allDiscountList != null && allDiscountList.Any())
                {
                    var itemOrderLevelDiscount = allDiscountList.FirstOrDefault(t => t.Name.Equals(Constants.ORDERLEVELDISCOUNT));
                    if (itemOrderLevelDiscount != null)
                        xDoc.DocumentElement.SelectSingleNode("DinerwareOrderLevelDiscountName").InnerText = Constants.ORDERLEVELDISCOUNT;
                    else
                        xDoc.DocumentElement.SelectSingleNode("DinerwareOrderLevelDiscountName").InnerText = string.Empty;

                    var itemLevelDiscount = allDiscountList.FirstOrDefault(t => t.Name.Equals(Constants.ITEMLEVELDISCOUNT));
                    if (itemLevelDiscount != null)
                        xDoc.DocumentElement.SelectSingleNode("DinerwareItemLevelDiscountName").InnerText = Constants.ITEMLEVELDISCOUNT;
                    else
                        xDoc.DocumentElement.SelectSingleNode("DinerwareItemLevelDiscountName").InnerText = string.Empty;

                    var itemLevelSalePrice = allDiscountList.FirstOrDefault(t => t.Name.Equals(Constants.ITEMLEVELSALEPRICE));
                    if (itemLevelSalePrice != null)
                        xDoc.DocumentElement.SelectSingleNode("DinerwareSalesPriceLevelDiscountName").InnerText = Constants.ITEMLEVELSALEPRICE;
                    else
                        xDoc.DocumentElement.SelectSingleNode("DinerwareSalesPriceLevelDiscountName").InnerText = string.Empty;
                }
                else
                {
                    xDoc.DocumentElement.SelectSingleNode("DinerwareOrderLevelDiscountName").InnerText = string.Empty;
                    xDoc.DocumentElement.SelectSingleNode("DinerwareItemLevelDiscountName").InnerText = string.Empty;
                    xDoc.DocumentElement.SelectSingleNode("DinerwareSalesPriceLevelDiscountName").InnerText = string.Empty;
                }

                return virtualDinerwareClient;
            }
            catch
            {
                return null;
            }
        }

        private DinerwareEngineService.VirtualClientClient GetTenderConfiguration(XmlDocument xDoc)
        {
            try
            {
                DinerwareEngineService.VirtualClientClient virtualDinerwareClient;
                var endPointAddress = new System.ServiceModel.EndpointAddress(txtVirtualClientURL.Text);
                var binding = new System.ServiceModel.BasicHttpBinding();
                binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.None;
                virtualDinerwareClient = new DinerwareEngineService.VirtualClientClient(binding, endPointAddress);
                var allTenders = virtualDinerwareClient.GetAllTenderTypes(0);
                if (allTenders != null && allTenders.Any())
                {
                    var itemLoyaltyTender = allTenders.FirstOrDefault(t => t.TenderTypeName.Equals(Constants.BLOYALLOYALTYTENDER, StringComparison.InvariantCultureIgnoreCase));
                    if (itemLoyaltyTender != null)
                        xDoc.DocumentElement.SelectSingleNode("DinerwareLoyaltyTenderName").InnerText = Constants.BLOYALLOYALTYTENDER;
                    else
                        xDoc.DocumentElement.SelectSingleNode("DinerwareLoyaltyTenderName").InnerText = string.Empty;

                    var itemGiftCardTender = allTenders.FirstOrDefault(t => t.TenderTypeName.Equals(Constants.BLOYALGIFTCARDTENDER, StringComparison.InvariantCultureIgnoreCase));
                    if (itemGiftCardTender != null)
                        xDoc.DocumentElement.SelectSingleNode("DinerwareGiftCardTenderName").InnerText = Constants.BLOYALGIFTCARDTENDER;
                    else
                        xDoc.DocumentElement.SelectSingleNode("DinerwareGiftCardTenderName").InnerText = string.Empty;
                }
                else
                {
                    xDoc.DocumentElement.SelectSingleNode("DinerwareLoyaltyTenderName").InnerText = string.Empty;
                    xDoc.DocumentElement.SelectSingleNode("DinerwareGiftCardTenderName").InnerText = string.Empty;
                }

                return virtualDinerwareClient;
            }
            catch
            {
                MessageBox.Show("Unable to Configure Tenders from Dinerware.");
                return null;
            }
        }

        public async Task<bool> TestbLoyalConnectionAsync()
        {
            try
            {

                bool isValid = ValidDomainURL();
                if (!isValid)
                    return false; ;

                string domainURL = !string.IsNullOrEmpty(domainUrltxt.Text) ? domainUrltxt.Text : "https://domain.bloyal.com";
                var serviceUrls = GetServiceUrls(txtLoginDomain.Text, domainURL);
                if (serviceUrls != null && !string.IsNullOrWhiteSpace(_bLoyalAccessApiKey) && !string.IsNullOrWhiteSpace(txtLoginDomain.Text))
                {
                    ContextInfo contextInfo = await GetConnectorContextInfoAsync(serviceUrls).ConfigureAwait(true);

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
                var configuration = new ConfigurationHelper(false);
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
        }

        private bool ValidateTextboxes()
        {
            bool saveFlag = true;
            if (!string.IsNullOrWhiteSpace(txtSnippetURL.Text))
            {
                if (ValidateSnippetURL())
                {
                    lblCustomSnippetURL.Text = string.Empty;
                }
                else
                {
                    MessageBox.Show("Please enter valid snippet url.");
                    txtSnippetURL.Select();
                    txtSnippetURL.BackColor = Color.LightCoral;
                    saveFlag = false;
                }
            }
            if (!string.IsNullOrWhiteSpace(domainUrltxt.Text))
            {
                if (ValidateDomainURL())
                {
                    lblCustomSnippetURL.Text = string.Empty;
                }
                else
                {
                    MessageBox.Show("Please enter valid domain url.");
                    domainUrltxt.Select();
                    domainUrltxt.BackColor = Color.LightCoral;
                    saveFlag = false;
                }
            }
            return saveFlag;
        }

        private bool ValidateDomainURL()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(domainUrltxt.Text))
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
                _logger.WriteLogError(ex, "ValidateDomainURL");
                return false;
            }
        }

        private bool ValidateSnippetURL()
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtSnippetURL.Text))
                {
                    var uri = new Uri(txtSnippetURL.Text);
                    var host = uri.Host;
                    string scheme = uri.Scheme;
                    int port = uri.Port;
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "ValidateSnippetURL");
                return false;
            }
        }

        private void Close_Window_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblLoyaltyengine_Click(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblVIEWCUSTOMER_Click(object sender, EventArgs e)
        {

        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblAPPID_Click(object sender, EventArgs e)
        {
        }

        private void lblFindCustomerUrl_Click(object sender, EventArgs e)
        {

        }

        private void groupBox_bLoyal_Cred_Enter(object sender, EventArgs e)
        {

        }

        private void txtApiKey_TextChanged(object sender, EventArgs e)
        {

        }

        private void ApiKey_Click(object sender, EventArgs e)
        {

        }

        private void lblAccessKeyError_Click(object sender, EventArgs e)
        {

        }

        private void disableRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch
            {

            }
        }

        private void enableRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch
            {

            }
        }

        private void bypassChkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!bypassChkBox.Checked)
            {
                bypassChkBox.Checked = false;
                bypassChkBox.Text = "OFF";
                bypassChkBox.ForeColor = Color.Green;
            }
            else
            {
                bypassChkBox.Checked = true;
                bypassChkBox.Text = "ON";
                bypassChkBox.ForeColor = Color.Red;
            }
        }

        private void chkBypassLoyaltySummary_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkBypassLoyaltySummary.Checked)
            {
                chkBypassLoyaltySummary.Checked = false;
                chkBypassLoyaltySummary.Text = "OFF";
                chkBypassLoyaltySummary.ForeColor = Color.Green;
            }
            else
            {
                chkBypassLoyaltySummary.Checked = true;
                chkBypassLoyaltySummary.Text = "ON";
                chkBypassLoyaltySummary.ForeColor = Color.Red;
            }
        }

        private void chkBypassCalculateDiscounts_CheckedChanged(object sender, EventArgs e)
        {
            if (!chkBypassCalculateDiscounts.Checked)
            {
                chkBypassCalculateDiscounts.Checked = false;
                chkBypassCalculateDiscounts.Text = "OFF";
                chkBypassCalculateDiscounts.ForeColor = Color.Green;
            }
            else
            {
                chkBypassCalculateDiscounts.Checked = true;
                chkBypassCalculateDiscounts.Text = "ON";
                chkBypassCalculateDiscounts.ForeColor = Color.Red;
            }
        }


        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConfiguration));
            this.btnAddConfiguration = new System.Windows.Forms.Button();
            this.lblLoyaltyengine = new System.Windows.Forms.Label();
            this.lblFindCustomerUrl = new System.Windows.Forms.Label();
            this.txtSnippetURL = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.gbSnippetURLs = new System.Windows.Forms.GroupBox();
            this.lblCustomSnippetURL = new System.Windows.Forms.Label();
            this.domainUrllbl = new System.Windows.Forms.Label();
            this.domainUrltxt = new System.Windows.Forms.TextBox();
            this.groupBox_bLoyal_Cred = new System.Windows.Forms.GroupBox();
            this.lblConnectionName = new System.Windows.Forms.Label();
            this.lblConnection = new System.Windows.Forms.Label();
            this.txtLoginDomain = new System.Windows.Forms.TextBox();
            this.btnTestbLoyalConnection = new System.Windows.Forms.Button();
            this.lockBtn = new System.Windows.Forms.Button();
            this.lblAccessKeyError = new System.Windows.Forms.Label();
            this.txtApiKey = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DisableEnablePanel = new System.Windows.Forms.GroupBox();
            this.bypassChkBox = new System.Windows.Forms.CheckBox();
            this.groupBoxBLoyalLoyaltySummary = new System.Windows.Forms.GroupBox();
            this.chkBypassLoyaltySummary = new System.Windows.Forms.CheckBox();
            this.groupBoxBloyalCalculateDiscounts = new System.Windows.Forms.GroupBox();
            this.chkBypassCalculateDiscounts = new System.Windows.Forms.CheckBox();
            this.groupBox_bLoyal_Tender = new System.Windows.Forms.GroupBox();
            this.cmbGiftCardTenderCode = new System.Windows.Forms.ComboBox();
            this.cmbLoyaltytenderCode = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblGiftCardCode = new System.Windows.Forms.Label();
            this.gbVirtualClient = new System.Windows.Forms.GroupBox();
            this.lblVirtualConnName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnTextClientApi = new System.Windows.Forms.Button();
            this.lblVirtualClientURL = new System.Windows.Forms.Label();
            this.txtVirtualClientURL = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.loadGiftCardItemTxt = new System.Windows.Forms.TextBox();
            this.removeLoadGiftCardItemBtn = new System.Windows.Forms.Button();
            this.addLoadGiftCardItemBtn = new System.Windows.Forms.Button();
            this.loadGiftCardListBox = new System.Windows.Forms.ListBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.gbSnippetURLs.SuspendLayout();
            this.groupBox_bLoyal_Cred.SuspendLayout();
            this.DisableEnablePanel.SuspendLayout();
            this.groupBoxBLoyalLoyaltySummary.SuspendLayout();
            this.groupBoxBloyalCalculateDiscounts.SuspendLayout();
            this.groupBox_bLoyal_Tender.SuspendLayout();
            this.gbVirtualClient.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAddConfiguration
            // 
            this.btnAddConfiguration.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.btnAddConfiguration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnAddConfiguration.ForeColor = System.Drawing.Color.Snow;
            this.btnAddConfiguration.Location = new System.Drawing.Point(664, 453);
            this.btnAddConfiguration.Name = "btnAddConfiguration";
            this.btnAddConfiguration.Size = new System.Drawing.Size(163, 55);
            this.btnAddConfiguration.TabIndex = 0;
            this.btnAddConfiguration.Text = "Save";
            this.btnAddConfiguration.UseVisualStyleBackColor = false;
            this.btnAddConfiguration.Click += new System.EventHandler(this.btnAddConfiguration_Click);
            // 
            // lblLoyaltyengine
            // 
            this.lblLoyaltyengine.AutoSize = true;
            this.lblLoyaltyengine.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoyaltyengine.Location = new System.Drawing.Point(10, 26);
            this.lblLoyaltyengine.Name = "lblLoyaltyengine";
            this.lblLoyaltyengine.Size = new System.Drawing.Size(112, 17);
            this.lblLoyaltyengine.TabIndex = 3;
            this.lblLoyaltyengine.Text = "Login Domain:";
            this.lblLoyaltyengine.Click += new System.EventHandler(this.lblLoyaltyengine_Click);
            // 
            // lblFindCustomerUrl
            // 
            this.lblFindCustomerUrl.AutoSize = true;
            this.lblFindCustomerUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFindCustomerUrl.Location = new System.Drawing.Point(12, 24);
            this.lblFindCustomerUrl.Name = "lblFindCustomerUrl";
            this.lblFindCustomerUrl.Size = new System.Drawing.Size(141, 17);
            this.lblFindCustomerUrl.TabIndex = 15;
            this.lblFindCustomerUrl.Text = "POS Snippet URL:";
            this.lblFindCustomerUrl.Click += new System.EventHandler(this.lblFindCustomerUrl_Click);
            // 
            // txtSnippetURL
            // 
            this.txtSnippetURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSnippetURL.Location = new System.Drawing.Point(14, 46);
            this.txtSnippetURL.Name = "txtSnippetURL";
            this.txtSnippetURL.Size = new System.Drawing.Size(297, 23);
            this.txtSnippetURL.TabIndex = 31;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Snow;
            this.button1.Location = new System.Drawing.Point(834, 452);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(163, 55);
            this.button1.TabIndex = 36;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.Close_Window_Click);
            // 
            // gbSnippetURLs
            // 
            this.gbSnippetURLs.Controls.Add(this.lblCustomSnippetURL);
            this.gbSnippetURLs.Controls.Add(this.lblFindCustomerUrl);
            this.gbSnippetURLs.Controls.Add(this.domainUrllbl);
            this.gbSnippetURLs.Controls.Add(this.txtSnippetURL);
            this.gbSnippetURLs.Controls.Add(this.domainUrltxt);
            this.gbSnippetURLs.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbSnippetURLs.Location = new System.Drawing.Point(12, 379);
            this.gbSnippetURLs.Name = "gbSnippetURLs";
            this.gbSnippetURLs.Size = new System.Drawing.Size(331, 129);
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
            // domainUrllbl
            // 
            this.domainUrllbl.AutoSize = true;
            this.domainUrllbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.domainUrllbl.Location = new System.Drawing.Point(12, 75);
            this.domainUrllbl.Name = "domainUrllbl";
            this.domainUrllbl.Size = new System.Drawing.Size(103, 17);
            this.domainUrllbl.TabIndex = 28;
            this.domainUrllbl.Text = "Domain URL:";
            // 
            // domainUrltxt
            // 
            this.domainUrltxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.domainUrltxt.Location = new System.Drawing.Point(14, 96);
            this.domainUrltxt.Name = "domainUrltxt";
            this.domainUrltxt.Size = new System.Drawing.Size(297, 23);
            this.domainUrltxt.TabIndex = 27;
            // 
            // groupBox_bLoyal_Cred
            // 
            this.groupBox_bLoyal_Cred.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(255)))), ((int)(((byte)(223)))));
            this.groupBox_bLoyal_Cred.Controls.Add(this.lblConnectionName);
            this.groupBox_bLoyal_Cred.Controls.Add(this.lblConnection);
            this.groupBox_bLoyal_Cred.Controls.Add(this.txtLoginDomain);
            this.groupBox_bLoyal_Cred.Controls.Add(this.btnTestbLoyalConnection);
            this.groupBox_bLoyal_Cred.Controls.Add(this.lockBtn);
            this.groupBox_bLoyal_Cred.Controls.Add(this.lblAccessKeyError);
            this.groupBox_bLoyal_Cred.Controls.Add(this.txtApiKey);
            this.groupBox_bLoyal_Cred.Controls.Add(this.label1);
            this.groupBox_bLoyal_Cred.Controls.Add(this.lblLoyaltyengine);
            this.groupBox_bLoyal_Cred.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_bLoyal_Cred.Location = new System.Drawing.Point(13, 28);
            this.groupBox_bLoyal_Cred.Name = "groupBox_bLoyal_Cred";
            this.groupBox_bLoyal_Cred.Size = new System.Drawing.Size(331, 186);
            this.groupBox_bLoyal_Cred.TabIndex = 40;
            this.groupBox_bLoyal_Cred.TabStop = false;
            this.groupBox_bLoyal_Cred.Text = "Loyalty Engine Credentials ";
            this.groupBox_bLoyal_Cred.Enter += new System.EventHandler(this.groupBox_bLoyal_Cred_Enter);
            // 
            // lblConnectionName
            // 
            this.lblConnectionName.AutoSize = true;
            this.lblConnectionName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConnectionName.Location = new System.Drawing.Point(158, 161);
            this.lblConnectionName.Name = "lblConnectionName";
            this.lblConnectionName.Size = new System.Drawing.Size(34, 17);
            this.lblConnectionName.TabIndex = 60;
            this.lblConnectionName.Text = "Fail";
            // 
            // lblConnection
            // 
            this.lblConnection.AutoSize = true;
            this.lblConnection.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConnection.Location = new System.Drawing.Point(14, 161);
            this.lblConnection.Name = "lblConnection";
            this.lblConnection.Size = new System.Drawing.Size(150, 17);
            this.lblConnection.TabIndex = 59;
            this.lblConnection.Text = "Connection Status :";
            // 
            // txtLoginDomain
            // 
            this.txtLoginDomain.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLoginDomain.Location = new System.Drawing.Point(13, 51);
            this.txtLoginDomain.Name = "txtLoginDomain";
            this.txtLoginDomain.Size = new System.Drawing.Size(300, 23);
            this.txtLoginDomain.TabIndex = 58;
            // 
            // btnTestbLoyalConnection
            // 
            this.btnTestbLoyalConnection.BackColor = System.Drawing.Color.Silver;
            this.btnTestbLoyalConnection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnTestbLoyalConnection.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTestbLoyalConnection.ForeColor = System.Drawing.Color.Black;
            this.btnTestbLoyalConnection.Location = new System.Drawing.Point(147, 128);
            this.btnTestbLoyalConnection.Name = "btnTestbLoyalConnection";
            this.btnTestbLoyalConnection.Size = new System.Drawing.Size(167, 30);
            this.btnTestbLoyalConnection.TabIndex = 57;
            this.btnTestbLoyalConnection.Text = "Test bLoyal Connection";
            this.btnTestbLoyalConnection.UseVisualStyleBackColor = false;
            this.btnTestbLoyalConnection.Click += new System.EventHandler(this.btnTestbLoyalConnection_Click);
            // 
            // lockBtn
            // 
            this.lockBtn.BackColor = System.Drawing.Color.Silver;
            this.lockBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.lockBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lockBtn.ForeColor = System.Drawing.Color.Black;
            this.lockBtn.Location = new System.Drawing.Point(13, 127);
            this.lockBtn.Name = "lockBtn";
            this.lockBtn.Size = new System.Drawing.Size(88, 30);
            this.lockBtn.TabIndex = 56;
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
            this.lblAccessKeyError.Click += new System.EventHandler(this.lblAccessKeyError_Click);
            // 
            // txtApiKey
            // 
            this.txtApiKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtApiKey.Location = new System.Drawing.Point(14, 99);
            this.txtApiKey.Name = "txtApiKey";
            this.txtApiKey.Size = new System.Drawing.Size(299, 23);
            this.txtApiKey.TabIndex = 23;
            this.txtApiKey.TextChanged += new System.EventHandler(this.txtApiKey_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 17);
            this.label1.TabIndex = 22;
            this.label1.Text = "API Key:";
            this.label1.Click += new System.EventHandler(this.ApiKey_Click);
            // 
            // DisableEnablePanel
            // 
            this.DisableEnablePanel.Controls.Add(this.bypassChkBox);
            this.DisableEnablePanel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DisableEnablePanel.Location = new System.Drawing.Point(664, 204);
            this.DisableEnablePanel.Name = "DisableEnablePanel";
            this.DisableEnablePanel.Size = new System.Drawing.Size(333, 68);
            this.DisableEnablePanel.TabIndex = 43;
            this.DisableEnablePanel.TabStop = false;
            this.DisableEnablePanel.Text = "Bypass bLoyal Functionality";
            // 
            // bypassChkBox
            // 
            this.bypassChkBox.AutoSize = true;
            this.bypassChkBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bypassChkBox.Location = new System.Drawing.Point(14, 33);
            this.bypassChkBox.Name = "bypassChkBox";
            this.bypassChkBox.Size = new System.Drawing.Size(56, 20);
            this.bypassChkBox.TabIndex = 0;
            this.bypassChkBox.Text = "OFF";
            this.bypassChkBox.UseVisualStyleBackColor = true;
            this.bypassChkBox.CheckedChanged += new System.EventHandler(this.bypassChkBox_CheckedChanged);
            // 
            // groupBoxBLoyalLoyaltySummary
            // 
            this.groupBoxBLoyalLoyaltySummary.Controls.Add(this.chkBypassLoyaltySummary);
            this.groupBoxBLoyalLoyaltySummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxBLoyalLoyaltySummary.Location = new System.Drawing.Point(664, 28);
            this.groupBoxBLoyalLoyaltySummary.Name = "groupBoxBLoyalLoyaltySummary";
            this.groupBoxBLoyalLoyaltySummary.Size = new System.Drawing.Size(333, 68);
            this.groupBoxBLoyalLoyaltySummary.TabIndex = 44;
            this.groupBoxBLoyalLoyaltySummary.TabStop = false;
            this.groupBoxBLoyalLoyaltySummary.Text = "Bypass bLoyal Loyalty Summary Screen";
            // 
            // chkBypassLoyaltySummary
            // 
            this.chkBypassLoyaltySummary.AutoSize = true;
            this.chkBypassLoyaltySummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBypassLoyaltySummary.Location = new System.Drawing.Point(14, 33);
            this.chkBypassLoyaltySummary.Name = "chkBypassLoyaltySummary";
            this.chkBypassLoyaltySummary.Size = new System.Drawing.Size(56, 20);
            this.chkBypassLoyaltySummary.TabIndex = 0;
            this.chkBypassLoyaltySummary.Text = "OFF";
            this.chkBypassLoyaltySummary.UseVisualStyleBackColor = true;
            this.chkBypassLoyaltySummary.CheckedChanged += new System.EventHandler(this.chkBypassLoyaltySummary_CheckedChanged);
            // 
            // groupBoxBloyalCalculateDiscounts
            // 
            this.groupBoxBloyalCalculateDiscounts.Controls.Add(this.chkBypassCalculateDiscounts);
            this.groupBoxBloyalCalculateDiscounts.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxBloyalCalculateDiscounts.Location = new System.Drawing.Point(664, 111);
            this.groupBoxBloyalCalculateDiscounts.Name = "groupBoxBloyalCalculateDiscounts";
            this.groupBoxBloyalCalculateDiscounts.Size = new System.Drawing.Size(333, 68);
            this.groupBoxBloyalCalculateDiscounts.TabIndex = 45;
            this.groupBoxBloyalCalculateDiscounts.TabStop = false;
            this.groupBoxBloyalCalculateDiscounts.Text = "Bypass bLoyal Apply Discounts Warning";
            // 
            // chkBypassCalculateDiscounts
            // 
            this.chkBypassCalculateDiscounts.AutoSize = true;
            this.chkBypassCalculateDiscounts.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBypassCalculateDiscounts.Location = new System.Drawing.Point(14, 33);
            this.chkBypassCalculateDiscounts.Name = "chkBypassCalculateDiscounts";
            this.chkBypassCalculateDiscounts.Size = new System.Drawing.Size(56, 20);
            this.chkBypassCalculateDiscounts.TabIndex = 0;
            this.chkBypassCalculateDiscounts.Text = "OFF";
            this.chkBypassCalculateDiscounts.UseVisualStyleBackColor = true;
            this.chkBypassCalculateDiscounts.CheckedChanged += new System.EventHandler(this.chkBypassCalculateDiscounts_CheckedChanged);
            // 
            // groupBox_bLoyal_Tender
            // 
            this.groupBox_bLoyal_Tender.Controls.Add(this.cmbGiftCardTenderCode);
            this.groupBox_bLoyal_Tender.Controls.Add(this.cmbLoyaltytenderCode);
            this.groupBox_bLoyal_Tender.Controls.Add(this.label4);
            this.groupBox_bLoyal_Tender.Controls.Add(this.lblGiftCardCode);
            this.groupBox_bLoyal_Tender.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox_bLoyal_Tender.Location = new System.Drawing.Point(13, 226);
            this.groupBox_bLoyal_Tender.Name = "groupBox_bLoyal_Tender";
            this.groupBox_bLoyal_Tender.Size = new System.Drawing.Size(332, 147);
            this.groupBox_bLoyal_Tender.TabIndex = 46;
            this.groupBox_bLoyal_Tender.TabStop = false;
            this.groupBox_bLoyal_Tender.Text = "bLoyal Tender Code";
            // 
            // cmbGiftCardTenderCode
            // 
            this.cmbGiftCardTenderCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbGiftCardTenderCode.FormattingEnabled = true;
            this.cmbGiftCardTenderCode.Location = new System.Drawing.Point(13, 113);
            this.cmbGiftCardTenderCode.Name = "cmbGiftCardTenderCode";
            this.cmbGiftCardTenderCode.Size = new System.Drawing.Size(301, 26);
            this.cmbGiftCardTenderCode.TabIndex = 55;
            // 
            // cmbLoyaltytenderCode
            // 
            this.cmbLoyaltytenderCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbLoyaltytenderCode.FormattingEnabled = true;
            this.cmbLoyaltytenderCode.Location = new System.Drawing.Point(14, 52);
            this.cmbLoyaltytenderCode.Name = "cmbLoyaltytenderCode";
            this.cmbLoyaltytenderCode.Size = new System.Drawing.Size(301, 26);
            this.cmbLoyaltytenderCode.TabIndex = 54;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(10, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(217, 17);
            this.label4.TabIndex = 25;
            this.label4.Text = "bLoyal Loyalty Tender Code:";
            // 
            // lblGiftCardCode
            // 
            this.lblGiftCardCode.AutoSize = true;
            this.lblGiftCardCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGiftCardCode.Location = new System.Drawing.Point(11, 87);
            this.lblGiftCardCode.Name = "lblGiftCardCode";
            this.lblGiftCardCode.Size = new System.Drawing.Size(230, 17);
            this.lblGiftCardCode.TabIndex = 31;
            this.lblGiftCardCode.Text = "bLoyal Gift Card Tender Code:";
            // 
            // gbVirtualClient
            // 
            this.gbVirtualClient.Controls.Add(this.lblVirtualConnName);
            this.gbVirtualClient.Controls.Add(this.label2);
            this.gbVirtualClient.Controls.Add(this.btnTextClientApi);
            this.gbVirtualClient.Controls.Add(this.lblVirtualClientURL);
            this.gbVirtualClient.Controls.Add(this.txtVirtualClientURL);
            this.gbVirtualClient.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbVirtualClient.Location = new System.Drawing.Point(355, 28);
            this.gbVirtualClient.Name = "gbVirtualClient";
            this.gbVirtualClient.Size = new System.Drawing.Size(297, 170);
            this.gbVirtualClient.TabIndex = 47;
            this.gbVirtualClient.TabStop = false;
            this.gbVirtualClient.Text = "Dinerware Virtual Client API";
            // 
            // lblVirtualConnName
            // 
            this.lblVirtualConnName.AutoSize = true;
            this.lblVirtualConnName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVirtualConnName.Location = new System.Drawing.Point(154, 141);
            this.lblVirtualConnName.Name = "lblVirtualConnName";
            this.lblVirtualConnName.Size = new System.Drawing.Size(34, 17);
            this.lblVirtualConnName.TabIndex = 61;
            this.lblVirtualConnName.Text = "Fail";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(10, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 17);
            this.label2.TabIndex = 60;
            this.label2.Text = "Connection Status :";
            // 
            // btnTextClientApi
            // 
            this.btnTextClientApi.BackColor = System.Drawing.Color.Silver;
            this.btnTextClientApi.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTextClientApi.ForeColor = System.Drawing.Color.Black;
            this.btnTextClientApi.Location = new System.Drawing.Point(13, 93);
            this.btnTextClientApi.Name = "btnTextClientApi";
            this.btnTextClientApi.Size = new System.Drawing.Size(167, 29);
            this.btnTextClientApi.TabIndex = 52;
            this.btnTextClientApi.Text = "Test Virtual Client API";
            this.btnTextClientApi.UseVisualStyleBackColor = false;
            this.btnTextClientApi.Click += new System.EventHandler(this.btnTextClientApi_Click);
            // 
            // lblVirtualClientURL
            // 
            this.lblVirtualClientURL.AutoSize = true;
            this.lblVirtualClientURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVirtualClientURL.Location = new System.Drawing.Point(8, 25);
            this.lblVirtualClientURL.Name = "lblVirtualClientURL";
            this.lblVirtualClientURL.Size = new System.Drawing.Size(44, 17);
            this.lblVirtualClientURL.TabIndex = 1;
            this.lblVirtualClientURL.Text = "URL:";
            // 
            // txtVirtualClientURL
            // 
            this.txtVirtualClientURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVirtualClientURL.Location = new System.Drawing.Point(11, 50);
            this.txtVirtualClientURL.Name = "txtVirtualClientURL";
            this.txtVirtualClientURL.Size = new System.Drawing.Size(244, 23);
            this.txtVirtualClientURL.TabIndex = 2;
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
            this.groupBox1.Location = new System.Drawing.Point(355, 204);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(297, 304);
            this.groupBox1.TabIndex = 48;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Dinerware Gift Card Product Names";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(3, 146);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(206, 13);
            this.label12.TabIndex = 40;
            this.label12.Text = "Dinerware Gift Card Product Name:";
            // 
            // loadGiftCardItemTxt
            // 
            this.loadGiftCardItemTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadGiftCardItemTxt.Location = new System.Drawing.Point(6, 168);
            this.loadGiftCardItemTxt.Name = "loadGiftCardItemTxt";
            this.loadGiftCardItemTxt.Size = new System.Drawing.Size(249, 23);
            this.loadGiftCardItemTxt.TabIndex = 39;
            // 
            // removeLoadGiftCardItemBtn
            // 
            this.removeLoadGiftCardItemBtn.BackColor = System.Drawing.Color.Red;
            this.removeLoadGiftCardItemBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.removeLoadGiftCardItemBtn.ForeColor = System.Drawing.Color.Snow;
            this.removeLoadGiftCardItemBtn.Location = new System.Drawing.Point(151, 209);
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
            this.addLoadGiftCardItemBtn.Location = new System.Drawing.Point(44, 209);
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
            this.loadGiftCardListBox.Size = new System.Drawing.Size(249, 106);
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(841, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 17);
            this.label3.TabIndex = 61;
            this.label3.Text = "Version :";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoSize = true;
            this.lblVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.Location = new System.Drawing.Point(920, 8);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(58, 17);
            this.lblVersion.TabIndex = 62;
            this.lblVersion.Text = "..........";
            // 
            // frmConfiguration
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(255)))), ((int)(((byte)(223)))));
            this.ClientSize = new System.Drawing.Size(1006, 520);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbVirtualClient);
            this.Controls.Add(this.groupBox_bLoyal_Tender);
            this.Controls.Add(this.groupBoxBloyalCalculateDiscounts);
            this.Controls.Add(this.groupBoxBLoyalLoyaltySummary);
            this.Controls.Add(this.DisableEnablePanel);
            this.Controls.Add(this.groupBox_bLoyal_Cred);
            this.Controls.Add(this.gbSnippetURLs);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnAddConfiguration);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmConfiguration";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Master Settings";
            this.Load += new System.EventHandler(this.frmConfiguration_Load);
            this.gbSnippetURLs.ResumeLayout(false);
            this.gbSnippetURLs.PerformLayout();
            this.groupBox_bLoyal_Cred.ResumeLayout(false);
            this.groupBox_bLoyal_Cred.PerformLayout();
            this.DisableEnablePanel.ResumeLayout(false);
            this.DisableEnablePanel.PerformLayout();
            this.groupBoxBLoyalLoyaltySummary.ResumeLayout(false);
            this.groupBoxBLoyalLoyaltySummary.PerformLayout();
            this.groupBoxBloyalCalculateDiscounts.ResumeLayout(false);
            this.groupBoxBloyalCalculateDiscounts.PerformLayout();
            this.groupBox_bLoyal_Tender.ResumeLayout(false);
            this.groupBox_bLoyal_Tender.PerformLayout();
            this.gbVirtualClient.ResumeLayout(false);
            this.gbVirtualClient.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void txtLoginDomain_TextChanged(object sender, EventArgs e)
        {

        }

        #endregion

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
                await CreatebLoyalAccessKeyAsync(true).ConfigureAwait(true);
                if (serviceUrls != null && !string.IsNullOrWhiteSpace(_bLoyalAccessApiKey) && !string.IsNullOrWhiteSpace(txtLoginDomain.Text))
                {
                    ContextInfo contextInfo = await GetConnectorContextInfoAsync(serviceUrls).ConfigureAwait(true);
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
                        MessageBox.Show("API Key locked successfully.");
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
                if ((!string.IsNullOrEmpty(_conFigHelper.LOYALTY_TENDER_CODE)))
                    cmbLoyaltytenderCode.SelectedValue = _conFigHelper.LOYALTY_TENDER_CODE;
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
                if (!string.IsNullOrEmpty(_conFigHelper.GIFTCARD_TENDER_CODE))
                    cmbGiftCardTenderCode.SelectedValue = _conFigHelper.GIFTCARD_TENDER_CODE;
                else
                    cmbGiftCardTenderCode.SelectedValue = 0;

                cmbGiftCardTenderCode.DropDownStyle = ComboBoxStyle.DropDownList;
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
                }
                else
                {
                    //domainUrlErrorMsg.Visible = true;
                    //domainUrlErrorMsg.Text = "Please enter valid domain url";
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
            serviceUrls = AsyncHelper.RunSync(() => bLoyalService.GetServiceUrlsAsync(loginDomain, domainURL));
            return serviceUrls;
        }

        /// <summary>
        /// Get Connector ContextInfo
        /// </summary>
        /// <param name="serviceUrls"></param>
        /// <returns></returns>
        private async Task<ContextInfo> GetConnectorContextInfoAsync(ServiceUrls serviceUrls)
        {
            var gridService = new GridService(serviceUrls.GridApiUrl, _bLoyalAccessApiKey);
            ContextInfo contextInfo = null;

            contextInfo = await gridService.GetConnectorContextInfoAsync().ConfigureAwait(true); //Get the info about our current key so we can validate it.

            return contextInfo;
        }

        private async Task<string> CreatebLoyalAccessKeyAsync(bool isCreateBtn = false)
        {
            try
            {
                //bLoyalApiErrorMsg = string.Empty;
                //domainUrlErrorMsg.Text = string.Empty;
                string loginDomain = txtLoginDomain.Text;
                string domainUrl = domainUrltxt.Text;
                string apiKey = txtApiKey.Text;
                lblAccessKeyError.Text = string.Empty;
                string accessKey = string.Empty;
                accessKey = await GetAccessKeyAsync(loginDomain, domainUrl, apiKey).ConfigureAwait(true);
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
                    LoggerHelper.Instance.WriteLogError(ex, "*** Error in GetAccessKey for Configuration = " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.Instance.WriteLogError(ex, "*** Error in GetAccessKey for Configuration = " + ex.Message);
            }
            return string.Empty;
        }

        private async Task<string> GetAccessKeyAsync(string loginDomain, string domainUrltxt, string apiKey)
        {
            //bLoyalApiErrorMsg = string.Empty;
            TextBox.CheckForIllegalCrossThreadCalls = false;
            try
            {
                ConfigurationHelper conFigHelper = new ConfigurationHelper(true);
                _serviceUrls = await GetServiceURLAsync(loginDomain, domainUrltxt).ConfigureAwait(true);
                if (_serviceUrls != null)
                {
                    var dispenser = new KeyDispenser(loginDomain, _serviceUrls.GridApiUrl);

                    return await dispenser.GetAccessKeyAsync(Constants.CONNECTORKEY, apiKey).ConfigureAwait(true);
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

        public async Task<bLoyal.Connectors.ServiceUrls> GetServiceURLAsync(string loginDomain, string domainUrltxt)
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
                        var configuration = new ConfigurationHelper(true);
                        sourceFilePath = configuration.GetFilePath();
                        xDoc.Load(@"" + sourceFilePath);
                    }
                    catch
                    {

                    }
                    _serviceUrls = await bLoyal.Connectors.bLoyalService.GetServiceUrlsAsync(loginDomain, !string.IsNullOrEmpty(domainUrltxt) ? domainUrltxt : "https://domain.bloyal.com").ConfigureAwait(true);
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
                    ContextInfo contextInfo = AsyncHelper.RunSync(() => GetConnectorContextInfoAsync(serviceUrls));

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

        private void btnTextClientApi_Click(object sender, EventArgs e)
        {
            try
            {
                DinerwareEngineService.VirtualClientClient virtualDinerwareClient;
                var endPointAddress = new System.ServiceModel.EndpointAddress(txtVirtualClientURL.Text);
                var binding = new System.ServiceModel.BasicHttpBinding();
                binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.None;
                virtualDinerwareClient = new DinerwareEngineService.VirtualClientClient(binding, endPointAddress);
                var allCustomer = virtualDinerwareClient.GetAllCustomers(10000, 10000, "");
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
    }
}
