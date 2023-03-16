using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Collections.Generic;

public enum TerminalMode
{
    VT100,
    VT220,
    XTerm
}

public class TerminalEmulatorControl : RichTextBox
{
    private List<char> inputBuffer;

    public int CharWidth => TextRenderer.MeasureText("M", Font).Width;
    public int CharHeight => TextRenderer.MeasureText("M", Font).Height;

    public string InputText
    {
        get => new string(inputBuffer.ToArray());
        set
        {
            inputBuffer.Clear();
            inputBuffer.AddRange(value);
            RefreshTerminal();
        }
    }

    public TerminalMode TerminalMode { get; set; } = TerminalMode.XTerm;

    public int OutputLength { get; private set; }
    public string Prompt { get; set; } = "";

    public event EventHandler<string> CommandEntered;
    public event EventHandler InterruptRequested;

    private Form mainForm;

    public TerminalEmulatorControl(Form mainForm)
    {
        this.mainForm = mainForm;
        ReadOnly = false;
        WordWrap = false;
        inputBuffer = new List<char>();

        Font = new Font("Consolas", 12, FontStyle.Regular);

        BackColor = Color.Black;
        ForeColor = Color.White;

        KeyPress += TerminalEmulatorControl_KeyPress;
        KeyDown += TerminalEmulatorControl_KeyDown;
        MouseUp += TerminalEmulatorControl_MouseUp;
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
            inputBuffer.Add(e.KeyChar);
        }

        RefreshTerminal();
        e.Handled = true;
    }

    private void TerminalEmulatorControl_KeyDown(object sender, KeyEventArgs e)
    {

    }

    private void TerminalEmulatorControl_MouseUp(object sender, MouseEventArgs e)
    {
        if (SelectedText.Length > 0)
        {
            Clipboard.SetText(SelectedText);
        }
    }

    protected virtual void OnCommandEntered(string command)
    {
        CommandEntered?.Invoke(this, command);
    }

    public void ClearOutput()
    {
        OutputLength = 0;
        RefreshTerminal();
    }

    public new void AppendText(string text)
    {
        string processedText = EscapeSequenceHandler.HandleEscapeSequences(text, TerminalMode, this, mainForm);
        Console.WriteLine(processedText);
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
        string processedText = EscapeSequenceHandler.HandleEscapeSequences($"{Text.Substring(0, outputLength)}{Prompt}{new string(inputBuffer.ToArray())}", TerminalMode, this, mainForm);
        Text = processedText;
        SelectionStart = Text.Length;
    }
}
