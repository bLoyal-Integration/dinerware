using System;
using System.Windows.Forms;

namespace DinerwareSystem
{
    public partial class frmCheckGiftCardWarning : Form
    {
        public frmCheckGiftCardWarning()
        {
            InitializeComponent();
        }

        private void frmCheckGiftCardWarning_Load(object sender, EventArgs e)
        {

        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
