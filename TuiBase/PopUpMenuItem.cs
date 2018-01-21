using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TuiBase
{
    public class PopUpMenuItem
    {
        public String Text { get;private set; }
        public Action Action { get;private set; }

        public PopUpMenuItem(string text, Action action)
        {
            Text = text;
            Action = action;
        }
    }
}
