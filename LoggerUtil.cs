using BepInEx.Configuration;
using BepInEx.Logging;

namespace MonitorLabels;

public static class LoggerUtil //TODO: Add log levels to the config so people can choose what messages they see (flag?)
{
	private static ManualLogSource logger;

	private static ConfigEntry<bool> configEnableLogger;

	public static bool IsLoggerEnabled => configEnableLogger.Value;

	internal static void Initialize(ConfigEntry<bool> enableLoggerEntry, ManualLogSource logSource)
	{
		configEnableLogger = enableLoggerEntry;

		logger = logSource;
	}

	internal static void Log(LogLevel logLevel, object data)
	{
		if (!IsLoggerEnabled)
		{
			return;
		}
		
		logger.Log(logLevel, data);
	}
	
	internal static void LogMessage(object data)
	{
		Log(LogLevel.Message, data);
	}

	internal static void LogInfo(object data)
	{
		Log(LogLevel.Info, data);
	}
	
	internal static void LogDebug(object data)
	{
		Log(LogLevel.Debug, data);
	}

	internal static void LogError(object data)
	{
		Log(LogLevel.Error, data);
	}

	internal static void LogWarning(object data)
	{
		Log(LogLevel.Warning, data);
	}

	internal static void LogFatal(object data)
	{
		Log(LogLevel.Fatal, data);
	}
}