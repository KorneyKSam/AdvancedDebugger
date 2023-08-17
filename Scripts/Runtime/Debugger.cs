using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AdvancedDebugger
{
    public static class Debugger
    {
        public static bool EnableMarkupFormat { get; set; }

        /// <summary>
        /// Log writing does not work without logFilePath
        /// </summary>
        public static bool EnableLogWriting { get; set; }

        private static List<DebuggerLogType> m_LogTypes;
        private static List<DebuggerColorization> m_CallersColorization;
        private static LogMethod m_DefaultLogMethod;
        private static string m_LogFilePath;
        private static string m_DateTimeFormat;

        public static void Initialize(List<DebuggerLogType> logTypes, LogMethod defaultLogMethod)
        {
            Initialize(logTypes, defaultLogMethod, null, null);
        }

        public static void Initialize(List<DebuggerLogType> logTypes, LogMethod defaultLogMethod, List<DebuggerColorization> callersColorization)
        {
            Initialize(logTypes, callersColorization, defaultLogMethod, null);
        }

        public static void Initialize(List<DebuggerLogType> logTypes, LogMethod defaultLogMethod, string logFilePath, string dateTimeFormat = DebuggerConstants.DefaultDateTimeFormat)
        {
            Initialize(logTypes, null, defaultLogMethod, logFilePath, dateTimeFormat);
        }

        public static void Initialize(List<DebuggerLogType> logTypes, List<DebuggerColorization> callersColorization,
                                      LogMethod logMethod, string logFilePath, string dateTimeFormat = DebuggerConstants.DefaultDateTimeFormat)
        {
            m_LogTypes = logTypes;
            m_DefaultLogMethod = logMethod;
            m_LogFilePath = logFilePath;
            m_DateTimeFormat = dateTimeFormat;
            m_CallersColorization = callersColorization;
        }

        public static void Log(string message, string logType,
                              [CallerMemberName] string callerMemberName = "",
                              [CallerFilePath] string callerPath = "",
                              [CallerLineNumber] int callerLine = 0)
        {
            var callerName = Path.GetFileNameWithoutExtension(callerPath);
            var callerInfo = $"{callerName}.{callerMemberName} : line {callerLine} - ";


            var debuggerLogType = GetLogType(logType);

            LogMethod logMethod;

            if (debuggerLogType != null)
            {
                logMethod = debuggerLogType.Log;

                if (EnableLogWriting && debuggerLogType.IsLoggedToFile)
                {
                    WriteLogToFile(GetDateTimeNow(), logType, callerInfo, message);
                }
            }
            else
            {
                logMethod = m_DefaultLogMethod;
                m_DefaultLogMethod?.Invoke($"{DebuggerConstants.DebuggerPrefix}{DebuggerConstants.LogTypeNotFoundMessage}");
            }

            if (EnableMarkupFormat)
            {
                logMethod?.Invoke($"{Colorize(callerInfo, GetCallerHexColor(callerName), bold: true)} " +
                                  $"{Colorize(message, debuggerLogType?.HexColor ?? DebuggerConstants.DefaultColor)}");
            }
            else
            {
                logMethod?.Invoke($"{callerInfo} {message}");
            }
        }

        private static void WriteLogToFile(string time, string logType, string callerInfo, string message)
        {
            if (!string.IsNullOrEmpty(m_LogFilePath))
            {
                try
                {
                    using var fileStream = File.Open(m_LogFilePath, FileMode.OpenOrCreate);
                    fileStream.Seek(0, SeekOrigin.End);
                    using var streamWriter = new StreamWriter(fileStream);
                    var writedLog = $"{time} {logType} {callerInfo}{message}";
                    streamWriter.WriteLine(writedLog);
                }
                catch (Exception exception)
                {
                    EnableLogWriting = false;
                    m_DefaultLogMethod?.Invoke($"{DebuggerConstants.DebuggerPrefix}{DebuggerConstants.LogWritingFailedMessage}\n{exception.Message} ");
                }
            }
        }

        private static string GetDateTimeNow()
        {
            return DateTime.UtcNow.ToString(m_DateTimeFormat, CultureInfo.InvariantCulture);
        }

        private static string GetCallerHexColor(string callerName)
        {
            var foundColorization = m_CallersColorization?.FirstOrDefault(c => c.Name == callerName);
            return foundColorization?.HexColor ?? DebuggerConstants.DefaultColor;
        }

        private static DebuggerLogType GetLogType(string logType)
        {
            return m_LogTypes?.FirstOrDefault(c => c.Name == logType);
        }

        private static string Colorize(string text, string color, bool bold = false)
        {
            return $"<color={color}>{(bold ? "<b><size=13>" : "")}{text}{(bold ? "</size></b>" : "")}</color>";
        }
    }
}