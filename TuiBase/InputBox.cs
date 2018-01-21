using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TuiBase.Console;

namespace TuiBase
{
    public class InputBox : Window
    {

        TextBox _txt;

        public InputBox(string title, string label, int boxWidth)
        {
            int width = Math.Max(title.Length + 4, label.Length + boxWidth + 5);


            Text = title;
            _txt = new TuiBase.TextBox(label, label.Length);
            _txt.Size = new Coordinates(label.Length + boxWidth + 1, 1);
            _txt.Location = new Coordinates(1, 0);

            Location = new Coordinates(5, 5);
            AddControl(_txt, new Coordinates(1, 1));
            Size = new Coordinates(width, 4);
        }

        public static string Show(string title, string label, int boxWidth)
        {
            return Show(title, label, boxWidth, string.Empty);
        }

        public static string Show(string title, string label, int boxWidth, string boxText)
        {
            InputBox i = new InputBox(title, label, boxWidth);
            i._txt.Text = boxText;
            if (i.ShowDialog() == DialogResultType.Ok)
                return i._txt.Text;
            else
                return null;
        }


    }
}
