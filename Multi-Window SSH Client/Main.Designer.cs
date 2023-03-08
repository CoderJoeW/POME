namespace POME
{
    partial class Main
    {
        private System.ComponentModel.IContainer components = null;
        private ComponentPro.Net.Terminal.SshTerminalControl sshTerminalControl1;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.sshTerminalControl1 = new ComponentPro.Net.Terminal.SshTerminalControl();
            this.SuspendLayout();
            // 
            // sshTerminalControl1
            // 
            this.sshTerminalControl1.BackColor = System.Drawing.Color.Magenta;
            this.sshTerminalControl1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.sshTerminalControl1.CursorMouse = System.Windows.Forms.Cursors.Arrow;
            this.sshTerminalControl1.CursorText = System.Windows.Forms.Cursors.IBeam;
            this.sshTerminalControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sshTerminalControl1.Location = new System.Drawing.Point(0, 0);
            this.sshTerminalControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.sshTerminalControl1.Name = "sshTerminalControl1";
            this.sshTerminalControl1.Size = new System.Drawing.Size(1050, 720);
            this.sshTerminalControl1.TabIndex = 0;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 720);
            this.Controls.Add(this.sshTerminalControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main";
            this.Load += new System.EventHandler(this.Main_Load);
            this.ResumeLayout(false);

        }
    }
}