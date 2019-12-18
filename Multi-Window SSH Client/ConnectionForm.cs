using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Multi_Window_SSH_Client {
    public partial class ConnectionForm : Form {
        public delegate void ClickButton();
        public event ClickButton ButtonWasClicked;
        public ConnectionForm() {
            InitializeComponent();
        }

        private void ConnectionForm_Load(object sender, EventArgs e) {
            
        }

        public string GetHost() {
            return hostInfo.Text;
        }

        public string GetUsername() {
            return usernameInfo.Text;
        }

        public string GetPassword() {
            return passwordInfo.Text;
        }

        public int GetPort() {
            return int.Parse(portInfo.Text);
        }

        private void button1_Click(object sender, EventArgs e) {
            Console.WriteLine("Button clicked");
        }
    }
}
