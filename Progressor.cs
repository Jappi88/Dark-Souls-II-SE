using System;

namespace Dark_Souls_II_Save_Editor
{
    [Serializable]
    public class Progressor
    {
        public bool Cancel = false;
        public bool isbusy { get; set; }
        public event ProgressHandler AtProgress;
        public event CompletedHandler Completed;

        public void DoProgress(string message, int percent, long current, long max)
        {
            AtProgress?.Invoke(message, percent, current, max);
        }

        public void DoCompleted(object instance, object result, bool canceled, Exception ex = null)
        {
            Completed?.Invoke(instance, result, canceled, ex);
        }
    }
}