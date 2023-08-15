namespace AdvancedDebugger
{
    public enum LogType
    {
        /// <summary>
        /// Simple type of message that will not be written to the log file
        /// </summary>
        Debbug = 0,
        /// <summary>
        /// Special type of message that will be written to the log file
        /// </summary>
        Info = 1,
        /// <summary>
        /// Warning message that will be written to the log file
        /// </summary>
        Warning = 2,
        /// <summary>
        /// Error message that will be written to the log file
        /// </summary>
        Error = 3,
    }
}
