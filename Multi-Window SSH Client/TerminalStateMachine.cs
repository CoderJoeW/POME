using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TerminalEmulator
{
    public class TerminalStateMachine
    {
        private enum State { Normal, Escape, Bracket }
        private State currentState;
        private StringBuilder escapeSequenceBuffer;
        private RichTextBox terminalDisplay;

        public TerminalStateMachine(RichTextBox terminalDisplay)
        {
            this.terminalDisplay = terminalDisplay;
            currentState = State.Normal;
            escapeSequenceBuffer = new StringBuilder();
        }

        public void ProcessBuffer(StringBuilder dataBuffer)
        {
            foreach (char currentChar in dataBuffer.ToString())
            {
                switch (currentState)
                {
                    case State.Normal:
                        currentState = (currentChar == '\x1B') ? State.Escape : State.Normal;
                        if (currentState == State.Normal) terminalDisplay.AppendText(currentChar.ToString());
                        break;
                    case State.Escape:
                        currentState = (currentChar == '[') ? State.Bracket : State.Normal;
                        break;
                    case State.Bracket:
                        escapeSequenceBuffer.Append(currentChar);
                        if (currentChar >= '@' && currentChar <= '~')
                        {
                            HandleEscapeSequence(escapeSequenceBuffer.ToString());
                            escapeSequenceBuffer.Clear();
                            currentState = State.Normal;
                        }
                        break;
                }
            }
        }

        private void HandleEscapeSequence(string escapeSequence)
        {
            string content = escapeSequence.Substring(0, escapeSequence.Length - 1);
            char commandChar = escapeSequence[escapeSequence.Length - 1];
            string[] codes = content.Split(';');

            switch (commandChar)
            {
                case 'm': HandleGraphicsMode(codes); break;
                case 'A':
                case 'B':
                case 'C':
                case 'D': MoveCursor(commandChar, content); break;
                case 'J':
                case 'K': ClearScreenOrLine(commandChar, content); break;
                case 'H':
                case 'f': SetCursorPosition(content); break;
            }
        }

        private void SetCursorPosition(string content)
        {
            var parts = content.Split(';');
            if (parts.Length != 2) return;
            int.TryParse(parts[0], out int row);
            int.TryParse(parts[1], out int col);
            int position = terminalDisplay.GetFirstCharIndexFromLine(row - 1) + (col - 1);
            terminalDisplay.SelectionStart = Math.Min(terminalDisplay.Text.Length, position);
        }

        private void MoveCursor(char command, string content)
        {
            int.TryParse(content, out int distance);
            distance = distance == 0 ? 1 : distance;
            int[] indexChange = { -terminalDisplay.GetFirstCharIndexFromLine(1), terminalDisplay.GetFirstCharIndexFromLine(1), 1, -1 };
            int idx = "ABCD".IndexOf(command);
            terminalDisplay.SelectionStart = Math.Max(0, Math.Min(terminalDisplay.Text.Length, terminalDisplay.SelectionStart + distance * indexChange[idx]));
        }

        private void ClearScreenOrLine(char command, string content)
        {
            int.TryParse(content, out int mode);
            int currentLine = terminalDisplay.GetLineFromCharIndex(terminalDisplay.SelectionStart);
            int currentColumn = terminalDisplay.SelectionStart - terminalDisplay.GetFirstCharIndexFromLine(currentLine);
            int[] startIdx = { terminalDisplay.SelectionStart, 0, terminalDisplay.GetFirstCharIndexFromLine(currentLine) };
            int[] lengthIdx = { terminalDisplay.Text.Length - terminalDisplay.SelectionStart, currentLine * terminalDisplay.GetFirstCharIndexFromLine(1) + currentColumn
                    , terminalDisplay.GetFirstCharIndexFromLine(currentLine + 1) - terminalDisplay.SelectionStart };
            int idx = (command == 'J') ? mode : mode + 3;
            terminalDisplay.SelectionStart = startIdx[idx];
            terminalDisplay.SelectionLength = lengthIdx[idx];
            terminalDisplay.SelectedText = "";
        }

        private void HandleGraphicsMode(string[] codes)
        {
            foreach (string codeStr in codes)
            {
                if (int.TryParse(codeStr, out int code))
                {
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
                        default:
                            if (code >= 30 && code <= 37)
                                terminalDisplay.SelectionColor = AnsiCodeToColor(code - 30);
                            else if (code >= 40 && code <= 47)
                                terminalDisplay.SelectionBackColor = AnsiCodeToColor(code - 40);
                            break;
                    }
                }
            }
        }

        private Color AnsiCodeToColor(int code)
        {
            Color[] colors = { Color.Black, Color.Red, Color.Green, Color.Yellow, Color.Blue, Color.Magenta, Color.Cyan, Color.White };
            return (code >= 0 && code < colors.Length) ? colors[code] : Color.White;
        }
    }
}