using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace AdvancedDebugger
{
    public static class Debugger
    {
        public delegate void LogMethod(string log);

        private const string AddColorMessage = "Added a color for the class:";
        private const string LogWritingFailedMessage = "Log writing switched to: false";
        private const string ReplaceColorMessage = "Replace color class:";
        private const string DefaultColor = "#BDC7F0";
        private const char SharpSymbol = '#';

        private static Dictionary<string, string> m_ClassColors = new Dictionary<string, string>();
        private static Dictionary<LogType, LogMethod>? m_LogMethods;
        private static string m_LogFilePath;
        private static string m_DateTimeFormat;
        private static string m_DebuggerColor = "#45C9B0";
        private static bool m_UseMarkupFormat;
        private static bool m_EnableLogWriting;

        private static Dictionary<LogType, string> m_LogColors = new Dictionary<LogType, string>()
        {
            { LogType.Debbug, DefaultColor },
            { LogType.Info, "#ffffff" },
            { LogType.Warning, "#FFA500" },
            { LogType.Error, "#FF0000" },
        };

        public static void Initialize(LogMethod debugMethod, LogMethod infoMethod,
                                      LogMethod warningMethod, LogMethod errorMethod,
                                      bool useMarkupFormat = false)
        {
            m_LogMethods = new Dictionary<LogType, LogMethod>()
            {
                { LogType.Debbug, debugMethod },
                { LogType.Info, infoMethod },
                { LogType.Warning, warningMethod },
                { LogType.Error, errorMethod },
            };

            m_UseMarkupFormat = useMarkupFormat;
            AddPrefixColorForClass(nameof(Debugger), m_DebuggerColor);
        }

        public static void InitializeLogWriting(string logFilePath, bool enableLogWriting = true, string dateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff")
        {
            m_EnableLogWriting = enableLogWriting;
            m_LogFilePath = logFilePath;
            m_DateTimeFormat = dateTimeFormat;
        }

        public static void SetLogColors(Color debuggerPrefixColor, Color debugColor, Color infoColor, Color warningColor, Color errorColor)
        {
            SetLogColors(ColorHexConverter.GetStringFromColor(debuggerPrefixColor),
                         ColorHexConverter.GetStringFromColor(debugColor),
                         ColorHexConverter.GetStringFromColor(infoColor),
                         ColorHexConverter.GetStringFromColor(warningColor),
                         ColorHexConverter.GetStringFromColor(errorColor));
        }

        public static void SetLogColors(string debuggerPrefixColor, string debuggColor, string infoColor, string warningColor, string errorColor)
        {
            m_DebuggerColor = debuggerPrefixColor;
            AddPrefixColorForClass(nameof(Debugger), m_DebuggerColor);
            m_LogColors[LogType.Debbug] = debuggColor;
            m_LogColors[LogType.Info] = infoColor;
            m_LogColors[LogType.Warning] = warningColor;
            m_LogColors[LogType.Error] = errorColor;
        }

        public static void AddPrefixColorForClass<T>(Color color)
        {
            var hexColor = ColorHexConverter.GetStringFromColor(color);
            AddPrefixColorForClass<T>(hexColor);
        }

        public static void AddPrefixColorForClass<T>(string hexColor)
        {
            var className = typeof(T).Name;
            AddPrefixColorForClass(className, hexColor);
        }

        public static void AddPrefixColorForClass(string className, string hexColor)
        {
            hexColor = AddSharpIfNeed(hexColor);
            if (!m_ClassColors.ContainsKey(className))
            {
                m_ClassColors.Add(className, hexColor);
                Log($"{AddColorMessage} {className} ({hexColor})");
            }
            else
            {
                m_ClassColors[className] = hexColor;
                Log($"{ReplaceColorMessage} {className} ({hexColor})");
            }
        }

        public static void UseMarkupFormat(bool useMarkupFormat)
        {
            m_UseMarkupFormat = useMarkupFormat;
        }

        public static void UseLogWriting(bool enableLogWriting)
        {
            m_EnableLogWriting = enableLogWriting;
        }

        public static void Log(string message, LogType logType = LogType.Debbug,
                 [CallerMemberName] string callerName = "",
                 [CallerFilePath] string callerPath = "",
                 [CallerLineNumber] int callerLine = 0)
        {
            var callerInfo = CreateCallerInfo(callerName, callerPath, callerLine);
            var infoPrefix = GetInfoPrefix(callerInfo, m_UseMarkupFormat);
            var logMessage = $"{infoPrefix}{(m_UseMarkupFormat ? Colorize(message, m_LogColors[logType]) : message)}";

            TryToWriteLog(message, logType, callerInfo, logMessage);

            m_LogMethods?[logType].Invoke(logMessage);
        }

        private static void TryToWriteLog(string message, LogType logType, CallerInfo callerInfo, string logMessage)
        {
            if (m_EnableLogWriting && logType != LogType.Debbug)
            {
                try
                {
                    using var fileStream = File.Open(m_LogFilePath, FileMode.OpenOrCreate);
                    fileStream.Seek(0, SeekOrigin.End);
                    using var streamWriter = new StreamWriter(fileStream);
                    var writedLog = $"{GetDateTimeNow()} {logType} {(m_UseMarkupFormat ? $"{GetInfoPrefix(callerInfo, false)}{message}" : logMessage)}";
                    streamWriter.WriteLine(writedLog);
                }
                catch (Exception exception)
                {
                    m_EnableLogWriting = false;
                    Log($"{exception.Message} {LogWritingFailedMessage}", LogType.Error);
                }
            }
        }

        private static string GetDateTimeNow()
        {
            return DateTime.UtcNow.ToString(m_DateTimeFormat, CultureInfo.InvariantCulture);
        }

        private static string AddSharpIfNeed(string hexColor)
        {
            if (hexColor.First() != SharpSymbol)
            {
                hexColor = $"{SharpSymbol}{hexColor}";
            }

            return hexColor;
        }

        private static string GetInfoPrefix(CallerInfo callerInfo, bool useMrakupFormat)
        {
            string hexColor = GetHexColor(callerInfo);
            string prefix = $"{callerInfo.ClassName}.{callerInfo.MethodName} : line {callerInfo.Line} - ";
            return useMrakupFormat ? Colorize(prefix, hexColor ?? DefaultColor, bold: true) : prefix;
        }

        private static string GetHexColor(CallerInfo callerInfo)
        {
            m_ClassColors.TryGetValue(callerInfo.ClassName, out var hexColor);
            return string.IsNullOrEmpty(hexColor) ? DefaultColor : hexColor;
        }

        private static CallerInfo CreateCallerInfo(string callerName, string callerPath, int callerLine)
        {
            return new CallerInfo()
            {
                ClassName = Path.GetFileNameWithoutExtension(callerPath),
                MethodName = callerName,
                Line = callerLine
            };
        }

        private static string Colorize(string text, string color, bool bold = false)
        {
            return $"<color={color}>{(bold ? "<b><size=13>" : "")}{text}{(bold ? "</size></b>" : "")}</color>";
        }
    }
}