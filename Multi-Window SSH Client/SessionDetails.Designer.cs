using System.Windows.Forms;

namespace POME
{
    partial class SessionDetails
    {
        private TableLayoutPanel tblLayoutSessionDetails;
        private Label lblHost;
        private Label lblUsername;
        private Label lblPassword;
        private Label lblPort;
        private TextBox txtHost;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private TextBox txtPort;
        private Button btnSubmitSessionDetails;
        private void InitializeComponent()
        {
            this.tblLayoutSessionDetails = new System.Windows.Forms.TableLayoutPanel();
            this.lblHost = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblPort = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtPort = new System.Windows.Forms.TextBox();
            this.btnSubmitSessionDetails = new System.Windows.Forms.Button();
            this.tblLayoutSessionDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // tblLayoutSessionDetails
            // 
            this.tblLayoutSessionDetails.ColumnCount = 2;
            this.tblLayoutSessionDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tblLayoutSessionDetails.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tblLayoutSessionDetails.Controls.Add(this.lblHost, 0, 0);
            this.tblLayoutSessionDetails.Controls.Add(this.lblUsername, 0, 1);
            this.tblLayoutSessionDetails.Controls.Add(this.lblPassword, 0, 2);
            this.tblLayoutSessionDetails.Controls.Add(this.lblPort, 0, 3);
            this.tblLayoutSessionDetails.Controls.Add(this.txtHost, 1, 0);
            this.tblLayoutSessionDetails.Controls.Add(this.txtUsername, 1, 1);
            this.tblLayoutSessionDetails.Controls.Add(this.txtPassword, 1, 2);
            this.tblLayoutSessionDetails.Controls.Add(this.txtPort, 1, 3);
            this.tblLayoutSessionDetails.Controls.Add(this.btnSubmitSessionDetails, 1, 4);
            this.tblLayoutSessionDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblLayoutSessionDetails.Location = new System.Drawing.Point(0, 0);
            this.tblLayoutSessionDetails.Name = "tblLayoutSessionDetails";
            this.tblLayoutSessionDetails.Padding = new System.Windows.Forms.Padding(20);
            this.tblLayoutSessionDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblLayoutSessionDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblLayoutSessionDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblLayoutSessionDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblLayoutSessionDetails.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tblLayoutSessionDetails.Size = new System.Drawing.Size(772, 479);
            this.tblLayoutSessionDetails.TabIndex = 0;
            // 
            // lblHost
            // 
            this.lblHost.AutoSize = true;
            this.lblHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHost.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblHost.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblHost.Location = new System.Drawing.Point(23, 20);
            this.lblHost.Name = "lblHost";
            this.lblHost.Size = new System.Drawing.Size(213, 87);
            this.lblHost.TabIndex = 0;
            this.lblHost.Text = "Host:";
            this.lblHost.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblUsername.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblUsername.Location = new System.Drawing.Point(23, 107);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(213, 87);
            this.lblUsername.TabIndex = 1;
            this.lblUsername.Text = "Username:";
            this.lblUsername.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblPassword.Location = new System.Drawing.Point(23, 194);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(213, 87);
            this.lblPassword.TabIndex = 2;
            this.lblPassword.Text = "Password:";
            this.lblPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPort
            // 
            this.lblPort.AutoSize = true;
            this.lblPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblPort.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblPort.Location = new System.Drawing.Point(23, 281);
            this.lblPort.Name = "lblPort";
            this.lblPort.Size = new System.Drawing.Size(213, 87);
            this.lblPort.TabIndex = 3;
            this.lblPort.Text = "Port:";
            this.lblPort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtHost
            // 
            this.txtHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHost.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtHost.Location = new System.Drawing.Point(242, 23);
            this.txtHost.Name = "txtHost";
            this.txtHost.Size = new System.Drawing.Size(507, 35);
            this.txtHost.TabIndex = 4;
            // 
            // txtUsername
            // 
            this.txtUsername.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtUsername.Location = new System.Drawing.Point(242, 110);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(507, 35);
            this.txtUsername.TabIndex = 5;
            // 
            // txtPassword
            // 
            this.txtPassword.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtPassword.Location = new System.Drawing.Point(242, 197);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(507, 35);
            this.txtPassword.TabIndex = 6;
            // 
            // txtPort
            // 
            this.txtPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtPort.Location = new System.Drawing.Point(242, 284);
            this.txtPort.Name = "txtPort";
            this.txtPort.Size = new System.Drawing.Size(507, 35);
            this.txtPort.TabIndex = 7;
            // 
            // btnSubmitSessionDetails
            // 
            this.btnSubmitSessionDetails.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnSubmitSessionDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(144)))), ((int)(((byte)(255)))));
            this.btnSubmitSessionDetails.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSubmitSessionDetails.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.btnSubmitSessionDetails.ForeColor = System.Drawing.Color.White;
            this.btnSubmitSessionDetails.Location = new System.Drawing.Point(557, 386);
            this.btnSubmitSessionDetails.Name = "btnSubmitSessionDetails";
            this.btnSubmitSessionDetails.Size = new System.Drawing.Size(192, 54);
            this.btnSubmitSessionDetails.TabIndex = 8;
            this.btnSubmitSessionDetails.Text = "Submit";
            this.btnSubmitSessionDetails.UseVisualStyleBackColor = false;
            this.btnSubmitSessionDetails.Click += new System.EventHandler(this.btnSubmitSessionDetails_Click);
            // 
            // SessionDetails
            // 
            this.ClientSize = new System.Drawing.Size(772, 479);
            this.Controls.Add(this.tblLayoutSessionDetails);
            this.Name = "SessionDetails";
            this.Text = "Session Details";
            this.tblLayoutSessionDetails.ResumeLayout(false);
            this.tblLayoutSessionDetails.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
