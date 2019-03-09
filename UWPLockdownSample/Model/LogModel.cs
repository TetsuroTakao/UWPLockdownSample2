using System;

namespace UWPLockdownSample.Model
{
    public class LogModel
    {
        public string Message { get; set; }
        public DateTime OccurredTime { get; set; }
        public string OperatorName { get; set; }
        public LogType LogType { get; set; }
    }

    public enum LogType
    {
        Default = 0,
        Information = 1,
        Error = 2,
        Operation = 4
    }
}
