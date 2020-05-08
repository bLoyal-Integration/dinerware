namespace DinerwareSystem
{
    partial class frmApplyCoupon
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmApplyCoupon));
            this.applyCouponWebBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // applyCouponWebBrowser
            // 
            this.applyCouponWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.applyCouponWebBrowser.Location = new System.Drawing.Point(0, 0);
            this.applyCouponWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.applyCouponWebBrowser.Name = "applyCouponWebBrowser";
            this.applyCouponWebBrowser.Size = new System.Drawing.Size(989, 542);
            this.applyCouponWebBrowser.TabIndex = 0;
            this.applyCouponWebBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // frmApplyCoupon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 542);
            this.Controls.Add(this.applyCouponWebBrowser);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmApplyCoupon";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Apply Coupon";
            this.Load += new System.EventHandler(this.frmApplyCoupon_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser applyCouponWebBrowser;
    }
}