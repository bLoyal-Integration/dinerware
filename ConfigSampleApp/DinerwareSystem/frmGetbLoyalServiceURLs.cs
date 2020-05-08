using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConfigApp
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
