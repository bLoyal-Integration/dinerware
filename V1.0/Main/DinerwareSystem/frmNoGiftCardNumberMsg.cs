using System;
using System.Drawing;
using System.Windows.Forms;

namespace DinerwareSystem
{
    public partial class frmNoGiftCardNumberMsg : Form
    {
        public frmNoGiftCardNumberMsg()
        {
            InitializeComponent();
        }

        private void frmNoGiftCardNumberMsg_Load(object sender, EventArgs e)
        {
            lblMsg.TextAlign = ContentAlignment.MiddleCenter;
            lblMsg.AutoSize = false;
            lblMsg.Left = (this.ClientSize.Width - lblMsg.Size.Width) / 2;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblMsg_Click(object sender, EventArgs e)
        {
            lblMsg.Left = (this.ClientSize.Width - lblMsg.Size.Width) / 2;
        }
    }
}
