using bLoyal.Connectors;
using bLoyal.Utilities;
using DataSyncService.Helper;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;

namespace DataSyncService
{
    public partial class Scheduler : ServiceBase
    {
        private Timer _schedulerTimer = null;
        ConfigurationHelper _configurationHelper = new ConfigurationHelper();
        private int _schedulerTimerInterval = 60000;
        ServiceUrls _serviceUrls = null;
        Logger _logger = new Logger();
        LoggingHelper _log = new LoggingHelper();
        bool _isServiceRunning = false;

        /// <summary>
        /// Scheduler
        /// </summary>
        public Scheduler()
        {
            InitializeComponent();
        }

        //-----------------------------------------------------------------------------------------
        // Open a new integration batch to retrieve replication profile information.
        //-----------------------------------------------------------------------------------------
        private async Task<bool> ConnectorCheckinAsync()
        {
#if DEBUG
            return true;
#endif
            try
            {
                bLoyal.Connectors.Grid.GridService _gridService = null;
                _configurationHelper = new ConfigurationHelper();

                if (!_configurationHelper.IS_Test_BLoyal_Connection || !_configurationHelper.IS_Test_Database_Connection)
                    return false;

                var _serviceUrls = await bLoyalService.GetServiceUrlsAsync(_configurationHelper.LOGIN_DOMAIN, _configurationHelper.DOMAIN_URL).ConfigureAwait(true);

                if (_serviceUrls == null)
                {
                    _log.LogInfo("--------------------------------------------------------------------------------------------------------------");
                    _log.LogInfo(string.Format(Constants.UNABLETOGETSERVICEWARNING, _configurationHelper.LOGIN_DOMAIN, _configurationHelper.DOMAIN_URL));
                    _log.LogInfo("--------------------------------------------------------------------------------------------------------------");
                    return false;
                }

                if (_serviceUrls != null)
                    _gridService = new bLoyal.Connectors.Grid.GridService(_serviceUrls.GridApiUrl, _configurationHelper.ACCESS_KEY);

                var checkin = await _gridService.ConnectorCheckinAsync().ConfigureAwait(true);

                if (checkin == null)
                {
                    _log.LogInfo("--------------------------------------------------------------------------------------------------------------");
                    _log.LogInfo("ConnectorCheckinAsync is returning null");
                    _log.LogInfo("--------------------------------------------------------------------------------------------------------------");

                    return false;
                }

                if (checkin.SecondsToDelay > 0)
                {
                    _schedulerTimerInterval = Convert.ToInt32(TimeSpan.FromSeconds(checkin.SecondsToDelay).TotalMilliseconds);

                    _log.LogInfo("--------------------------------------------------------------------------------------------------------------");
                    _log.LogInfo($"ConnectorCheckinAsync SecondsToDelay = {checkin.SecondsToDelay}");
                    _log.LogInfo($"Next Scheduler Timer Interval = {_schedulerTimerInterval}");
                    _log.LogInfo("--------------------------------------------------------------------------------------------------------------");

                    _schedulerTimer = new Timer();
                    _schedulerTimer.Interval = _schedulerTimerInterval;
                }

                _log.LogInfo("--------------------------------------------------------------------------------------------------------------");
                _log.LogInfo($"Dinerware ConnectorCheckin = {checkin.SyncNow}");
                _log.LogInfo("--------------------------------------------------------------------------------------------------------------");

                return checkin.SyncNow;

            }
            catch (ApiException ex)
            {
                LogApiException(ex, "ConnectorCheckinAsync");
            }
            catch (Exception ex)
            {
                LogException(ex, "ConnectorCheckinAsync");
            }

            SetDefaultSchedulerTimer();

            return false;
        }


        /// <summary>
        /// Set Default Scheduler Timer
        /// </summary>
        private void SetDefaultSchedulerTimer()
        {
            try
            {
                _schedulerTimer = new Timer();
                _schedulerTimer.Interval = 60000;
            }
            catch (Exception ex)
            {
                LogException(ex, "SetDefaultSchedulerTimer");
            }
        }

        /// <summary>
        /// Log Exception
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="method"></param>
        private void LogException(Exception ex, string method)
        {
            try
            {
                _log.LogInfo("--------------------------------------------------------------------------------------------------------------");
                _log.LogException(ex, method);
                _log.LogInfo("--------------------------------------------------------------------------------------------------------------");

                _logger.LogErrorEx(ex, method);

                if (_serviceUrls != null)
                    AsyncHelper.RunSync(() => _logger.SubmitLogsAsync(_serviceUrls.LoggingApiUrl, _configurationHelper.ACCESS_KEY));

            }
            catch
            {
                // We are not able to access bLoyal services  - We can not log in this area... 
            }
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        /// <summary>
        /// Start window service
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            Task task = OnStartAsync();
            task.GetAwaiter().GetResult();
        }

        private async Task OnStartAsync()
        {
            try
            {
                _log.LogInfo("--------------------------------------------------------------------------------------------------------------");
                _log.LogInfo("Dinerware Data Sync Service - OnStart");
                _log.LogInfo("--------------------------------------------------------------------------------------------------------------");

                bool isConnectorCheckIn = false;
                isConnectorCheckIn = await ConnectorCheckinAsync().ConfigureAwait(true);

                // When service start - Then force to Backend Connector for Data syncing                     
                if (isConnectorCheckIn)
                    await RunBackendConnectorAsync().ConfigureAwait(true);

                _schedulerTimer = new Timer();
                _schedulerTimer.Interval = _schedulerTimerInterval;
                _schedulerTimer.Elapsed += new ElapsedEventHandler(SchedulerWindowService);
                _schedulerTimer.Enabled = true;
            }
            catch (ApiException ex)
            {
                LogApiException(ex, "OnStart");
            }
            catch (Exception ex)
            {
                LogException(ex, "OnStart");
            }
        }

        /// <summary>
        /// Scheduler window service
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SchedulerWindowService(object sender, ElapsedEventArgs e)
        {
            try
            {
                bool isConnectorCheckIn = false;

                if (_isServiceRunning)
                    return;

                isConnectorCheckIn = AsyncHelper.RunSync(() => ConnectorCheckinAsync());

                if (!isConnectorCheckIn)
                    return;

                _isServiceRunning = true;
                AsyncHelper.RunSync(() => RunBackendConnectorAsync());
                _isServiceRunning = false;

            }
            catch (ApiException ex)
            {
                _isServiceRunning = false;
                LogApiException(ex, "schedulerTimer_Tick");
            }
            catch (Exception ex)
            {
                _isServiceRunning = false;
                LogException(ex, "schedulerTimer_Tick");
            }
        }

        /// <summary>
        /// OnStop
        /// </summary>
        protected override void OnStop()
        {
            try
            {
                _schedulerTimer.Enabled = false;

                _log.LogInfo("--------------------------------------------------------------------------------------------------------------");
                _log.LogInfo("Dinerware Data Sync Service - OnStop");
                _log.LogInfo("--------------------------------------------------------------------------------------------------------------");
            }
            catch (Exception ex)
            {
                LogException(ex, "Dinerware Data Sync Service - OnStop");
            }
        }

        /// <summary>
        /// Data syncing
        /// </summary>
        /// <returns></returns>
        private async Task RunBackendConnectorAsync()
        {
            try
            {
                var dataSync = new DinerwareDataSync();
                await dataSync.DataSyncAsync().ConfigureAwait(true);
            }
            catch (Exception ex)
            {
                LogException(ex, "RunBackendConnectorAsync");
            }
        }

        /// <summary>
        /// Log Api Exception
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="method"></param>
        private void LogApiException(ApiException ex, string method)
        {
            try
            {
                _log.LogInfo("--------------------------------------------------------------------------------------------------------------");
                _log.LogException(ex, method);
                _log.LogInfo("--------------------------------------------------------------------------------------------------------------");

                _logger.LogErrorEx(ex, method);

                if (_serviceUrls != null)
                    AsyncHelper.RunSync(() => _logger.SubmitLogsAsync(_serviceUrls.LoggingApiUrl, _configurationHelper.ACCESS_KEY));
            }
            catch
            {
                // We are not able to access bLoyal services  - We can not log in this area... 
            }
        }

        /// <summary>
        /// Setup Resolver
        /// </summary>
        private static void SetupResolver()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainOnAssemblyResolve;
        }

        /// <summary>
        /// Current Domain On Assembly Resolve
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private static Assembly CurrentDomainOnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.StartsWith("Newtonsoft.Json"))
                return typeof(JsonSerializer).Assembly;
            return null;
        }
    }
}
