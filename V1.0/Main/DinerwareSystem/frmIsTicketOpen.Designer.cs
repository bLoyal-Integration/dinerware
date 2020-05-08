namespace DinerwareSystem
{
    partial class frmIsTicketOpen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmIsTicketOpen));
            this.okBtn = new System.Windows.Forms.Button();
            this.lblTicketMsg = new System.Windows.Forms.Label();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // okBtn
            // 
            this.okBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.okBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.okBtn.ForeColor = System.Drawing.Color.Snow;
            this.okBtn.Location = new System.Drawing.Point(143, 78);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(147, 43);
            this.okBtn.TabIndex = 2;
            this.okBtn.Text = "OK";
            this.okBtn.UseVisualStyleBackColor = false;
            this.okBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblTicketMsg
            // 
            this.lblTicketMsg.AllowDrop = true;
            this.lblTicketMsg.AutoSize = true;
            this.lblTicketMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTicketMsg.Location = new System.Drawing.Point(12, 22);
            this.lblTicketMsg.Name = "lblTicketMsg";
            this.lblTicketMsg.Size = new System.Drawing.Size(449, 22);
            this.lblTicketMsg.TabIndex = 3;
            this.lblTicketMsg.Text = "There is already an open ticket for this customer:";
            this.lblTicketMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.AllowDrop = true;
            this.lblCustomerName.AutoSize = true;
            this.lblCustomerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerName.Location = new System.Drawing.Point(42, 44);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(396, 22);
            this.lblCustomerName.TabIndex = 4;
            this.lblCustomerName.Text = "{Cutomer LastName},{Customer FirstName}";
            this.lblCustomerName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCustomerName.Click += new System.EventHandler(this.lblCustomerName_Click);
            // 
            // frmIsTicketOpen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(255)))), ((int)(((byte)(223)))));
            this.ClientSize = new System.Drawing.Size(446, 127);
            this.Controls.Add(this.lblCustomerName);
            this.Controls.Add(this.lblTicketMsg);
            this.Controls.Add(this.okBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmIsTicketOpen";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ticket";
            this.Load += new System.EventHandler(this.frmIsTicketOpen_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okBtn;
        private System.Windows.Forms.Label lblTicketMsg;
        private System.Windows.Forms.Label lblCustomerName;
    }
}