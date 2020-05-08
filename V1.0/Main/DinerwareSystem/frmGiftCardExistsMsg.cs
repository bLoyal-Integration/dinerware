using System;
using System.Drawing;
using System.Windows.Forms;

namespace DinerwareSystem
{
    public partial class frmGiftCardExistsMsg : Form
    {
        private string _giftCardNumber;
        public frmGiftCardExistsMsg(string giftCardNumber)
        {
            InitializeComponent();
            _giftCardNumber = giftCardNumber;
        }

        private void frmGiftCardExistsMsg_Load(object sender, EventArgs e)
        {
            lblMsg.Text = $"There is already an active GiftCard with the GiftCard Number: {_giftCardNumber}";
            lblMsg.TextAlign = ContentAlignment.MiddleCenter;
            lblMsg.AutoSize = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblMsg_Click(object sender, EventArgs e)
        {

        }
    }
}
