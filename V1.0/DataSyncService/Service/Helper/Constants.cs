namespace DataSyncService.Helper
{
    #region Constants

    /// <summary>
    /// Container class for all constant values for Dinerware 
    /// </summary>
    public sealed class Constants
    {
        public const string API_COMPANY_NAME = "Winedemo";      
        public const string API_REST_URL = "http://api-staging.bloyal.com/";
        public const string GET_SNIPPET_CODE = "API";
        public const string USER_AGENT = "Rest Request XML";
        public const string METHOD_GET = "GET";
        public const string REQUEST_CONTENT_TYPE = "text/xml; charset=UTF-8";
        public const string REQUEST_ACCESPT_XML = "text/xml";
        public const string REQUEST_ACCESPT_JSON = "text/json";
        public const string CONTENTTYPE_XML = "application/xml";
        public const string CONTENTTYPE_JSON = "application/json";
        public const string API_USER_NAME = "ws";
        public const string API_PASSWORD = "aloha@123";
        public const string POST = "POST";
        public const string PUT = "PUT";
        public const string CATALOG_SECTIONS = "CatalogSections";
        public const string SUMMARY_IMAGE = "SummaryImage";
        public const string DETAIL_IMAGE = "DetailImage";
        public const string INVENTORY = "Inventory";
        public const string PRODUCTS = "Products";
        public const string LIST_ENTRY = "ListEntry";


        public const string TRANSTOKEN = "&TransToken=";
        public const string BLANK_SPACE = " ";
        public const string COMMA_BLANK_SPACE = ", ";
        public const string SAPERATOR = "/";
        public const string PRODUCTSYNCMSG = "Sync process successfully.";
        public const string PRODUCTSYNCINPROMSG = "Syncing is in-progress...";

        public const string CUSTOMERSYNCINPROMSG = "Customer syncing is in-progress...";
        public const string CUSTOMERSYNCMSG = "Customer sync successfully.";

        public const string DISCOUNTSYNCINPROMSG = "Discount syncing is in-progress...";
        public const string DISCOUNTSYNCMSG = "Discount sync successfully.";
        public const string DISCOUNTAPPLYMSG = "Successfully Apply Discount.";

        public const string UPDATEPROGRESSBAR = "Update ProgressBar.";

        public const string TICKET = "Ticket";
        public const string ITEMS = "Items";

        public const string ORDERLEVELDISCOUNT = "bLoyal Order-level Discount";
        public const string ITEMLEVELDISCOUNT = "bLoyal Item-level Discount";
        public const string BLOYALLOYALTY = "bLoyal Loyalty";

        //public const string BLOYALLOYALTYTENDER = "bLoyal LoyaltyTender";
        public const string BLOYALLOYALTYTENDER = "bLoyal Loyalty";

        public const string URL_QUICKSIGNUP = "https://snippets1-test.bloyal.com/POS/quicksignup?DeviceUID=";
        public const string URL_FINDCUSTOMER = "https://snippets1-test.bloyal.com/POS/FindCustomer?DeviceUID=";
        public const string URL_VIEWCUSTOMER = "https://snippets1-test.bloyal.com/POS/viewcustomer?DeviceUID=";
        public const string URL_APPLYCOUPON = "https://snippets1-test.bloyal.com/POS/ApplyCoupon?DeviceUID=";
        public const string URL_CREATEORDER = "https://snippets1-test.bloyal.com/POS/createorder?DeviceUID=";

        public const string URL_VIRTUALCLIENT = "http://192.168.8.194:84/VirtualClient";
        public const string URL_LOYALTYENGINE = "https://ws1.bLoyal.com/v3.5/LoyaltyEngine.svc";
        public const string URL_ORDERENGINE = "https://ws1.bLoyal.com/v3.5/OrderEngine.svc";

        public const string CREDENTIAL_DOMAIN = "evttest";
        public const string CREDENTIAL_USERNAME = "ws";
        public const string CREDENTIAL_PASSWORD = "easy1234";
        public const string CREDENTIAL_DEVICEKEY = "PCBMGVFYSY-PGUWIXGINU-POXIELVUNI";
        public const string CREDENTIAL_APPID = "C0AACE45-65A2-41EE-8C87-F501BDB19259";

        public const string DATABASE_CONNECTION_STR = "Data Source=836-1073;Initial Catalog=dinerware;User ID=sa;Password=aloha@123";


        public const string FINDCUSTOMER = "/POS/FindCustomer?SessionKey=";
        public const string QUICKSIGNUP = "/POS/quicksignup?SessionKey=";
        public const string QUICKEDIT = "/POS/quickedit?SessionKey=";
        public const string APPLYCOUPON = "/POS/ApplyCoupon?SessionKey=";
        public const string VIEWCUSTOMER = "/POS/viewcustomer?SessionKey=";
        public const string CREATEORDER = "/POS/createorder?SessionKey=";

        public const string LOGINDOMAIN = "&LoginDomain=";
        public const string CARTEXTERNALID = "&CartExternalId=";
        public const string CARTUID = "&CartUID=";
        public const string CARTSOURCEEXTERNALID = "&CartSourceExternalId=";

        public const string CONNECTORSERVERURL = "ConnectorServerUrl";

        public const string PROCESSING_HEADMSG = "Processing Dinerware Data Sync Service for Client: {0}";
        public const string REPORT_TITLE = "Dinerware Data Sync";
        public const string UNABLE_TO_GET_SERVICEURLS = "Unable to get service URLs from domain service. LoginDomain {0} - ACCESS_KEY {1}";
        public const string UNABLETOGETSERVICEWARNING = "Unable to get service URLs from domain service. LoginDomain {0} - Domain Service Url {1}";
        public const string GRID_SERVER_NOT_CONNECTING = "Problem connecting the server using LoginDomain {0} - AccessKey{1}";
        public const string BATCHTITLE = "bLoyal Dinerware Integration for Data Import/Export";
        public const string VERSION = "1.1.374.2";

        public const string NO_ENTITY_DEFINED = "No entity sync profiles defined.";

        public const string TOTALDEPARTMENTS = "TotalDepartments";
        public const string LASTSYNCPRODUCTS = "LastSyncProducts";
        public const string LASTSYNCCUSTOMERS = "LastSyncCustomers";
        public const string LASTSYNCSALESTRANSACTION = "LastSyncSalesTransaction";

        public const string DATASYNCCLOSETIME = "Dinerware Data Sync Close Integration";
        public const string MIGRATIONDOESNOTSUPPORT = "Migration {0} does not support for Dinerware Integration.";

        public const string DEPARTMENT = "Department";
        public const string PRODUCT = "Product";
        public const string SALESTRANSACTION = "SalesTransaction";
        public const string CUSTOMER = "Customer"; 
    }

    #endregion
}
