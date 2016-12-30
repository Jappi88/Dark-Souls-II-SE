namespace Dark_Souls_II_Save_Editor.STFSPackage
{
    public class ProgressChangedArg
    {
        public string Message { get; set; }
        public int Percentage { get; set; }
        public long Processed { get; set; }
        public long Total { get; set; }
        public object Argument { get; set; }
        public static ProgressChangedArg Empty => new ProgressChangedArg();
    }
}