using bLoyal.Connectors;
using bLoyal.Utilities;
using DinerwareSystem.Helpers;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Timers;

namespace DinerwareSystem
{
    public class LoggerHelper
    {
        private static bool _isBackgroundWorker = false;

        ConfigurationHelper _configHelper = ConfigurationHelper.Instance;           
        private BackgroundWorker _worker;
        Logger _logger = new Logger();

        private static LoggerHelper _instance;

        public static LoggerHelper Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new LoggerHelper();

                return _instance;
            }
        }        


        /// <summary>
        /// Write the Logger for LogError 
        /// </summary>
        /// <param name="logMessage"></param>
        public void WriteLogError(Exception ex, string method)
        {
            try
            {
                if (_configHelper.ENABLE_bLOYAL)
                {                  
                    _logger.LogErrorEx(ex, method);

                    if (!_isBackgroundWorker)
                    {
                        _isBackgroundWorker = true;
                        BackgroundLogWorker();
                    }
                }
            }
            catch 
            {
                // We are not able to access bLoyal services  - We can not log in this area... 
            }
        }

        /// <summary>
        /// Write the Logger for LogInfo 
        /// </summary>
        /// <param name="logMessage"></param>
        public void WriteLogInfo(string logMessage)
        {
            try
            {
                if (_configHelper.ENABLE_bLOYAL)
                    _logger.LogInfo(logMessage);

                if (!_isBackgroundWorker)
                {
                    _isBackgroundWorker = true;
                    BackgroundLogWorker();
                }
            }
            catch
            {
                // We are not able to access bLoyal services  - We can not log in this area... 
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
                // We are not able to access bLoyal services  - We can not log in this area... 
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
                // We are not able to access bLoyal services  - We can not log in this area... 
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
                if (_configHelper.ENABLE_bLOYAL)
                    AsyncHelper.RunSync(() => _logger.SubmitLogsAsync(ServiceURLHelper.Service_Urls.LoggingApiUrl, _configHelper.ACCESS_KEY));
            }
            catch
            {
                // We are not able to access bLoyal services  - We can not log in this area... 
            }
        }
    }
}
