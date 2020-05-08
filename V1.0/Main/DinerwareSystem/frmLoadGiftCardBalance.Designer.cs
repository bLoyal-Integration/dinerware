namespace DinerwareSystem
{
    partial class frmLoadGiftCardBalance
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLoadGiftCardBalance));
            this.txtGiftCardNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.applyBtn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblEmptyCardNumberError = new System.Windows.Forms.Label();
            this.Cancel = new System.Windows.Forms.Button();
            this.btnKeyboard = new System.Windows.Forms.Button();
            this.productNameLbl = new System.Windows.Forms.Label();
            this.productPriceLbl = new System.Windows.Forms.Label();
            this.priceLabel = new System.Windows.Forms.Label();
            this.productnameLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtGiftCardNumber
            // 
            this.txtGiftCardNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGiftCardNumber.Location = new System.Drawing.Point(445, 69);
            this.txtGiftCardNumber.Name = "txtGiftCardNumber";
            this.txtGiftCardNumber.Size = new System.Drawing.Size(251, 23);
            this.txtGiftCardNumber.TabIndex = 55;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(442, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 17);
            this.label1.TabIndex = 54;
            this.label1.Text = "Gift Card Number";
            // 
            // applyBtn
            // 
            this.applyBtn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.applyBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.applyBtn.ForeColor = System.Drawing.Color.Snow;
            this.applyBtn.Location = new System.Drawing.Point(315, 122);
            this.applyBtn.Name = "applyBtn";
            this.applyBtn.Size = new System.Drawing.Size(124, 46);
            this.applyBtn.TabIndex = 56;
            this.applyBtn.Text = "Submit";
            this.applyBtn.UseVisualStyleBackColor = false;
            this.applyBtn.Click += new System.EventHandler(this.applyBtn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(113, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(476, 20);
            this.label2.TabIndex = 57;
            this.label2.Text = "Please enter gift card number to load Gift Card balance";
            // 
            // lblEmptyCardNumberError
            // 
            this.lblEmptyCardNumberError.AutoSize = true;
            this.lblEmptyCardNumberError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEmptyCardNumberError.ForeColor = System.Drawing.Color.Red;
            this.lblEmptyCardNumberError.Location = new System.Drawing.Point(442, 95);
            this.lblEmptyCardNumberError.Name = "lblEmptyCardNumberError";
            this.lblEmptyCardNumberError.Size = new System.Drawing.Size(34, 13);
            this.lblEmptyCardNumberError.TabIndex = 58;
            this.lblEmptyCardNumberError.Text = "Error";
            this.lblEmptyCardNumberError.Click += new System.EventHandler(this.lblEmptyCardNumberError_Click);
            // 
            // Cancel
            // 
            this.Cancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.Cancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Cancel.ForeColor = System.Drawing.Color.Snow;
            this.Cancel.Location = new System.Drawing.Point(445, 122);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(124, 46);
            this.Cancel.TabIndex = 59;
            this.Cancel.Text = "Cancel ";
            this.Cancel.UseVisualStyleBackColor = false;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // btnKeyboard
            // 
            this.btnKeyboard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(98)))), ((int)(((byte)(86)))), ((int)(((byte)(241)))));
            this.btnKeyboard.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnKeyboard.ForeColor = System.Drawing.Color.Snow;
            this.btnKeyboard.Location = new System.Drawing.Point(575, 122);
            this.btnKeyboard.Name = "btnKeyboard";
            this.btnKeyboard.Size = new System.Drawing.Size(124, 46);
            this.btnKeyboard.TabIndex = 60;
            this.btnKeyboard.Text = "Keyboard";
            this.btnKeyboard.UseVisualStyleBackColor = false;
            this.btnKeyboard.Click += new System.EventHandler(this.btnKeyboard_Click);
            // 
            // productNameLbl
            // 
            this.productNameLbl.AutoSize = true;
            this.productNameLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.productNameLbl.Location = new System.Drawing.Point(27, 75);
            this.productNameLbl.Name = "productNameLbl";
            this.productNameLbl.Size = new System.Drawing.Size(98, 17);
            this.productNameLbl.TabIndex = 61;
            this.productNameLbl.Text = "Product Name";
            // 
            // productPriceLbl
            // 
            this.productPriceLbl.AutoSize = true;
            this.productPriceLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.productPriceLbl.Location = new System.Drawing.Point(312, 75);
            this.productPriceLbl.Name = "productPriceLbl";
            this.productPriceLbl.Size = new System.Drawing.Size(40, 17);
            this.productPriceLbl.TabIndex = 62;
            this.productPriceLbl.Text = "Price";
            // 
            // priceLabel
            // 
            this.priceLabel.AutoSize = true;
            this.priceLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.priceLabel.Location = new System.Drawing.Point(312, 49);
            this.priceLabel.Name = "priceLabel";
            this.priceLabel.Size = new System.Drawing.Size(45, 17);
            this.priceLabel.TabIndex = 63;
            this.priceLabel.Text = "Price";
            // 
            // productnameLabel
            // 
            this.productnameLabel.AutoSize = true;
            this.productnameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.productnameLabel.Location = new System.Drawing.Point(27, 49);
            this.productnameLabel.Name = "productnameLabel";
            this.productnameLabel.Size = new System.Drawing.Size(110, 17);
            this.productnameLabel.TabIndex = 64;
            this.productnameLabel.Text = "Product Name";
            // 
            // frmLoadGiftCardBalance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(255)))), ((int)(((byte)(223)))));
            this.ClientSize = new System.Drawing.Size(708, 173);
            this.Controls.Add(this.productnameLabel);
            this.Controls.Add(this.priceLabel);
            this.Controls.Add(this.productPriceLbl);
            this.Controls.Add(this.productNameLbl);
            this.Controls.Add(this.btnKeyboard);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.lblEmptyCardNumberError);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.applyBtn);
            this.Controls.Add(this.txtGiftCardNumber);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmLoadGiftCardBalance";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Load Gift Card";
            this.Load += new System.EventHandler(this.frmLoadGiftCardBalance_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtGiftCardNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button applyBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblEmptyCardNumberError;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Button btnKeyboard;
        private System.Windows.Forms.Label productNameLbl;
        private System.Windows.Forms.Label productPriceLbl;
        private System.Windows.Forms.Label priceLabel;
        private System.Windows.Forms.Label productnameLabel;
    }
}