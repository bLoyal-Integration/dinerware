namespace DinerwareSystem
{
    partial class frmGiftCardTenderWarning
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGiftCardTenderWarning));
            this.msglbl = new System.Windows.Forms.Label();
            this.okBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // msglbl
            // 
            this.msglbl.AutoSize = true;
            this.msglbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msglbl.Location = new System.Drawing.Point(21, 9);
            this.msglbl.Name = "msglbl";
            this.msglbl.Size = new System.Drawing.Size(421, 17);
            this.msglbl.TabIndex = 6;
            this.msglbl.Text = "Gift card not exist - number is auto provisioned disabled.";
            // 
            // okBtn
            // 
            this.okBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.okBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.okBtn.ForeColor = System.Drawing.Color.Snow;
            this.okBtn.Location = new System.Drawing.Point(154, 29);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(123, 41);
            this.okBtn.TabIndex = 7;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = false;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // frmGiftCardTenderWarning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(255)))), ((int)(((byte)(223)))));
            this.ClientSize = new System.Drawing.Size(450, 79);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.msglbl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmGiftCardTenderWarning";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gift Card Tender Warning";
            this.Load += new System.EventHandler(this.frmGiftCardTenderWarning_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label msglbl;
        private System.Windows.Forms.Button okBtn;
    }
}