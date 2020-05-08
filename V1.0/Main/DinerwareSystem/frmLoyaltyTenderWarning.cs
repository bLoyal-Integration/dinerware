using System;
using System.Windows.Forms;

namespace DinerwareSystem
{
    public partial class frmLoyaltyTenderWarning : Form
    {
        public frmLoyaltyTenderWarning()
        {
            InitializeComponent();
        }

        private void frmLoyaltyTenderWarning_Load(object sender, EventArgs e)
        {

        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
