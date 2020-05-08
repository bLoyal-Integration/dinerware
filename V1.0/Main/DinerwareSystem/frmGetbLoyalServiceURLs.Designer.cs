namespace DinerwareSystem
{
    partial class frmGetbLoyalServiceURLs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGetbLoyalServiceURLs));
            this.tbResults = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbResults
            // 
            this.tbResults.Location = new System.Drawing.Point(12, 12);
            this.tbResults.Multiline = true;
            this.tbResults.Name = "tbResults";
            this.tbResults.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbResults.Size = new System.Drawing.Size(421, 275);
            this.tbResults.TabIndex = 1;
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.btnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.ForeColor = System.Drawing.Color.Snow;
            this.btnOk.Location = new System.Drawing.Point(156, 293);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(143, 53);
            this.btnOk.TabIndex = 37;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // frmGetbLoyalServiceURLs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(255)))), ((int)(((byte)(223)))));
            this.ClientSize = new System.Drawing.Size(442, 358);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tbResults);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmGetbLoyalServiceURLs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Get bLoyal Service URLs";
            this.Load += new System.EventHandler(this.frmGetbLoyalServiceURLs_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbResults;
        private System.Windows.Forms.Button btnOk;
    }
}