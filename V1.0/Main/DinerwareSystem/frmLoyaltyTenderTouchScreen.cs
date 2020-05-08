using System;
using System.Linq;
using System.Windows.Forms;

namespace DinerwareSystem
{
    public partial class frmLoyaltyTenderTouchScreen : Form
    {
        private bLoyalLoyaltyTender _frmbLoyalLoyaltyTender;
        private TextBox _focusedTextbox = null;
        private Control _focusedControl;
        LoggerHelper _logger = LoggerHelper.Instance;
        private string _loyaltyTenderAmount;

        public frmLoyaltyTenderTouchScreen()
        {
            InitializeComponent();
        }

        public frmLoyaltyTenderTouchScreen(ref bLoyalLoyaltyTender frmtender, string tenderAmount)
        {
            try
            {
                _frmbLoyalLoyaltyTender = frmtender;
                _loyaltyTenderAmount = tenderAmount;
                InitializeComponent();
                foreach (TextBox tb in this.Controls.OfType<TextBox>())
                {
                    tb.Enter += textBox_Enter;
                }
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "frmLoyaltyTenderTouchScreen");
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
                loyaltyTenderAmountTxt.Focus();
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
                loyaltyTenderAmountTxt.Text = _loyaltyTenderAmount;
                loyaltyTenderAmountTxt.Focus();
                this.ActiveControl = loyaltyTenderAmountTxt;               
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "frmTouchscreenKeyboard_Load");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            _frmbLoyalLoyaltyTender.LoyaltyTenderAmount = loyaltyTenderAmountTxt.Text;
            this.Close();
            _frmbLoyalLoyaltyTender.Show();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            _frmbLoyalLoyaltyTender.LoyaltyTenderAmount = loyaltyTenderAmountTxt.Text;
            this.Close();
            _frmbLoyalLoyaltyTender.Show();
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