using System;
using System.Drawing;
using System.Windows.Forms;

namespace DinerwareSystem
{
    public partial class frmLoadGiftCardWarning : Form
    {
        string msg = string.Empty;

        public frmLoadGiftCardWarning()
        {
            InitializeComponent();
        }

        public frmLoadGiftCardWarning(string warningMsg)
        {
            InitializeComponent();
            msg = warningMsg;
        }

        private void frmLoadGiftCardWarning_Load(object sender, EventArgs e)
        {
            lblMsg.Text = msg;
            lblMsg.TextAlign = ContentAlignment.MiddleCenter;           
            lblMsg.Left = (this.ClientSize.Width - lblMsg.Size.Width) / 2;
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblMsg_Click(object sender, EventArgs e)
        {
            lblMsg.Left = (this.ClientSize.Width - lblMsg.Size.Width) / 2;
        }
    }
}
