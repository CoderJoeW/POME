using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Renci.SshNet;
using Renci.SshNet.Common;
using TerminalEmulator;

namespace POME
{
    public partial class Main : Form
    {
        private SshClient _sshClient;
        private ShellStream _shellStream;
        private TerminalControl _terminalEmulatorControl;

        public Main()
        {
            InitializeComponent();

            CheckForIllegalCrossThreadCalls = false;

            // Create and configure a custom TerminalEmulatorControl
            /*_terminalEmulatorControl = new TerminalEmulatorControl(this)
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 10),
                Prompt = ">",
                TerminalMode = TerminalMode.XTerm
            };
            Controls.Add(_terminalEmulatorControl);

            // Subscribe to CommandEntered event
            _terminalEmulatorControl.CommandEntered += _terminalEmulatorControl_CommandEntered;*/

            // Create and configure a custom TerminalEmulatorControl
            _terminalEmulatorControl = new TerminalControl()
            {
                Dock = DockStyle.Fill,
            };
            Controls.Add(_terminalEmulatorControl);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            ShowSessionDetailsDialog();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            _terminalEmulatorControl.Disconnect();
        }

        private void ShowSessionDetailsDialog()
        {
            using (var form = new SessionDetails())
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    _terminalEmulatorControl.Connect(form.Host, form.Username, form.Password);

                    _terminalEmulatorControl.Focus();
                }
            }
        }

    }
}
