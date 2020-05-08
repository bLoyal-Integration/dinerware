namespace DinerwareSystem
{
    partial class frmFindCustomer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFindCustomer));
            this.findCustomerWebBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // findCustomerWebBrowser
            // 
            this.findCustomerWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.findCustomerWebBrowser.Location = new System.Drawing.Point(0, 0);
            this.findCustomerWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.findCustomerWebBrowser.Name = "findCustomerWebBrowser";
            this.findCustomerWebBrowser.Size = new System.Drawing.Size(1003, 586);
            this.findCustomerWebBrowser.TabIndex = 0;
            this.findCustomerWebBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // frmFindCustomer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1003, 586);
            this.Controls.Add(this.findCustomerWebBrowser);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmFindCustomer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Find Customer";
            this.Load += new System.EventHandler(this.frmFindCustomer_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser findCustomerWebBrowser;
    }
}

