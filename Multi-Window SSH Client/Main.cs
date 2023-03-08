using System;
using System.Windows.Forms;
using System.Windows.Input;
using EasyTabs;

namespace POME
{
    public partial class Main : Form
    {
        private string host = "";
        private int port = 0;
        private string username = "";
        private string password = "";
        private readonly ErrorWindow error_window = new ErrorWindow();

        protected TitleBarTabs ParentTabs => (ParentForm as TitleBarTabs);

        public Main(string args = "")
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(args)) SetSessionDetails(args);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            ShowSessionDetailsDialog();
            sshTerminalControl1.AllowCopyingToClipboard = true;
            sshTerminalControl1.AllowPastingFromClipboard = true;
        }

        private void ConnectToSSH()
        {
            try
            {
                sshTerminalControl1.Connect(host, port);
                sshTerminalControl1.Authenticate(username, password);
                sshTerminalControl1.Focus();
                Text = host;
            }
            catch (Exception e) { DisplayError(e.Message); }
        }

        private void DisplayError(string msg)
        {
            error_window.SetErrorMessage(msg);
            error_window.Show();
        }

        private void SetSessionDetails(SessionDetails form)
        {
            host = form.Host;
            port = form.Port;
            username = form.Username;
            password = form.Password;
            ConnectToSSH();
        }

        private void SetSessionDetails(string args)
        {
            host = args.Split('|')[0];
            port = int.Parse(args.Split('|')[1]);
            username = args.Split('|')[2];
            password = args.Split('|')[3];
            ConnectToSSH();
        }

        private void ShowSessionDetailsDialog()
        {
            using (var form = new SessionDetails())
            {
                if (form.ShowDialog() == DialogResult.OK) SetSessionDetails(form);
            }
        }

        private void DuplicateSession()
        {
            string args = $"{host}|{port}|{username}|{password}";
            Main newMain = new Main(args);
            Program.container.Tabs.Add(new TitleBarTab(Program.container) { Content = newMain });
            Program.container.SelectedTabIndex += 1;
        }
    }
}