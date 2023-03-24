using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POME
{
    internal class XTermActions
    {

        public static void SaveCursorPosition(RichTextBox terminalDisplay, Point savedCursorPosition)
        {
            int currentLine = terminalDisplay.GetLineFromCharIndex(terminalDisplay.SelectionStart);
            int currentColumn = terminalDisplay.SelectionStart - terminalDisplay.GetFirstCharIndexFromLine(currentLine);
            savedCursorPosition = new Point(currentColumn, currentLine);
        }

        public static void RestoreCursorPosition(RichTextBox terminalDisplay, Point savedCursorPosition)
        {
            int position = terminalDisplay.GetFirstCharIndexFromLine(savedCursorPosition.Y) + savedCursorPosition.X;
            terminalDisplay.SelectionStart = Math.Min(terminalDisplay.Text.Length, position);
        }

        public static void SetCursorPosition(string content, RichTextBox terminalDisplay)
        {
            var parts = content.Split(';');
            if (parts.Length != 2) return;
            int.TryParse(parts[0], out int row);
            int.TryParse(parts[1], out int col);
            int position = terminalDisplay.GetFirstCharIndexFromLine(row - 1) + (col - 1);
            terminalDisplay.SelectionStart = Math.Min(terminalDisplay.Text.Length, position);
        }

        public static void MoveCursor(char command, string content, RichTextBox terminalDisplay)
        {
            int.TryParse(content, out int distance);
            distance = distance == 0 ? 1 : distance;
            int[] indexChange = { -terminalDisplay.GetFirstCharIndexFromLine(1), terminalDisplay.GetFirstCharIndexFromLine(1), 1, -1 };
            int idx = "ABCD".IndexOf(command);
            terminalDisplay.SelectionStart = Math.Max(0, Math.Min(terminalDisplay.Text.Length, terminalDisplay.SelectionStart + distance * indexChange[idx]));
        }

        public static void ClearScreenOrLine(char command, string content, RichTextBox terminalDisplay)
        {
            int.TryParse(content, out int mode);
            int currentLine = terminalDisplay.GetLineFromCharIndex(terminalDisplay.SelectionStart);
            int currentColumn = terminalDisplay.SelectionStart - terminalDisplay.GetFirstCharIndexFromLine(currentLine);

            if (command == 'J')
            {
                switch (mode)
                {
                    case 0:
                        terminalDisplay.SelectionStart = terminalDisplay.SelectionStart;
                        terminalDisplay.SelectionLength = terminalDisplay.Text.Length - terminalDisplay.SelectionStart;
                        break;
                    case 1:
                        terminalDisplay.SelectionStart = 0;
                        terminalDisplay.SelectionLength = terminalDisplay.GetFirstCharIndexFromLine(currentLine) + currentColumn;
                        break;
                    case 2:
                        terminalDisplay.SelectionStart = 0;
                        terminalDisplay.SelectionLength = terminalDisplay.Text.Length;
                        break;
                    default:
                        return;
                }
            }
            else if (command == 'K')
            {
                switch (mode)
                {
                    case 0:
                        terminalDisplay.SelectionStart = terminalDisplay.SelectionStart;
                        int nextLineIndex = terminalDisplay.GetFirstCharIndexFromLine(currentLine + 1);
                        int remainingLength = nextLineIndex - terminalDisplay.SelectionStart;
                        terminalDisplay.SelectionLength = Math.Max(0, remainingLength);
                        break;
                    case 1:
                        terminalDisplay.SelectionStart = terminalDisplay.GetFirstCharIndexFromLine(currentLine);
                        terminalDisplay.SelectionLength = currentColumn;
                        break;
                    case 2:
                        terminalDisplay.SelectionStart = terminalDisplay.GetFirstCharIndexFromLine(currentLine);
                        terminalDisplay.SelectionLength = terminalDisplay.GetFirstCharIndexFromLine(currentLine + 1) - terminalDisplay.GetFirstCharIndexFromLine(currentLine);
                        break;
                    default:
                        return;
                }
            }

            terminalDisplay.SelectedText = new string('\n', terminalDisplay.SelectedText.Count(c => c == '\n'));
        }


        public static void HandleGraphicsMode(string[] codes, RichTextBox terminalDisplay)
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
                            else if (code == 38 || code == 48)
                            {
                                if (codes.Length > 2 && int.TryParse(codes[1], out int mode) && mode == 5)
                                {
                                    if (int.TryParse(codes[2], out int colorCode))
                                    {
                                        Color color = AnsiCodeToColor(colorCode);
                                        if (code == 38)
                                            terminalDisplay.SelectionColor = color;
                                        else
                                            terminalDisplay.SelectionBackColor = color;
                                    }
                                }
                            }
                            break;
                    }
                }
            }
        }

        public static void HandleOperatingSystemCommand(string content)
        {
            if (content.StartsWith("0;"))
            {
                // This is a request to set the terminal title.
                string title = content.Substring(2, content.Length - 3); // Exclude the starting "0;" and the ending '\x07'

                // Raise the event to notify the parent form or control to set the title externally.
                //TerminalTitleChanged?.Invoke(title);
            }
            // Add handling for other operating system commands if needed.
        }

        public static void InsertLines(string content, RichTextBox terminalDisplay)
        {
            int.TryParse(content, out int lines);
            lines = lines == 0 ? 1 : lines;
            int currentLine = terminalDisplay.GetLineFromCharIndex(terminalDisplay.SelectionStart);

            for (int i = 0; i < lines; i++)
            {
                terminalDisplay.SelectionStart = terminalDisplay.GetFirstCharIndexFromLine(currentLine);
                terminalDisplay.SelectedText = "\n";
            }
        }

        public static void DeleteLines(string content, RichTextBox terminalDisplay)
        {
            int.TryParse(content, out int lines);
            lines = lines == 0 ? 1 : lines;
            int currentLine = terminalDisplay.GetLineFromCharIndex(terminalDisplay.SelectionStart);

            for (int i = 0; i < lines; i++)
            {
                int start = terminalDisplay.GetFirstCharIndexFromLine(currentLine);
                int end = terminalDisplay.GetFirstCharIndexFromLine(currentLine + 1);
                terminalDisplay.SelectionStart = start;
                terminalDisplay.SelectionLength = end - start;
                terminalDisplay.SelectedText = "";
            }
        }

        public static void DeleteCharacters(string content, RichTextBox terminalDisplay)
        {
            int.TryParse(content, out int chars);
            chars = chars == 0 ? 1 : chars;
            terminalDisplay.SelectionLength = chars;
            terminalDisplay.SelectedText = "";
        }

        public static void EraseCharacters(string content, RichTextBox terminalDisplay)
        {
            int.TryParse(content, out int chars);
            chars = chars == 0 ? 1 : chars;
            terminalDisplay.SelectionLength = chars;

            for (int i = 0; i < chars; i++)
            {
                terminalDisplay.SelectedText += " ";
            }

            terminalDisplay.SelectionStart -= chars;
        }

        private static Color AnsiCodeToColor(int code)
        {
            if (code >= 0 && code < 16)
            {
                Color[] baseColors = { Color.Black, Color.Red, Color.Green, Color.Yellow, Color.Blue, Color.Magenta, Color.Cyan, Color.White };
                return baseColors[code % 8];
            }
            else if (code >= 16 && code < 232)
            {
                int red = (code - 16) / 36;
                int green = ((code - 16) % 36) / 6;
                int blue = (code - 16) % 6;

                red = red * 42 + (red == 0 ? 0 : 55);
                green = green * 42 + (green == 0 ? 0 : 55);
                blue = blue * 42 + (blue == 0 ? 0 : 55);

                return Color.FromArgb(red, green, blue);
            }
            else if (code >= 232 && code < 256)
            {
                int gray = (code - 232) * 10 + 8;
                return Color.FromArgb(gray, gray, gray);
            }

            return Color.White;
        }
    }
}