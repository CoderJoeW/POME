using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Multi_Window_SSH_Client {
    public static class InputManager {
        public static bool CheckControlDown() {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)) {
                return true;
            }
            else {
                return false;
            }
        }

        public static bool CheckShiftDown() {
            if(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) {
                return true;
            }
            else {
                return false;
            }
        }
    }
}
