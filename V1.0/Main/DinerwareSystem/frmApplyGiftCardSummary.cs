using System;
using System.Drawing;
using System.Windows.Forms;

namespace DinerwareSystem
{
    public partial class frmApplyGiftCardSummary : Form
    {
        public decimal _netAmount { get; set; }
        public string _giftCardNumber { get; set; }
        public bool _isProvisioned { get; set; }
        public bool _isNewCard { get; set; }


        public frmApplyGiftCardSummary()
        {
            InitializeComponent();
        }

        public frmApplyGiftCardSummary(decimal netAmount, string giftCardNumber, bool isProvisioned, bool isNewCard)
        {
            InitializeComponent();
            _netAmount = netAmount;
            _giftCardNumber = giftCardNumber;
            _isProvisioned = isProvisioned;
            _isNewCard = isNewCard;
        }

        private void frmApplyGiftCardSummary_Load(object sender, EventArgs e)
        {
            try
            {
                string msg = string.Format("${0} has been NOT been provisioned on gift card {1}.", Math.Round(_netAmount, 2), _giftCardNumber);
                if (!_isNewCard)
                    msg = string.Format("${0} has been Loaded on existing gift card {1}.", Math.Round(_netAmount, 2), _giftCardNumber);
               else if (_isProvisioned)
                    msg = string.Format("${0} has been provisioned on gift card {1}.", Math.Round(_netAmount, 2), _giftCardNumber);

                msglbl.Text = msg;
                msglbl.TextAlign = ContentAlignment.MiddleCenter;
                msglbl.AutoSize = true;
                msglbl.Left = (this.ClientSize.Width - msglbl.Size.Width) / 2;

            }
            catch(Exception ex)
            {
                LoggerHelper.Instance.WriteLogError(ex, "frmApplyGiftCardSummary_Load");
            }
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void msglbl_Click(object sender, EventArgs e)
        {
            msglbl.Left = (this.ClientSize.Width - msglbl.Size.Width) / 2;
        }
    }
}
