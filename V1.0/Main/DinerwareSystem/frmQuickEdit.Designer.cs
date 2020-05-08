namespace DinerwareSystem
{
    partial class frmQuickEdit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmQuickEdit));
            this.QuickEditWebBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // QuickEditWebBrowser
            // 
            this.QuickEditWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuickEditWebBrowser.Location = new System.Drawing.Point(0, 0);
            this.QuickEditWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.QuickEditWebBrowser.Name = "QuickEditWebBrowser";
            this.QuickEditWebBrowser.Size = new System.Drawing.Size(941, 520);
            this.QuickEditWebBrowser.TabIndex = 1;
            // 
            // frmQuickEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(941, 520);
            this.Controls.Add(this.QuickEditWebBrowser);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmQuickEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quick Edit";
            this.Load += new System.EventHandler(this.frmQuickEdit_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser QuickEditWebBrowser;
    }
}