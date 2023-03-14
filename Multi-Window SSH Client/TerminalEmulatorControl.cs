using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

public class TerminalEmulatorControl : RichTextBox
{
    private StringBuilder inputBuffer;
    private List<string> commandHistory;
    private int commandHistoryIndex;

    public int CharWidth => TextRenderer.MeasureText("M", Font).Width;
    public int CharHeight => TextRenderer.MeasureText("M", Font).Height;

    public string InputText
    {
        get => inputBuffer.ToString();
        set
        {
            inputBuffer.Clear();
            inputBuffer.Append(value);
            RefreshTerminal();
        }
    }

    public int OutputLength { get; private set; }
    public string Prompt { get; set; } = "> ";

    public Color PromptColor { get; set; } = Color.Green;
    public Color InputColor { get; set; } = Color.White;
    public Color OutputColor { get; set; } = Color.Gray;

    public event EventHandler<string> CommandEntered;

    public TerminalEmulatorControl()
    {
        ReadOnly = true;
        WordWrap = false;
        inputBuffer = new StringBuilder();
        commandHistory = new List<string>();
        commandHistoryIndex = -1;

        KeyPress += TerminalEmulatorControl_KeyPress;
        KeyDown += TerminalEmulatorControl_KeyDown;
    }

    private void TerminalEmulatorControl_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == '\r')
        {
            OnCommandEntered(InputText);
            commandHistory.Add(InputText);
            commandHistoryIndex = commandHistory.Count;
            inputBuffer.Clear();
        }
        else
        {
            inputBuffer.Append(e.KeyChar);
        }

        RefreshTerminal();
        e.Handled = true;
    }

    private void TerminalEmulatorControl_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Back)
        {
            if (inputBuffer.Length > 0)
            {
                inputBuffer.Remove(inputBuffer.Length - 1, 1);
                RefreshTerminal();
            }
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.Delete)
        {
            if (inputBuffer.Length > 0)
            {
                inputBuffer.Clear();
                RefreshTerminal();
            }
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.Up)
        {
            if (commandHistoryIndex > 0)
            {
                commandHistoryIndex--;
                InputText = commandHistory[commandHistoryIndex];
            }
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.Down)
        {
            if (commandHistoryIndex < commandHistory.Count - 1)
            {
                commandHistoryIndex++;
                InputText = commandHistory[commandHistoryIndex];
            }
            else if (commandHistoryIndex == commandHistory.Count - 1)
            {
                commandHistoryIndex++;
                InputText = string.Empty;
            }
            e.Handled = true;
        }
        // Additional key handling can be added here if necessary
    }

    protected virtual void OnCommandEntered(string command)
    {
        CommandEntered?.Invoke(this, command);
    }

    public void AppendText(string text, Color color)
    {
        SelectionStart = Text.Length;
        SelectionColor = color;
        base.AppendText(text);
        SelectionColor = InputColor;
        OutputLength += text.Length;
        RefreshTerminal();
    }

    public void ClearInput()
    {
        inputBuffer.Clear();
        RefreshTerminal();
    }

    private void RefreshTerminal()
    {
        int outputLength = Math.Min(OutputLength, Text.Length);
        SuspendDrawing();
        Text = $"{Text.Substring(0, outputLength)}";
        AppendText(Prompt, PromptColor);
        AppendText(inputBuffer.ToString(), InputColor);
        ResumeDrawing();
        SelectionStart = Text.Length;
        ScrollToCaret();
    }
    private void SuspendDrawing()
    {
        SendMessage(Handle, WM_SETREDRAW, 0, IntPtr.Zero);
    }

    private void ResumeDrawing()
    {
        SendMessage(Handle, WM_SETREDRAW, 1, IntPtr.Zero);
        Refresh();
    }

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

    private const int WM_SETREDRAW = 0x0B;
}