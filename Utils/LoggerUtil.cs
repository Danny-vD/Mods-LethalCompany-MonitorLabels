using BepInEx.Configuration;
using BepInEx.Logging;

namespace MonitorLabels.Utils;

public static class LoggerUtil
{
	private static ManualLogSource logger;

	private static ConfigEntry<LogLevel> configLoggingLevel;

	public static bool IsLoggerEnabled => configLoggingLevel.Value > 0;

	internal static void Initialize(ConfigEntry<LogLevel> enableLoggerEntry, ManualLogSource logSource)
	{
		configLoggingLevel = enableLoggerEntry;

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
		if (!configLoggingLevel.Value.HasFlag(LogLevel.Message))
		{
			return;
		}
		
		Log(LogLevel.Message, data);
	}

	internal static void LogInfo(object data)
	{
		if (!configLoggingLevel.Value.HasFlag(LogLevel.Info))
		{
			return;
		}
		
		Log(LogLevel.Info, data);
	}
	
	internal static void LogDebug(object data)
	{
		if (!configLoggingLevel.Value.HasFlag(LogLevel.Debug))
		{
			return;
		}
		
		Log(LogLevel.Debug, data);
	}

	internal static void LogError(object data)
	{
		if (!configLoggingLevel.Value.HasFlag(LogLevel.Error))
		{
			return;
		}
		
		Log(LogLevel.Error, data);
	}

	internal static void LogWarning(object data)
	{
		if (!configLoggingLevel.Value.HasFlag(LogLevel.Warning))
		{
			return;
		}
		
		Log(LogLevel.Warning, data);
	}

	internal static void LogFatal(object data)
	{
		if (!configLoggingLevel.Value.HasFlag(LogLevel.Fatal))
		{
			return;
		}
		
		Log(LogLevel.Fatal, data);
	}
}