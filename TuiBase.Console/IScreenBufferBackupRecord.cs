using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TuiBase.Console
{
    public interface IScreenBufferBackupRecord
    {
        void Restore();
    }
}
