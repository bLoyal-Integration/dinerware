using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace ConfigApp
{
    public partial class DisableEnablebLoyalFunctionality : Form
    {
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
            frmCloseTimer.Tick += new EventHandler(Ticks);
        }

        private void Ticks(object sender, EventArgs e)
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
                string sourceFilePath = string.Format("{0}{1}{2}{3}{4}{5}{6}", Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), System.IO.Path.DirectorySeparatorChar, "bLoyal", System.IO.Path.DirectorySeparatorChar, "Dinerware Integration", System.IO.Path.DirectorySeparatorChar, "DinerwareConfigurationFile.xml");
                xDoc.Load(@"" + sourceFilePath);
                xDoc.DocumentElement.SelectSingleNode("EnablebLoyal").InnerText = "true";
                var permissionSet = new System.Security.PermissionSet(System.Security.Permissions.PermissionState.None);
                string sourceFolderPath = string.Format("{0}{1}{2}{3}{4}", Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), System.IO.Path.DirectorySeparatorChar, "bLoyal", System.IO.Path.DirectorySeparatorChar, "Dinerware Integration");
                var writePermission = new System.Security.Permissions.FileIOPermission(System.Security.Permissions.FileIOPermissionAccess.Write, @"" + sourceFolderPath);
                permissionSet.AddPermission(writePermission);
                if (permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet))
                {
                    xDoc.Save(@"" + sourceFilePath);
                }
            }
            catch (Exception ex)
            {

            }
            this.Close();
        }
    }
}
