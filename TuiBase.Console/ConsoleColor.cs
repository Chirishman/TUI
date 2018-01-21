using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuiBase.Console
{
    public enum ConsColor : ushort
    {
        Black = 0x0000,
        DarkBlue = 0x0001,
        DarkGreen = 0x0002,
        DarkRed = 0x0004,
        INTENSITY = 0x0008,

        DarkCyan = DarkBlue | DarkGreen,
        DarkMagenta = DarkBlue | DarkRed,
        DarkYellow = DarkGreen | DarkRed,
        Gray = DarkRed | DarkGreen | DarkBlue,

        DarkGray = Black | INTENSITY,
        Blue = DarkBlue | INTENSITY,
        Green = DarkGreen | INTENSITY,
        Red = DarkRed | INTENSITY,

        Cyan = DarkCyan | INTENSITY,
        Magenta = DarkMagenta | INTENSITY,
        Yellow = DarkYellow | INTENSITY,
        White = Gray | INTENSITY,

    }
}
