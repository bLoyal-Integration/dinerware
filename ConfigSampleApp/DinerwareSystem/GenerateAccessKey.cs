using bLoyal.Connectors;
using ConfigApp.Helpers;
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
    public partial class GenerateAccessKey : Form
    {
        public GenerateAccessKey()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            frmConfiguration frmConfiguration = new frmConfiguration();
            frmConfiguration.Show();
        }

        private void GenerateAccessKey_Load(object sender, EventArgs e)
        {
            ConfigurationHelper conFigHelper = new ConfigurationHelper();
            txtLoginDomain.Text = conFigHelper.LOGIN_DOMAIN;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnGenerateAccessKey_Click(object sender, EventArgs e)
        {
            frmConfiguration frmConfiguration = new frmConfiguration();
            XmlDocument xDoc = new XmlDocument();
            string sourceFilePath = string.Format("{0}{1}{2}{3}{4}{5}{6}", Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), System.IO.Path.DirectorySeparatorChar, "bLoyal", System.IO.Path.DirectorySeparatorChar, "Dinerware Integration", System.IO.Path.DirectorySeparatorChar, "DinerwareConfigurationFile.xml");
            xDoc.Load(@"" + sourceFilePath);
            string accessKey = GetAccessKey();
            if (!string.IsNullOrEmpty(accessKey))
                xDoc.DocumentElement.SelectSingleNode("ACCESS_KEY").InnerText = accessKey;
            //xDoc.Save(@"C:\Dinerware Configuration File\DinerwareConfigurationFile.xml");
            this.Hide();
            frmConfiguration.Show();
        }

        private string GetAccessKey()
        {
            try
            {
                var dispenser = new KeyDispenser(txtLoginDomain.Text, string.Empty);
                return dispenser.GetAccessKey(txtConnectorKey.Text, txtApiKey.Text);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }
}
