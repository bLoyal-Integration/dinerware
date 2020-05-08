using System;
using System.Windows.Forms;

namespace DinerwareSystem
{
    public partial class frmDisableEnablebLoyalMsg : Form
    {
        public frmDisableEnablebLoyalMsg()
        {
            InitializeComponent();
        }

        private void frmDisableEnablebLoyalMsg_Load(object sender, EventArgs e)
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

        private void okbtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
