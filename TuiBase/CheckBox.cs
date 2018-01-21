using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TuiBase.Console;

namespace TuiBase
{
    public class CheckBox : Control
    {

        public bool Checked { get; set; }



        protected override void OnPaint()
        {
            string text = Text ?? string.Empty;


            uint w;
            Coordinates pos = new Coordinates(Location.X, Location.Y);

            text = string.Format("[{0}] {1}", Checked ? "X" : " ", text.Length > Size.X - 4 ? text.Substring(0, Size.X - 4) : text.PadRight(Size.X - 4));
            Cons.WriteConsoleOutputCharacterEncoded(text, pos, Foreground, Background);

        }


        protected override void OnActivate()
        {
            base.OnActivate();
            Cons.CursorPosition = Location.AddX(1);
            Cons.CursorVisible = true;
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();
            Cons.CursorVisible = false;
        }

        protected override bool OnKeyPress(ConsoleKeyInfo keyInfo)
        {
            if (base.OnKeyPress(keyInfo))
                return true;
            else
            {
                Coordinates pos = new Coordinates(Location.X + 1, Location.Y);

                if (keyInfo.Key == ConsoleKey.Spacebar)
                {
                    Checked = !Checked;
                    Cons.WriteConsoleOutputCharacterEncoded(Checked ? "X" : " ", pos, Foreground, Background);
                    return true;
                }
                return false;
            }
        }
    }
}
