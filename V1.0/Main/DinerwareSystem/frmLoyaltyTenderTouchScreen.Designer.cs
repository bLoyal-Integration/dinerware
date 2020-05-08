namespace DinerwareSystem
{
    partial class frmLoyaltyTenderTouchScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLoyaltyTenderTouchScreen));
            this.loyaltyTenderAmountTxt = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnKids = new System.Windows.Forms.Button();
            this.btnAlphabetical = new System.Windows.Forms.Button();
            this.btnStandard = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.keyboardcontrol = new KeyboardClassLibrary.Keyboardcontrol();
            this.SuspendLayout();
            // 
            // loyaltyTenderAmountTxt
            // 
            this.loyaltyTenderAmountTxt.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loyaltyTenderAmountTxt.Location = new System.Drawing.Point(12, 38);
            this.loyaltyTenderAmountTxt.Multiline = false;
            this.loyaltyTenderAmountTxt.Name = "loyaltyTenderAmountTxt";
            this.loyaltyTenderAmountTxt.Size = new System.Drawing.Size(993, 41);
            this.loyaltyTenderAmountTxt.TabIndex = 1;
            this.loyaltyTenderAmountTxt.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 17);
            this.label1.TabIndex = 54;
            this.label1.Text = "Gift Card Number:";
            // 
            // btnKids
            // 
            this.btnKids.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.btnKids.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnKids.ForeColor = System.Drawing.Color.Snow;
            this.btnKids.Location = new System.Drawing.Point(328, 373);
            this.btnKids.Name = "btnKids";
            this.btnKids.Size = new System.Drawing.Size(151, 60);
            this.btnKids.TabIndex = 59;
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
            this.btnAlphabetical.Location = new System.Drawing.Point(171, 373);
            this.btnAlphabetical.Name = "btnAlphabetical";
            this.btnAlphabetical.Size = new System.Drawing.Size(151, 60);
            this.btnAlphabetical.TabIndex = 58;
            this.btnAlphabetical.Text = "Alphabetical";
            this.btnAlphabetical.UseVisualStyleBackColor = false;
            this.btnAlphabetical.Click += new System.EventHandler(this.btnAlphabetical_Click);
            // 
            // btnStandard
            // 
            this.btnStandard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.btnStandard.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnStandard.ForeColor = System.Drawing.Color.Snow;
            this.btnStandard.Location = new System.Drawing.Point(14, 373);
            this.btnStandard.Name = "btnStandard";
            this.btnStandard.Size = new System.Drawing.Size(151, 60);
            this.btnStandard.TabIndex = 57;
            this.btnStandard.Text = "Standard";
            this.btnStandard.UseVisualStyleBackColor = false;
            this.btnStandard.Click += new System.EventHandler(this.btnStandard_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnOK.ForeColor = System.Drawing.Color.Snow;
            this.btnOK.Location = new System.Drawing.Point(699, 373);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(151, 60);
            this.btnOK.TabIndex = 56;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.Snow;
            this.btnCancel.Location = new System.Drawing.Point(856, 373);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(151, 60);
            this.btnCancel.TabIndex = 55;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // keyboardcontrol
            // 
            this.keyboardcontrol.KeyboardType = KeyboardClassLibrary.BoW.Standard;
            this.keyboardcontrol.Location = new System.Drawing.Point(12, 85);
            this.keyboardcontrol.Name = "keyboardcontrol";
            this.keyboardcontrol.Size = new System.Drawing.Size(993, 282);
            this.keyboardcontrol.TabIndex = 0;
            this.keyboardcontrol.UserKeyPressed += new KeyboardClassLibrary.KeyboardDelegate(this.keyboardcontrol_UserKeyPressed);
            // 
            // frmLoyaltyTenderTouchScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(255)))), ((int)(((byte)(223)))));
            this.ClientSize = new System.Drawing.Size(1017, 444);
            this.Controls.Add(this.btnKids);
            this.Controls.Add(this.btnAlphabetical);
            this.Controls.Add(this.btnStandard);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.loyaltyTenderAmountTxt);
            this.Controls.Add(this.keyboardcontrol);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmLoyaltyTenderTouchScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "bLoyal Touch Screen KeyBoard";
            this.Load += new System.EventHandler(this.frmTouchscreenKeyboard_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private KeyboardClassLibrary.Keyboardcontrol keyboardcontrol;
        private System.Windows.Forms.RichTextBox loyaltyTenderAmountTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnKids;
        private System.Windows.Forms.Button btnAlphabetical;
        private System.Windows.Forms.Button btnStandard;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
    }
}

