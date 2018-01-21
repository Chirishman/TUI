using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TuiBase.Console;

namespace Tui.TextConsole
{

    /// <summary>
    /// Summary description for ClearConsole.
    /// </summary><BR/>
    internal class ScreenBufferBackupRecord : IScreenBufferBackupRecord
    {


        private Coord _coord;
        private Coord _size;
        private string[] _characters;
        private ushort[][] _attributes;



        public ScreenBufferBackupRecord(Coord coord, Coord size)
        {
            _characters = new string[size.Y];
            _attributes = new ushort[size.Y][];
            _size = size;
            _coord = coord;

          


            for (short y = 0; y < size.Y; y++)
            {
                uint read;
                StringBuilder sb = new StringBuilder(size.X);
                _attributes[y] = new ushort[size.X];
                Cons.ReadConsoleOutputCharacter(Cons.ConsoleHandle, sb, (uint)size.X, coord.AddY(y), out read);
                Cons.ReadConsoleOutputAttribute(Cons.ConsoleHandle, _attributes[y], (uint)size.X, coord.AddY(y), out read);
                _characters[y] = sb.ToString();
            }


        }

        public void Restore()
        {

            for (short y = 0; y < _size.Y; y++)
            {
                uint written;
                Cons.WriteConsoleOutputCharacter(Cons.ConsoleHandle, _characters[y], (uint)_size.X, _coord.AddY(y), out written);
                Cons.WriteConsoleOutputAttribute(Cons.ConsoleHandle, _attributes[y], (uint)_size.X, _coord.AddY(y), out written);
            }
        }



    }



}
