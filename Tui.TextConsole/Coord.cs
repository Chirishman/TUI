using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Tui.TextConsole
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Coord
    {
        public short X;
        public short Y;

        public static Coord operator +(Coord a, Coord b)
        {
            return new Coord() { X = (short)(a.X + b.X), Y = (short)(a.Y + b.Y) };
        }

        public static Coord operator -(Coord a, Coord b)
        {
            return new Coord() { X = (short)(a.X - b.X), Y = (short)(a.Y - b.Y) };
        }


        public Coord(short x, short y)
        {
            X = x;
            Y = y;
        }

        public Coord(int x, int y)
        {
            X = (short)x;
            Y = (short)y;
        }

        public Coord AddY(int offset)
        {
            return new Coord(X, (short)(Y + offset));
        }


       
 

 


    }
}
