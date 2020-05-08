using bLoyal.Connectors;
using bLoyal.Connectors.Grid;
using ConfigApp.Helpers;
using ConfigApp.Provider;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Timers;

namespace ConfigApp
{
    public class LoggerHelper
    {

        ConfigurationHelper _config = new ConfigurationHelper();
        private static bool _isBackgroundWorker = false;
        ServiceUrls _serviceUrls = null;
        private BackgroundWorker _worker;
        Logger _logger = new Logger();

        /// <summary>
        /// Write the Logger for LogInfo 
        /// </summary>
        /// <param name="logMessage"></param>
        public void WriteLogInfo(string logMessage)
        {
            try
            {
                ConfigurationHelper configObj = new ConfigurationHelper();
                if (configObj.ENABLE_bLOYAL == "true")
                    _logger.LogInfo(logMessage);

                if (!_isBackgroundWorker)
                {
                    _isBackgroundWorker = true;
                    BackgroundLogWorker();
                }
            }
            catch
            {
            }
        }


        /// <summary>
        /// Write the Logger for LogError 
        /// </summary>
        /// <param name="logMessage"></param>
        public void WriteLogError(string logMessage)
        {
            try
            {
                ConfigurationHelper configObj = new ConfigurationHelper();
                if (configObj.ENABLE_bLOYAL == "true")
                    _logger.LogError(logMessage);

            }
            catch
            {
            }
        }

        /// <summary>
        ///  Start background service for Logging
        /// </summary>
        public void BackgroundLogWorker()
        {
            try
            {
                _worker = new BackgroundWorker();
                _worker.DoWork += worker_DoWork;
                Timer timer = new Timer(300000);
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
                if (!_worker.IsBusy)
                    _worker.RunWorkerAsync();
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
                ConfigurationHelper configObj = new ConfigurationHelper();
                if (configObj.ENABLE_bLOYAL == "true")
                {
                    if (_serviceUrls == null)
                    {
                        var getServiceTask = Task.Run(async () => { _serviceUrls = await bLoyalService.GetServiceUrlsAsync(_config.LOGIN_DOMAIN, _config.DOMAIN_URL); });
                        getServiceTask.Wait();
                    }

                    var task = Task.Run(async () => { await _logger.SubmitLogsAsync(_serviceUrls.LoggingApiUrl, _config.ACCESS_KEY); });
                    task.Wait();
                }
            }
            catch 
            {

            }
        }
    }
}
