using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using POME;

public enum TerminalMode
{
    VT100,
    VT220,
    XTerm
}

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

    public TerminalMode TerminalMode { get; set; } = TerminalMode.XTerm;

    public int OutputLength { get; private set; }
    public string Prompt { get; set; } = "> ";

    public event EventHandler<string> CommandEntered;
    public event EventHandler InterruptRequested;

    private Main mainForm;

    public TerminalEmulatorControl(Main mainForm)
    {
        this.mainForm = mainForm;
        ReadOnly = true;
        WordWrap = false;
        inputBuffer = new StringBuilder();

        KeyPress += TerminalEmulatorControl_KeyPress;
        KeyDown += TerminalEmulatorControl_KeyDown;
        ShortcutsEnabled = false;
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
        else if (e.KeyCode == Keys.C && e.Control)
        {
            if (SelectedText.Length > 0)
            {
                Clipboard.SetText(SelectedText);
            }
            else
            {
                OnCommandEntered("CTRL+C");
            }
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.L && e.Control)
        {
            Clear();
            RefreshTerminal();
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.V && e.Control)
        {
            inputBuffer.Append(Clipboard.GetText());
            RefreshTerminal();
            e.Handled = true;
        }
        else if (e.KeyCode == Keys.A && e.Control)
        {
            SelectAll();
            e.Handled = true;
        }
        // Additional key handling can be added here if necessary
    }

    protected virtual void OnCommandEntered(string command)
    {
        if (command.Trim().ToLower() == "clear")
        {
            ClearOutput();
        }
        else if (command == "CTRL+C")
        {
            InterruptRequested?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            CommandEntered?.Invoke(this, command);
        }
    }

    public void ClearOutput()
    {
        OutputLength = 0;
        RefreshTerminal();
    }

    public new void AppendText(string text)
    {
        string processedText = EscapeSequenceHandler.HandleEscapeSequences(text, TerminalMode, this, mainForm);
        base.AppendText(processedText);
        OutputLength += processedText.Length;
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
        Text = $"{Text.Substring(0, outputLength)}{Prompt}{inputBuffer}";
        SelectionStart = Text.Length;
    }

}