using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Renci.SshNet;

namespace TerminalEmulator
{
    public class TerminalControl : UserControl
    {
        private RichTextBox terminalDisplay;
        private SshClient sshClient;
        private ShellStream shellStream;
        private StringBuilder dataBuffer;
        private TerminalStateMachine stateMachine;

        public TerminalControl()
        {
            InitializeComponents();
            dataBuffer = new StringBuilder();
            stateMachine = new TerminalStateMachine(terminalDisplay);
        }

        private void InitializeComponents()
        {
            terminalDisplay = new RichTextBox()
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 12),
                ForeColor = Color.White,
                BackColor = Color.Black,
                ReadOnly = true
            };
            Controls.Add(terminalDisplay);

            terminalDisplay.KeyPress += TerminalDisplay_KeyPress;
        }

        public void Connect(string host, string username, string password)
        {
            sshClient = new SshClient(host, username, password);
            sshClient.Connect();
            shellStream = sshClient.CreateShellStream("xterm", 80, 24, 800, 600, 4096);
            terminalDisplay.AppendText($"Connected to {host}\n");

            // Start reading from the shell
            shellStream.DataReceived += ShellStream_DataReceived;
        }

        private void ShellStream_DataReceived(object sender, Renci.SshNet.Common.ShellDataEventArgs e)
        {
            string data = Encoding.UTF8.GetString(e.Data);
            dataBuffer.Append(data);
            terminalDisplay.Invoke((MethodInvoker)delegate
            {
                ParseAndUpdateBuffer();
            });
        }

        private void ParseAndUpdateBuffer()
        {
            stateMachine.ProcessBuffer(dataBuffer);
            dataBuffer.Clear();
        }

        private void TerminalDisplay_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (shellStream != null && shellStream.CanWrite)
            {
                shellStream.Write(new[] { (byte)e.KeyChar }, 0, 1);
                shellStream.Flush();
                e.Handled = true;
            }
        }

        public void Disconnect()
        {
            if (sshClient != null && sshClient.IsConnected)
            {
                shellStream.Close();
                sshClient.Disconnect();
            }
        }
    }
}
