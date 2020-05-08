using System;
using System.Drawing;
using System.Windows.Forms;

namespace DinerwareSystem
{
    public partial class frmIsTicketOpen : Form
    {
        LoggerHelper _logger = LoggerHelper.Instance;
        bLoyal.Connectors.LoyaltyEngine.Customer customer = null;
        int userId = 0;

        public frmIsTicketOpen()
        {
            InitializeComponent();
        }

        public frmIsTicketOpen(bLoyal.Connectors.LoyaltyEngine.Customer tranCustomer, int currentUserId)
        {
            customer = tranCustomer;
            userId = currentUserId;
            InitializeComponent();
        }

        private void frmIsTicketOpen_Load(object sender, EventArgs e)
        {
            try
            {             
                // Close snippets automatically
                Timer frmCloseTimer = new Timer();
                frmCloseTimer.Interval = 300000;
                frmCloseTimer.Start();
                frmCloseTimer.Tick += new EventHandler(CloseWindow);

                lblCustomerName.TextAlign = ContentAlignment.MiddleCenter;
                lblCustomerName.AutoSize = false;
                lblCustomerName.Left = (this.ClientSize.Width - lblCustomerName.Size.Width) / 2;             

                lblTicketMsg.Text = string.Empty;

                if (customer != null)
                    lblCustomerName.Text = string.Format("{0}, {1}", customer.LastName, customer.FirstName);

                lblTicketMsg.Text = string.Format("There is already an open ticket for this customer:");
               
            }
            catch(Exception ex)
            {
                _logger.WriteLogError(ex, "frmIsTicketOpen_Load");
            }
        }

        private void CloseWindow(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblCustomerName_Click(object sender, EventArgs e)
        {
            lblCustomerName.Left = (this.ClientSize.Width - lblCustomerName.Size.Width) / 2;
        }
    }
}
