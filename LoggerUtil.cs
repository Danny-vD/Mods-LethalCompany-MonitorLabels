using BepInEx.Configuration;
using BepInEx.Logging;

namespace PlayerMapName;

public static class LoggerUtil
{
	private static ManualLogSource logger;

	private static ConfigEntry<bool> configEnableLogger;

	public static bool IsLoggerEnabled => configEnableLogger.Value;

	public static void Initialize(ConfigEntry<bool> enableLoggerEntry, ManualLogSource logSource)
	{
		configEnableLogger = enableLoggerEntry;

		logger = logSource;
	}

	public static void Log(LogLevel logLevel, object data)
	{
		if (!IsLoggerEnabled)
		{
			return;
		}
		
		logger.Log(logLevel, data);
	}
	
	public static void LogMessage(object data)
	{
		Log(LogLevel.Message, data);
	}

	public static void LogInfo(object data)
	{
		Log(LogLevel.Info, data);
	}
	
	public static void LogDebug(object data)
	{
		Log(LogLevel.Debug, data);
	}

	public static void LogError(object data)
	{
		Log(LogLevel.Error, data);
	}

	public static void LogWarning(object data)
	{
		Log(LogLevel.Warning, data);
	}

	public static void LogFatal(object data)
	{
		Log(LogLevel.Fatal, data);
	}
}