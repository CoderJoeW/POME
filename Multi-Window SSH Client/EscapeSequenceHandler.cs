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

        Match m = Regex.Match(input, pat);
        while (m.Success)
        {
            int mIdx = m.Index;
            output.Append(input.Substring(curIdx, mIdx - curIdx));

            string seq = m.Groups[1].Value, oscSeq = m.Groups[2].Value, tmuxSeq = m.Groups[4].Value;

            if (!string.IsNullOrEmpty(seq)) ApplyXTermSequence(seq, rtb);
            else if (!string.IsNullOrEmpty(oscSeq)) HandleOscSequence(oscSeq, mainForm);
            else if (!string.IsNullOrEmpty(tmuxSeq)) HandleTmuxSequence(tmuxSeq, rtb);

            curIdx = mIdx + m.Length;
            m = m.NextMatch();
        }

        if (curIdx < input.Length)
        {
            bool isBPMEnabled = rtb.Tag is bool isEnabled && isEnabled;

            if (isBPMEnabled) output.Append("\x1B[200~"); // BPM start sequence

            output.Append(input.Substring(curIdx));

            if (isBPMEnabled) output.Append("\x1B[201~"); // BPM end sequence
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
            case 'A': CursorUp(richTextBox, GetValue()); break;
            case 'B': CursorDown(richTextBox, GetValue()); break;
            case 'C': CursorForward(richTextBox, GetValue()); break;
            case 'D': CursorBackward(richTextBox, GetValue()); break;
            case 'H':
            case 'f':
                if (parameters.Length == 2 && int.TryParse(parameters[0], out parsedParam) && int.TryParse(parameters[1], out int parsedParam2))
                {
                    SetCursorPosition(richTextBox, parsedParam - 1, parsedParam2 - 1);
                }
                break;
            case 'S': ScrollUp(richTextBox, GetValue()); break;
            case 'T': ScrollDown(richTextBox, GetValue()); break;
            case 'm': ApplySGRParameters(richTextBox, parameters); break;
            case 'J': EraseInDisplay(richTextBox, GetValue(0)); break;
            case '@': InsertBlankCharacters(richTextBox, GetValue()); break;
            case '[': ApplyPrivateModeSet(sequence, richTextBox); break;
            default: break; // Handle other XTerm escape sequences or ignore unknown ones
        }
    }

    private static void ApplySGRParameters(RichTextBox rtb, string[] parameters)
    {
        for (int i = 0; i < parameters.Length; i++)
    {
            if (int.TryParse(parameters[i], out int p))
        {
                rtb.SelectionColor = (p >= 30 && p <= 37) || (p >= 90 && p <= 97) ? GetColorFromSGRCode(p) : p == 0 ? Color.White : p == 38 ? GetExtendedColor (parameters[i], parameters) : rtb.SelectionColor;
                rtb.SelectionBackColor = (p >= 40 && p <= 47) || (p >= 100 && p <= 107) ? GetColorFromSGRCode(p) : p == 0 ? Color.Black : p == 48 ? GetExtendedColor (parameters[i], parameters) : rtb.SelectionBackColor;
            }
        }
    }


    private static void ApplyPrivateModeSet(string sequence, RichTextBox richTextBox)
    {
        if (sequence.StartsWith("?2004"))
        {
            if (sequence.EndsWith("h")) // Enable Bracketed Paste Mode
            {
                richTextBox.Tag = true;
            }
            else if (sequence.EndsWith("l")) // Disable Bracketed Paste Mode
            {
                richTextBox.Tag = false;
            }
        }
    }


    private static void CursorUp(RichTextBox richTextBox, int lines)
    {
        int currentLine = richTextBox.GetLineFromCharIndex(richTextBox.SelectionStart);
        richTextBox.SelectionStart = richTextBox.GetFirstCharIndexFromLine(Math.Max(currentLine - lines, 0));
        richTextBox.ScrollToCaret();
    }

    private static void CursorDown(RichTextBox richTextBox, int lines)
    {
        int currentLine = richTextBox.GetLineFromCharIndex(richTextBox.SelectionStart);
        richTextBox.SelectionStart = richTextBox.GetFirstCharIndexFromLine(Math.Min(currentLine + lines, richTextBox.Lines.Length - 1));
        richTextBox.ScrollToCaret();
    }

    private static void CursorForward(RichTextBox richTextBox, int columns)
    {
        richTextBox.SelectionStart = Math.Min(richTextBox.SelectionStart + columns, richTextBox.Text.Length);
        richTextBox.ScrollToCaret();
    }

    private static void CursorBackward(RichTextBox richTextBox, int columns)
    {
        richTextBox.SelectionStart = Math.Max(richTextBox.SelectionStart - columns, 0);
        richTextBox.ScrollToCaret();
    }

    private static void SetCursorPosition(RichTextBox richTextBox, int row, int col)
    {
        if (row >= 0 && row < richTextBox.Lines.Length)
        {
            int lineStart = richTextBox.GetFirstCharIndexFromLine(row);
            int lineLength = richTextBox.Lines[row].Length;
            richTextBox.SelectionStart = lineStart + Math.Min(col, lineLength);
            richTextBox.ScrollToCaret();
        }
    }

    private static void ScrollUp(RichTextBox richTextBox, int lines)
    {
        int currentLine = richTextBox.GetLineFromCharIndex(richTextBox.SelectionStart);
        if (currentLine - lines >= 0)
        {
            richTextBox.SelectionStart = richTextBox.GetFirstCharIndexFromLine(currentLine - lines);
        }
        else
        {
            richTextBox.SelectionStart = 0;
        }
        richTextBox.ScrollToCaret();
    }

    private static void ScrollDown(RichTextBox richTextBox, int lines)
    {
        int currentLine = richTextBox.GetLineFromCharIndex(richTextBox.SelectionStart);
        if (currentLine + lines < richTextBox.Lines.Length)
        {
            richTextBox.SelectionStart = richTextBox.GetFirstCharIndexFromLine(currentLine + lines);
        }
        else
        {
            richTextBox.SelectionStart = richTextBox.GetFirstCharIndexFromLine(richTextBox.Lines.Length - 1);
        }
        richTextBox.ScrollToCaret();
    }

    private static void EraseInDisplay(RichTextBox richTextBox, int mode)
    {
        int start, length;
        switch (mode)
        {
            case 0: // Erase from cursor to end of display
                start = richTextBox.SelectionStart;
                length = richTextBox.Text.Length - start;
                break;
            case 1: // Erase from start of display to cursor
                start = 0;
                length = richTextBox.SelectionStart;
                break;
            case 2: // Erase entire display
                start = 0;
                length = richTextBox.Text.Length;
                break;
            default:
                return;
        }

        richTextBox.SelectionStart = start;
        richTextBox.SelectionLength = length;
        richTextBox.SelectedText = new string(' ', length);
        richTextBox.SelectionLength = 0;
    }

    private static void InsertBlankCharacters(RichTextBox richTextBox, int count)
    {
        int currentLine = richTextBox.GetLineFromCharIndex(richTextBox.SelectionStart);
        int lineStart = richTextBox.GetFirstCharIndexFromLine(currentLine);
        int lineLength = richTextBox.Lines[currentLine].Length;
        int currentColumn = richTextBox.SelectionStart - lineStart;

        // Determine the number of spaces to insert
        int spacesToInsert = Math.Min(count, lineLength - currentColumn);

        // Insert spaces at the cursor position
        richTextBox.SelectionLength = 0;
        richTextBox.SelectedText = new string(' ', spacesToInsert);
        richTextBox.SelectionLength = 0;
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

    private static Color GetColorFromSGRCode(int code)
    {
        // Return the corresponding color for the given SGR color code
        switch (code)
        {
            case 30: return Color.Black;
            case 31: return Color.Red;
            case 32: return Color.Green;
            case 33: return Color.Yellow;
            case 34: return Color.Blue;
            case 35: return Color.Magenta;
            case 36: return Color.Cyan;
            case 37: return Color.White;
            case 90: return Color.FromArgb(128, 128, 128); // Bright Black
            case 91: return Color.FromArgb(255, 0, 0); // Bright Red
            case 92: return Color.FromArgb(0, 255, 0); // Bright Green
            case 93: return Color.FromArgb(255, 255, 0); // Bright Yellow
            case 94: return Color.FromArgb(0, 0, 255); // Bright Blue
            case 95: return Color.FromArgb(255, 0, 255); // Bright Magenta
            case 96: return Color.FromArgb(0, 255, 255); // Bright Cyan
            case 97: return Color.FromArgb(255, 255, 255); // Bright White
            default:
                return Color.Empty;
        }
    }

    private static Color GetExtendedColor(string currentParam, string[] parameters)
    {
        int currentIndex = Array.IndexOf(parameters, currentParam);
        int colorMode;
        int red, green, blue;

        if (currentIndex < 0 || currentIndex + 1 >= parameters.Length)
        {
            return Color.Empty;
        }

        if (int.TryParse(parameters[currentIndex + 1], out colorMode))
        {
            if (colorMode == 2 && currentIndex + 4 < parameters.Length) // RGB mode
            {
                if (int.TryParse(parameters[currentIndex + 2], out red) &&
                    int.TryParse(parameters[currentIndex + 3], out green) &&
                    int.TryParse(parameters[currentIndex + 4], out blue))
                {
                    return Color.FromArgb(red, green, blue);
                }
            }
            else if (colorMode == 5 && currentIndex + 2 < parameters.Length) // 256-color mode
            {
                int colorIndex;
                if (int.TryParse(parameters[currentIndex + 2], out colorIndex))
                {
                    return GetColorFrom256ColorPalette(colorIndex);
                }
            }
        }

        return Color.Empty;
    }

    private static Color GetColorFrom256ColorPalette(int index)
    {
        if (index < 0 || index >= 256) return Color.Empty;

        if (index < 16) return GetColorFromBasic16ColorPalette(index);

        if (index < 232)
        {
            index -= 16;
            int red = index / 36 * 51, green = (index % 36) / 6 * 51, blue = index % 6 * 51;
            return Color.FromArgb(red, green, blue);
        }

        int gray = (index - 232) * 10 + 8;
        return Color.FromArgb(gray, gray, gray);
    }

    private static Color GetColorFromBasic16ColorPalette(int index)
    {
        // Map the basic 16-color palette indices to the actual colors
        switch (index)
        {
            case 0: return Color.Black;
            case 1: return Color.Maroon;
            case 2: return Color.Green;
            case 3: return Color.Olive;
            case 4: return Color.Navy;
            case 5: return Color.Purple;
            case 6: return Color.Teal;
            case 7: return Color.Silver;
            case 8: return Color.Gray;
            case 9: return Color.Red;
            case 10: return Color.Lime;
            case 11: return Color.Yellow;
            case 12: return Color.Blue;
            case 13: return Color.Magenta;
            case 14: return Color.Cyan;
            case 15: return Color.White;
            default: return Color.Empty;
        }
    }
}