using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using EasyTabs;

namespace POME {
    public partial class AppContainer : TitleBarTabs {
        public AppContainer() {
            InitializeComponent();

            AeroPeekEnabled = true;
            TabRenderer = new ChromeTabRenderer(this);
        }

        private void AppContainer_Load(object sender, EventArgs e) {
            //Causes error if not present in compiler
        }

        public override TitleBarTab CreateTab() {
            return new TitleBarTab(this) {
                // The content will be an instance of another Form
                // In our example, we will create a new instance of the Form1
                Content = new Main {
                    Text = "SSH Instance"
                }
            };
        }
    }
}
