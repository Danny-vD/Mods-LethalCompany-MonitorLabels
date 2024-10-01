using BepInEx.Bootstrap;
using MonitorLabels.Dependencies.LethalConfig;

namespace MonitorLabels.Utils.ModUtils
{
	public static class DependencyUtils
	{
		public static bool LethalConfigPresent { get; private set; }
		
		internal static void CheckDependencies()
		{
			if (Chainloader.PluginInfos.TryGetValue(LethalConfigUtils.LETHAL_CONFIG_GUID, out BepInEx.PluginInfo pluginInfo)) // Soft dependency
			{
				LoggerUtil.LogInfo($"{pluginInfo.Metadata.Name} has been found!\nAdding apply button to config...");

				LethalConfigPresent = true;
				LethalConfigUtils.SetUpConfig();
			}
		}
	}
}