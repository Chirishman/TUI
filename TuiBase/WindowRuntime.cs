using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TuiBase.Console;

namespace TuiBase
{
    public static class WindowRuntime
    {


        static IConsole _console;

        static internal IConsole Console { get { return _console; } }

        /// <summary>
        /// das aktive Fenster
        /// </summary>
        private static Window _activeWindow;

        /// <summary>
        /// angezeigte Dialoge (ist ein Stack, da ein Dialog ja wieder einen Dialog aufmachen kann
        /// </summary>
        private static Stack<Control> _dialogStack = new Stack<Control>();


        private static List<Window> _windows = new List<Window>();

        private static Window _masterWindow;

        public static event Action<Exception> UnhandledException;


        internal static bool IsDialogOpen(Window window)
        {
            return _dialogStack.Contains(window);
        }

        /// <summary>
        /// Anzeigen eines Dialoges
        /// </summary>
        /// <param name="dialog"></param>
        /// <returns></returns>
        public static DialogResultType ShowDialog(Window dialog)
        {
            if (_windows.Contains(dialog))
                throw new InvalidOperationException("Cannot open window modal, when already open!");
            if (_dialogStack.Contains(dialog))
                throw new InvalidOperationException("Window is already modal open!!");


            if (_dialogStack.Count > 0)
            {
                // Den Dialog der den aktuellen Dialog anzeigt deaktivieren
                _dialogStack.Peek().SendMessage(Message.Deactivate);
            }
            else
            {
                // Das aktive Fenster deaktivieren
                if (_activeWindow != null)
                    _activeWindow.SendMessage(Message.Deactivate);
            }

            _dialogStack.Push(dialog);

            var backup = _console.Backup(dialog.Location, dialog.Size);

            dialog.SendMessage(Message.Paint);
            dialog.SendMessage(Message.Activate);
            do
            {
                ConsoleKeyInfo keyInfo;
                keyInfo = _console.ReadKey();
                if (!dialog.SendMessage(Message.KeyPress, new MessageParameter(keyInfo)))
                {
                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.Enter:
                            dialog.DialogResult = DialogResultType.Ok;
                            break;
                        case ConsoleKey.Escape:
                            dialog.DialogResult = DialogResultType.Cancel;
                            break;
                    }
                }

            }
            while (!dialog.DialogResult.HasValue);

            dialog.SendMessage(Message.Deactivate);
            backup.Restore();
            _dialogStack.Pop();

            if (_dialogStack.Count > 0)
            {
                _dialogStack.Peek().SendMessage(Message.Activate);
            }
            else
            {
                if (_activeWindow != null)
                    _activeWindow.SendMessage(Message.Activate);
            }

            return dialog.DialogResult.Value;

        }




        internal static void ShowWindow(Window window)
        {
            if (_dialogStack.Count > 0)
                throw new InvalidOperationException("Cannot show window, if Dialogs are open!");
            if (_windows.Contains(window))
                throw new InvalidOperationException("Window is already open!");

            _windows.Add(window);
            SetActiveWindow(window);

        }



        internal static void CloseWindow(Window window)
        {
            if (!_windows.Contains(window))
                throw new InvalidOperationException("Window is not open!");

            if (window == _masterWindow)
            {
                foreach (Window w in _windows)
                    if (w != window)
                    {
                        w.SendMessage(Message.Deactivate);
                        HideWindow(w);
                    }
            }

            Window nextWindow = GetNextWindow();
            _windows.Remove(window);
            _activeWindow = null;
            window.SendMessage(Message.Deactivate);
            HideWindow(window);

            if (_windows.Count > 0)
                SetActiveWindow(nextWindow);
        }




        private static void SetActiveWindow(Window window)
        {
            if (_activeWindow != null)
            {
                _activeWindow.SendMessage(Message.Deactivate);
            }
            _activeWindow = window;
            window.SendMessage(Message.Paint);
            window.SendMessage(Message.Activate);
        }

        private static Window GetNextWindow()
        {
            int index = _windows.IndexOf(_activeWindow);
            index = (index + 1) % _windows.Count;
            return _windows[index];
        }


        public static void Run(IConsole console, Window masterWindow)
        {
            try
            {
                _console = console;
                _masterWindow = masterWindow;
                ShowWindow(masterWindow);


                for (; ; )
                {
                    ConsoleKeyInfo keyInfo = _console.ReadKey();

                    if (keyInfo.Key == ConsoleKey.F4 && keyInfo.Modifiers == ConsoleModifiers.Alt)
                    {
                        _activeWindow.Close();
                    }
                    else if (keyInfo.Key == ConsoleKey.Tab && keyInfo.Modifiers == ConsoleModifiers.Control && _windows.Count > 1)
                    {
                        SetActiveWindow(GetNextWindow());
                    }
                    else
                        try
                        {
                            _activeWindow.SendMessage(Message.KeyPress, new MessageParameter(keyInfo));
                        }
                        catch (Exception x)
                        {
                            if (UnhandledException != null)
                                UnhandledException(x);
                            else
                                throw new Exception("Unhandled exception occured!", x);
                        }

                    if (!_windows.Contains(masterWindow))
                        break;

                }

            }
            catch (Exception x)
            {
                _console.Panic(x);
            }

        }

        internal static void HideWindow(Window window)
        {
            if (_dialogStack.Count > 0)
                throw new InvalidOperationException("Cannot clear window, if Dialogs are open!");

            _console.ClearRegion(window.Location, window.Size);

            foreach (Window w in _windows)
                if (Helpers.IntersectsWith(window.Location, window.Size, window.Location, window.Size))
                    w.SendMessage(Message.Paint);

        }




        public static void Initialize()
        {


        }
    }
}
