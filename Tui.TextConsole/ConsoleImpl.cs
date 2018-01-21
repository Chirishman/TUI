using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TuiBase.Console;

namespace Tui.TextConsole
{
    public class ConsoleImpl : IConsole
    {

        public ConsoleImpl()
        {
            Console.TreatControlCAsInput = false;
        }

        public ConsoleKeyInfo ReadKey()
        {
            return Console.ReadKey(true);
        }


        public bool CursorVisible
        {
            set
            {
                Console.CursorVisible = value;
            }
        }

        public void WriteConsoleOutputCharacterEncoded(string s, Coordinates location, ConsColor foreground, ConsColor background)
        {
            uint w;
            var coord = Map(location);
            Cons.WriteConsoleOutputCharacterEncoded(s, (uint)s.Length, coord, out w);
            Cons.FillConsoleOutputAttribute(Cons.ConsoleHandle, Attr(foreground, background), (uint)s.Length, coord, out  w);
        }

        public static ushort Attr(ConsColor foreground, ConsColor background)
        {
            return (ushort)((ushort)foreground + ((ushort)background << 4));
        }


        public IScreenBufferBackupRecord Backup(Coordinates location, Coordinates size)
        {
            return new ScreenBufferBackupRecord(Map(location), Map(size));
        }

        public void ClearRegion(Coordinates location, Coordinates size)
        {
            Cons.CHAR_INFO[] chars = new Cons.CHAR_INFO[size.X * size.Y];

            for (int i = 0; i < chars.Length; i++)
            {
                chars[i].AsciiChar = ' ';
                chars[i].Attributes = ConsoleColor.Black;
            }

            Cons.SMALL_RECT region;
            region.Top = location.Y;
            region.Bottom = (short)(region.Top + size.Y);
            region.Left = location.X;
            region.Right = (short)(region.Left + size.X);

            Cons.WriteConsoleOutput(Cons.ConsoleHandle, chars, Map(size), new Coord(0, 0), ref region);

        }


        public void Panic(Exception x)
        {
            Console.SetCursorPosition(0, 0);
            Console.BackgroundColor = System.ConsoleColor.Yellow;
            Console.ForegroundColor = System.ConsoleColor.Red;

            Console.WriteLine("**********************************************************");
            Console.WriteLine("*****************UNHANDLED EXCEPTION**********************");
            Console.WriteLine("**********************************************************");

            Console.WriteLine(x.ToString());

            Console.WriteLine("**********************************************************");
            Console.WriteLine("**********************************************************");
            Console.WriteLine("**********************************************************");
            Console.WriteLine("PRESS ANY KEY TO EXIT");

            Console.ReadKey();

        }


        Coord Map(Coordinates coord)
        {
            return new Coord(coord.X, coord.Y);
        }



        public Coordinates CursorPosition
        {
            get
            {
                return new Coordinates(Console.CursorLeft, Console.CursorTop);
            }
            set
            {
                Console.SetCursorPosition(value.X, value.Y);
            }
        }
    }
}
