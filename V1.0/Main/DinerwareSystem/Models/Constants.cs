namespace DinerwareSystem.Models
{

    public sealed class DinerwareUrls
    {
        public const string FINDCUSTOMER = "/POS/FindCustomer?SessionKey=";
        public const string QUICKSIGNUP = "/POS/quicksignup?SessionKey=";
        public const string QUICKEDIT = "/POS/quickedit?SessionKey=";
        public const string APPLYCOUPON = "/POS/ApplyCoupon?SessionKey=";
        public const string VIEWCUSTOMER = "/POS/viewcustomer?SessionKey=";
        public const string CREATEORDER = "/POS/createorder?SessionKey=";
        public const string ALERTS = "/POS/Alerts?SessionKey=";
    }

    public sealed class Messages
    {
        public const string APPLY_PAYMENT_WARNING = "bLoyal services are not available to tickets with a partial or full payment.";
        public const string ADD_LOAD_GIFTCARD_WARNING = "Please add load gift card product in ticket.";
        public const string ADD_IETM_WARNING = "Please add at least one item in order to calculate discounts.";
        public const string SERVICE_UNAVAILBLE_WARNING = "Service unavailable. Please try again later.";
        public const string GC_TENDER_NOT_CONFIGURED = "Dinerware Gift Card Tender Type Name Not Configured Properly.";
        public const string LOYALTY_TENDER_NOT_CONFIGURED = "Dinerware Loyalty Tender Type Name Not Configured Properly.";
        public const string DISCOUNT_APPLIED_MSG = "Discount applied successfully.";
        public const string CALCULATE_DISCOUNT_WARNING = "Please calculate bLoyal discounts before applying payments to ensure accurate loyalty benefits.";
        public const string TICKET_CHANGED_WARNING = "The ticket has changed and needs to be recalculated with bLoyal to ensure accurate loyalty benefits.";
    }

    public sealed class Constants
    {
        public const string LOGINDOMAIN = "&LoginDomain=";
        public const string CARTEXTERNALID = "&CartExternalId=";
        public const string CARTUID = "&CartUID=";
        public const string CARTSOURCEEXTERNALID = "&CartSourceExternalId=";

        public const string ORDERLEVELDISCOUNT = "bLoyal Order-level Discount";
        public const string ITEMLEVELDISCOUNT = "bLoyal Item-level Discount";
        public const string ITEMLEVELSALEPRICE = "bLoyal Item-level Sale Price";
        public const string BLOYALLOYALTYTENDER = "bLoyal Loyalty";
        public const string BLOYALGIFTCARDTENDER = "bLoyal Gift Card";
        public const string CALCULATEDNOTCURRENT = "CalculatedNotCurrent";
        public const string CALCULATEDCURRENT = "CalculatedCurrent";
        public const string NOTCALCULATED = "NotCalculated";

        public const string ADDINGUID = "0F5DCE55-D0BE-4ebb-A4D0-D24551D66173";
        public const string AUTHORPROPERTY = "This is the Author property.";

        public const string CONNECTORKEY = "FCE78496-863A-4A69-A017-526B5DADFCED";
    }
}
