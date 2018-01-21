using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TuiBase.Console;

namespace TuiBase
{
    public class MessageBox : Window
    {
        TextPanel _textPanel;
        public MessageBox(string title, string[] lines)
        {
            int width = lines.Max(l => l.Length);
            Text = title;
            _textPanel = new TuiBase.TextPanel();
            _textPanel.Size = new Coordinates(Math.Max(3, width),Math.Max(3, lines.Length));
            _textPanel.Foreground = ConsColor.White;
            _textPanel.Background = ConsColor.Red;
            _textPanel.Lines = lines;

            Location = new Coordinates(5, 5);
            AddControl(_textPanel, new Coordinates(0, 0));
            Size = _textPanel.Size + new Coordinates(2, 2);
        }

        public static DialogResultType Show(string title, string[] lines)
        {
            MessageBox b = new MessageBox(title,lines);
            return b.ShowDialog();
        }

        public static DialogResultType Show(string title, string[] lines, ConsColor background)
        {
            MessageBox b = new MessageBox(title, lines);
            b.Foreground = ConsColor.Gray;
            b.Background = background;
            return b.ShowDialog();
        }

        public static DialogResultType Show(string title, string[] lines, ConsColor background, ConsColor foreground)
        {
            MessageBox b = new MessageBox(title, lines);
            b.Foreground = foreground;
            b.Background = background;
            return b.ShowDialog();
        }


    }
}
