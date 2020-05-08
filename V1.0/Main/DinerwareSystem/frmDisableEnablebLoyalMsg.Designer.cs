namespace DinerwareSystem
{
    partial class frmDisableEnablebLoyalMsg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDisableEnablebLoyalMsg));
            this.lblMsg = new System.Windows.Forms.Label();
            this.okbtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.Location = new System.Drawing.Point(27, 20);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(686, 22);
            this.lblMsg.TabIndex = 6;
            this.lblMsg.Text = "bLoyal functionality is currently disabled. Please select enable radio button.";
            // 
            // okbtn
            // 
            this.okbtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.okbtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.okbtn.ForeColor = System.Drawing.Color.Snow;
            this.okbtn.Location = new System.Drawing.Point(287, 54);
            this.okbtn.Name = "okbtn";
            this.okbtn.Size = new System.Drawing.Size(151, 43);
            this.okbtn.TabIndex = 8;
            this.okbtn.Text = "OK";
            this.okbtn.UseVisualStyleBackColor = false;
            this.okbtn.Click += new System.EventHandler(this.okbtn_Click);
            // 
            // frmDisableEnablebLoyalMsg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(255)))), ((int)(((byte)(223)))));
            this.ClientSize = new System.Drawing.Size(738, 116);
            this.Controls.Add(this.okbtn);
            this.Controls.Add(this.lblMsg);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmDisableEnablebLoyalMsg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmDisableEnablebLoyalMsg";
            this.Load += new System.EventHandler(this.frmDisableEnablebLoyalMsg_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.Button okbtn;
    }
}