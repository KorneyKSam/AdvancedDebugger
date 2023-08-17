namespace AdvancedDebugger
{
    public delegate void LogMethod(string log);

    internal static class DebuggerConstants
    {
        internal const string DebuggerPrefix = "[AdvancedDebugger]: ";
        internal const string DefaultColor = "#FFFFFF";
        internal const string DefaultDateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
        internal const string LogWritingFailedMessage = "Log wrinting failed!";
        internal const string LogTypeNotFoundMessage = "Log type not found, use default method for log message!";
    }
}
