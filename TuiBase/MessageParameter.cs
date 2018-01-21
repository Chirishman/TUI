using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TuiBase
{
    public class MessageParameter
    {
        public ConsoleKeyInfo KeyInfo { get; private set; }

        public MessageParameter(ConsoleKeyInfo keyInfo)
        {
            KeyInfo = keyInfo;
        }
    }


}
