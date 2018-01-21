using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TuiBase
{
    public class KeyPressEventArgs : EventArgs
    {
        public bool Handled { get; set; }
        public ConsoleKeyInfo KeyInfo { get; private set; }

        public KeyPressEventArgs(ConsoleKeyInfo keyInfo)
        {
            KeyInfo = keyInfo;
        }

    }
}
