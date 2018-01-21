using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuiBase.Console
{
    public interface IConsole
    {
        Coordinates CursorPosition { get; set; }
        bool CursorVisible { set; }
        
        ConsoleKeyInfo ReadKey();

        void ClearRegion(Coordinates location, Coordinates size);
        IScreenBufferBackupRecord Backup(Coordinates location, Coordinates size);
        void WriteConsoleOutputCharacterEncoded(string s, Coordinates location, ConsColor foreground, ConsColor background);
        
        void Panic(Exception x);
    }
}
