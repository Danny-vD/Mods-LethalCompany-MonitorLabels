using System.Collections.Generic;
using System.Linq;
using BepInEx.Bootstrap;
using MonitorLabels.Dependencies.LethalConfig;
using MonitorLabels.Dependencies.LobbyCompatibility;
using UnityEngine;

namespace MonitorLabels.Utils.ModUtils
{
	public static class DependencyUtils
	{
		public static bool LethalConfigPresent { get; private set; }
		public static bool LobbyCompatibilityPresent { get; private set; }

		internal static void CheckDependencies()
		{
			CheckSoftDependencies();
		}

		private static void CheckSoftDependencies()
		{
			if (Chainloader.PluginInfos.TryGetValue(LethalConfigUtils.LETHAL_CONFIG_GUID, out BepInEx.PluginInfo pluginInfo))
			{
				LoggerUtil.LogInfo($"{pluginInfo.Metadata.Name} has been found!\nAdding apply button to config...");

				LethalConfigPresent = true;
				LethalConfigUtils.SetUpConfig();
			}

			if (Chainloader.PluginInfos.TryGetValue(LobbyCompatibilityUtils.LOBBY_COMPATIBILITY_GUID, out pluginInfo))
			{
				LoggerUtil.LogInfo($"{pluginInfo.Metadata.Name} has been found!\nRegistering {MonitorLabelsPlugin.PLUGIN_NAME}...");

				LobbyCompatibilityPresent = true;
				LobbyCompatibilityUtils.RegisterMod();
			}
		}
	}
}