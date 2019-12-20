namespace POME {
	partial class ErrorWindow {
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
			this.error_msg = new System.Windows.Forms.RichTextBox();
			this.SuspendLayout();
			// 
			// error_msg
			// 
			this.error_msg.Dock = System.Windows.Forms.DockStyle.Fill;
			this.error_msg.Location = new System.Drawing.Point(0, 0);
			this.error_msg.Name = "error_msg";
			this.error_msg.Size = new System.Drawing.Size(255, 83);
			this.error_msg.TabIndex = 0;
			this.error_msg.Text = "";
			// 
			// ErrorWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Info;
			this.ClientSize = new System.Drawing.Size(255, 83);
			this.Controls.Add(this.error_msg);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ErrorWindow";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ErrorWindow";
			this.Load += new System.EventHandler(this.ErrorWindow_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.RichTextBox error_msg;
	}
}