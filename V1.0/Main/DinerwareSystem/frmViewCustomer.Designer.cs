namespace DinerwareSystem
{
    partial class frmViewCustomer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmViewCustomer));
            this.viewCustomerWebBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // viewCustomerWebBrowser
            // 
            this.viewCustomerWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewCustomerWebBrowser.Location = new System.Drawing.Point(0, 0);
            this.viewCustomerWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.viewCustomerWebBrowser.Name = "viewCustomerWebBrowser";
            this.viewCustomerWebBrowser.Size = new System.Drawing.Size(957, 602);
            this.viewCustomerWebBrowser.TabIndex = 0;
            this.viewCustomerWebBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // frmViewCustomer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(957, 602);
            this.Controls.Add(this.viewCustomerWebBrowser);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmViewCustomer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "View Customer";
            this.Load += new System.EventHandler(this.frmViewCustomer_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser viewCustomerWebBrowser;

    }
}