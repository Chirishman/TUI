using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TuiBase.Console;

namespace TuiBase
{
    public class TextPanel : Control
    {

        private string[] _lines = new string[] { };

        /// <summary>
        /// Maximale Y-Scrollposition
        /// </summary>
        private int _maxYScrollPos;
        /// <summary>
        /// Maximale X-Scrollposition
        /// </summary>
        private int _maxXScrollPos;
        /// <summary>
        /// Aktuelle X-Scrollposition
        /// </summary>
        private int _currXScrollPos;
        /// <summary>
        /// Akturlle Y-Scrollposition
        /// </summary>
        private int _currYScrollPos;

        /// <summary>
        /// X-Scrollbar vorhanden ?
        /// </summary>
        bool _hasXScroll;
        /// <summary>
        /// Y-Scrollbar vorhanden
        /// </summary>
        bool _hasYScroll;

        /// <summary>
        /// Exklusive x-Grenze des Textbereiches
        /// </summary>
        int _textAreaWidth;
        /// <summary>
        /// Exklusive y-Grenze des Textbereiches
        /// </summary>
        int _textAreaHeight;

        /// <summary>
        /// Exklusive Grenze des x-Scrollindikators
        /// </summary>
        int _xScrollIndLim = 0;
        /// <summary>
        /// Exklusive Grenze des y-Scrollindikators
        /// </summary>
        int _yScrollIndLim = 0;

        /// <summary>
        /// Bis zur vievielten Zeile wird Text angezeigt
        /// </summary>
        int _textTillY;

        /// <summary>
        /// Ausgewählte Linie 
        /// </summary>
        int? _activeLine;

        List<int> _markedLines = new List<int>();

        /// <summary>
        /// Linie in der sich der Cursor befindet
        /// </summary>
        public int? ActiveLine
        {
            get
            {
                return _activeLine;
            }
            set
            {
                _activeLine = value.HasValue && value >= _lines.Length ? Math.Max(_lines.Length - 1, 0) : value;
                if (!IsActiveLineVisible())
                {
                    _currYScrollPos = Math.Min(_activeLine.Value, _maxYScrollPos);
                }
            }

        }


        public List<int> MarkedLines
        {
            get
            {
                return _markedLines;
            }
        }

        public Action LineSelected { get; set; }

        public bool AcceptReturnForLineSelection { get; set; }




        private readonly string[] LINESEP = new string[] { "\r\n" };

        public void AddLine(string line)
        {
            _lines = (_lines ?? new string[] { }).Union(new[] { line }).ToArray();
            CalcLayout();
        }


        public string[] Lines
        {

            set
            {
                _markedLines.Clear();
                _currXScrollPos = 0;
                _currYScrollPos = 0;
                if (value == null)
                {
                    _lines = new string[] { };
                    _maxXScrollPos = 0;
                    _maxYScrollPos = 0;
                }
                else
                {
                    _lines = value;

                    CalcLayout();
                }
            }

        }

        protected override void OnTextChanged()
        {
            if (string.IsNullOrEmpty(Text))
            {
                Lines = null;
            }
            else
            {

                Lines = Text.Split(LINESEP, StringSplitOptions.None);
            }
        }

        private bool IsActiveLineVisible()
        {
            int pos = _activeLine.Value - _currYScrollPos;
            return pos >= 0 && pos < _textAreaHeight;
        }


        protected override void OnActivate()
        {
            base.OnActivate();
            Cons.CursorPosition = Location;
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
                bool needRepaint = false;
                bool handled = false;
                switch (keyInfo.Key)
                {
                    case ConsoleKey.Spacebar:
                        if (handled = _activeLine.HasValue)
                        {
                            if (_markedLines.Contains(_activeLine.Value))
                                _markedLines.Remove(_activeLine.Value);
                            else
                                _markedLines.Add(_activeLine.Value);
                            UpdateTextLine(_activeLine.Value);
                        }
                        break;
                    case ConsoleKey.Enter:
                        if (AcceptReturnForLineSelection)
                        {
                            handled = true;
                            if (LineSelected != null)
                                LineSelected();
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        handled = true;
                        if (_activeLine.HasValue)
                        {
                            if (_activeLine > 0)
                            {
                                _activeLine = _activeLine.Value - 1;

                                if (IsActiveLineVisible())
                                {
                                    UpdateTextLine(_activeLine.Value + 1);
                                    UpdateTextLine(_activeLine.Value);
                                }
                                else
                                {
                                    needRepaint = ScrollUp(false);
                                }
                            }

                        }
                        else
                            needRepaint = ScrollUp(false);

                        break;
                    case ConsoleKey.DownArrow:
                        handled = true;
                        if (_activeLine.HasValue)
                        {
                            // selektierte Line runter
                            if (_activeLine < _lines.Length - 1)
                            {
                                _activeLine = _activeLine.Value + 1;

                                if (IsActiveLineVisible())
                                {
                                    UpdateTextLine(_activeLine.Value - 1);
                                    UpdateTextLine(_activeLine.Value);
                                }
                                else
                                {
                                    needRepaint = ScrollDown(false);
                                }
                            }

                        }
                        else
                        {
                            // keine Selektion möglich => nur runterscrollen
                            needRepaint = ScrollDown(false);
                        }
                        break;
                    case ConsoleKey.PageUp:
                        handled = true;

                        if (_activeLine.HasValue)
                        {
                            if (_activeLine == _currYScrollPos)
                            {
                                if (needRepaint = ScrollUp(true))
                                    _activeLine = _currYScrollPos;
                            }
                            else
                            {
                                int lastActive = _activeLine.Value;
                                _activeLine = _currYScrollPos;
                                UpdateTextLine(lastActive);
                                UpdateTextLine(_activeLine.Value);

                            }

                        }
                        else
                            needRepaint = ScrollUp(true);

                        break;
                    case ConsoleKey.PageDown:
                        handled = true;

                        if (_activeLine.HasValue)
                        {
                            if (_activeLine == _currYScrollPos + _textAreaHeight - 1)
                            {
                                if (needRepaint = ScrollDown(true))
                                    _activeLine = _currYScrollPos + _textAreaHeight - 1;
                            }
                            else
                            {
                                int lastActive = _activeLine.Value;
                                _activeLine = _currYScrollPos + _textAreaHeight - 1;
                                UpdateTextLine(lastActive);
                                UpdateTextLine(_activeLine.Value);

                            }

                        }
                        else
                            needRepaint = ScrollDown(true);

                        break;
                    case ConsoleKey.LeftArrow:
                        handled = true;
                        if (_currXScrollPos > 0)
                        {
                            _currXScrollPos--;
                            needRepaint = true;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        handled = true;
                        if (_currXScrollPos < _maxXScrollPos)
                        {
                            _currXScrollPos++;
                            needRepaint = true;
                        }
                        break;
                }
                if (needRepaint)
                    OnPaint();

                return handled;
            }
        }




        /// <summary>
        /// Runterscrollen
        /// </summary>
        /// <param name="page">false: 1Zeile, true:1Bildschirmseite</param>
        /// <returns>true, wenn gescrollt wurde</returns>
        private bool ScrollDown(bool page)
        {
            if (_currYScrollPos >= _maxYScrollPos)
                return false;

            if (page)
            {
                _currYScrollPos = Math.Min(_maxYScrollPos, _currYScrollPos + Math.Max(1, _textAreaHeight - PAGESCROLLOVERLAP));
            }
            else
            {
                _currYScrollPos++;
            }
            return true;

        }

        private const int PAGESCROLLOVERLAP = 2;

        /// <summary>
        /// Raufscrollen
        /// </summary>
        /// <param name="page">false: 1Zeile, true:1Bildschirmseite</param>
        /// <returns>true, wenn gescrollt wurde</returns>
        private bool ScrollUp(bool page)
        {
            if (_currYScrollPos <= 0)
                return false;

            if (page)
            {

                _currYScrollPos = Math.Max(0, _currYScrollPos - Math.Max(1, _textAreaHeight - PAGESCROLLOVERLAP));
            }
            else
            {
                _currYScrollPos--;
            }
            return true;

        }


        /// <summary>
        /// Berechen folgender relativer Angaben:
        /// _hasXScroll
        /// _hasYScroll
        /// _maxYScrollPos
        /// _maxXScrollPos
        /// _textAreaXLim
        /// _textAreaYLim
        /// _yScrollIndLim
        /// _xScrollIndLim
        /// _textTillY
        /// muss aufgerufen werden bei änderung von
        /// 1) änderung der Länge der längsten TextZeile
        /// 2) Anzahl der TextZeilen
        /// 3) Size
        /// </summary>
        private void CalcLayout()
        {
            _hasYScroll = _lines.Length > Size.Y;

            int longestLine = _lines.Max(line => line.Length);

            _hasXScroll = longestLine > Size.X;

            // wenn ein scrollbalken erforderlich ist, so kann auch deswegen der anderen erforderlich werden, da
            // der scrollbalken ja auch eine grösse hat!
            if (_hasXScroll) _hasYScroll = _lines.Length > Size.Y - 2;
            if (_hasYScroll) _hasXScroll = longestLine > Size.X - 2;


            _maxYScrollPos = Math.Max(0, _lines.Length - Size.Y + (_hasXScroll ? 2 : 0));
            _maxXScrollPos = Math.Max(0, longestLine - Size.X + (_hasYScroll ? 2 : 0));


            if (_hasYScroll)
            {
                _textAreaWidth = Size.X - 2;
                _yScrollIndLim = _hasXScroll ? Size.Y - 1 : Size.Y;
            }
            else
            {
                _textAreaWidth = Size.X;
            }

            if (_hasXScroll)
            {
                _textAreaHeight = Size.Y - 2;
                _xScrollIndLim = _hasYScroll ? Size.X - 1 : Size.X;
            }
            else
            {
                _textAreaHeight = Size.Y;
            }

            _textTillY = Math.Min(_lines.Length, _textAreaHeight);

        }



        protected override void OnSizeChanged()
        {
            if (Size.X < 3 || Size.Y < 3)
                throw new Exception("Invalid size!");
        }

        /// <summary>
        /// Updaten des LineTextes
        /// </summary>
        /// <param name="line">logische LineNummer(muss sichtbar sein)</param>
        private void UpdateTextLine(int line)
        {
            StringBuilder sb = new StringBuilder();
            Coordinates pos = new Coordinates(Location.X, (short)(Location.Y + line - _currYScrollPos));
            string s = _lines[line];
            int len = s.Length;
            int lenToWrite = Math.Min(Math.Max(0, s.Length - _currXScrollPos), _textAreaWidth);
            if (lenToWrite > 0) sb.Append(s.Substring(_currXScrollPos, lenToWrite));
            if (lenToWrite < _textAreaWidth) sb.Append(new string(' ', _textAreaWidth - lenToWrite));

            var lineColor = GetLineColor(line);
            Cons.WriteConsoleOutputCharacterEncoded(sb.ToString(), pos, lineColor.Item1, lineColor.Item2);
        }

        private Tuple<ConsColor, ConsColor> GetLineColor(int line)
        {
            if (_activeLine.HasValue && _activeLine.Value == line)
            {
                if (_markedLines.Contains(line))
                {
                    return Tuple.Create(Background ^ ConsColor.INTENSITY, Foreground ^ ConsColor.INTENSITY);
                }
                else
                    return Tuple.Create(Background, Foreground);
            }
            else if (_markedLines.Contains(line))
            {
                return Tuple.Create(Foreground ^ ConsColor.INTENSITY, Background ^ ConsColor.INTENSITY);
            }
            else
                return Tuple.Create(Foreground, Background);
        }



        protected override void OnPaint()
        {

            int yScrollInd = 0;
            int xScrollInd = 0;

            if (_hasYScroll) yScrollInd = (int)(((double)_currYScrollPos / _maxYScrollPos) * (_yScrollIndLim - 1) + 0.5);
            if (_hasXScroll) xScrollInd = (int)(((double)_currXScrollPos / _maxXScrollPos) * (_xScrollIndLim - 1) + 0.5);
            Coordinates pos;

            for (int y = 0; y < _textTillY; y++)
            {
                StringBuilder sb = new StringBuilder();

                pos = new Coordinates(Location.X, (short)(Location.Y + y));
                string s = _lines[y + _currYScrollPos];
                int len = s.Length;
                int lenToWrite = Math.Min(Math.Max(0, s.Length - _currXScrollPos), _textAreaWidth);
                if (lenToWrite > 0) sb.Append(s.Substring(_currXScrollPos, lenToWrite));
                if (lenToWrite < _textAreaWidth) sb.Append(new string(' ', _textAreaWidth - lenToWrite));

                var color = GetLineColor(y + _currYScrollPos);
                Cons.WriteConsoleOutputCharacterEncoded(sb.ToString(), pos, color.Item1, color.Item2);
                pos = pos.AddX(sb.Length);
                if (_hasYScroll)
                {
                    Cons.WriteConsoleOutputCharacterEncoded((y == yScrollInd) ? "│█" : "│▒", pos, Foreground, Background);
                }
            }

            if (_textTillY < _textAreaHeight)
            {
                for (int y = _textTillY; y < _textAreaHeight; y++)
                {
                    pos = new Coordinates(Location.X, (short)(Location.Y + y));
                    var s = new string(' ', _textAreaWidth);
                    Cons.WriteConsoleOutputCharacterEncoded(s, pos, Foreground, Background);
                }
            }

            if (_hasXScroll)
            {
                pos = new Coordinates(Location.X, (short)(Location.Y + Size.Y - 2));
                string s = new string('─', _xScrollIndLim - 1);
                if (_hasYScroll)
                {
                    s += yScrollInd == Size.Y - 2 ? "┘█" : "┘▒";
                }
                else
                    s += "─";
                Cons.WriteConsoleOutputCharacterEncoded(s, pos, Foreground, Background);

                pos = new Coordinates(Location.X, (short)(Location.Y + Size.Y - 1));
                s = new string('▒', Size.X - 1).Insert(xScrollInd, "█");
                Cons.WriteConsoleOutputCharacterEncoded(s, pos, Foreground, Background);
            }

            // string s = "╔╗╚╝║═";
            // string v = "┌┐└┘│─▒█";

            //System.Console.BackgroundColor = ConsoleColor.re
        }


    }
}
