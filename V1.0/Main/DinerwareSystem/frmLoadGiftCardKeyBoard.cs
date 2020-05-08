using System;
using System.Linq;
using System.Windows.Forms;

namespace DinerwareSystem
{
    public partial class frmLoadGiftCardKeyBoard : Form
    {

        private frmbLoyalGiftCardTender _frmLoadGiftCardBalance;
        private TextBox _focusedTextbox = null;
        private Control _focusedControl;
        LoggerHelper _logger = LoggerHelper.Instance;
        private bool _isOpenTicket;
        private string _amount;
        private string _giftCardNumber;

        public frmLoadGiftCardKeyBoard()
        {
            InitializeComponent();
        }

        public frmLoadGiftCardKeyBoard(ref frmbLoyalGiftCardTender frmLoadGiftCardBalance, bool isOpenTicket, string amount, string giftCardNumber)
        {
            try
            {
                _frmLoadGiftCardBalance = frmLoadGiftCardBalance;
                _isOpenTicket = isOpenTicket;
                _amount = amount;
                _giftCardNumber = giftCardNumber;
                InitializeComponent();
                foreach (TextBox tb in this.Controls.OfType<TextBox>())
                {
                    tb.Enter += textBox_Enter;
                }
            }
            catch(Exception ex)
            {
                _logger.WriteLogError(ex, "frmLoadGiftCardKeyBoard");
            }
        }

        private void TextBox_GotFocus(object sender, EventArgs e)
        {
            _focusedControl = (Control)sender;
        }

        void textBox_Enter(object sender, EventArgs e)
        {
            _focusedTextbox = (TextBox)sender;
        }

        private void keyboardcontrol_UserKeyPressed(object sender, KeyboardClassLibrary.KeyboardEventArgs e)
        {
            try
            {
                if (giftCardNumbertxt.Focused)
                {
                    giftCardNumbertxt.Focus();
                }
                else if (giftCardAmt.Focused)
                {
                    giftCardAmt.Focus();
                }
                SendKeys.Send(e.KeyboardKeyPressed);
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "keyboardcontrol_UserKeyPressed");
            }
        }

        private void frmLoadGiftCardKeyBoard_Load(object sender, EventArgs e)
        {
            try
            {
                giftCardNumbertxt.Focus();
                this.ActiveControl = giftCardNumbertxt;
                if (!_isOpenTicket)
                {
                    giftCardAmt.ReadOnly = true;
                }
                giftCardAmt.Text = _amount;
                giftCardNumbertxt.Text = _giftCardNumber;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "frmLoadGiftCardKeyBoard_Load");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            CloseKeyBoard();
        }

        private void CloseKeyBoard()
        {
            try
            {
                _frmLoadGiftCardBalance.GiftCardNumber = giftCardNumbertxt.Text;
                _frmLoadGiftCardBalance.GiftCardAmount = giftCardAmt.Text;
                this.Close();
                _frmLoadGiftCardBalance.Show();
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "CloseKeyBoard");
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            CloseKeyBoard();
        }

        private void btnStandard_Click(object sender, EventArgs e)
        {
            keyboardcontrol.KeyboardType = KeyboardClassLibrary.BoW.Standard;
        }

        private void btnAlphabetical_Click(object sender, EventArgs e)
        {
            keyboardcontrol.KeyboardType = KeyboardClassLibrary.BoW.Alphabetical;
        }

        private void btnKids_Click(object sender, EventArgs e)
        {
            keyboardcontrol.KeyboardType = KeyboardClassLibrary.BoW.Kids;
        }
    }
}