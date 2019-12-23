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
using Renci.SshNet;

namespace POME {
    public partial class Main : Form { 
        private string host = "";
        private int port = 0;
        private string username = "";
        private string password = "";

        private SshClient ssh_client = null;
        private ShellStream shell_stream = null;

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

        /*
     --------------------------------
             BEGIN LISTENERS
     --------------------------------
     */

        #region Private
        private void Main_Load(object sender, EventArgs e) {
            System.Threading.ThreadStart thread_start = new System.Threading.ThreadStart(RecieveData);
            System.Threading.Thread thread = new System.Threading.Thread(thread_start);

            thread.IsBackground = true;
            thread.Start();

            hostInfo.Focus();
            RegisterPlaceholderTextboxEvents();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e) {
            this.shell_stream.Close();
            this.ssh_client.Disconnect();
        }

        private void button1_Click(object sender, EventArgs e) {
            ConnectToSSH();
        }

        private void form_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter) {
                ConnectToSSH();
            }
        }

        private void commandInput_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
            try {
                if (e.KeyCode == Keys.Enter) {
                    this.shell_stream.Write(commandInput.Text + ";\r\n");
                    this.shell_stream.Flush();
                }
            }
            catch (Exception exc) {
                throw exc;
            }
        }

        private void ShowExtraFunctionsMenu(object sender, System.Windows.Forms.KeyEventArgs e) {
            if (InputManager.CheckControlDown() && InputManager.CheckShiftDown() && Keyboard.IsKeyDown(Key.N)) {
                DuplicateSession();
            }
        }

        private void RegisterPlaceholderTextboxEvents() {
            hostInfo.GotFocus += PlaceholderRemoveText;
            hostInfo.LostFocus += PlaceholderAddText;

            usernameInfo.GotFocus += PlaceholderRemoveText;
            usernameInfo.LostFocus += PlaceholderAddText;

            passwordInfo.GotFocus += PlaceholderRemoveText;
            passwordInfo.LostFocus += PlaceholderAddText;

            portInfo.GotFocus += PlaceholderRemoveText;
            portInfo.LostFocus += PlaceholderAddText;
        }
        #endregion

        #region Public
        public void PlaceholderRemoveText(object sender, EventArgs e) {
            TextBox tx = sender as TextBox;
            tx.Text = "";
        }

        public void PlaceholderAddText(object sender, EventArgs e) {
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
                BEGIN METHODS
        --------------------------------
        */

        #region Private
        private void ConnectToSSH() {
            if (host == "") {
                host = hostInfo.Text;
                port = int.Parse(portInfo.Text);
                username = usernameInfo.Text;
                password = passwordInfo.Text;
            }

            try {
                this.ssh_client = new SshClient(host,port, username, password);
                this.ssh_client.ConnectionInfo.Timeout = TimeSpan.FromSeconds(120);
                this.ssh_client.Connect();

                this.shell_stream = this.ssh_client.CreateShellStream("newTerminal", 80, 60, 800, 600, 65536);

                panel1.Visible = false;
                this.Text = host;
            }
            catch (Exception e) {
                DisplayError(e.Message);
            }
        }

        private void DisplayError(string msg) {
            error_window.SetErrorMessage(msg);
            error_window.Show();
        }

        private void RecieveData() {
            while (true) {
                try {
                    if (this.shell_stream != null && this.shell_stream.DataAvailable) {
                        string data = this.shell_stream.Read();
                        AppendTextboxInThread(sshConsole, data);
                    }
                }catch(Exception e) {
                    throw e;
                }

                System.Threading.Thread.Sleep(200);
            }
        }

        private void AppendTextboxInThread(TextBox t, String s) {
            if (t.InvokeRequired) {
                t.Invoke(new Action<TextBox, string>(AppendTextboxInThread), new object[] { t, s });
            }
            else {
                t.AppendText(s);
            }
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
