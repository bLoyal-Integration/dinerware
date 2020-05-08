using bLoyal.Utilities;
using System.Threading.Tasks;

namespace DinerwareSystem.Helpers
{
    public static class ServiceURLHelper
    {       

        private static ConfigurationHelper _configurationHelper = ConfigurationHelper.Instance;

        public static bLoyal.Connectors.ServiceUrls Service_Urls;

        public static bool IsbLoyalServiceUrlDown;

        public static bLoyal.Connectors.ServiceUrls GetServiceURL()
        {
            try
            {               
                if (Service_Urls == null)
                {
                    _configurationHelper = ConfigurationHelper.NewInstance;
                    Service_Urls = AsyncHelper.RunSync(() => bLoyal.Connectors.bLoyalService.GetServiceUrlsAsync(_configurationHelper.LOGIN_DOMAIN, _configurationHelper.DOMAIN_URL));

                    if (Service_Urls != null)
                        IsbLoyalServiceUrlDown = false;
                }

                return Service_Urls;
            }
            catch 
            {
                // We are not able to access bLoyal services  - We can not log in this area... 
            }
            return null;
        }

        public static bLoyal.Connectors.ServiceUrls UpdateServiceURL()
        {
            try
            {
                _configurationHelper =  ConfigurationHelper.NewInstance;
                Service_Urls = AsyncHelper.RunSync(() => bLoyal.Connectors.bLoyalService.GetServiceUrlsAsync(_configurationHelper.LOGIN_DOMAIN, _configurationHelper.DOMAIN_URL));

                if (Service_Urls != null)
                    IsbLoyalServiceUrlDown = false;

                return Service_Urls;
            }
            catch
            {
                // We are not able to access bLoyal services  - We can not log in this area... 
            }
            return null;
        }      

    }
}
