using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POME {
	public partial class ErrorWindow : Form {
		public ErrorWindow() {
			InitializeComponent();
		}

		private void ErrorWindow_Load(object sender, EventArgs e) {
			error_msg.ReadOnly = true;
		}

		public void SetErrorMessage(string msg) {
			error_msg.Text = msg;
		}
	}
}
