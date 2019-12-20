using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using EasyTabs;

namespace Multi_Window_SSH_Client {
    static class Program {
        public static AppContainer container = new AppContainer();
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            Application.EnableVisualStyles();

            if(args.Length > 0) {
                container.Tabs.Add(
                    new TitleBarTab(container) {
                        Content = new Main(args[0]) {
                            Text = "SSH Instance"
                        }
                    }
                );
            }
            else {
                container.Tabs.Add(
                    new TitleBarTab(container) {
                        Content = new Main {
                            Text = "SSH Instance"
                        }
                    }
                );
            }

            // Set initial tab the first one
            container.SelectedTabIndex = 0;

            // Create tabs and start application
            TitleBarTabsApplicationContext applicationContext = new TitleBarTabsApplicationContext();
            applicationContext.Start(container);
            Application.Run(applicationContext);
        }
    }
}
