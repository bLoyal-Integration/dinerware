namespace DinerwareSystem
{
    partial class frmUpdatebLoyalServiceUrlMsg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUpdatebLoyalServiceUrlMsg));
            this.lblMsg = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.Location = new System.Drawing.Point(55, 18);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblMsg.Size = new System.Drawing.Size(540, 20);
            this.lblMsg.TabIndex = 2;
            this.lblMsg.Text = "bLoyal service is not currently available. Please try again later.";
            this.lblMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMsg.Click += new System.EventHandler(this.lblMsg_Click);
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.btnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnOk.ForeColor = System.Drawing.Color.Snow;
            this.btnOk.Location = new System.Drawing.Point(237, 52);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(146, 44);
            this.btnOk.TabIndex = 3;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // frmUpdatebLoyalServiceUrlMsg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(255)))), ((int)(((byte)(223)))));
            this.ClientSize = new System.Drawing.Size(645, 106);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblMsg);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmUpdatebLoyalServiceUrlMsg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Update bLoyal Service URL";
            this.Load += new System.EventHandler(this.frmUpdatebLoyalServiceUrlMsg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Button btnOk;
    }
}