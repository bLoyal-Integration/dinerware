using Newtonsoft.Json;
using System;
using System.IO;
using System.Reflection;
using System.Xml;

namespace DataSyncService.Helper
{
    public class ConfigurationHelper
    {
        #region Members

        public string SNIPPET_URL { get; set; }
        public string LOGIN_DOMAIN { get; set; }
        public string ACCESS_KEY { get; set; }
        public string API_KEY { get; set; }
        public string CONNECTOR_KEY { get; set; }

        public string DATABASE_CONNECTION_STR { get; set; }
        public string URL_VIRTUALCLIENT { get; set; }

        public string DATASOURCE { get; set; }
        public string DATABASENAME { get; set; }
        public string USERID { get; set; }
        public string PASSWORD { get; set; }

        public string TENDERCODE { get; set; }
        public string GIFTCARDTENDERCODE { get; set; }
        public string SCHEDULERTIME { get; set; }
        public string ENABLE_bLOYAL { get; set; }
        public string DOMAIN_URL { get; set; }

        public string DW_GIFTCARTTENDERNAME { get; set; }
        public string DW_LOYALTYTENDERNAME { get; set; }
        public string DW_ORDER_LEVEL_DISCOUNT_TYPE_NAME { get; set; }
        public string DW_ITEM_LEVEL_DISCOUNT_TYPE_NAME { get; set; }
        public string DW_SALEPRICE_LEVEL_DISCOUNT_TYPE_NAME { get; set; }

        private string _connectorKey = "FCE78496-863A-4A69-A017-526B5DADFCED";  // This will be hardcoded for any connector
        //CONNECTOR_KEY = "75939349-3f27-4631-b3e1-34a4438aa38b";

        public bool IS_Test_BLoyal_Connection { get; set; }
        public bool IS_Test_Database_Connection { get; set; }
        public bool IS_Test_Virtual_Client_Connection { get; set; }

        #endregion

        #region Constructor

        public ConfigurationHelper()
        {
            try
            {
                SetupResolver();
                XmlDocument xDoc = new XmlDocument();
                string sourceFilePath = GetFilePath();

                xDoc.Load(sourceFilePath);

                SNIPPET_URL = !string.IsNullOrEmpty(xDoc.DocumentElement.SelectSingleNode("SNIPPET_URL").InnerText) ? xDoc.DocumentElement.SelectSingleNode("SNIPPET_URL").InnerText : "https://possnippets.bloyal.com";

                LOGIN_DOMAIN = xDoc.DocumentElement.SelectSingleNode("LOGIN_DOMAIN").InnerText;

                ACCESS_KEY = CryptoGraphy.DecryptCipherTextToPlainText(xDoc.DocumentElement.SelectSingleNode("ACCESS_KEY").InnerText);

                API_KEY = xDoc.DocumentElement.SelectSingleNode("API_KEY").InnerText;

                CONNECTOR_KEY = _connectorKey;

                DATABASE_CONNECTION_STR = CryptoGraphy.DecryptCipherTextToPlainText(xDoc.DocumentElement.SelectSingleNode("DATABASE_CONNECTION_STR").InnerText);

                URL_VIRTUALCLIENT = xDoc.DocumentElement.SelectSingleNode("URL_VIRTUALCLIENT").InnerText;

                DATASOURCE = xDoc.DocumentElement.SelectSingleNode("DATASOURCE").InnerText;

                DATABASENAME = xDoc.DocumentElement.SelectSingleNode("DATABASENAME").InnerText;

                USERID = xDoc.DocumentElement.SelectSingleNode("USERID").InnerText;

                PASSWORD = CryptoGraphy.DecryptCipherTextToPlainText(xDoc.DocumentElement.SelectSingleNode("PASSWORD").InnerText);

                TENDERCODE = xDoc.DocumentElement.SelectSingleNode("LoyaltyTenderCode").InnerText;

                GIFTCARDTENDERCODE = xDoc.DocumentElement.SelectSingleNode("GiftCardTenderCode").InnerText;

                ENABLE_bLOYAL = xDoc.DocumentElement.SelectSingleNode("EnablebLoyal").InnerText;

                DOMAIN_URL = !string.IsNullOrEmpty(xDoc.DocumentElement.SelectSingleNode("DomainUrl").InnerText) ? xDoc.DocumentElement.SelectSingleNode("DomainUrl").InnerText : "https://domain.bloyal.com";

                DW_GIFTCARTTENDERNAME = xDoc.DocumentElement.SelectSingleNode("DinerwareGiftCardTenderName").InnerText;

                DW_LOYALTYTENDERNAME = xDoc.DocumentElement.SelectSingleNode("DinerwareLoyaltyTenderName").InnerText;

                DW_ORDER_LEVEL_DISCOUNT_TYPE_NAME = xDoc.DocumentElement.SelectSingleNode("DinerwareOrderLevelDiscountName").InnerText;

                DW_ITEM_LEVEL_DISCOUNT_TYPE_NAME = xDoc.DocumentElement.SelectSingleNode("DinerwareItemLevelDiscountName").InnerText;

                DW_SALEPRICE_LEVEL_DISCOUNT_TYPE_NAME = xDoc.DocumentElement.SelectSingleNode("DinerwareSalesPriceLevelDiscountName").InnerText;

                IS_Test_BLoyal_Connection = xDoc.DocumentElement.SelectSingleNode("ISTestBLoyalConnection").InnerText == "true" ? true : false;
                IS_Test_Database_Connection = xDoc.DocumentElement.SelectSingleNode("ISTestDatabaseConnection").InnerText == "true" ? true : false;
                IS_Test_Virtual_Client_Connection = xDoc.DocumentElement.SelectSingleNode("ISTestVirtualClientConnection").InnerText == "true" ? true : false;

            }
            catch
            {
                DOMAIN_URL = "https://domain.bloyal.com";
                CONNECTOR_KEY = "FCE78496-863A-4A69-A017-526B5dADFCED";
                // We can not log in this area...
            }
        }

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
    }
}
