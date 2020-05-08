using DinerwareSystem.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml;

namespace DinerwareSystem.Helpers
{
    public class ConfigurationHelper : SnippetConfiguration
    {
        private string _connectorKey = Constants.CONNECTORKEY;

        private static ConfigurationHelper _instance;

        public static ConfigurationHelper Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ConfigurationHelper(true);
                else if (_instance != null && string.IsNullOrWhiteSpace(_instance.SNIPPET_URL))
                    _instance.SNIPPET_URL = SNIPPET_Config_URL;

                return _instance;
            }
        }

        public static ConfigurationHelper NewInstance
        {
            get
            {
                return _instance = new ConfigurationHelper(true);
            }
        }

        #region Constructor


        public ConfigurationHelper(bool isUpdate)
        {
            try
            {
                SetupResolver();
                if (isUpdate)
                {
                    XmlDocument xDoc = new XmlDocument();

                    //string sourceFilePath = string.Empty;

                    //IS_BRAIN = IsDinerwareBrain();

                    string sourceFilePath = GetFilePath();

                    xDoc.Load(@"" + sourceFilePath);

                    ENABLE_bLOYAL = Convert.ToBoolean(xDoc.DocumentElement.SelectSingleNode("EnablebLoyal").InnerText);// == "true";

                    SNIPPET_URL = !string.IsNullOrWhiteSpace(xDoc.DocumentElement.SelectSingleNode("SNIPPET_URL").InnerText) ? xDoc.DocumentElement.SelectSingleNode("SNIPPET_URL").InnerText : SNIPPET_Config_URL;

                    LOGIN_DOMAIN = xDoc.DocumentElement.SelectSingleNode("LOGIN_DOMAIN").InnerText;

                    ACCESS_KEY = !string.IsNullOrWhiteSpace(xDoc.DocumentElement.SelectSingleNode("ACCESS_KEY").InnerText) ? CryptoGraphyHelper.DecryptCipherTextToPlainText(xDoc.DocumentElement.SelectSingleNode("ACCESS_KEY").InnerText) : string.Empty;

                    API_KEY = xDoc.DocumentElement.SelectSingleNode("API_KEY").InnerText;

                    CONNECTOR_KEY = _connectorKey;

                    URL_VIRTUALCLIENT = xDoc.DocumentElement.SelectSingleNode("URL_VIRTUALCLIENT").InnerText;

                    LOYALTY_TENDER_CODE = xDoc.DocumentElement.SelectSingleNode("LoyaltyTenderCode").InnerText;

                    DOMAIN_URL = !string.IsNullOrWhiteSpace(xDoc.DocumentElement.SelectSingleNode("DomainUrl").InnerText) ? xDoc.DocumentElement.SelectSingleNode("DomainUrl").InnerText : "https://domain.bloyal.io";

                    CUSTOM_DOMAIN_URL = xDoc.DocumentElement.SelectSingleNode("DomainUrl").InnerText;

                    POS_SNIPPET_URL = xDoc.DocumentElement.SelectSingleNode("SNIPPET_URL").InnerText;

                    GIFTCARD_TENDER_CODE = xDoc.DocumentElement.SelectSingleNode("GiftCardTenderCode").InnerText;

                    GIFTCARD_SKU = xDoc.DocumentElement.SelectSingleNode("GiftCardSKU").InnerText;

                    IS_DISCOUNT_SUMMARY = Convert.ToBoolean(xDoc.DocumentElement.SelectSingleNode("ISDISCOUNTSUMMARY").InnerText);// == "true";

                    IS_CALCULATE_DISCOUNT_WARNING = Convert.ToBoolean(xDoc.DocumentElement.SelectSingleNode("ISCALCULATEDISCOUNT").InnerText);// == "true";

                    IS_Test_BLoyal_Connection = xDoc.DocumentElement.SelectSingleNode("ISTestBLoyalConnection").InnerText == "true" ? true : false;
                    IS_Test_Database_Connection = xDoc.DocumentElement.SelectSingleNode("ISTestDatabaseConnection").InnerText == "true" ? true : false;
                    IS_Test_Virtual_Client_Connection = xDoc.DocumentElement.SelectSingleNode("ISTestVirtualClientConnection").InnerText == "true" ? true : false;

                    DW_GIFTCARD_TENDER_NAME = xDoc.DocumentElement.SelectSingleNode("DinerwareGiftCardTenderName").InnerText;

                    DW_LOYALTY_TENDER_NAME = xDoc.DocumentElement.SelectSingleNode("DinerwareLoyaltyTenderName").InnerText;

                    DW_ORDER_LEVEL_DISCOUNT_TYPE_NAME = xDoc.DocumentElement.SelectSingleNode("DinerwareOrderLevelDiscountName").InnerText;

                    DW_ITEM_LEVEL_DISCOUNT_TYPE_NAME = xDoc.DocumentElement.SelectSingleNode("DinerwareItemLevelDiscountName").InnerText;

                    DW_SALEPRICE_LEVEL_DISCOUNT_TYPE_NAME = xDoc.DocumentElement.SelectSingleNode("DinerwareSalesPriceLevelDiscountName").InnerText;

                    LOAD_GIFTCARD_ITEMS = GetLoadGiftCradItems(GIFTCARD_SKU);

                    _instance = this;
                }

            }
            catch
            {
                ENABLE_bLOYAL = false;
                IS_CALCULATE_DISCOUNT_WARNING = false;
                IS_DISCOUNT_SUMMARY = false;
                //IS_BRAIN = false;
                IS_Test_BLoyal_Connection = false;
                IS_Test_Virtual_Client_Connection = false;
                _instance = this;

                // We are not able to access configuration - We can not log in this area...
            }
        }

        #endregion

        #region private methods

        private List<string> GetLoadGiftCradItems(string loadGiftCardItems)
        {
            var skuItems = new List<string>();
            try
            {
                if (!string.IsNullOrWhiteSpace(loadGiftCardItems))
                {
                    string[] items = loadGiftCardItems.Split(',');
                    if (items.Length > 0)
                        skuItems.AddRange(items);
                }
            }
            catch
            {
                // We are not able to access configuration - We can not log in this area...
            }
            return skuItems;
        }

        private string GetConfigurationFilePath(bool isBrain)
        {
            //string sourceFilePath;
            //string folderName = isBrain ? "Dinerware Integration" : "Workstation";
            //sourceFilePath = Environment.Is64BitOperatingSystem ? GetSourceFilePath(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), folderName, "DinerwareConfigurationFile.xml") : GetSourceFilePath(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), folderName, "DinerwareConfigurationFile.xml");
            return GetFilePath();
        }

        private bool IsDinerwareBrain()
        {
            bool isBrain = false;
            try
            {
                XmlDocument xBrainDoc = new XmlDocument();
                //string sourceFilePath = Environment.Is64BitOperatingSystem ? GetSourceFilePath(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Dinerware Integration", "DinerwareBrainMachine.xml") : GetSourceFilePath(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Dinerware Integration", "DinerwareBrainMachine.xml");
                string sourceFilePath = GetFilePath();
                xBrainDoc.Load(@"" + sourceFilePath);
                bool.TryParse(xBrainDoc.DocumentElement.SelectSingleNode("IsBrain").InnerText, out isBrain);
            }
            catch (Exception ex)
            {
                // We are not able to access configuration - We can not log in this area...
            }
            return isBrain;
        }

        private string GetSourceFilePath(string programFilesType, string rootFolderName, string fileName)
        {
            try
            {
                return string.Format("{0}{1}{2}{3}{4}{5}{6}", programFilesType, System.IO.Path.DirectorySeparatorChar, "bLoyal", System.IO.Path.DirectorySeparatorChar, rootFolderName, System.IO.Path.DirectorySeparatorChar, fileName);
            }
            catch
            {
                // We are not able to access configuration - We can not log in this area...
            }
            return string.Empty;
        }

        private static void SetupResolver()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;
        }

        private static Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("Newtonsoft.Json"))
                return typeof(JsonSerializer).Assembly;
            return null;
        }

        #endregion

        #region Members

        public string SNIPPET_URL { get; set; }

        public string LOGIN_DOMAIN { get; set; }

        public string ACCESS_KEY { get; set; }

        public string API_KEY { get; set; }

        public string CONNECTOR_KEY { get; set; }

        public string URL_VIRTUALCLIENT { get; set; }

        public string LOYALTY_TENDER_CODE { get; set; }

        public string GIFTCARD_TENDER_CODE { get; set; }

        public string GIFTCARD_SKU { get; set; }

        public string POS_SNIPPET_URL { get; set; }

        public bool ENABLE_bLOYAL { get; set; }

        public bool IS_BRAIN { get; set; }

        public string DOMAIN_URL { get; set; }

        public string CUSTOM_DOMAIN_URL { get; set; }

        public bool IS_DISCOUNT_SUMMARY { get; set; }

        public bool IS_CALCULATE_DISCOUNT_WARNING { get; set; }

        public string DW_GIFTCARD_TENDER_NAME { get; set; }

        public string DW_LOYALTY_TENDER_NAME { get; set; }

        public string DW_ORDER_LEVEL_DISCOUNT_TYPE_NAME { get; set; }

        public string DW_ITEM_LEVEL_DISCOUNT_TYPE_NAME { get; set; }

        public string DW_SALEPRICE_LEVEL_DISCOUNT_TYPE_NAME { get; set; }

        public List<string> LOAD_GIFTCARD_ITEMS { get; set; }

        public bool IS_Test_BLoyal_Connection { get; set; }
        public bool IS_Test_Database_Connection { get; set; }
        public bool IS_Test_Virtual_Client_Connection { get; set; }

        #endregion

        #region Public Methods

        public string GetFilePath()
        {
            string sourceFilePath = Path.Combine(Environment.GetFolderPath(
                      Environment.SpecialFolder.CommonApplicationData),
                      @"bLoyal\Settings"
                    );

            if (!Directory.Exists(sourceFilePath))
            {
                Directory.CreateDirectory(sourceFilePath);
            }

            return Path.Combine(Environment.GetFolderPath(
                   Environment.SpecialFolder.CommonApplicationData),
                   @"bLoyal\Settings\ConnectorSettings-V-37.xml"
                   );
        }

        #endregion

    }

}
