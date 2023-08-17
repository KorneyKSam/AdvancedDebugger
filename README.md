AdvancedDebugger is 100% free to use.

# Initialization example

public static class DebuggerLog //String keys of log types
{
    public static string Debug => "Debug";
    public static string Info => "Info";
    public static string InfoWarning => "InfoWarning";
    public static string Warning => "Warning";
    public static string Error => "Error";     
}

public static class DebuggerInitializerExample
{
    private const string DateTimeFormat = "dd-MM-yyyy HH:mm:ss.ffff";

    public static void Initialize()
    {
        //If you want use log writing, set this to true, set logFilePath and dateTimeFormat(optional) in Initialization
        Debugger.EnableLogWriting = true;

        //If you want use debug colorization, set this to true, add add DebuggerColorization collection in Initialization
        Debugger.EnableMarkupFormat = true;

        //More arguments are optional
        Debugger.Initialize(GetDebuggerLogTypes(), GetColorizations(), Debug.Log, GetLogPath(), DateTimeFormat);
    }

    private static List<DebuggerLogType> GetDebuggerLogTypes()
    {
        return new List<DebuggerLogType>()
        {
            new DebuggerLogType(DebuggerLog.Debug, Debug.Log, "#FFFFFF", false),
            new DebuggerLogType(DebuggerLog.Info, Debug.Log, "#BDC7F0", true),
            new DebuggerLogType(DebuggerLog.InfoWarning, Debug.LogWarning, "#FFFFFF", false),
            new DebuggerLogType(DebuggerLog.Warning, Debug.LogWarning, "#FF5500", true),
            new DebuggerLogType(DebuggerLog.Error, Debug.LogError, "#FF0000", true),
        };
    }

    private static List<DebuggerColorization> GetColorizations()//Optional if you using EnableMarkupFormat
    {
        return new List<DebuggerColorization>()//When these classes use Debugger.Log their names will be colorized
        {
            new DebuggerColorization(nameof(TestClass1), Color.green),
            new DebuggerColorization(nameof(TestClass2), Color.yellow),
            new DebuggerColorization(nameof(TestClass3), Color.blue),
        };
    }
    
    private static string GetLogPath()
    {
        return $@"{Application.persistentDataPath}/Logs/Logs.txt";
    }
}