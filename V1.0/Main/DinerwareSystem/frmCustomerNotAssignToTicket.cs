using System;
using System.Windows.Forms;

namespace DinerwareSystem
{
    public partial class frmCustomerNotAssignToTicket : Form
    {

        public frmCustomerNotAssignToTicket()
        {
            InitializeComponent();
        }
       
        private void CustomerNotAssignToTicket_Load(object sender, EventArgs e)
        {
            // Close snippets automatically
            Timer frmCloseTimer = new Timer();
            frmCloseTimer.Interval = 300000;
            frmCloseTimer.Start();
            frmCloseTimer.Tick += new EventHandler(CloseWindow);
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            this.Close();
        }

        private void okBtn_Click(object sender, EventArgs e)
        {               
            this.Close();
        }
    }
}
