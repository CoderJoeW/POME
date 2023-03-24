using System;
using System.Drawing;
using System.Windows.Forms;

public class XTermBuffered : RichTextBox
{
    private Image buffer;

    protected override CreateParams CreateParams
    {
        get
        {
            CreateParams createParams = base.CreateParams;
            createParams.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
            return createParams;
        }
    }

    public XTermBuffered() : base()
    {
        this.DoubleBuffered = true;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        if (buffer == null || buffer.Size != this.ClientSize)
        {
            if (buffer != null)
                buffer.Dispose();

            buffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
        }

        using (Graphics bufferGraphics = Graphics.FromImage(buffer))
        {
            Rectangle rect = new Rectangle(0, 0, this.ClientSize.Width, this.ClientSize.Height);
            bufferGraphics.FillRectangle(new SolidBrush(this.BackColor), rect);
            base.OnPaint(new PaintEventArgs(bufferGraphics, rect));
        }

        e.Graphics.DrawImageUnscaled(buffer, 0, 0);
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && buffer != null)
        {
            buffer.Dispose();
        }

        base.Dispose(disposing);
    }
}
