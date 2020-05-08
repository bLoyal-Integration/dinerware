namespace DinerwareSystem
{
    partial class frmLoadGiftCardKeyBoard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLoadGiftCardKeyBoard));
            this.giftCardNumbertxt = new System.Windows.Forms.RichTextBox();
            this.giftCardAmt = new System.Windows.Forms.RichTextBox();
            this.btnKids = new System.Windows.Forms.Button();
            this.btnAlphabetical = new System.Windows.Forms.Button();
            this.btnStandard = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblApplyBalance = new System.Windows.Forms.Label();
            this.keyboardcontrol = new KeyboardClassLibrary.Keyboardcontrol();
            this.SuspendLayout();
            // 
            // giftCardNumbertxt
            // 
            this.giftCardNumbertxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.giftCardNumbertxt.Location = new System.Drawing.Point(12, 35);
            this.giftCardNumbertxt.Multiline = false;
            this.giftCardNumbertxt.Name = "giftCardNumbertxt";
            this.giftCardNumbertxt.Size = new System.Drawing.Size(993, 41);
            this.giftCardNumbertxt.TabIndex = 1;
            this.giftCardNumbertxt.Text = "";
            // 
            // giftCardAmt
            // 
            this.giftCardAmt.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.giftCardAmt.Location = new System.Drawing.Point(12, 115);
            this.giftCardAmt.Multiline = false;
            this.giftCardAmt.Name = "giftCardAmt";
            this.giftCardAmt.Size = new System.Drawing.Size(993, 41);
            this.giftCardAmt.TabIndex = 8;
            this.giftCardAmt.Text = "";
            // 
            // btnKids
            // 
            this.btnKids.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.btnKids.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnKids.ForeColor = System.Drawing.Color.Snow;
            this.btnKids.Location = new System.Drawing.Point(329, 454);
            this.btnKids.Name = "btnKids";
            this.btnKids.Size = new System.Drawing.Size(151, 60);
            this.btnKids.TabIndex = 16;
            this.btnKids.Text = "Kids";
            this.btnKids.UseVisualStyleBackColor = false;
            this.btnKids.Visible = false;
            this.btnKids.Click += new System.EventHandler(this.btnKids_Click);
            // 
            // btnAlphabetical
            // 
            this.btnAlphabetical.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.btnAlphabetical.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnAlphabetical.ForeColor = System.Drawing.Color.Snow;
            this.btnAlphabetical.Location = new System.Drawing.Point(169, 454);
            this.btnAlphabetical.Name = "btnAlphabetical";
            this.btnAlphabetical.Size = new System.Drawing.Size(151, 60);
            this.btnAlphabetical.TabIndex = 15;
            this.btnAlphabetical.Text = "Alphabetical";
            this.btnAlphabetical.UseVisualStyleBackColor = false;
            this.btnAlphabetical.Click += new System.EventHandler(this.btnAlphabetical_Click);
            // 
            // btnStandard
            // 
            this.btnStandard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.btnStandard.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnStandard.ForeColor = System.Drawing.Color.Snow;
            this.btnStandard.Location = new System.Drawing.Point(12, 454);
            this.btnStandard.Name = "btnStandard";
            this.btnStandard.Size = new System.Drawing.Size(151, 60);
            this.btnStandard.TabIndex = 14;
            this.btnStandard.Text = "Standard";
            this.btnStandard.UseVisualStyleBackColor = false;
            this.btnStandard.Click += new System.EventHandler(this.btnStandard_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnOK.ForeColor = System.Drawing.Color.Snow;
            this.btnOK.Location = new System.Drawing.Point(700, 454);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(151, 60);
            this.btnOK.TabIndex = 13;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.Snow;
            this.btnCancel.Location = new System.Drawing.Point(857, 454);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(151, 60);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 17);
            this.label1.TabIndex = 53;
            this.label1.Text = "Gift Card Number:";
            // 
            // lblApplyBalance
            // 
            this.lblApplyBalance.AutoSize = true;
            this.lblApplyBalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApplyBalance.Location = new System.Drawing.Point(13, 88);
            this.lblApplyBalance.Name = "lblApplyBalance";
            this.lblApplyBalance.Size = new System.Drawing.Size(186, 17);
            this.lblApplyBalance.TabIndex = 54;
            this.lblApplyBalance.Text = "Amount Being Applied $:";
            // 
            // keyboardcontrol
            // 
            this.keyboardcontrol.KeyboardType = KeyboardClassLibrary.BoW.Standard;
            this.keyboardcontrol.Location = new System.Drawing.Point(12, 166);
            this.keyboardcontrol.Name = "keyboardcontrol";
            this.keyboardcontrol.Size = new System.Drawing.Size(993, 282);
            this.keyboardcontrol.TabIndex = 0;
            this.keyboardcontrol.UserKeyPressed += new KeyboardClassLibrary.KeyboardDelegate(this.keyboardcontrol_UserKeyPressed);
            // 
            // frmLoadGiftCardKeyBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(255)))), ((int)(((byte)(223)))));
            this.ClientSize = new System.Drawing.Size(1017, 526);
            this.Controls.Add(this.lblApplyBalance);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnKids);
            this.Controls.Add(this.btnAlphabetical);
            this.Controls.Add(this.btnStandard);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.giftCardAmt);
            this.Controls.Add(this.giftCardNumbertxt);
            this.Controls.Add(this.keyboardcontrol);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmLoadGiftCardKeyBoard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "bLoyal Touch Screen KeyBoard";
            this.Load += new System.EventHandler(this.frmLoadGiftCardKeyBoard_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private KeyboardClassLibrary.Keyboardcontrol keyboardcontrol;
        private System.Windows.Forms.RichTextBox giftCardNumbertxt;
        private System.Windows.Forms.RichTextBox giftCardAmt;
        private System.Windows.Forms.Button btnKids;
        private System.Windows.Forms.Button btnAlphabetical;
        private System.Windows.Forms.Button btnStandard;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblApplyBalance;
    }
}

