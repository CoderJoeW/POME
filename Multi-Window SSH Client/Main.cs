using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Renci.SshNet;
using Renci.SshNet.Common;

namespace POME
{
    public partial class Main : Form
    {
        private SshClient _sshClient;
        private ShellStream _shellStream;
        private TerminalEmulatorControl _terminalEmulatorControl;
        public Main()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            // Create and configure a custom TerminalEmulatorControl
            _terminalEmulatorControl = new TerminalEmulatorControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 10)
            };
            Controls.Add(_terminalEmulatorControl);

            // Subscribe to CommandEntered event
            _terminalEmulatorControl.CommandEntered += _terminalEmulatorControl_CommandEntered;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            ShowSessionDetailsDialog();
        }

        private void ShellStream_DataReceived(object sender, ShellDataEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => _terminalEmulatorControl.AppendText(_shellStream.Read())));
            }
            else
            {
                _terminalEmulatorControl.AppendText(_shellStream.Read());
            }
        }

        private void _terminalEmulatorControl_CommandEntered(object sender, string command)
        {
            if (_shellStream != null && _shellStream.CanWrite)
            {
                _shellStream.WriteLine(command);
                _terminalEmulatorControl.ClearInput();
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            _sshClient?.Dispose();
        }

        private void ShowSessionDetailsDialog()
        {
            using (var form = new SessionDetails())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var connectionInfo = new ConnectionInfo(form.Host, form.Port, form.Username, new PasswordAuthenticationMethod(form.Username, form.Password));

                    _sshClient = new SshClient(connectionInfo);
                    _sshClient.Connect();

                    int columns = _terminalEmulatorControl.ClientSize.Width / _terminalEmulatorControl.CharWidth;
                    int rows = _terminalEmulatorControl.ClientSize.Height / _terminalEmulatorControl.CharHeight;

                    _shellStream = _sshClient.CreateShellStream("xterm", (uint)columns, (uint)rows, 800, 600, 1024);
                    _shellStream.DataReceived += ShellStream_DataReceived;

                    // Set focus to the TerminalEmulatorControl so it can accept keyboard input
                    _terminalEmulatorControl.Focus();
                }
            }
        }
    }
}