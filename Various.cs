using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dark_Souls_II_Save_Editor
{
    public delegate void CompletedHandler(object instance, object result, bool canceled, Exception error = null);

    public delegate void ProgressHandler(string message, int percent, long current, long max);

    public enum MessageType
    {
        Error,
        Info,
        Warning,
        NewMessage,
        Message,
        Bussy
    }
}
