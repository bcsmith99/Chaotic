using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Chaotic.User
{
    public class UserKeyboardKey
    {
        public KeyboardModifier Modifier { get; set; }
        public Key InputKey { get; set; }

        public string Display { get; set; } = String.Empty;

        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is UserKeyboardKey))
                return false;

            return ((UserKeyboardKey)obj).InputKey == this.InputKey && ((UserKeyboardKey)obj).Modifier == this.Modifier;
        }
    }

    public enum KeyboardModifier
    {
        None = 0,
        Alt = 1,
        Ctrl = 2,
        Shift = 3
    }
}
