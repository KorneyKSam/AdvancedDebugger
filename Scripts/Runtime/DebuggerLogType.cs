namespace AdvancedDebugger
{
    public class DebuggerLogType : DebuggerColorization
    {
        public bool IsLoggedToFile { get; private set; }
        private LogMethod m_LogMethod;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">This name used to call up this log from Debugger</param>
        /// <param name="logMethod">LogType calling this method</param>
        /// <param name="hexColor">Hex color format: #FFFFFF</param>
        /// <param name="isLoggedToFile">To write this type of logs to the log file, set to true</param>
        public DebuggerLogType(string name, LogMethod logMethod, string hexColor = DebuggerConstants.DefaultColor, bool isLoggedToFile = false)
                        : base(name, hexColor)
        {
            IsLoggedToFile = isLoggedToFile;
            m_LogMethod = logMethod;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">This name used to call up this log from Debugger</param>
        /// <param name="logMethod">LogType calling this method</param>
        /// <param name="color">Unity color that will be converted to hex color</param>
        /// <param name="isLoggedToFile">To write this type of logs to the log file, set to true</param>
        public DebuggerLogType(string name, LogMethod logMethod, UnityEngine.Color color, bool isLoggedToFile = false)
                        : this(name, logMethod, ColorHexConverter.GetHexColor(color), isLoggedToFile) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">This name used to call up this log from Debugger</param>
        /// <param name="logMethod">LogType calling this method</param>
        /// <param name="color">System color that will be converted to hex color</param>
        /// <param name="isLoggedToFile">To write this type of logs to the log file, set to true</param>
        public DebuggerLogType(string name, LogMethod logMethod, System.Drawing.Color color, bool isLoggedToFile = false)
                        : this(name, logMethod, ColorHexConverter.GetHexColor(color), isLoggedToFile) { }

        public void Log(string log)
        {
            m_LogMethod?.Invoke(log);
        }
    }
}