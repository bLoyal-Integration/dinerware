namespace DinerwareSystem
{
    partial class bLoyalLoyaltyTender
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(bLoyalLoyaltyTender));
            this.applyBtn = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.checkBalanceBtn = new System.Windows.Forms.Button();
            this.lblApplyBalance = new System.Windows.Forms.Label();
            this.textApplyBalance = new System.Windows.Forms.TextBox();
            this.lblAvailableBalance = new System.Windows.Forms.Label();
            this.labAvailableBalanceVal = new System.Windows.Forms.Label();
            this.lblApplyBalanceError = new System.Windows.Forms.Label();
            this.lblCustomerNotAssigntoTicket = new System.Windows.Forms.Label();
            this.btnkeyboard = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // applyBtn
            // 
            this.applyBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.applyBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.applyBtn.ForeColor = System.Drawing.Color.Snow;
            this.applyBtn.Location = new System.Drawing.Point(471, 61);
            this.applyBtn.Name = "applyBtn";
            this.applyBtn.Size = new System.Drawing.Size(149, 49);
            this.applyBtn.TabIndex = 45;
            this.applyBtn.Text = "Apply";
            this.applyBtn.UseVisualStyleBackColor = false;
            this.applyBtn.Click += new System.EventHandler(this.applyBtn_Click);
            // 
            // Cancel
            // 
            this.Cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancel.ForeColor = System.Drawing.Color.Snow;
            this.Cancel.Location = new System.Drawing.Point(471, 116);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(149, 49);
            this.Cancel.TabIndex = 44;
            this.Cancel.Text = "Cancel ";
            this.Cancel.UseVisualStyleBackColor = false;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // checkBalanceBtn
            // 
            this.checkBalanceBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.checkBalanceBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.checkBalanceBtn.ForeColor = System.Drawing.Color.Snow;
            this.checkBalanceBtn.Location = new System.Drawing.Point(471, 6);
            this.checkBalanceBtn.Name = "checkBalanceBtn";
            this.checkBalanceBtn.Size = new System.Drawing.Size(149, 49);
            this.checkBalanceBtn.TabIndex = 41;
            this.checkBalanceBtn.Text = "Check Balance";
            this.checkBalanceBtn.UseVisualStyleBackColor = false;
            this.checkBalanceBtn.Click += new System.EventHandler(this.checkBalanceBtn_Click);
            // 
            // lblApplyBalance
            // 
            this.lblApplyBalance.AutoSize = true;
            this.lblApplyBalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApplyBalance.Location = new System.Drawing.Point(51, 49);
            this.lblApplyBalance.Name = "lblApplyBalance";
            this.lblApplyBalance.Size = new System.Drawing.Size(186, 17);
            this.lblApplyBalance.TabIndex = 48;
            this.lblApplyBalance.Text = "Amount Being Applied $:";
            // 
            // textApplyBalance
            // 
            this.textApplyBalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textApplyBalance.Location = new System.Drawing.Point(240, 46);
            this.textApplyBalance.Name = "textApplyBalance";
            this.textApplyBalance.Size = new System.Drawing.Size(144, 23);
            this.textApplyBalance.TabIndex = 49;
            // 
            // lblAvailableBalance
            // 
            this.lblAvailableBalance.AutoSize = true;
            this.lblAvailableBalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvailableBalance.Location = new System.Drawing.Point(95, 83);
            this.lblAvailableBalance.Name = "lblAvailableBalance";
            this.lblAvailableBalance.Size = new System.Drawing.Size(142, 17);
            this.lblAvailableBalance.TabIndex = 50;
            this.lblAvailableBalance.Text = "Available Balance:";
            // 
            // labAvailableBalanceVal
            // 
            this.labAvailableBalanceVal.AutoSize = true;
            this.labAvailableBalanceVal.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labAvailableBalanceVal.Location = new System.Drawing.Point(237, 83);
            this.labAvailableBalanceVal.Name = "labAvailableBalanceVal";
            this.labAvailableBalanceVal.Size = new System.Drawing.Size(0, 17);
            this.labAvailableBalanceVal.TabIndex = 51;
            // 
            // lblApplyBalanceError
            // 
            this.lblApplyBalanceError.AutoSize = true;
            this.lblApplyBalanceError.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApplyBalanceError.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblApplyBalanceError.Location = new System.Drawing.Point(12, 116);
            this.lblApplyBalanceError.Name = "lblApplyBalanceError";
            this.lblApplyBalanceError.Size = new System.Drawing.Size(142, 17);
            this.lblApplyBalanceError.TabIndex = 52;
            this.lblApplyBalanceError.Text = "Available Balance:";
            // 
            // lblCustomerNotAssigntoTicket
            // 
            this.lblCustomerNotAssigntoTicket.AutoSize = true;
            this.lblCustomerNotAssigntoTicket.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerNotAssigntoTicket.ForeColor = System.Drawing.Color.OrangeRed;
            this.lblCustomerNotAssigntoTicket.Location = new System.Drawing.Point(12, 229);
            this.lblCustomerNotAssigntoTicket.Name = "lblCustomerNotAssigntoTicket";
            this.lblCustomerNotAssigntoTicket.Size = new System.Drawing.Size(403, 17);
            this.lblCustomerNotAssigntoTicket.TabIndex = 53;
            this.lblCustomerNotAssigntoTicket.Text = "Please assign customer to ticket using bLoyal snippet.";
            // 
            // btnkeyboard
            // 
            this.btnkeyboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.btnkeyboard.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnkeyboard.ForeColor = System.Drawing.Color.Snow;
            this.btnkeyboard.Location = new System.Drawing.Point(471, 171);
            this.btnkeyboard.Name = "btnkeyboard";
            this.btnkeyboard.Size = new System.Drawing.Size(149, 49);
            this.btnkeyboard.TabIndex = 58;
            this.btnkeyboard.Text = "Keyboard";
            this.btnkeyboard.UseVisualStyleBackColor = false;
            this.btnkeyboard.Click += new System.EventHandler(this.btnkeyboard_Click);
            // 
            // bLoyalLoyaltyTender
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(255)))), ((int)(((byte)(223)))));
            this.ClientSize = new System.Drawing.Size(627, 252);
            this.Controls.Add(this.btnkeyboard);
            this.Controls.Add(this.lblCustomerNotAssigntoTicket);
            this.Controls.Add(this.lblApplyBalanceError);
            this.Controls.Add(this.labAvailableBalanceVal);
            this.Controls.Add(this.lblAvailableBalance);
            this.Controls.Add(this.textApplyBalance);
            this.Controls.Add(this.lblApplyBalance);
            this.Controls.Add(this.applyBtn);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.checkBalanceBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "bLoyalLoyaltyTender";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "bLoyal Loyalty Tender";
            this.Load += new System.EventHandler(this.bLoyalLoyaltyTender_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button applyBtn;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button checkBalanceBtn;
        private System.Windows.Forms.Label lblApplyBalance;
        private System.Windows.Forms.TextBox textApplyBalance;
        private System.Windows.Forms.Label lblAvailableBalance;
        private System.Windows.Forms.Label labAvailableBalanceVal;
        private System.Windows.Forms.Label lblApplyBalanceError;
        private System.Windows.Forms.Label lblCustomerNotAssigntoTicket;
        private System.Windows.Forms.Button btnkeyboard;
    }
}