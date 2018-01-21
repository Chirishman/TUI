using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TuiBase.Console;

namespace TuiBase
{
    public class Window : Control
    {


        private Control _activeControl;
        private List<Control> _controls = new List<Control>();

        public bool IsDialogOpen
        {
            get
            {
                return WindowRuntime.IsDialogOpen(this);
            }
        }

        public Window()
        {

        }

        public Control ActiveControl { get { return _activeControl; } }


        public void Show()
        {
            WindowRuntime.ShowWindow(this);
        }

        public void Close()
        {
            WindowRuntime.CloseWindow(this);
        }



        protected override void OnSizeChanged()
        {
            if (Size.X < 5 || Size.Y < 3)
                throw new Exception("Invalid size!");
        }

        protected override void OnBackgroundChanged()
        {
            foreach (Control c in _controls)
                c.Background = Background;
        }

        protected override void OnForegroundChanged()
        {
            foreach (Control c in _controls)
                c.Foreground = Foreground;
        }

        public void AddControl(Control control, Coordinates relativePosition)
        {
            _controls.Add(control);
            control.Location = Location + relativePosition + new Coordinates(1, 1);
            control.Foreground = Foreground;
            control.Background = Background;
        }

        public void AddControl(Control control)
        {
            AddControl(control, control.Location);
        }




        private void PaintInternal(bool onlyTitleBar)
        {

            string text = Text ?? string.Empty;

            int leftSpace = Size.X - 2;
            int titleLenght = Math.Min(text.Length, leftSpace);
            int rightSpace = leftSpace;
            leftSpace = (leftSpace - titleLenght) / 2;
            rightSpace = rightSpace - titleLenght - leftSpace;



            //if(Title.Length > Size.X-2)

            string s;

            if (IsActive)
            {
                s = string.Format("╒{0}{1}{2}╕", new string('═', leftSpace), text.Substring(0, titleLenght), new string('═', rightSpace));
            }
            else
            {
                s = string.Format("┌{0}{1}{2}┐", new string('─', leftSpace), text.Substring(0, titleLenght), new string('─', rightSpace));
            }

            Cons.WriteConsoleOutputCharacterEncoded(s, Location, Foreground, Background);

            if (!onlyTitleBar)
            {
                for (int y = 1; y < Size.Y - 1; y++)
                {
                    var pos = Location.AddY(y);
                    s = string.Format("│{0}│", new string(' ', Size.X - 2));
                    Cons.WriteConsoleOutputCharacterEncoded(s, pos, Foreground, Background);
                }

                var bottomPos = Location.AddY(Size.Y - 1);
                s = string.Format("└{0}┘", new string('─', Size.X - 2));
                Cons.WriteConsoleOutputCharacterEncoded(s, bottomPos, Foreground, Background);

                //string s = "╔╗╚╝║═";
                //string v = "┌┐└┘│─▒";

                foreach (Control c in _controls)
                    c.SendMessage(Message.Paint);
            }

        }

        protected override void OnPaint()
        {
            PaintInternal(false);
        }

        protected override void OnLocationChanged(Coordinates oldValue)
        {
            Coordinates offset = Location - oldValue;
            foreach (Control c in _controls)
                c.Location += offset;

        }

        protected void FocusNext()
        {
            Control next = null;
            if (_activeControl == null)
            {
                if (_controls.Count > 0)
                    next = _controls[0];
            }
            else
            {
                int i = _controls.IndexOf(_activeControl);
                i = (i + 1) % _controls.Count;
                next = _controls[i];
            }

            SetActiveControl(next);


        }

        protected void FocusPrev()
        {
            Control prev = null;
            if (_activeControl == null)
            {
                if (_controls.Count > 0)
                    prev = _controls[_controls.Count - 1];
            }
            else
            {
                int i = _controls.IndexOf(_activeControl);
                i = (i - 1 + _controls.Count) % _controls.Count;
                prev = _controls[i];
            }

            SetActiveControl(prev);


        }

        protected void SetActiveControl(Control c)
        {
            if (IsActive && _activeControl != null)
                _activeControl.SendMessage(Message.Deactivate);

            _activeControl = c;

            if (IsActive && _activeControl != null)
                _activeControl.SendMessage(Message.Activate);
        }


        public override bool SendMessage(Message message, MessageParameter parameter)
        {

            if (base.SendMessage(message, parameter))
                return true;
            else
            {
                if (_activeControl != null)
                    return _activeControl.SendMessage(message, parameter);
                else
                    return false;
            }


        }

        public DialogResultType ShowDialog()
        {
            return WindowRuntime.ShowDialog(this);
        }


        public virtual void OnWindowDeactivate(bool cancelled)
        {

        }

        protected override void OnActivate()
        {
            base.OnActivate();
            PaintInternal(true);
            if (_activeControl == null && _controls.Count > 0)
                _activeControl = _controls[0];
            if (_activeControl != null)
                _activeControl.SendMessage(Message.Activate);
        }

        protected override void OnDeactivate()
        {
            base.OnDeactivate();
            if (_activeControl != null)
                _activeControl.SendMessage(Message.Deactivate);
            PaintInternal(true);
        }


        protected override bool OnKeyPress(ConsoleKeyInfo keyInfo)
        {
            if (base.OnKeyPress(keyInfo))
                return true;
            else
                switch (keyInfo.Key)
                {
                    case ConsoleKey.Tab:
                        if (keyInfo.Modifiers == ConsoleModifiers.Shift)
                            FocusPrev();
                        else
                            FocusNext();
                        return true;
                    case ConsoleKey.F1:
                        MessageBox.Show("Help", HelpText, ConsColor.Yellow, ConsColor.Black);
                        return true;
                    case ConsoleKey.F6:
                        SendMessage(Message.Paint);
                        return true;
                    default:
                        return false;
                }
        }

        public DialogResultType? DialogResult { get; set; }

        protected virtual string[] HelpText
        {
            get
            {
                return _helpText;
            }
        }


        private static string[] _helpText = new[]
            {
                "Quit = [Esc]",
                "OK = [Return]"

            };
    }
}
