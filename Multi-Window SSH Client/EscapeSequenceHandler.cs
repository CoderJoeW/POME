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

    private static string HandleXTermEscapeSequences(string input, RichTextBox richTextBox, Form mainForm)
    {
        StringBuilder output = new StringBuilder();
        int currentIndex = 0;

        string pattern = @"\x1B\[([!\#$%&'()*+,-./:;<=>?@A-Z\[\\\]^_`a-z{|}~]*?)[ABCDEFGHJKSTfhilmnprsu]|" +
                         @"\x1B\](\d+;.+)\x07|" +
                         @"\x1BPtmux;(.+?)\x1B\\";
        var matches = Regex.Matches(input, pattern);
        foreach (Match match in matches)
        {
            int matchIndex = match.Index;
            output.Append(input.Substring(currentIndex, matchIndex - currentIndex));

            string sequence = match.Groups[1].Value;
            string oscSequence = match.Groups[2].Value;
            string tmuxSequence = match.Groups[3].Value;

            if (!string.IsNullOrEmpty(sequence))
            {
                ApplyXTermSequence(sequence, richTextBox);
            }
            else if (!string.IsNullOrEmpty(oscSequence))
            {
                HandleOscSequence(oscSequence, mainForm);
            }
            else if (!string.IsNullOrEmpty(tmuxSequence))
            {
                HandleTmuxSequence(tmuxSequence, richTextBox);
            }

            currentIndex = matchIndex + match.Length;
        }

        if (currentIndex < input.Length)
        {
            output.Append(input.Substring(currentIndex));
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
        // Implement the logic to apply XTerm escape sequences
        string[] parameters = sequence.Split(';');
        char command = sequence[sequence.Length - 1];
        int value;
        int parsedParam;

        switch (command)
        {
            case 'A': // Cursor up
                value = parameters.Length > 0 && int.TryParse(parameters[0], out parsedParam) ? parsedParam : 1;
                CursorUp(richTextBox, value);
                break;
            case 'B': // Cursor down
                value = parameters.Length > 0 && int.TryParse(parameters[0], out parsedParam) ? parsedParam : 1;
                CursorDown(richTextBox, value);
                break;
            case 'C': // Cursor forward (right)
                value = parameters.Length > 0 && int.TryParse(parameters[0], out parsedParam) ? parsedParam : 1;
                CursorForward(richTextBox, value);
                break;
            case 'D': // Cursor backward (left)
                value = parameters.Length > 0 && int.TryParse(parameters[0], out parsedParam) ? parsedParam : 1;
                CursorBackward(richTextBox, value);
                break;
            case 'H': // Cursor position (row and column)
            case 'f':
                if (parameters.Length == 2 && int.TryParse(parameters[0], out parsedParam) && int.TryParse(parameters[1], out int parsedParam2))
                {
                    SetCursorPosition(richTextBox, parsedParam - 1, parsedParam2 - 1);
                }
                break;
            case 'S': // Scroll up
                value = parameters.Length > 0 && int.TryParse(parameters[0], out parsedParam) ? parsedParam : 1;
                ScrollUp(richTextBox, value);
                break;
            case 'T': // Scroll down
                value = parameters.Length > 0 && int.TryParse(parameters[0], out parsedParam) ? parsedParam : 1;
                ScrollDown(richTextBox, value);
                break;
            case 'm': // Select Graphic Rendition (SGR)
                for (int i = 0; i < parameters.Length; i++)
                {
                    if (int.TryParse(parameters[i], out parsedParam))
                    {
                        if (parsedParam >= 30 && parsedParam <= 37) // Set foreground color (basic 8 colors)
                        {
                            richTextBox.SelectionColor = GetBasicColor(parsedParam - 30);
                        }
                        else if (parsedParam >= 40 && parsedParam <= 47) // Set background color (basic 8 colors)
                        {
                            richTextBox.SelectionBackColor = GetBasicColor(parsedParam - 40);
                        }
                        else if (parsedParam == 38 || parsedParam == 48) // Set foreground (38) or background (48) extended color
                        {
                            Color color = GetExtendedColor(parameters[i], parameters);
                            if (!color.IsEmpty)
                            {
                                if (parsedParam == 38)
                                {
                                    richTextBox.SelectionColor = color;
                                }
                                else
                                {
                                    richTextBox.SelectionBackColor = color;
                                }
                            }
                            i += color.IsEmpty ? 0 : 2;
                        }
                        else if (parsedParam == 0) // Reset all attributes
                        {
                            richTextBox.SelectionColor = Color.White;
                            richTextBox.SelectionBackColor = Color.Black;
                        }
                        // Add more cases for other SGR parameters as needed
                    }
                }
                break;

            // Add and update cases for additional XTerm escape sequences

            default:
                // Handle other XTerm escape sequences or ignore unknown ones
                break;
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

    private static void EraseInLine(RichTextBox richTextBox, int mode)
    {
        int currentLine = richTextBox.GetLineFromCharIndex(richTextBox.SelectionStart);
        int lineStart = richTextBox.GetFirstCharIndexFromLine(currentLine);
        int lineLength = richTextBox.Lines[currentLine].Length;
        richTextBox.SelectionLength = 0;
        switch (mode)
        {
            case 0: // Erase from cursor to end of line
                richTextBox.SelectionLength = lineStart + lineLength - richTextBox.SelectionStart;
                richTextBox.SelectedText = new string(' ', richTextBox.SelectionLength);
                break;
            case 1: // Erase from beginning of line to cursor
                int cursorPos = richTextBox.SelectionStart;
                richTextBox.SelectionStart = lineStart;
                richTextBox.SelectionLength = cursorPos - lineStart;
                richTextBox.SelectedText = new string(' ', richTextBox.SelectionLength);
                break;
            case 2: // Erase entire line
                richTextBox.SelectionStart = lineStart;
                richTextBox.SelectionLength = lineLength;
                richTextBox.SelectedText = new string(' ', lineLength);
                break;
        }

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

    private static Color GetBasicColor(int index)
    {
        Color[] basicColors = new Color[]
        {
Color.Black,
Color.Red,
Color.Green,
Color.Yellow,
Color.Blue,
Color.Magenta,
Color.Cyan,
Color.White
        };

        return basicColors[index];
    }

    private static Color GetExtendedColor(string param, string[] parameters)
    {
        int startIndex = Array.IndexOf(parameters, param);
        if (startIndex + 1 < parameters.Length)
        {
            int colorMode = int.Parse(parameters[startIndex + 1]);
            if (colorMode == 5 && startIndex + 2 < parameters.Length)
            {
                int colorIndex = int.Parse(parameters[startIndex + 2]);
                return Get256Color(colorIndex);
            }
            else if (colorMode == 2 && startIndex + 4 < parameters.Length)
            {
                int r = int.Parse(parameters[startIndex + 2]);
                int g = int.Parse(parameters[startIndex + 3]);
                int b = int.Parse(parameters[startIndex + 4]);
                return Color.FromArgb(r, g, b);
            }
        }
        return Color.Empty;
    }

    private static Color Get256Color(int index)
    {
        // Implement 256-color palette logic
        // You can use a predefined color palette array or calculate the
        // color basedon the index value
        Color[] colorPalette = new Color[256];
        // ... Fill the colorPalette array with 256 colors
        // Calculate the 256-color palette based on the index value
        if (index >= 0 && index <= 15)
        {
            // Standard colors
            return GetBasicColor(index % 8);
        }
        else if (index >= 16 && index <= 231)
        {
            // 216 RGB colors
            index -= 16;
            int r = index / 36;
            int g = (index / 6) % 6;
            int b = index % 6;

            return Color.FromArgb(r * 51, g * 51, b * 51);
        }
        else if (index >= 232 && index <= 255)
        {
            // Grayscale colors
            int gray = (index - 232) * 10 + 8;
            return Color.FromArgb(gray, gray, gray);
        }

        return Color.Empty;
    }
}

