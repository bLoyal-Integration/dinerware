﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ConfigApp
{
    public partial class frmWarningMessage : Form
    {
        public frmWarningMessage()
        {
            InitializeComponent();
        }

        private void frmWarningMessage_Load(object sender, EventArgs e)
        {

        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
