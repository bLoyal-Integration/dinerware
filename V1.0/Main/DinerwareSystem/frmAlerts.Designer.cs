namespace DinerwareSystem
{
    partial class frmAlerts
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAlerts));
            this.alertsWebBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // alertsWebBrowser
            // 
            this.alertsWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.alertsWebBrowser.Location = new System.Drawing.Point(0, 0);
            this.alertsWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.alertsWebBrowser.Name = "alertsWebBrowser";
            this.alertsWebBrowser.Size = new System.Drawing.Size(989, 573);
            this.alertsWebBrowser.TabIndex = 1;
            this.alertsWebBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.applyCouponWebBrowser_DocumentCompleted);
            // 
            // frmAlerts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 573);
            this.Controls.Add(this.alertsWebBrowser);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmAlerts";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Alerts";
            this.Load += new System.EventHandler(this.Alerts_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser alertsWebBrowser;
    }
}