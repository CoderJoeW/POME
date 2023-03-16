using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public enum FontStyleExtended
{
    Regular = 0,
    Bold = 1,
    Italic = 2,
    Underline = 4,
}

namespace POME
{
    internal class EscapeSequenceActions
    {
        public static void CursorUp(RichTextBox richTextBox, int lines)
        {
            int currentLine = richTextBox.GetLineFromCharIndex(richTextBox.SelectionStart);
            richTextBox.SelectionStart = richTextBox.GetFirstCharIndexFromLine(Math.Max(currentLine - lines, 0));
            richTextBox.ScrollToCaret();
        }

        public static void CursorDown(RichTextBox richTextBox, int lines)
        {
            int currentLine = richTextBox.GetLineFromCharIndex(richTextBox.SelectionStart);
            richTextBox.SelectionStart = richTextBox.GetFirstCharIndexFromLine(Math.Min(currentLine + lines, richTextBox.Lines.Length - 1));
            richTextBox.ScrollToCaret();
        }

        public static void CursorForward(RichTextBox richTextBox, int columns)
        {
            richTextBox.SelectionStart = Math.Min(richTextBox.SelectionStart + columns, richTextBox.Text.Length);
            richTextBox.ScrollToCaret();
        }

        public static void CursorBackward(RichTextBox richTextBox, int columns)
        {
            richTextBox.SelectionStart = Math.Max(richTextBox.SelectionStart - columns, 0);
            richTextBox.ScrollToCaret();
        }

        public static void SetCursorPosition(RichTextBox richTextBox, int row, int col)
        {
            if (row >= 0 && row < richTextBox.Lines.Length)
            {
                int lineStart = richTextBox.GetFirstCharIndexFromLine(row);
                int lineLength = richTextBox.Lines[row].Length;
                richTextBox.SelectionStart = lineStart + Math.Min(col, lineLength);
                richTextBox.ScrollToCaret();
            }
        }

        public static void ScrollUp(RichTextBox richTextBox, int lines)
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

        public static void ScrollDown(RichTextBox richTextBox, int lines)
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

        public static void StartBracketPastingMode(RichTextBox richTextBox)
        {
            richTextBox.Tag = true;
        }

        public static void EndBracketPastingMode(RichTextBox richTextBox)
        {
            richTextBox.Tag = false;
        }

        public static void EraseInDisplay(RichTextBox richTextBox, int mode)
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

        public static void InsertBlankCharacters(RichTextBox richTextBox, int count)
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

        public static void ApplySGRParameters(RichTextBox rtb, string[] parameters)
        {
            FontStyleExtended fontStyle = FontStyleExtended.Regular;

            for (int i = 0; i < parameters.Length; i++)
            {
                if (int.TryParse(parameters[i], out int p))
                {
                    if (p >= 30 && p <= 37 || p >= 90 && p <= 97)
                    {
                        rtb.SelectionColor = ColorHandler.GetColorFromSGRCode(p);
                    }
                    else if (p >= 40 && p <= 47 || p >= 100 && p <= 107)
                    {
                        rtb.SelectionBackColor = ColorHandler.GetColorFromSGRCode(p);
                    }
                    else if (p == 0)
                    {
                        fontStyle = FontStyleExtended.Regular;
                        rtb.SelectionColor = Color.White;
                        rtb.SelectionBackColor = Color.Black;
                    }
                    else if (p == 38)
                    {
                        rtb.SelectionColor = ColorHandler.GetExtendedColor(parameters[i], parameters);
                    }
                    else if (p == 48)
                    {
                        rtb.SelectionBackColor = ColorHandler.GetExtendedColor(parameters[i], parameters);
                    }
                    else if (p == 1)
                    {
                        fontStyle |= FontStyleExtended.Bold;
                    }
                    else if (p == 3)
                    {
                        fontStyle |= FontStyleExtended.Italic;
                    }
                    else if (p == 4)
                    {
                        fontStyle |= FontStyleExtended.Underline;
                    }
                }
            }

            FontStyle newFontStyle = FontStyle.Regular;

            if (fontStyle.HasFlag(FontStyleExtended.Bold))
            {
                newFontStyle |= FontStyle.Bold;
            }

            if (fontStyle.HasFlag(FontStyleExtended.Italic))
            {
                newFontStyle |= FontStyle.Italic;
            }

            if (fontStyle.HasFlag(FontStyleExtended.Underline))
            {
                newFontStyle |= FontStyle.Underline;
            }

            rtb.SelectionFont = new Font(rtb.SelectionFont, newFontStyle);
        }

        public static void EraseCharacters(RichTextBox richTextBox, int count)
        {
            int start = richTextBox.SelectionStart;
            int length = Math.Min(count, richTextBox.Text.Length - start);

            richTextBox.SelectionLength = length;
            richTextBox.SelectedText = new string(' ', length);
            richTextBox.SelectionLength = 0;
            richTextBox.SelectionStart = start;
        }

        public static void ApplyPrivateModeSet(string sequence, RichTextBox richTextBox)
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
    }
}
