namespace DinerwareSystem
{
    partial class frmConfigurationSettingsWarning
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmConfigurationSettingsWarning));
            this.msglbl = new System.Windows.Forms.Label();
            this.okBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // msglbl
            // 
            this.msglbl.AutoSize = true;
            this.msglbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.msglbl.Location = new System.Drawing.Point(38, 9);
            this.msglbl.Name = "msglbl";
            this.msglbl.Size = new System.Drawing.Size(709, 17);
            this.msglbl.TabIndex = 11;
            this.msglbl.Text = "Please configure bLoyal and Dinerware settings as per setup guide using Master Se" +
    "ttings Form. ";
            // 
            // okBtn
            // 
            this.okBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.okBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.okBtn.ForeColor = System.Drawing.SystemColors.Control;
            this.okBtn.Location = new System.Drawing.Point(327, 39);
            this.okBtn.Name = "okBtn";
            this.okBtn.Size = new System.Drawing.Size(120, 46);
            this.okBtn.TabIndex = 12;
            this.okBtn.Text = "Ok";
            this.okBtn.UseVisualStyleBackColor = false;
            this.okBtn.Click += new System.EventHandler(this.okBtn_Click);
            // 
            // frmConfigurationSettingsWarning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 96);
            this.Controls.Add(this.okBtn);
            this.Controls.Add(this.msglbl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(792, 135);
            this.MinimumSize = new System.Drawing.Size(792, 135);
            this.Name = "frmConfigurationSettingsWarning";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configuration Settings Warning";
            this.Load += new System.EventHandler(this.frmConfigurationSettingsWarning_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label msglbl;
        private System.Windows.Forms.Button okBtn;
    }
}