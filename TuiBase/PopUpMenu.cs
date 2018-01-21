using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TuiBase.Console;

namespace TuiBase
{
    public class PopUpMenu : Window
    {
        TextPanel _textPanel;
        IList<PopUpMenuItem> _menuItems;

        public PopUpMenu(string caption, string[] items)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            int longestLine = items.Max(item => item.Length);
            _textPanel = new TextPanel();
            _textPanel.Size = new Coordinates(longestLine,Math.Max(3, items.Length));
            _textPanel.Lines = items;
            Size = _textPanel.Size + new Coordinates(2, 2);
            AddControl(_textPanel, new Coordinates(0, 0));
            Text = caption;
            _textPanel.ActiveLine = 0;
          
        }

        public PopUpMenu(string caption, IList<PopUpMenuItem> items)
            : this(caption, items.Select(i => i.Text).ToArray())
        {
            _menuItems = items;
        }


        protected override bool OnKeyPress(ConsoleKeyInfo keyInfo)
        {
            if (base.OnKeyPress(keyInfo))
            {
                return true;
            }
            else
            {
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    if (_menuItems != null)
                        _menuItems[SelectedItem].Action();
                    DialogResult = TuiBase.DialogResultType.Ok;
                    return true;
                }
                else
                    return false;
            }
        }


        public int SelectedItem { get { return _textPanel.ActiveLine.Value; } }
    }
}
