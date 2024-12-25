using System;
using LobbyCompatibility.Enums;
using LobbyCompatibility.Features;

namespace MonitorLabels.Dependencies.LobbyCompatibility
{
	internal static class LobbyCompatibilityUtils
	{
		internal const string LOBBY_COMPATIBILITY_GUID = global::LobbyCompatibility.PluginInfo.PLUGIN_GUID;
		
		internal static void RegisterMod()
		{
			PluginHelper.RegisterPlugin(MonitorLabelsPlugin.GUID, new Version(MonitorLabelsPlugin.PLUGIN_VERSION), CompatibilityLevel.ClientOnly, VersionStrictness.None);
		}
	}
}