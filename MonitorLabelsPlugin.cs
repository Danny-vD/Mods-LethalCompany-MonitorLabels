using BepInEx;
using BepInEx.Logging;
using MonitorLabels.Utils;

namespace MonitorLabels
{
	[BepInPlugin(GUID, PLUGIN_NAME, PLUGIN_VERSION)]
	[BepInProcess("Lethal Company.exe")]
	public class MonitorLabelsPlugin : BaseUnityPlugin
	{
		public const string GUID = "DannyVD.mods.LethalCompany.MonitorLabels";
		public const string PLUGIN_NAME = "MonitorLabels";
		public const string PLUGIN_VERSION = "1.1.0"; //TODO: Consider making this 2.0.0

		private void Awake()
		{
			ConfigUtil.Initialize(Config);
			ConfigUtil.ReadConfig();
			
			LoggerUtil.Initialize(ConfigUtil.LoggingLevel, Logger);

			// Plugin startup logic
			LoggerUtil.Log(LogLevel.Info, $"Plugin {GUID} is loaded!"); // Using the Log function circumvents the configuration option, this is by design

			PatchUtil.PatchFunctions();
		}
	}
}