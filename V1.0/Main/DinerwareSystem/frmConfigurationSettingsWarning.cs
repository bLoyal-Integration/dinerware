using System;
using System.Windows.Forms;

namespace DinerwareSystem
{
    public partial class frmConfigurationSettingsWarning : Form
    {
        public frmConfigurationSettingsWarning()
        {
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            InitializeComponent();
        }

        private void frmConfigurationSettingsWarning_Load(object sender, EventArgs e)
        {

        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
