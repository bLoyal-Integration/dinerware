using System;
using System.IO;

namespace DataSyncService.Helper
{
    public class LoggingHelper
    {
        /// <summary>
        /// Log Exception local environment
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="method"></param>
        public void LogException(Exception ex, string method)
        {
            try
            {
                string sourceFilePath = Path.Combine(Environment.GetFolderPath(
               Environment.SpecialFolder.CommonApplicationData),
               @"bLoyal\DinerwareLogging.txt"
               );

                using (StreamWriter writer = new StreamWriter(sourceFilePath, true))
                {
                    writer.WriteLine($"Error here in {method} : ex.Message = {ex.Message}");
                    string errorMsg = ex.InnerException != null ? ex.InnerException.Message : string.Empty;
                    writer.WriteLine($"Error here in {method} : ex.InnerException = {errorMsg}");
                    writer.WriteLine($"Error here in {method} : ex.StackTrace = {ex.StackTrace}");
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Log Info local environment
        /// </summary>
        /// <param name="message"></param>
        public void LogInfo(string message)
        {
            try
            {
                string sourceFilePath = Path.Combine(Environment.GetFolderPath(
               Environment.SpecialFolder.CommonApplicationData),
               @"bLoyal\DinerwareLogging.txt"
               );

                using (StreamWriter writer = new StreamWriter(sourceFilePath, true))
                {
                    writer.WriteLine(message);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Clear local environment Logs
        /// </summary>
        public void ClearLogs()
        {
            try
            {
                string sourceFilePath = Path.Combine(Environment.GetFolderPath(
               Environment.SpecialFolder.CommonApplicationData),
               @"bLoyal\DinerwareLogging.txt"
               );

                if (File.Exists(sourceFilePath))
                {
                    File.Delete(sourceFilePath);
                }
            }
            catch
            {
            }
        }
    }
}
