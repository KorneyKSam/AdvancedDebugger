namespace AdvancedDebugger
{
    public class DebuggerLogType : DebuggerColorization
    {
        public bool IsLoggedToFile { get; private set; }
        private LogMethod m_LogMethod;

        public DebuggerLogType(string name, LogMethod logMethod, string hexColor = DebuggerConstants.DefaultColor, bool isLoggedToFile = false)
                        : base(name, hexColor)
        {
            IsLoggedToFile = isLoggedToFile;
            m_LogMethod = logMethod;
        }

        public DebuggerLogType(string name, UnityEngine.Color color, bool isLoggedToFile = false)
                        : this(name, null, ColorHexConverter.GetHexColor(color), isLoggedToFile) { }

        public DebuggerLogType(string name, System.Drawing.Color color, bool isLoggedToFile = false)
                        : this(name, null, ColorHexConverter.GetHexColor(color), isLoggedToFile) { }

        public DebuggerLogType(string name, string hexColor = DebuggerConstants.DefaultColor, bool isLoggedToFile = false)
                        : this(name, null, hexColor, isLoggedToFile) { }

        public void Log(string log)
        {
            m_LogMethod?.Invoke(log);
        }
    }
}