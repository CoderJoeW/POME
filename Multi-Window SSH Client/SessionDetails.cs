using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POME
{
    public partial class SessionDetails : Form
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public SessionDetails()
        {
            InitializeComponent();
        }

        private void btnSubmitSessionDetails_Click(object sender, EventArgs e)
        {
            Host = txtHost.Text;
            Port = int.Parse(txtPort.Text);
            Username = txtUsername.Text;
            Password = txtPassword.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
