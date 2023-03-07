namespace POME
{
    partial class Main
    {
        private System.ComponentModel.IContainer components = null;

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
            this.panel1 = new System.Windows.Forms.Panel();
            this.portInfo = new System.Windows.Forms.TextBox();
            this.passwordInfo = new System.Windows.Forms.TextBox();
            this.usernameInfo = new System.Windows.Forms.TextBox();
            this.hostInfo = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.sshConsole = new System.Windows.Forms.TextBox();
            this.commandInput = new System.Windows.Forms.TextBox();
            this.tmpOutput = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.portInfo);
            this.panel1.Controls.Add(this.passwordInfo);
            this.panel1.Controls.Add(this.usernameInfo);
            this.panel1.Controls.Add(this.hostInfo);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.TabIndex = 2;
            // 
            // portInfo
            // 
            this.portInfo.Location = new System.Drawing.Point(3, 33);
            this.portInfo.Name = "portInfo";
            this.portInfo.Size = new System.Drawing.Size(100, 20);
            this.portInfo.TabIndex = 8;
            this.portInfo.Text = "22";
            // 
            // passwordInfo
            // 
            this.passwordInfo.Location = new System.Drawing.Point(215, 7);
            this.passwordInfo.Name = "passwordInfo";
            this.passwordInfo.Size = new System.Drawing.Size(100, 20);
            this.passwordInfo.TabIndex = 7;
            this.passwordInfo.Text = "Password";
            // 
            // usernameInfo
            // 
            this.usernameInfo.Location = new System.Drawing.Point(109, 7);
            this.usernameInfo.Name = "usernameInfo";
            this.usernameInfo.Size = new System.Drawing.Size(100, 20);
            this.usernameInfo.TabIndex = 6;
            this.usernameInfo.Text = "Username";
            // 
            // hostInfo
            // 
            this.hostInfo.Location = new System.Drawing.Point(3, 7);
            this.hostInfo.Name = "hostInfo";
            this.hostInfo.Size = new System.Drawing.Size(100, 20);
            this.hostInfo.TabIndex = 5;
            this.hostInfo.Text = "Host";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(321, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(149, 20);
            this.button1.TabIndex = 4;
            this.button1.Text = "Connect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(0, 59);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // sshConsole
            // 
            this.sshConsole.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sshConsole.Location = new System.Drawing.Point(0, 56);
            this.sshConsole.Multiline = true;
            this.sshConsole.Name = "sshConsole";
            this.sshConsole.Size = new System.Drawing.Size(712, 412);
            this.sshConsole.TabIndex = 3;
            // 
            // commandInput
            // 
            this.commandInput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.commandInput.Location = new System.Drawing.Point(0, 468);
            this.commandInput.Name = "commandInput";
            this.commandInput.Size = new System.Drawing.Size(712, 20);
            this.commandInput.TabIndex = 4;
            this.commandInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.commandInput_KeyDown);
            // 
            // tmpOutput
            // 
            this.tmpOutput.Dock = System.Windows.Forms.DockStyle.Right;
            this.tmpOutput.Location = new System.Drawing.Point(712, 56);
            this.tmpOutput.Multiline = true;
            this.tmpOutput.Name = "tmpOutput";
            this.tmpOutput.Size = new System.Drawing.Size(493, 432);
            this.tmpOutput.TabIndex = 5;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1205, 488);
            this.Controls.Add(this.sshConsole);
            this.Controls.Add(this.tmpOutput);
            this.Controls.Add(this.commandInput);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.Load += new System.EventHandler(this.Main_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox portInfo;
        private System.Windows.Forms.TextBox passwordInfo;
        private System.Windows.Forms.TextBox usernameInfo;
        private System.Windows.Forms.TextBox hostInfo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox sshConsole;
        private System.Windows.Forms.TextBox commandInput;
        private System.Windows.Forms.TextBox tmpOutput;
    }
}