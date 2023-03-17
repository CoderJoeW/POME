using System;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
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

        public TerminalControl()
        {
            InitializeComponents();
            dataBuffer = new StringBuilder();
        }

        private void InitializeComponents()
        {
            terminalDisplay = new RichTextBox
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
            shellStream = sshClient.CreateShellStream("dumb", 80, 24, 800, 600, 4096);
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
            // ANSI escape sequence pattern
            string pattern = @"\x1B\[[^@-~]*[@-~]";
            var regex = new Regex(pattern);

            var matches = regex.Matches(dataBuffer.ToString());
            int lastIndex = 0;

            foreach (Match match in matches)
            {
                // Append text before the escape sequence
                string textBeforeEscape = dataBuffer.ToString().Substring(lastIndex, match.Index - lastIndex);
                lastIndex = match.Index + match.Length;

                AppendTextWithStyle(textBeforeEscape);

                // Handle escape sequence
                string escapeSequence = match.Value;
                HandleEscapeSequence(escapeSequence);
            }

            // Append remaining text
            string remainingText = dataBuffer.ToString().Substring(lastIndex);
            AppendTextWithStyle(remainingText);

            // Clear buffer
            dataBuffer.Clear();
        }

        private void HandleEscapeSequence(string escapeSequence)
        {
            // Example: "\x1B[31m" for red foreground color
            // Remove the leading '\x1B[' and trailing 'm'
            string content = escapeSequence.Substring(2, escapeSequence.Length - 3);
            string[] codes = content.Split(';');

            foreach (string codeStr in codes)
            {
                if (int.TryParse(codeStr, out int code))
                {
                    // Handle common ANSI escape codes
                    switch (code)
                    {
                        case 0:
                            terminalDisplay.SelectionColor = Color.White;
                            terminalDisplay.SelectionBackColor = Color.Black;
                            terminalDisplay.SelectionFont = new Font(terminalDisplay.Font, FontStyle.Regular);
                            break;
                        case 1:
                            terminalDisplay.SelectionFont = new Font(terminalDisplay.Font, FontStyle.Bold);
                            break;
                        case 4:
                            terminalDisplay.SelectionFont = new Font(terminalDisplay.Font, FontStyle.Underline);
                            break;
                        case 30:
                        case 31:
                        case 32:
                        case 33:
                        case 34:
                        case 35:
                        case 36:
                        case 37:
                            // Set foreground color based on the ANSI code
                            terminalDisplay.SelectionColor = AnsiCodeToColor(code - 30);
                            break;
                        case 40:
                        case 41:
                        case 42:
                        case 43:
                        case 44:
                        case 45:
                        case 46:
                        case 47:
                            // Set background color based on the ANSI code
                            terminalDisplay.SelectionBackColor = AnsiCodeToColor(code - 40);
                            break;
                    }
                }
            }
        }

        private Color AnsiCodeToColor(int code)
        {
            // Basic ANSI colors
            Color[] colors = new Color[]
            {
                Color.Black,     // 0
                Color.Red,       // 1
                Color.Green,     // 2
                Color.Yellow,    // 3
                Color.Blue,      // 4
                Color.Magenta,   // 5
                Color.Cyan,      // 6
                Color.White      // 7
            };

            if (code >= 0 && code < colors.Length)
            {
                return colors[code];
            }

            return Color.White;
        }

        private void AppendTextWithStyle(string text)
        {
            int startIndex = terminalDisplay.TextLength;
            terminalDisplay.AppendText(text);
            terminalDisplay.SelectionStart = startIndex;
            terminalDisplay.SelectionLength = text.Length;
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
