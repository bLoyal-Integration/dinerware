namespace ConfigApp
{
    partial class DisableEnablebLoyalFunctionality
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DisableEnablebLoyalFunctionality));
            this.lblMsg = new System.Windows.Forms.Label();
            this.yesBtn = new System.Windows.Forms.Button();
            this.nobtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.Location = new System.Drawing.Point(23, 27);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(671, 22);
            this.lblMsg.TabIndex = 5;
            this.lblMsg.Text = "bLoyal functionality is currently disabled. Would you like to enable it now?";
            // 
            // yesBtn
            // 
            this.yesBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.yesBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.yesBtn.ForeColor = System.Drawing.Color.Snow;
            this.yesBtn.Location = new System.Drawing.Point(166, 67);
            this.yesBtn.Name = "yesBtn";
            this.yesBtn.Size = new System.Drawing.Size(151, 43);
            this.yesBtn.TabIndex = 6;
            this.yesBtn.Text = "Yes";
            this.yesBtn.UseVisualStyleBackColor = false;
            this.yesBtn.Click += new System.EventHandler(this.yesBtn_Click);
            // 
            // nobtn
            // 
            this.nobtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.nobtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.nobtn.ForeColor = System.Drawing.Color.Snow;
            this.nobtn.Location = new System.Drawing.Point(345, 67);
            this.nobtn.Name = "nobtn";
            this.nobtn.Size = new System.Drawing.Size(151, 43);
            this.nobtn.TabIndex = 7;
            this.nobtn.Text = "No";
            this.nobtn.UseVisualStyleBackColor = false;
            this.nobtn.Click += new System.EventHandler(this.nobtn_Click);
            // 
            // DisableEnablebLoyalFunctionality
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(255)))), ((int)(((byte)(223)))));
            this.ClientSize = new System.Drawing.Size(722, 130);
            this.Controls.Add(this.nobtn);
            this.Controls.Add(this.yesBtn);
            this.Controls.Add(this.lblMsg);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DisableEnablebLoyalFunctionality";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Disable/Enable bLoyal Functionality";
            this.Load += new System.EventHandler(this.DisableEnablebLoyalFunctionality_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Button yesBtn;
        private System.Windows.Forms.Button nobtn;
    }
}