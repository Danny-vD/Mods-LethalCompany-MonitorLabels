using LethalConfig;
using LethalConfig.ConfigItems;
using MonitorLabels.Utils.ModUtils;

namespace MonitorLabels.Dependencies.LethalConfig
{
	internal static class LethalConfigUtils
	{
		internal const string LETHAL_CONFIG_GUID = "ainavt.lc.lethalconfig";
		
		internal static void SetUpConfig()
		{
			LethalConfigManager.AddConfigItem(new GenericButtonConfigItem("RELOAD CONFIG", "Apply all changes", $"Reloads the config for {MonitorLabelsPlugin.PLUGIN_NAME}\nThis way the settings will take immediate effect.", "Reload Configs", ApplyChanges));
		}

		private static void ApplyChanges()
		{
			ConfigUtil.ReadConfig();
		}
	}
}