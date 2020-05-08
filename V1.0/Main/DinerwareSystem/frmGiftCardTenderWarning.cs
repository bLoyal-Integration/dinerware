using System;
using System.Windows.Forms;

namespace DinerwareSystem
{
    public partial class frmGiftCardTenderWarning : Form
    {
        public frmGiftCardTenderWarning()
        {
            InitializeComponent();
        }

        private void frmGiftCardTenderWarning_Load(object sender, EventArgs e)
        {

        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
