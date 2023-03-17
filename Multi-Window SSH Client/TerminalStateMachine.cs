using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TerminalEmulator
{
    public class TerminalStateMachine
    {
        private enum State
        {
            Normal,
            Escape,
            Bracket
        }

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
            for (int i = 0; i < dataBuffer.Length; i++)
            {
                char currentChar = dataBuffer[i];

                switch (currentState)
                {
                    case State.Normal:
                        if (currentChar == '\x1B')
                        {
                            currentState = State.Escape;
                        }
                        else
                        {
                            terminalDisplay.AppendText(currentChar.ToString());
                        }
                        break;

                    case State.Escape:
                        if (currentChar == '[')
                        {
                            currentState = State.Bracket;
                        }
                        else
                        {
                            currentState = State.Normal;
                        }
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
                case 'm': // Set graphics mode
                    HandleGraphicsMode(codes);
                    break;

                    // Add cases to handle other commands as needed
            }
        }

        private void HandleGraphicsMode(string[] codes)
        {
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
    }
}
