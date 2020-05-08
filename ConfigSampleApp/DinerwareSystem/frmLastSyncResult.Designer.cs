namespace ConfigApp
{
    partial class frmLastSyncResult
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLastSyncResult));
            this.txtLastSyncResult = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtLastSyncResult
            // 
            this.txtLastSyncResult.Location = new System.Drawing.Point(13, 13);
            this.txtLastSyncResult.Multiline = true;
            this.txtLastSyncResult.Name = "txtLastSyncResult";
            this.txtLastSyncResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLastSyncResult.Size = new System.Drawing.Size(852, 514);
            this.txtLastSyncResult.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.BackColor = System.Drawing.Color.Green;
            this.btnOk.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOk.ForeColor = System.Drawing.Color.Snow;
            this.btnOk.Location = new System.Drawing.Point(753, 534);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(108, 46);
            this.btnOk.TabIndex = 49;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = false;
            this.btnOk.Click += new System.EventHandler(this.btnLastSyncResult_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.Green;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.ForeColor = System.Drawing.Color.Snow;
            this.btnClear.Location = new System.Drawing.Point(639, 534);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(108, 46);
            this.btnClear.TabIndex = 51;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // frmLastSyncResult
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 586);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtLastSyncResult);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(889, 624);
            this.MinimumSize = new System.Drawing.Size(889, 624);
            this.Name = "frmLastSyncResult";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Last Sync Result";
            this.Load += new System.EventHandler(this.frmLastSyncResult_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLastSyncResult;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnClear;
    }
}