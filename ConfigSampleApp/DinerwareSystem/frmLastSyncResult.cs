using System;
using System.IO;
using System.Windows.Forms;

namespace ConfigApp
{
    public partial class frmLastSyncResult : Form
    {
        private string _result;

        public frmLastSyncResult()
        {
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            InitializeComponent();
        }

        public frmLastSyncResult(string result)
        {
            _result = result;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            InitializeComponent();
        }
        private void frmLastSyncResult_Load(object sender, EventArgs e)
        {
            txtLastSyncResult.Text = _result;

            txtLastSyncResult.ReadOnly = true;

            // automatically get result
            Timer frmCloseTimer = new Timer();
            frmCloseTimer.Interval = 5000;
            frmCloseTimer.Start();
            frmCloseTimer.Tick += new EventHandler(GetResult);
        }

        private void btnLastSyncResult_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GetResult(object sender, EventArgs e)
        {
            try
            {
                string sourceFilePath = Path.Combine(Environment.GetFolderPath(
                  Environment.SpecialFolder.CommonApplicationData),
                  @"bLoyal\DinerwareLogging.txt"
               );

                txtLastSyncResult.ReadOnly = false;

                string result = File.ReadAllText(sourceFilePath);

                txtLastSyncResult.Text = result;

                txtLastSyncResult.ReadOnly = true;
            }
            catch (Exception)
            {

            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearLogs();

            LogInfo("---------------------------------------- Cleared Last Sync Result ------------------------------------------------");
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

        /// <summary>
        /// Log Info
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
    }
}
