using System;
using System.Drawing;
using System.Windows.Forms;

namespace DinerwareSystem
{
    public partial class frmUpdatebLoyalServiceUrlMsg : Form
    {
        private bool isbLoyalAPIDown = false;

        public frmUpdatebLoyalServiceUrlMsg()
        {
            InitializeComponent();
        }

        public frmUpdatebLoyalServiceUrlMsg(bool bLoyalAPIDownStatus)
        {
            InitializeComponent();
            isbLoyalAPIDown = bLoyalAPIDownStatus;
        }

        private void frmUpdatebLoyalServiceUrlMsg_Load(object sender, EventArgs e)
        {
            lblMsg.Text = isbLoyalAPIDown ? "bLoyal service is not currently available. Please try again later." : "Service URL Updated Successfully.";

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
