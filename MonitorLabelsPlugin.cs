using System;
using BepInEx;
using HarmonyLib;
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

			LoggerUtil.Initialize(ConfigUtil.EnableLogger, Logger);

			// Plugin startup logic
			Logger.LogInfo($"Plugin {GUID} is loaded!"); // Deliberately circumvent the loggerUtil so that we always log

			Harmony harmonyInstance = new Harmony(GUID);
			LoggerUtil.LogInfo("Attempting to patch with Harmony!");

			try
			{
				harmonyInstance.PatchAll();
				LoggerUtil.LogInfo("Patching success!");
			}
			catch (Exception ex)
			{
				Logger.LogError("Failed to patch: " + ex); // Always log the error
			}
		}
	}
}