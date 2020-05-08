namespace ConfigApp
{
    partial class GenerateAccessKey
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenerateAccessKey));
            this.btnGenerateAccessKey = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.lblLoyaltyengine = new System.Windows.Forms.Label();
            this.txtLoginDomain = new System.Windows.Forms.TextBox();
            this.Acc = new System.Windows.Forms.Label();
            this.txtConnectorKey = new System.Windows.Forms.TextBox();
            this.lblApiKey = new System.Windows.Forms.Label();
            this.txtApiKey = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnGenerateAccessKey
            // 
            this.btnGenerateAccessKey.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.btnGenerateAccessKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.btnGenerateAccessKey.ForeColor = System.Drawing.Color.Snow;
            this.btnGenerateAccessKey.Location = new System.Drawing.Point(151, 165);
            this.btnGenerateAccessKey.Name = "btnGenerateAccessKey";
            this.btnGenerateAccessKey.Size = new System.Drawing.Size(149, 53);
            this.btnGenerateAccessKey.TabIndex = 42;
            this.btnGenerateAccessKey.Text = "Generate Access Key";
            this.btnGenerateAccessKey.UseVisualStyleBackColor = false;
            this.btnGenerateAccessKey.Click += new System.EventHandler(this.btnGenerateAccessKey_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.Color.Snow;
            this.button1.Location = new System.Drawing.Point(310, 165);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(131, 53);
            this.button1.TabIndex = 43;
            this.button1.Text = "Cancel ";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblLoyaltyengine
            // 
            this.lblLoyaltyengine.AutoSize = true;
            this.lblLoyaltyengine.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoyaltyengine.Location = new System.Drawing.Point(33, 18);
            this.lblLoyaltyengine.Name = "lblLoyaltyengine";
            this.lblLoyaltyengine.Size = new System.Drawing.Size(112, 17);
            this.lblLoyaltyengine.TabIndex = 44;
            this.lblLoyaltyengine.Text = "Login Domain:";
            // 
            // txtLoginDomain
            // 
            this.txtLoginDomain.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLoginDomain.Location = new System.Drawing.Point(151, 15);
            this.txtLoginDomain.Name = "txtLoginDomain";
            this.txtLoginDomain.Size = new System.Drawing.Size(290, 23);
            this.txtLoginDomain.TabIndex = 45;
            // 
            // Acc
            // 
            this.Acc.AutoSize = true;
            this.Acc.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Acc.Location = new System.Drawing.Point(33, 61);
            this.Acc.Name = "Acc";
            this.Acc.Size = new System.Drawing.Size(114, 17);
            this.Acc.TabIndex = 46;
            this.Acc.Text = "ConnectorKey:";
            // 
            // txtConnectorKey
            // 
            this.txtConnectorKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConnectorKey.Location = new System.Drawing.Point(151, 58);
            this.txtConnectorKey.Name = "txtConnectorKey";
            this.txtConnectorKey.Size = new System.Drawing.Size(290, 23);
            this.txtConnectorKey.TabIndex = 47;
            this.txtConnectorKey.UseSystemPasswordChar = true;
            // 
            // lblApiKey
            // 
            this.lblApiKey.AutoSize = true;
            this.lblApiKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApiKey.Location = new System.Drawing.Point(33, 105);
            this.lblApiKey.Name = "lblApiKey";
            this.lblApiKey.Size = new System.Drawing.Size(63, 17);
            this.lblApiKey.TabIndex = 48;
            this.lblApiKey.Text = "ApiKey:";
            // 
            // txtApiKey
            // 
            this.txtApiKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtApiKey.Location = new System.Drawing.Point(151, 99);
            this.txtApiKey.Name = "txtApiKey";
            this.txtApiKey.Size = new System.Drawing.Size(290, 23);
            this.txtApiKey.TabIndex = 49;
            this.txtApiKey.UseSystemPasswordChar = true;
            this.txtApiKey.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // GenerateAccessKey
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(255)))), ((int)(((byte)(223)))));
            this.ClientSize = new System.Drawing.Size(588, 262);
            this.Controls.Add(this.txtApiKey);
            this.Controls.Add(this.lblApiKey);
            this.Controls.Add(this.txtConnectorKey);
            this.Controls.Add(this.Acc);
            this.Controls.Add(this.txtLoginDomain);
            this.Controls.Add(this.lblLoyaltyengine);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnGenerateAccessKey);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GenerateAccessKey";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Generate AccessKey";
            this.Load += new System.EventHandler(this.GenerateAccessKey_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGenerateAccessKey;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblLoyaltyengine;
        private System.Windows.Forms.TextBox txtLoginDomain;
        private System.Windows.Forms.Label Acc;
        private System.Windows.Forms.TextBox txtConnectorKey;
        private System.Windows.Forms.Label lblApiKey;
        private System.Windows.Forms.TextBox txtApiKey;
    }
}