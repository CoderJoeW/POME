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

namespace POME {
    public partial class Main : Form { 
        private string host = "";
        private int port = 0;
        private string username = "";
        private string password = "";

        private readonly ErrorWindow error_window = new ErrorWindow();

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
            txtHost.Focus();
            sshTerminalControl1.AllowCopyingToClipboard = true;
            sshTerminalControl1.AllowPastingFromClipboard = true;
            RegisterPlaceholderTextboxEvents();
        }

        /*
        --------------------------------
                BEGIN METHODS
        --------------------------------
        */

        #region Private
        private void ConnectToSSH() {
            if (host == "") {
                host = txtHost.Text;
                port = int.Parse(txtPort.Text);
                username = txtUsername.Text;
                password = txtPassword.Text;
            }

            try {
                sshTerminalControl1.Connect(host, port);
                sshTerminalControl1.Authenticate(username, password);
                tblConnectionInfo.Visible = false;
                sshTerminalControl1.Focus();
                this.Text = host;
            }
            catch (Exception e) {
                DisplayError(e.Message);
                //sshTerminalControl1.Disconnect();
            }
        }

        private void DisplayError(string msg) {
            error_window.SetErrorMessage(msg);
            error_window.Show();
        }
        #endregion

        #region Public

        #endregion

        /*
        --------------------------------
                END METHODS
        --------------------------------
        */

        /*
        --------------------------------
                BEGIN LISTENERS
        --------------------------------
        */

        #region Private
        private void button1_Click(object sender, EventArgs e) {
            ConnectToSSH();
        }

        private void form_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                ConnectToSSH();
            }
        }

        private void ShowExtraFunctionsMenu(object sender, System.Windows.Forms.KeyEventArgs e) {
            if (InputManager.CheckControlDown() && InputManager.CheckShiftDown() && Keyboard.IsKeyDown(Key.N)) {
                DuplicateSession();
            }
        }

        private void RegisterPlaceholderTextboxEvents() {
            txtHost.GotFocus += PlaceholderRemoveText;
            txtHost.LostFocus += PlaceholderAddText;

            txtUsername.GotFocus += PlaceholderRemoveText;
            txtUsername.LostFocus += PlaceholderAddText;

            txtPassword.GotFocus += PlaceholderRemoveText;
            txtPassword.LostFocus += PlaceholderAddText;

            txtPort.GotFocus += PlaceholderRemoveText;
            txtPort.LostFocus += PlaceholderAddText;
        }
        #endregion

        #region Public
        public void PlaceholderRemoveText(object sender, EventArgs e) {
            TextBox tx = sender as TextBox;
            tx.Text = "";
        }

        public void PlaceholderAddText(object sender,EventArgs e) {
            TextBox tx = sender as TextBox;
            if (string.IsNullOrWhiteSpace(tx.Text)) {
                switch (tx.Name) {
                    case "hostInfo":
                        tx.Text = "Host";
                        break;
                    case "usernameInfo":
                        tx.Text = "Username";
                        break;
                    case "passwordInfo":
                        tx.Text = "Password";
                        break;
                    case "portInfo":
                        tx.Text = "Port";
                        break;
                    default:
                        tx.Text = "";
                        break;
                }
            }
        }
        #endregion

        /*
        --------------------------------
                END LISTENERS
        --------------------------------
        */

        /*
        --------------------------------
                BEGIN COMMANDS
        --------------------------------
        */

        #region Private
        private void DuplicateSession() {
            string args = host + "|" + port + "|" + username + "|" + password;
            Main newMain = new Main(args);
            Program.container.Tabs.Add(new TitleBarTab(Program.container) {

                Content = newMain
            });
            Program.container.SelectedTabIndex += 1;
        }
        #endregion

        #region Public

        #endregion

        /*
        --------------------------------
                END COMMANDS
        --------------------------------
        */
    }
}
