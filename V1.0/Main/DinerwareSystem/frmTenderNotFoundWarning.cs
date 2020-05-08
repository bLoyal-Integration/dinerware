using System;
using System.Windows.Forms;

namespace DinerwareSystem
{
    public partial class frmTenderNotFoundWarning : Form
    {
        string _msg;

        public frmTenderNotFoundWarning()
        {
            InitializeComponent();
        }

        public frmTenderNotFoundWarning(string msg)
        {
            _msg = msg;
            InitializeComponent();
        }

        private void frmTenderNotFoundWarning_Load(object sender, EventArgs e)
        {
            msglbl.Text = _msg;
        }

        private void okBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
