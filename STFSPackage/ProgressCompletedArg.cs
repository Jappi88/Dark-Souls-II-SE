using System;

namespace Dark_Souls_II_Save_Editor.STFSPackage
{
    public class ProgressCompletedArg
    {
        public string Message { get; set; }
        public object Result { get; set; }
        public bool Canceled { get; set; }
        public Exception Error { get; set; }
    }
}