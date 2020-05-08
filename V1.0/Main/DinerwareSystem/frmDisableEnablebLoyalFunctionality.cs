using DinerwareSystem.Helpers;
using System;
using System.Windows.Forms;
using System.Xml;

namespace DinerwareSystem
{
    public partial class DisableEnablebLoyalFunctionality : Form
    {
        LoggerHelper _logger = LoggerHelper.Instance;

        public DisableEnablebLoyalFunctionality()
        {
            InitializeComponent();
        }

        private void DisableEnablebLoyalFunctionality_Load(object sender, EventArgs e)
        {
            // Close snippets automatically
            Timer frmCloseTimer = new Timer();
            frmCloseTimer.Interval = 300000;
            frmCloseTimer.Start();
            frmCloseTimer.Tick += new EventHandler(CloseWindow);
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            this.Close();
        }

        private void nobtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void yesBtn_Click(object sender, EventArgs e)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                //string sourceFilePath = string.Format("{0}{1}{2}{3}{4}{5}{6}", Environment.Is64BitOperatingSystem ? Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) : Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), System.IO.Path.DirectorySeparatorChar, "bLoyal", System.IO.Path.DirectorySeparatorChar, "Dinerware Integration", System.IO.Path.DirectorySeparatorChar, "DinerwareConfigurationFile.xml");
                //if (!System.IO.File.Exists(sourceFilePath))
                //    sourceFilePath = string.Format("{0}{1}{2}{3}{4}{5}{6}", Environment.Is64BitOperatingSystem ? Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) : Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), System.IO.Path.DirectorySeparatorChar, "bLoyal", System.IO.Path.DirectorySeparatorChar, "Workstation", System.IO.Path.DirectorySeparatorChar, "DinerwareConfigurationFile.xml");
                var configuration = new ConfigurationHelper(true);
                string sourceFilePath = configuration.GetFilePath();

                xDoc.Load(@"" + sourceFilePath);
                xDoc.DocumentElement.SelectSingleNode("EnablebLoyal").InnerText = "true";
                var permissionSet = new System.Security.PermissionSet(System.Security.Permissions.PermissionState.None);
                var writePermission = new System.Security.Permissions.FileIOPermission(System.Security.Permissions.FileIOPermissionAccess.Write, @"" + sourceFilePath);
                permissionSet.AddPermission(writePermission);
                if (permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet))
                    xDoc.Save(@"" + sourceFilePath);
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "yesBtn_Click");
                this.Close();
            }
            var config = ConfigurationHelper.NewInstance; // Update the Configuration Instance
            this.Close();
        }
    }
}
