using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

public class TerminalEmulatorControl : RichTextBox
{
    private StringBuilder inputBuffer;
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

    public event EventHandler<string> CommandEntered;

    public TerminalEmulatorControl()
    {
        ReadOnly = true;
        WordWrap = false;
        inputBuffer = new StringBuilder();

        KeyPress += TerminalEmulatorControl_KeyPress;
    }

    private void TerminalEmulatorControl_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == '\r')
        {
            OnCommandEntered(InputText);
            inputBuffer.Clear();
        }
        else
        {
            inputBuffer.Append(e.KeyChar);
        }

        RefreshTerminal();
        e.Handled = true;
    }

    protected virtual void OnCommandEntered(string command)
    {
        CommandEntered?.Invoke(this, command);
    }

    public void AppendText(string text)
    {
        OutputLength += text.Length;
        base.AppendText(text);
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
        Text = $"{Text.Substring(0, outputLength)}{inputBuffer}";
        SelectionStart = Text.Length;
    }
}