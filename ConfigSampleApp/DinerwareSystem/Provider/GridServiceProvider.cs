using bLoyal.Connectors;
using bLoyal.Connectors.Grid;
using ConfigApp.Helpers;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace ConfigApp.Provider
{
    public class GridServiceProvider
    {

        GridService _gridService;
        ConfigurationHelper _conFigHelper = new ConfigurationHelper();
        ServiceUrls _serviceUrls = null;
        Logger _logger = new Logger();

        #region API Exception Properties

        public APIExceptions apiException { get; set; }

        #endregion

        public GridServiceProvider()
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            SetupResolver();
            GetServiceURL(_conFigHelper.LOGIN_DOMAIN);
            if (_serviceUrls != null)
                _gridService = new bLoyal.Connectors.Grid.GridService(_serviceUrls.GridApiUrl, _conFigHelper.ACCESS_KEY);
        }


        public ServiceUrls GetServiceURL(string loginDomain)
        {
            try
            {
                if (_serviceUrls == null)
                {
                    var getServiceTask = Task.Run(async () => { _serviceUrls = await bLoyal.Connectors.bLoyalService.GetServiceUrlsAsync(_conFigHelper.LOGIN_DOMAIN, _conFigHelper.DOMAIN_URL); });
                    getServiceTask.Wait();
                }
                return _serviceUrls;
            }
            catch (Exception ex)
            {
            }
            return null;
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

        public async Task SubmitLogsAsync()
        {
            try
            {
                await _logger.SubmitLogsAsync(_serviceUrls.LoggingApiUrl, _conFigHelper.ACCESS_KEY);
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "SubmitLogsAsync" };

            }
            catch (Exception ex)
            {
                //gridService.Logger.LogError("*** Logger.SubmitLogsAsync Exception in gridService = " + ex.Message);
            }
        }


        public void LogInfo(string logMessage)
        {
            try
            {
                _logger.LogInfo(logMessage);
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "Logger.LogInfo" };

            }
            catch (Exception ex)
            {
                //gridService.Logger.LogError("*** Logger.LogInfo Exception in gridService = " + ex.Message);
            }
        }

        public void LogError(string logMessage)
        {
            try
            {
                _logger.LogError(logMessage);
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "Logger.LogError" };

            }
            catch (Exception ex)
            {
                //gridService.Logger.LogError("*** Logger.LogError Exception in gridService = " + ex.Message);
            }
        }

        public async Task<Customer> GetAsync(Guid customerUid)
        {
            try
            {
                return await _gridService.CustomerResource.GetAsync(customerUid);
            }
            catch (ApiException ex)
            {
                apiException = new APIExceptions { ErrorCode = ex.Code, ErrorDescription = ex.Message, ErrorApi = "GetAsync" };
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError("*** GetAsync Exception in gridService = " + ex.Message);
                return null;
            }
        }

    }
}
