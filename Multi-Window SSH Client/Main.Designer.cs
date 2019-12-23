namespace Multi_Window_SSH_Client {
    partial class Main {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
			this.sshTerminalControl1 = new ComponentPro.Net.Terminal.SshTerminalControl();
			this.panel1 = new System.Windows.Forms.Panel();
			this.portInfo = new System.Windows.Forms.TextBox();
			this.passwordInfo = new System.Windows.Forms.TextBox();
			this.usernameInfo = new System.Windows.Forms.TextBox();
			this.hostInfo = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
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
			this.sshTerminalControl1.Name = "sshTerminalControl1";
			this.sshTerminalControl1.Size = new System.Drawing.Size(700, 468);
			this.sshTerminalControl1.TabIndex = 1;
			this.sshTerminalControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ShowExtraFunctionsMenu);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.portInfo);
			this.panel1.Controls.Add(this.passwordInfo);
			this.panel1.Controls.Add(this.usernameInfo);
			this.panel1.Controls.Add(this.hostInfo);
			this.panel1.Controls.Add(this.button1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(700, 56);
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
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(700, 468);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.sshTerminalControl1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "Main";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Main";
			this.Load += new System.EventHandler(this.Main_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion
        private ComponentPro.Net.Terminal.SshTerminalControl sshTerminalControl1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox portInfo;
		private System.Windows.Forms.TextBox passwordInfo;
		private System.Windows.Forms.TextBox usernameInfo;
		private System.Windows.Forms.TextBox hostInfo;
	}
}