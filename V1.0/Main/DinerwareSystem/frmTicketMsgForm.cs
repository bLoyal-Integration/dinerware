using DinerwareSystem.Helpers;
using DinerwareSystem.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DinerwareSystem
{
    public partial class frmTicketMsgForm : Form
    {
        #region Properties

        private Button _btnCancel;
        private Button _btnDiscount;
        private Label _lblApplyDiscountMsg;
        LoggerHelper _logger = LoggerHelper.Instance;

        public int UserId { get; set; }
        public int TicketId { get; set; }

        #endregion

        #region Public Methods

        public frmTicketMsgForm()
        {
            InitializeComponent();
        }

        public frmTicketMsgForm(int userID, int ticketID)
        {
            try
            {
                InitializeComponent();
                UserId = userID;
                TicketId = ticketID;

                if (TicketDictionary.Dictionary != null && TicketDictionary.Dictionary.ContainsKey(ticketID))
                {
                    if (TicketDictionary.Dictionary[ticketID] == "NotCalculated")
                    {
                        _lblApplyDiscountMsg.Text = Messages.CALCULATE_DISCOUNT_WARNING; 
                    }
                    if (TicketDictionary.Dictionary[ticketID] == "CalculatedNotCurrent")
                    {
                        _lblApplyDiscountMsg.Text = Messages.TICKET_CHANGED_WARNING;
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.WriteLogError(ex, "frmTicketMsgForm");
            }
        }

        #endregion

        #region Private Methods

        private void frmTicketMsgForm_Load(object sender, EventArgs e)
        {
            try
            {
                _lblApplyDiscountMsg.TextAlign = ContentAlignment.MiddleCenter;
                _lblApplyDiscountMsg.AutoSize = false;
                _lblApplyDiscountMsg.Left = (this.ClientSize.Width - _lblApplyDiscountMsg.Size.Width) / 2;
            }
            catch (Exception ex)
            {
                _logger.WriteLogError(ex, "frmTicketMsgForm_Load");
                this.Close();
            }
        }      

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            try
            {
                this.Hide();
                frmCalculateSalesTransaction frmCalculateTransaction = new frmCalculateSalesTransaction(UserId, TicketId, null);
                frmCalculateTransaction.Show();
                frmCalculateTransaction.Focus();
                if (frmCalculateTransaction.CloseWindowConfirm)
                {
                    this.Close();
                }
            }
            catch(Exception ex)
            {
                _logger.WriteLogError(ex, "btnDiscount_Click");
            }
        }

        private void lblApplyDiscountMsg_Click(object sender, EventArgs e)
        {
            _lblApplyDiscountMsg.Left = (this.ClientSize.Width - _lblApplyDiscountMsg.Size.Width) / 2;
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTicketMsgForm));
            this._lblApplyDiscountMsg = new System.Windows.Forms.Label();
            this._btnCancel = new System.Windows.Forms.Button();
            this._btnDiscount = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblApplyDiscountMsg
            // 
            this._lblApplyDiscountMsg.AutoSize = true;
            this._lblApplyDiscountMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._lblApplyDiscountMsg.Location = new System.Drawing.Point(8, 18);
            this._lblApplyDiscountMsg.Name = "lblApplyDiscountMsg";
            this._lblApplyDiscountMsg.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this._lblApplyDiscountMsg.Size = new System.Drawing.Size(862, 20);
            this._lblApplyDiscountMsg.TabIndex = 1;
            this._lblApplyDiscountMsg.Text = "The ticket has changed and needs to be recalculated with bLoyal to ensure accurat" +
    "e loyalty benefits. ";
            this._lblApplyDiscountMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this._lblApplyDiscountMsg.Click += new System.EventHandler(this.lblApplyDiscountMsg_Click);
            // 
            // btnCancel
            // 
            this._btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this._btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this._btnCancel.ForeColor = System.Drawing.Color.Snow;
            this._btnCancel.Location = new System.Drawing.Point(285, 50);
            this._btnCancel.Name = "btnCancel";
            this._btnCancel.Size = new System.Drawing.Size(187, 58);
            this._btnCancel.TabIndex = 0;
            this._btnCancel.Text = "Ok";
            this._btnCancel.UseVisualStyleBackColor = false;
            this._btnCancel.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnDiscount
            // 
            this._btnDiscount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this._btnDiscount.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this._btnDiscount.ForeColor = System.Drawing.Color.Snow;
            this._btnDiscount.Location = new System.Drawing.Point(12, 55);
            this._btnDiscount.Name = "btnDiscount";
            this._btnDiscount.Size = new System.Drawing.Size(187, 58);
            this._btnDiscount.TabIndex = 2;
            this._btnDiscount.Text = "bLoyal Apply Discount";
            this._btnDiscount.UseVisualStyleBackColor = false;
            this._btnDiscount.Visible = false;
            this._btnDiscount.Click += new System.EventHandler(this.btnDiscount_Click);
            // 
            // frmTicketMsgForm
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(255)))), ((int)(((byte)(223)))));
            this.ClientSize = new System.Drawing.Size(789, 120);
            this.Controls.Add(this._btnDiscount);
            this.Controls.Add(this._lblApplyDiscountMsg);
            this.Controls.Add(this._btnCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmTicketMsgForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Apply Discount Warning";
            this.Load += new System.EventHandler(this.frmTicketMsgForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion        
    }
}
