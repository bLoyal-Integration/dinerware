namespace DinerwareSystem
{
    partial class frmbLoyalGiftCardTender
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmbLoyalGiftCardTender));
            this.checkBalanceBtn = new System.Windows.Forms.Button();
            this.applyBtn = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.lblApplyBalance = new System.Windows.Forms.Label();
            this.textApplyBalance = new System.Windows.Forms.TextBox();
            this.lblApplyBalanceError = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGiftCardNumber = new System.Windows.Forms.TextBox();
            this.lblAvailableBalanceAmt = new System.Windows.Forms.Label();
            this.lblAvailableBalance = new System.Windows.Forms.Label();
            this.lblEmptyCardNumberError = new System.Windows.Forms.Label();
            this.btnkeyboard = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkBalanceBtn
            // 
            this.checkBalanceBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.checkBalanceBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.checkBalanceBtn.ForeColor = System.Drawing.Color.Snow;
            this.checkBalanceBtn.Location = new System.Drawing.Point(487, 12);
            this.checkBalanceBtn.Name = "checkBalanceBtn";
            this.checkBalanceBtn.Size = new System.Drawing.Size(149, 49);
            this.checkBalanceBtn.TabIndex = 42;
            this.checkBalanceBtn.Text = "Check Balance";
            this.checkBalanceBtn.UseVisualStyleBackColor = false;
            this.checkBalanceBtn.Click += new System.EventHandler(this.checkBalanceBtn_Click);
            // 
            // applyBtn
            // 
            this.applyBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.applyBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.applyBtn.ForeColor = System.Drawing.Color.Snow;
            this.applyBtn.Location = new System.Drawing.Point(487, 66);
            this.applyBtn.Name = "applyBtn";
            this.applyBtn.Size = new System.Drawing.Size(149, 49);
            this.applyBtn.TabIndex = 46;
            this.applyBtn.Text = "Apply";
            this.applyBtn.UseVisualStyleBackColor = false;
            this.applyBtn.Click += new System.EventHandler(this.applyBtn_Click);
            // 
            // Cancel
            // 
            this.Cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancel.ForeColor = System.Drawing.Color.Snow;
            this.Cancel.Location = new System.Drawing.Point(487, 121);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(149, 49);
            this.Cancel.TabIndex = 47;
            this.Cancel.Text = "Cancel ";
            this.Cancel.UseVisualStyleBackColor = false;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // lblApplyBalance
            // 
            this.lblApplyBalance.AutoSize = true;
            this.lblApplyBalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApplyBalance.Location = new System.Drawing.Point(64, 72);
            this.lblApplyBalance.Name = "lblApplyBalance";
            this.lblApplyBalance.Size = new System.Drawing.Size(186, 17);
            this.lblApplyBalance.TabIndex = 49;
            this.lblApplyBalance.Text = "Amount Being Applied $:";
            // 
            // textApplyBalance
            // 
            this.textApplyBalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textApplyBalance.Location = new System.Drawing.Point(256, 66);
            this.textApplyBalance.Name = "textApplyBalance";
            this.textApplyBalance.Size = new System.Drawing.Size(185, 23);
            this.textApplyBalance.TabIndex = 50;
            // 
            // lblApplyBalanceError
            // 
            this.lblApplyBalanceError.AutoSize = true;
            this.lblApplyBalanceError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApplyBalanceError.ForeColor = System.Drawing.Color.Red;
            this.lblApplyBalanceError.Location = new System.Drawing.Point(64, 229);
            this.lblApplyBalanceError.Name = "lblApplyBalanceError";
            this.lblApplyBalanceError.Size = new System.Drawing.Size(34, 13);
            this.lblApplyBalanceError.TabIndex = 51;
            this.lblApplyBalanceError.Text = "Error";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(67, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(183, 17);
            this.label1.TabIndex = 52;
            this.label1.Text = "Enter Gift Card Number:";
            // 
            // txtGiftCardNumber
            // 
            this.txtGiftCardNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGiftCardNumber.Location = new System.Drawing.Point(256, 22);
            this.txtGiftCardNumber.Name = "txtGiftCardNumber";
            this.txtGiftCardNumber.Size = new System.Drawing.Size(185, 23);
            this.txtGiftCardNumber.TabIndex = 53;
            // 
            // lblAvailableBalanceAmt
            // 
            this.lblAvailableBalanceAmt.AutoSize = true;
            this.lblAvailableBalanceAmt.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvailableBalanceAmt.Location = new System.Drawing.Point(256, 121);
            this.lblAvailableBalanceAmt.Name = "lblAvailableBalanceAmt";
            this.lblAvailableBalanceAmt.Size = new System.Drawing.Size(17, 17);
            this.lblAvailableBalanceAmt.TabIndex = 54;
            this.lblAvailableBalanceAmt.Text = "0";
            // 
            // lblAvailableBalance
            // 
            this.lblAvailableBalance.AutoSize = true;
            this.lblAvailableBalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvailableBalance.Location = new System.Drawing.Point(94, 121);
            this.lblAvailableBalance.Name = "lblAvailableBalance";
            this.lblAvailableBalance.Size = new System.Drawing.Size(156, 17);
            this.lblAvailableBalance.TabIndex = 55;
            this.lblAvailableBalance.Text = "Available Balance $:";
            // 
            // lblEmptyCardNumberError
            // 
            this.lblEmptyCardNumberError.AutoSize = true;
            this.lblEmptyCardNumberError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmptyCardNumberError.ForeColor = System.Drawing.Color.Red;
            this.lblEmptyCardNumberError.Location = new System.Drawing.Point(256, 48);
            this.lblEmptyCardNumberError.Name = "lblEmptyCardNumberError";
            this.lblEmptyCardNumberError.Size = new System.Drawing.Size(34, 13);
            this.lblEmptyCardNumberError.TabIndex = 56;
            this.lblEmptyCardNumberError.Text = "Error";
            // 
            // btnkeyboard
            // 
            this.btnkeyboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.btnkeyboard.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnkeyboard.ForeColor = System.Drawing.Color.Snow;
            this.btnkeyboard.Location = new System.Drawing.Point(487, 176);
            this.btnkeyboard.Name = "btnkeyboard";
            this.btnkeyboard.Size = new System.Drawing.Size(149, 49);
            this.btnkeyboard.TabIndex = 57;
            this.btnkeyboard.Text = "Keyboard";
            this.btnkeyboard.UseVisualStyleBackColor = false;
            this.btnkeyboard.Click += new System.EventHandler(this.btnkeyboard_Click);
            // 
            // frmbLoyalGiftCardTender
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(255)))), ((int)(((byte)(223)))));
            this.ClientSize = new System.Drawing.Size(648, 248);
            this.Controls.Add(this.btnkeyboard);
            this.Controls.Add(this.lblEmptyCardNumberError);
            this.Controls.Add(this.lblAvailableBalance);
            this.Controls.Add(this.lblAvailableBalanceAmt);
            this.Controls.Add(this.txtGiftCardNumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblApplyBalanceError);
            this.Controls.Add(this.textApplyBalance);
            this.Controls.Add(this.lblApplyBalance);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.applyBtn);
            this.Controls.Add(this.checkBalanceBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmbLoyalGiftCardTender";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "bLoyal Gift Card Tender";
            this.Load += new System.EventHandler(this.frmbLoyalGiftCardTender_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button checkBalanceBtn;
        private System.Windows.Forms.Button applyBtn;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label lblApplyBalance;
        private System.Windows.Forms.TextBox textApplyBalance;
        private System.Windows.Forms.Label lblApplyBalanceError;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtGiftCardNumber;
        private System.Windows.Forms.Label lblAvailableBalanceAmt;
        private System.Windows.Forms.Label lblAvailableBalance;
        private System.Windows.Forms.Label lblEmptyCardNumberError;
        private System.Windows.Forms.Button btnkeyboard;
    }
}