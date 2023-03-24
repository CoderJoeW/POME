using POME;
using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TerminalEmulator
{
    public class TerminalStateMachine
    {
        private enum State { Normal, Escape, Bracket, Parenthesis, OperatingSystemCommand }
        private State currentState;
        private StringBuilder escapeSequenceBuffer;
        private RichTextBox terminalDisplay;
        private Point savedCursorPosition;

        public TerminalStateMachine(RichTextBox terminalDisplay)
        {
            this.terminalDisplay = terminalDisplay;
            currentState = State.Normal;
            escapeSequenceBuffer = new StringBuilder();
            savedCursorPosition = new Point(0, 0);
        }

        public void ProcessBuffer(StringBuilder dataBuffer)
        {
            foreach (char currentChar in dataBuffer.ToString())
            {
                switch (currentState)
                {
                    case State.Normal:
                        currentState = (currentChar == '\x1B') ? State.Escape : State.Normal;

                        // Handle the backspace character
                        if (currentChar == '\b')
                        {
                            if (terminalDisplay.SelectionStart > 0)
                            {
                                terminalDisplay.SelectionStart--;
                                terminalDisplay.SelectionLength = 1;
                                terminalDisplay.SelectedText = "";
                            }
                        }
                        else if (currentState == State.Normal)
                        {
                            if (currentChar == '\n')
                            {
                                terminalDisplay.AppendText(Environment.NewLine);
                            }
                            else
                            {
                                terminalDisplay.AppendText(currentChar.ToString());
                            }
                        }
                        break;
                    case State.Escape:
                        if (currentChar == '[')
                            currentState = State.Bracket;
                        else if (currentChar == '(' || currentChar == ')')
                            currentState = State.Parenthesis;
                        else if (currentChar == ']')
                            currentState = State.OperatingSystemCommand;
                        else
                            currentState = State.Normal;
                        break;
                    case State.OperatingSystemCommand:
                        escapeSequenceBuffer.Append(currentChar);
                        if (currentChar == '\x07')
                        {
                            XTermActions.HandleOperatingSystemCommand(escapeSequenceBuffer.ToString());
                            escapeSequenceBuffer.Clear();
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
                    case State.Parenthesis:
                        // Skipping character set selection implementation as it's rarely used in modern terminals
                        currentState = State.Normal;
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
                case 'm': XTermActions.HandleGraphicsMode(codes, terminalDisplay); break;
                case 'A':
                case 'B':
                case 'C':
                case 'D': XTermActions.MoveCursor(commandChar, content, terminalDisplay); break;
                case 'J':
                case 'K': XTermActions.ClearScreenOrLine(commandChar, content, terminalDisplay); break;
                case 'H':
                case 'f': XTermActions.SetCursorPosition(content, terminalDisplay); break;
                case 's': XTermActions.SaveCursorPosition(terminalDisplay, savedCursorPosition); break;
                case 'u': XTermActions.RestoreCursorPosition(terminalDisplay, savedCursorPosition); break;
                case 'L': XTermActions.InsertLines(content, terminalDisplay); break;
                case 'M': XTermActions.DeleteLines(content, terminalDisplay); break;
                case 'P': XTermActions.DeleteCharacters(content, terminalDisplay); break;
                case 'X': XTermActions.EraseCharacters(content, terminalDisplay); break;
            }
        }
    }
}