namespace POME
{
    partial class Main
    {
        private System.ComponentModel.IContainer components = null;
        private ComponentPro.Net.Terminal.SshTerminalControl sshTerminalControl1;
        private System.Windows.Forms.TableLayoutPanel tblConnectionInfo;
        private System.Windows.Forms.Label lblHost;
        private System.Windows.Forms.Label lblPort;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.TextBox txtPort;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnConnect;

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
            this.tblConnectionInfo = new System.Windows.Forms.TableLayoutPanel();
            this.lblHost = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();

            this.tblConnectionInfo.SuspendLayout();
            this.SuspendLayout();

            // 
            // sshTerminalControl1
            // 
            this.sshTerminalControl1.BackColor = System.Drawing.Color.Magenta;
            this.sshTerminalControl1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.sshTerminalControl1.CursorMouse = System.Windows.Forms.Cursors.Arrow;
            this.sshTerminalControl1.CursorText = System.Windows.Forms.Cursors.IBeam;
            this.sshTerminalControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sshTerminalControl1.Location = new System.Drawing.Point(0, 88);
            this.sshTerminalControl1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.sshTerminalControl1.Name = "sshTerminalControl1";
            this.sshTerminalControl1.Size = new System.Drawing.Size(1050, 632);
            this.sshTerminalControl1.TabIndex = 0;
            this.sshTerminalControl1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ShowExtraFunctionsMenu);

            // 
            // tblConnectionInfo
            // 
            this.tblConnectionInfo.AutoSize = true;
            this.tblConnectionInfo.ColumnCount = 5;
            this.tblConnectionInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblConnectionInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblConnectionInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblConnectionInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblConnectionInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tblConnectionInfo.Controls.Add(this.lblHost, 0, 0);
            this.tblConnectionInfo.Controls.Add(this.lblPort, 2, 0);
            this.tblConnectionInfo.Controls.Add(this.lblUsername, 0, 1);
            this.tblConnectionInfo.Controls.Add(this.lblPassword, 2, 1);
            this.tblConnectionInfo.Controls.Add(this.txtHost, 1, 0);
            this.tblConnectionInfo.Controls.Add(this.txtPort, 3, 0);
            this.tblConnectionInfo.Controls.Add(this.txtUsername, 1, 1);
            this.tblConnectionInfo.Controls.Add(this.txtPassword, 3, 1);
            this.tblConnectionInfo.Controls.Add(this.btnConnect, 4, 1);
            this.tblConnectionInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.tblConnectionInfo.Location = new System.Drawing.Point(0, 0);
            this.tblConnectionInfo.Name = "tblConnectionInfo";
            this.tblConnectionInfo.RowCount = 2;
            this.tblConnectionInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblConnectionInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tblConnectionInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tblConnectionInfo.Size = new System.Drawing.Size(1050, 120);
            this.tblConnectionInfo.TabIndex = 1;

            // 
            // lblHost
            // 
            this.lblHost.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblHost.AutoSize = true;
            this.lblHost.Location = new System.Drawing.Point(45, 15);
            this.lblHost.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(47, 20);
            this.lblHost.TabIndex = 0;
            this.lblHost.Text = "Host:";

            // 
            // lblPort
            // 
            this.lblPort.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblPort.AutoSize = true;
            this.lblPort.Location = new System.Drawing.Point(474, 15);
            this.lblPort.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(42, 20);
            this.lblPort.TabIndex = 1;
            this.lblPort.Text = "Port:";

            // 
            // lblUsername
            // 
            this.lblUsername.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblUsername.AutoSize = true;
            this.lblUsername.Location = new System.Drawing.Point(5, 67);
            this.lblUsername.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(87, 20);
            this.lblUsername.TabIndex = 2;
            this.lblUsername.Text = "Username:";

            // 
            // lblPassword
            // 
            this.lblPassword.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(434, 67);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(82, 20);
            this.lblPassword.TabIndex = 3;
            this.lblPassword.Text = "Password:";

            // 
            // txtHost
            // 
            this.txtHost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHost.Location = new System.Drawing.Point(102, 12);
            this.txtHost.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(322, 26);
            this.txtHost.TabIndex = 4;
            this.txtHost.Text = "Host";
            this.txtHost.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

            // 
            // txtPort
            // 
            this.txtPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPort.Location = new System.Drawing.Point(524, 12);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(326, 26);
            this.txtPort.TabIndex = 5;
            this.txtPort.Text = "22";
            this.txtPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));


            // 
            // txtUsername
            // 
            this.txtUsername.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUsername.Location = new System.Drawing.Point(102, 64);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(322, 26);
            this.txtUsername.TabIndex = 6;
            this.txtUsername.Text = "Username";
            this.txtUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));


            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Location = new System.Drawing.Point(526, 64);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(322, 26);
            this.txtPassword.TabIndex = 7;
            this.txtPassword.Text = "Password";
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));


            // 
            // btnConnect
            // 
            this.btnConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnConnect.Location = new System.Drawing.Point(858, 61);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnConnect.Name = "btnConnect";
            this.tblConnectionInfo.SetRowSpan(this.btnConnect, 2);
            this.btnConnect.Size = new System.Drawing.Size(187, 30);
            this.btnConnect.TabIndex = 8;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.button1_Click);
            this.btnConnect.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));


            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 720);
            this.Controls.Add(this.sshTerminalControl1);
            this.Controls.Add(this.tblConnectionInfo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Main";
            this.Load += new System.EventHandler(this.Main_Load);
            this.tblConnectionInfo.ResumeLayout(false);
            this.tblConnectionInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}




