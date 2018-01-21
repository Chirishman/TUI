using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace TuiBase.Console
{
    public struct Coordinates
    {
        public readonly short X;
        public readonly short Y;

        public static Coordinates operator +(Coordinates a, Coordinates b)
        {
            return new Coordinates(
                x: (short)(a.X + b.X),
                y: (short)(a.Y + b.Y)
            );
        }

        public static Coordinates operator -(Coordinates a, Coordinates b)
        {
            return new Coordinates(
                x: (short)(a.X - b.X),
                y: (short)(a.Y - b.Y)
            );
        }


        public Coordinates(short x, short y)
        {
            X = x;
            Y = y;
        }

        public Coordinates(int x, int y)
        {
            X = (short)x;
            Y = (short)y;
        }

        public Coordinates AddY(int offset)
        {
            return new Coordinates(X, (short)(Y + offset));
        }


        public Coordinates AddX(int offset)
        {
            return new Coordinates((short)(X + offset), Y);
        }




    }
}
