using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TuiBase.Console;

namespace TuiBase
{
    public abstract class Control
    {

        protected IConsole Cons
        {
            get
            {
                return WindowRuntime.Console;
            }
        }

        private string _text;
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                OnTextChanged();
            }
        }
        protected virtual void OnTextChanged() { }


        private Coordinates _size;
        public Coordinates Size
        {
            get
            {
                return _size;
            }
            set
            {
                _size = value;
                OnSizeChanged();
            }
        }
        protected virtual void OnSizeChanged() { }



        private Coordinates _location;
        public Coordinates Location
        {
            get
            {
                return _location;
            }
            set
            {
                Coordinates oldValue = _location;
                _location = value;
                OnLocationChanged(oldValue);
            }
        }
        protected virtual void OnLocationChanged(Coordinates oldValue) { }



        private ConsColor _background;
        public ConsColor Background
        {
            get
            {
                return _background;
            }
            set
            {
                _background = value;
                OnBackgroundChanged();
            }
        }
        protected virtual void OnBackgroundChanged() { }



        private ConsColor _foreground;
        public ConsColor Foreground
        {
            get
            {
                return _foreground;
            }
            set
            {
                _foreground = value;
                OnForegroundChanged();
            }
        }
        protected virtual void OnForegroundChanged() { }





        public Control()
        {
            _background = ConsColor.Gray;
            _foreground = ConsColor.Black;
        }

        public bool IsActive { get; private set; }

        protected virtual void OnActivate()
        {
            IsActive = true;
        }

        protected virtual void OnDeactivate()
        {
            IsActive = false;
        }


        public event EventHandler<KeyPressEventArgs> KeyPress;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyInfo"></param>
        /// <returns>true, wenn der Key verarbeitet wurde</returns>
        protected virtual bool OnKeyPress(ConsoleKeyInfo keyInfo)
        {
            if (KeyPress != null)
            {
                KeyPressEventArgs args = new KeyPressEventArgs(keyInfo);
                KeyPress(this, args);
                return args.Handled;
            }
            else
                return false;
        }

        public bool SendMessage(Message message)
        {
            return SendMessage(message, null);
        }

        /// <summary>
        /// Senden einer Masseg
        /// </summary>
        /// <param name="message"></param>
        /// <param name="parameter"></param>
        /// <returns>true: Message wurde verarbeitet</returns>
        public virtual bool SendMessage(Message message, MessageParameter parameter)
        {
            switch (message)
            {
                case Message.Activate:
                    OnActivate();
                    return true;
                case Message.Deactivate:
                    OnDeactivate();
                    return true;
                case Message.KeyPress:
                    return OnKeyPress(parameter.KeyInfo);
                case Message.Paint:
                    // Paint breucht egentlich nicht verarbeitet werden , da Controls immer 
                    // vom Paint des Windows aufgerufen werden
                    OnPaint();
                    return true;
                default:
                    return false;
            }
        }


        protected abstract void OnPaint();
    }


}
