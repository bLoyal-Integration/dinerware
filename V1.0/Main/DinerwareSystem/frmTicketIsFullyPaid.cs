using System;
using System.Drawing;
using System.Windows.Forms;

namespace DinerwareSystem
{
    public partial class frmTicketIsFullyPaid : Form
    {
        public bool IsCallforCalcCart { get; set; }

        private string MSG = string.Empty;

        public frmTicketIsFullyPaid()
        {
            InitializeComponent();
            IsCallforCalcCart = false;
        }

        public frmTicketIsFullyPaid(bool isCalculateCartCall, string warningMsg)
        {
            InitializeComponent();
            MSG = warningMsg;
            IsCallforCalcCart = isCalculateCartCall ? isCalculateCartCall : false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Close snippets automatically
            Timer frmCloseTimer = new Timer();
            frmCloseTimer.Interval = 300000;
            frmCloseTimer.Start();
            frmCloseTimer.Tick += new EventHandler(CloseWindow);

            lblMsg.TextAlign = ContentAlignment.MiddleCenter;
            lblMsg.AutoSize = false;
            lblMsg.Left = (this.ClientSize.Width - lblMsg.Size.Width) / 2;

            if (IsCallforCalcCart)
                lblMsg.Text = MSG;

        }

        private void CloseWindow(object sender, EventArgs e)
        {
            this.Close();
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
