using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TuiBase.Console;

namespace TuiBase
{
    public class TextBox : Control
    {
        public TextBox(string caption, int captionLength)
        {
            Caption = caption;
            _captionLength = captionLength;
            _boxLength = Math.Max(0, Size.X - captionLength);
        }


        public TextBox(string caption)
            : this(caption, caption.Length)
        { }

        public bool NumberInput { get; set; }

        public string Caption { get; private set; }
        private int _captionLength;
        private int _boxLength;

        protected override void OnSizeChanged()
        {
            _boxLength = Math.Max(0, Size.X - _captionLength - 1);
        }

        protected override void OnTextChanged()
        {
            _boxLength = Math.Max(0, Size.X - _captionLength - 1);
        }

        protected override void OnPaint()
        {
            string s = "▐";
            if (!string.IsNullOrEmpty(Caption))
            {
                s = (Caption.Length > _captionLength ? Caption.Substring(0, _captionLength) : Caption.PadRight(_captionLength)) + s;
            }
            Cons.WriteConsoleOutputCharacterEncoded(s, Location, Foreground, Background);

            WriteBoxText(null);
        }

        private void WriteBoxText(string t)
        {

            string text = t ?? Text ?? string.Empty;
            text = text.Length > _boxLength ? text.Substring(0, _boxLength) : NumberInput ? text.PadLeft(_boxLength) : text.PadRight(_boxLength);

            Coordinates pos = new Coordinates((short)(Location.X + _captionLength + 1), Location.Y);
            Cons.WriteConsoleOutputCharacterEncoded(text, pos, Background, Foreground);
        }


        #region Funktionalität
        int _cursorPos;


        protected override void OnActivate()
        {
            base.OnActivate();
            Text = Text ?? string.Empty;
            // Bei NUmberInput wird die CursorPosition in bezug auf das rechte Ende angegeben
            _cursorPos = NumberInput ? Math.Max(0, _boxLength - Text.Length) - (Text.Length == 0 ? 1 : 0) : 0;
            Cons.CursorVisible = true;
            ResetCursor();
        }

        private void ResetCursor()
        {
            Cons.CursorPosition = Location.AddX(_captionLength + 1 + _cursorPos);
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
                Cons.CursorPosition = Location.AddX(_captionLength + 1 + _cursorPos);
                bool handled = true;
                switch (keyInfo.Key)
                {
                    case ConsoleKey.LeftArrow:

                        if (NumberInput ? _cursorPos > Math.Max(0, _boxLength - Text.Length) : _cursorPos > 0)
                        {
                            Cons.CursorPosition = Cons.CursorPosition.AddX(-1);
                            _cursorPos--;
                        }
                        break;
                    case ConsoleKey.RightArrow:

                        if (NumberInput ? _cursorPos < _boxLength - 1 : _cursorPos < Text.Length && _cursorPos < _boxLength - 1)
                        {
                            Cons.CursorPosition = Cons.CursorPosition.AddX(1);
                            _cursorPos++;
                        }
                        break;
                    case ConsoleKey.Delete:

                        if (NumberInput)
                        {
                            if (_cursorPos >= Math.Max(0, _boxLength - Text.Length))
                            {
                                Text = Text.Remove(_cursorPos - Math.Max(0, _boxLength - Text.Length), 1);
                                if (_cursorPos < _boxLength - 1)
                                {
                                    Cons.CursorPosition = Cons.CursorPosition.AddX(1);
                                    _cursorPos++;
                                }
                            }
                        }
                        else
                            if (Text.Length > 0 && _cursorPos < Text.Length)
                                Text = Text.Remove(_cursorPos, 1);



                        break;

                    case ConsoleKey.Backspace:


                        if (NumberInput)
                        {
                            if (_cursorPos > Math.Max(0, _boxLength - Text.Length))
                            {
                                Text = Text.Remove(_cursorPos - Math.Max(0, _boxLength - Text.Length) - 1, 1);
                            }
                        }
                        else
                            if (Text.Length > 0 && _cursorPos > 0)
                            {
                                Text = Text.Remove(_cursorPos - 1, 1);
                                Cons.CursorPosition = Cons.CursorPosition.AddX(-1);
                                _cursorPos--;
                            }
                        break;
                    default:

                        if (NumberInput)
                        {
                            if (!char.IsControl(keyInfo.KeyChar) && (char.IsDigit(keyInfo.KeyChar) || keyInfo.KeyChar == ',') && Text.Length < _boxLength)
                            {
                                Text = Text.Insert(Math.Max(0, _cursorPos - Math.Max(0, _boxLength - Text.Length)) + (_cursorPos == _boxLength - 1 && Text.Length > 0 ? 1 : 0), keyInfo.KeyChar.ToString());
                                //if (cursorPos < _boxLength - 1)
                                //{
                                //    Console.CursorLeft += 1;
                                //    cursorPos++;
                                //}

                            }
                            else
                                handled = false;
                        }
                        else
                            if (!char.IsControl(keyInfo.KeyChar) && Text.Length < _boxLength)
                            {
                                Text = Text.Insert(_cursorPos, keyInfo.KeyChar.ToString());
                                if (_cursorPos < _boxLength - 1)
                                {
                                    Cons.CursorPosition = Cons.CursorPosition.AddX(1);
                                    _cursorPos++;
                                }

                            }
                            else
                                handled = false;
                        break;
                }


                WriteBoxText(Text);
                ////////////
                //Console.SetCursorPosition(Location.X + _captionLength, Location.Y + 1);
                //Console.Write(text.PadRight(_boxLength) + " ");
                ////////////

                Cons.CursorPosition = Location.AddX(_captionLength + 1 + _cursorPos);
                return handled;
            }
        }
        #endregion

    }
}
