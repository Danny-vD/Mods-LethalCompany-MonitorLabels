using BepInEx;
using BepInEx.Logging;
using MonitorLabels.Dependencies.LethalConfig;
using MonitorLabels.Utils.ModUtils;

namespace MonitorLabels
{
	[BepInPlugin(GUID, PLUGIN_NAME, PLUGIN_VERSION)]
	[BepInProcess("Lethal Company.exe")]
	[BepInDependency(LethalConfigUtils.LETHAL_CONFIG_GUID, BepInDependency.DependencyFlags.SoftDependency)]
	public class MonitorLabelsPlugin : BaseUnityPlugin
	{
		//TODO: Make the icon of traps yellow when they are an immediate threat to a player
		public const string GUID = "DannyVD.mods.LethalCompany.MonitorLabels";
		public const string PLUGIN_NAME = "MonitorLabels";
		public const string PLUGIN_VERSION = "2.0.1";
		
		public const string DEPENDENCY_STRING = $"DannyVD-{PLUGIN_NAME}-{PLUGIN_VERSION}";

		private void Awake()
		{
			ConfigUtil.Initialize(Config);
			ConfigUtil.ReadConfig();
			
			LoggerUtil.Initialize(ConfigUtil.LoggingLevel, Logger);

			// Plugin startup logic
			LoggerUtil.Log(LogLevel.Info, $"Plugin {DEPENDENCY_STRING} is loaded!"); // Using the Log function circumvents the configuration option, this is by design

			PatchUtil.PatchFunctions();
			
			DependencyUtils.CheckDependencies();
		}
	}
}