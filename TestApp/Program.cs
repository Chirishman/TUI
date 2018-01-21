using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Tui.TextConsole;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowWidth = 120;
            Console.WindowHeight = 40;
            TuiBase.WindowRuntime.Initialize();



            TuiBase.PopUpMenu mainMenu = null;
            mainMenu = new TuiBase.PopUpMenu("Samples", new TuiBase.PopUpMenuItem[] {
                new TuiBase.PopUpMenuItem("Controls", ControlsSample),
                new TuiBase.PopUpMenuItem("TextPanel",TextPanelSample),
                new TuiBase.PopUpMenuItem("Windows and Dialogs",WindowsAndDialogSample),
                new TuiBase.PopUpMenuItem("Exit",() => mainMenu.Close())}
            );

            TuiBase.WindowRuntime.Run(new ConsoleImpl(), mainMenu);

        }


        static void ControlsSample()
        {
            ControlsSampleWindow w = new ControlsSampleWindow();
            w.ShowDialog();
        }

        static void TextPanelSample()
        {

        }

        static void WindowsAndDialogSample()
        {

        }


    }
}
