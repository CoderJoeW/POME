using POME;
using System;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

public class EscapeSequenceHandler
{
    public static string HandleEscapeSequences(string input, TerminalMode terminalMode, RichTextBox richTextBox, Form mainForm)
    {
        string output = input;

        switch (terminalMode)
        {
            case TerminalMode.VT100:
                // Handle VT100 escape sequences
                break;
            case TerminalMode.VT220:
                // Handle VT220 escape sequences
                break;
            case TerminalMode.XTerm:
                output = HandleXTermEscapeSequences(input, richTextBox, mainForm);
                break;
        }

        return output;
    }

    private static string HandleXTermEscapeSequences(string input, RichTextBox rtb, Form mainForm)
    {
        var output = new StringBuilder();
        int curIdx = 0;
        string pat = @"\x1B(?:\[((?:\d{1,3};)*\d{1,3}[ABCDEFGHJKSTfhilmnprsu]|\?1(?:l|h)|\d{1,3}(?:[ABCD]|[su])|6n|[=c]|[=>]|[=>]|[8Cm]|[7D]|[10q]|[\?12l]|[8Cm]|[\?25l]|[\?25h])|\](\d*);(.*?)\x1B\\|Ptmux;(\x1B(?:\[[^P]*P)*)\x1B\\)";

        input = input.Replace("\n", Environment.NewLine);

        Match m = Regex.Match(input, pat);
        while (m.Success)
        {
            int mIdx = m.Index;
            output.Append(input.Substring(curIdx, mIdx - curIdx));

            string seq = m.Groups[1].Value, oscSeq = m.Groups[2].Value, tmuxSeq = m.Groups[4].Value;

            if (!string.IsNullOrEmpty(seq))
            {
                if (seq == "[200~") // Start bracket pasting mode
                {
                    EscapeSequenceActions.StartBracketPastingMode(rtb);
                }
                else if (seq == "[201~") // End bracket pasting mode
                {
                    EscapeSequenceActions.EndBracketPastingMode(rtb);
                }
                else
                {
                    ApplyXTermSequence(seq, rtb);
                }
            }
            else if (!string.IsNullOrEmpty(oscSeq))
            {
                HandleOscSequence(oscSeq, mainForm);
                output.Append(input.Substring(curIdx, mIdx - curIdx).Replace(oscSeq, ""));
            }
            else if (!string.IsNullOrEmpty(tmuxSeq)) HandleTmuxSequence(tmuxSeq, rtb);

            curIdx = mIdx + m.Length;
            m = m.NextMatch();
        }

        if (curIdx < input.Length)
        {
            output.Append(input.Substring(curIdx));
        }

        return output.ToString();
    }

    private static void HandleTmuxSequence(string tmuxSequence, RichTextBox richTextBox)
    {
        // Strip tmux-specific parts and process the actual escape sequence
        string actualSequence = tmuxSequence.TrimStart('\x1B');
        ApplyXTermSequence(actualSequence, richTextBox);
    }

    private static void ApplyXTermSequence(string sequence, RichTextBox richTextBox)
    {
        string[] parameters = sequence.Split(';');
        char command = sequence[sequence.Length - 1];
        int parsedParam;

        int GetValue(int defaultValue = 1) =>
            parameters.Length > 0 && int.TryParse(parameters[0], out parsedParam) ? parsedParam : defaultValue;

        switch (command)
        {
            case 'A': EscapeSequenceActions.CursorUp(richTextBox, GetValue()); break;
            case 'B': EscapeSequenceActions.CursorDown(richTextBox, GetValue()); break;
            case 'C': EscapeSequenceActions.CursorForward(richTextBox, GetValue()); break;
            case 'D': EscapeSequenceActions.CursorBackward(richTextBox, GetValue()); break;
            case 'H':
            case 'f':
                if (parameters.Length == 2 && int.TryParse(parameters[0], out parsedParam) && int.TryParse(parameters[1], out int parsedParam2))
                {
                    EscapeSequenceActions.SetCursorPosition(richTextBox, parsedParam - 1, parsedParam2 - 1);
                }
                break;
            case 'S': EscapeSequenceActions.ScrollUp(richTextBox, GetValue()); break;
            case 'T': EscapeSequenceActions.ScrollDown(richTextBox, GetValue()); break;
            case 'm': EscapeSequenceActions.ApplySGRParameters(richTextBox, parameters); break;
            case 'J': EscapeSequenceActions.EraseInDisplay(richTextBox, GetValue(0)); break;
            case '@': EscapeSequenceActions.InsertBlankCharacters(richTextBox, GetValue()); break;
            case '[': EscapeSequenceActions.ApplyPrivateModeSet(sequence, richTextBox); break;
            case 'X': EscapeSequenceActions.EraseCharacters(richTextBox, GetValue(1)); break;
            default: break; // Handle other XTerm escape sequences or ignore unknown ones
        }
    }

    private static void HandleOscSequence(string oscSequence, Form mainForm)
    {
        // Extract the command and parameters
        int cmdStart = oscSequence.IndexOf(';') + 1;
        string command = oscSequence.Substring(cmdStart, oscSequence.Length - cmdStart - 1);

        // Handle the specific OSC command (e.g., change window title)
        if (oscSequence.StartsWith("\x1B]0;"))
        {
            // Change the window title to the value of the command
            mainForm.Text = command;
        }
    }
}