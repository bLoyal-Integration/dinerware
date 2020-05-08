using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DinerwareSystem
{
    public partial class frmGetbLoyalServiceURLs : Form
    {
        private List<string> _serviceUrls;

        public frmGetbLoyalServiceURLs()
        {
            InitializeComponent();
        }

        public frmGetbLoyalServiceURLs(List<string> serviceUrls)
        {
            InitializeComponent();
            _serviceUrls = serviceUrls;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmGetbLoyalServiceURLs_Load(object sender, EventArgs e)
        {
            tbResults.Lines = _serviceUrls.ToArray();
        }
    }
}
