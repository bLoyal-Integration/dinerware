using System;
using System.Linq;
using System.Windows.Forms;

namespace DinerwareSystem
{
    public partial class frmTouchscreenKeyboard : Form
    {
        private frmLoadGiftCardBalance _frmbLoyalGiftCard;
        private TextBox _focusedTextbox = null;
        private Control _focusedControl;
        LoggerHelper _logger = LoggerHelper.Instance;
        private string _giftCardNumber;

        public frmTouchscreenKeyboard()
        {
            InitializeComponent();
        }

        public frmTouchscreenKeyboard(ref frmLoadGiftCardBalance frmgiftCard, string giftCardNumber)
        {
            try
            {
                _frmbLoyalGiftCard = frmgiftCard;
                _giftCardNumber = giftCardNumber;
                InitializeComponent();
                foreach (TextBox tb in this.Controls.OfType<TextBox>())
                {
                    tb.Enter += textBox_Enter;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "frmTouchscreenKeyboard");
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
                giftCardNumbertxt.Focus();
                SendKeys.Send(e.KeyboardKeyPressed);
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "keyboardcontrol_UserKeyPressed");
            }
        }

        private void frmTouchscreenKeyboard_Load(object sender, EventArgs e)
        {
            try
            {
                giftCardNumbertxt.Focus();
                this.ActiveControl = giftCardNumbertxt;
                giftCardNumbertxt.Text = _giftCardNumber;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "frmTouchscreenKeyboard_Load");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _frmbLoyalGiftCard.GiftCardNumber = giftCardNumbertxt.Text;
            this.Close();
            _frmbLoyalGiftCard.Show();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _frmbLoyalGiftCard.GiftCardNumber = giftCardNumbertxt.Text;
            this.Close();
            _frmbLoyalGiftCard.Show();
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

        private void keyboardcontrol1_UserKeyPressed(object sender, KeyboardClassLibrary.KeyboardEventArgs e)
        {

        }
    }
}