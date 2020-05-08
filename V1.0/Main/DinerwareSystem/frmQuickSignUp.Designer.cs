namespace DinerwareSystem
{
    partial class frmQuickSignUp
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmQuickSignUp));
            this.QuickSignUPWebBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // QuickSignUPWebBrowser
            // 
            this.QuickSignUPWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.QuickSignUPWebBrowser.Location = new System.Drawing.Point(0, 0);
            this.QuickSignUPWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.QuickSignUPWebBrowser.Name = "QuickSignUPWebBrowser";
            this.QuickSignUPWebBrowser.Size = new System.Drawing.Size(1006, 584);
            this.QuickSignUPWebBrowser.TabIndex = 0;
            this.QuickSignUPWebBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.webBrowser1_DocumentCompleted);
            // 
            // frmQuickSignUp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 584);
            this.Controls.Add(this.QuickSignUPWebBrowser);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmQuickSignUp";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quick Sign Up";
            this.Load += new System.EventHandler(this.frmQuickSignUp_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.WebBrowser QuickSignUPWebBrowser;
    }
}