using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TuiBase.Console;

namespace TuiBase
{
    public class ComboBox<ValueType> : Control
    {
        public ComboBox(string caption, int captionLength)
        {
            Caption = caption;
            _captionLength = captionLength;
            _boxLength = Math.Max(0, Size.X - captionLength - 2);
        }

        public ComboBox(string caption)
            : this(caption, caption.Length)
        {
        }

        public string Caption { get; private set; }
        private int _captionLength;
        private int _boxLength;


        public IList<ComboBoxItem<ValueType>> Items { get; set; }

        private ValueType _selectedValue;

        public ValueType SelectedValue
        {
            get
            {
                return _selectedValue;
            }
            set
            {
                _selectedValue = value;
                ComboBoxItem<ValueType> item = Items.First(i => object.Equals(i.Value, value));
                if (item != null)
                    Text = item.Text;
            }
        }


        protected override void OnSizeChanged()
        {
            _boxLength = Math.Max(0, Size.X - _captionLength - 2);
        }

        protected override void OnTextChanged()
        {
            _boxLength = Math.Max(0, Size.X - _captionLength - 2);
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            Cons.CursorPosition = Location.AddX(_captionLength + 1);
            Cons.CursorVisible = true;
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();
            Cons.CursorVisible = false;
        }


        protected override void OnPaint()
        {


            Coordinates pos = Location;

            string s = "▐";
            if (!string.IsNullOrEmpty(Caption))
            {
                s = (Caption.Length > _captionLength ? Caption.Substring(0, _captionLength) : Caption.PadRight(_captionLength)) + s;
            }
            Cons.WriteConsoleOutputCharacterEncoded(s, pos, Foreground, Background);

            WriteBoxText(null);

            pos = new Coordinates((short)(Location.X + Size.X - 1), Location.Y);
            Cons.WriteConsoleOutputCharacterEncoded("▼", pos, Background, Foreground);
        }

        private void WriteBoxText(string t)
        {

            string text = t ?? Text ?? string.Empty;
            text = text.Length > _boxLength ? text.Substring(0, _boxLength) : text.PadRight(_boxLength);

            Coordinates pos = new Coordinates((short)(Location.X + _captionLength + 1), Location.Y);
            Cons.WriteConsoleOutputCharacterEncoded(text, pos, Background, Foreground);
        }


        #region Funktionalität


        public Action SelectionChanged { get; set; }

        protected override bool OnKeyPress(ConsoleKeyInfo keyInfo)
        {
            if (base.OnKeyPress(keyInfo))
                return true;
            else
            {
                if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    PopUpMenu popup = new PopUpMenu(string.Empty, Items.Select(i => i.Text).ToArray());
                    popup.Location = Location + new Coordinates(_captionLength, 1);
                    popup.Size = new Coordinates(Math.Max(5, _boxLength + 2), Math.Max(5, Math.Min(10, Items.Count + 2)));
                    if (popup.ShowDialog() == DialogResultType.Ok)
                    {
                        SelectedValue = Items[popup.SelectedItem].Value;
                        Text = Items[popup.SelectedItem].Text;
                        WriteBoxText(Text);
                        if (Items[popup.SelectedItem].Action != null)
                            Items[popup.SelectedItem].Action();
                        if (SelectionChanged != null)
                            SelectionChanged();
                    }

                    return true;
                }
                else
                    return false;
            }
        }
        #endregion
    }
}
