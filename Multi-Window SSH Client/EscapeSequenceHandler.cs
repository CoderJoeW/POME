using System;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Input;

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

        // Update the regex pattern to include cursor control sequences
        string pattern = @"\x1B\[([!\#$%&'()*+,-./:;<=>?@A-Z\[\\\]^_`a-z{|}~]*?)[ABCDEFGHJKSTfhilmnprsu]|" +
                         @"\x1B\[([ABCD])|" +
                         @"\x1B\](\d+;.+)\x07|" +
                         @"\x1BPtmux;(.+?)\x1B\\";
        var matches = Regex.Matches(input, pattern);
        foreach (Match match in matches)
        {
            int matchIndex = match.Index;
            output.Append(input.Substring(currentIndex, matchIndex - currentIndex));

            string sequence = match.Groups[1].Value;
            string cursorSequence = match.Groups[2].Value;
            string oscSequence = match.Groups[3].Value;
            string tmuxSequence = match.Groups[4].Value;

            if (!string.IsNullOrEmpty(sequence))
            {
                ApplyXTermSequence(sequence, richTextBox);
            }
            else if (!string.IsNullOrEmpty(cursorSequence))
            {
                ApplyXTermSequence(cursorSequence, richTextBox);
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
        foreach (string param in parameters)
        {
            int code = int.TryParse(param, out int parsedParam) ? parsedParam : -1;

            // Handle cursor control sequences
            if (param.Length == 1 && "ABCD".Contains(param))
            {
                int cursorMoveAmount = Math.Max(code, 1);
                switch (param)
                {
                    case "A": // Cursor Up
                        richTextBox.SelectionStart = Math.Max(richTextBox.SelectionStart - (cursorMoveAmount * richTextBox.GetFirstCharIndexFromLine(1)), 0);
                        break;
                    case "B": // Cursor Down
                        richTextBox.SelectionStart = Math.Min(richTextBox.SelectionStart + (cursorMoveAmount * richTextBox.GetFirstCharIndexFromLine(1)), richTextBox.Text.Length);
                        break;
                    case "C": // Cursor Forward
                        richTextBox.SelectionStart = Math.Min(richTextBox.SelectionStart + cursorMoveAmount, richTextBox.Text.Length);
                        break;
                    case "D": // Cursor Back
                        richTextBox.SelectionStart = Math.Max(richTextBox.SelectionStart - cursorMoveAmount, 0);
                        break;
                }
            }
            else
            {
                switch (code)
                {
                    // Add and update cases for additional XTerm escape sequences

                    default:
                        // Handle other XTerm escape sequences or ignore unknown ones
                        break;
                }
            }
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
        // color based on the index value
        Color[] colorPalette = new Color[256];
        // ... Fill the colorPalette array with 256 colors

        return colorPalette[index];
    }
}