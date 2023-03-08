using System;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;
using EasyTabs;
using ComponentPro.Net.Terminal;

namespace POME
{
    public partial class Main : Form
    {
        private string host;
        private int port;
        private string username;
        private string password;
        private readonly ErrorWindow error_window = new ErrorWindow();

        protected TitleBarTabs ParentTabs => ParentForm as TitleBarTabs;

        public Main()
        {
            InitializeComponent();
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
            catch (Exception e)
            {
                DisplayError(e.Message);
            }
        }

        private void DisplayError(string msg)
        {
            error_window.SetErrorMessage(msg);
            error_window.Show();
        }

        private void SetSessionDetails(string[] sessionDetails)
        {
            host = sessionDetails[0];
            port = int.Parse(sessionDetails[1]);
            username = sessionDetails[2];
            password = sessionDetails[3];
            ConnectToSSH();
        }

        private void ShowSessionDetailsDialog()
        {
            using (var form = new SessionDetails())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    string[] sessionDetails = { form.Host, form.Port.ToString(), form.Username, form.Password };
                    SetSessionDetails(sessionDetails);
                }
            }
        }
    }
}
