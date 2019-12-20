using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using EasyTabs;

namespace Multi_Window_SSH_Client {
    public partial class Main : Form { 
        string host = "";
        int port = 0;
        string username = "";
        string password = "";

        protected TitleBarTabs ParentTabs {
            get {
                return (ParentForm as TitleBarTabs);
            }
        }

        public Main(string args) {
            InitializeComponent();
            host = args.Split('|')[0];
            port = int.Parse(args.Split('|')[1]);
            username = args.Split('|')[2];
            password = args.Split('|')[3];
            ConnectToSSH();
        }

        public Main() {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e) {
            hostInfo.Focus();
            sshTerminalControl1.AllowCopyingToClipboard = true;
            sshTerminalControl1.AllowPastingFromClipboard = true;
        }

        private void ConnectToSSH() {
            if(host == "") {
                host = hostInfo.Text;
                port = int.Parse(portInfo.Text);
                username = usernameInfo.Text;
                password = passwordInfo.Text;
            }

            try {
                sshTerminalControl1.Connect(host, port);
                sshTerminalControl1.Authenticate(username, password);
                panel1.Visible = false;
                sshTerminalControl1.Focus();
                this.Text = host;
            }catch(Exception e) {
                #if DEBUG
                Console.WriteLine("\n\n An SSH execepton has occureed:{0}", e.Message);
                #endif
                sshTerminalControl1.Disconnect();
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            ConnectToSSH();
        }

        private void form_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                ConnectToSSH();
            }
        }

        private void ShowExtraFunctionsMenu(object sender, System.Windows.Forms.KeyEventArgs e) {
            if(InputManager.CheckControlDown() && InputManager.CheckShiftDown() && Keyboard.IsKeyDown(Key.N)) {
                DuplicateSession();
            }
        }

        private void DuplicateSession() {
            string args = host + "|" + port + "|" + username + "|" + password;
            Main newMain = new Main(args);
            Program.container.Tabs.Add(new TitleBarTab(Program.container) {

                Content = newMain
            });
            Program.container.SelectedTabIndex += 1;
        }
    }
}
