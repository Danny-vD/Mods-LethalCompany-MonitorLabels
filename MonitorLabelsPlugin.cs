using System;
using BepInEx;
using GameNetcodeStuff;
using HarmonyLib;
using TMPro;
using UnityEngine;

namespace MonitorLabels
{
	[BepInPlugin(GUID, PLUGIN_NAME, PLUGIN_VERSION)]
	[BepInProcess("Lethal Company.exe")]
	public class MonitorLabelsPlugin : BaseUnityPlugin
	{
		public const string GUID = "DannyVD.mods.LethalCompany.MonitorLabels";
		public const string PLUGIN_NAME = "MonitorLabels";
		public const string PLUGIN_VERSION = "1.1.0";

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

		public static void UpdateLabels()
		{
			if (StartOfRound.Instance == null || StartOfRound.Instance.mapScreen == null)
			{
				return;
			}

			for (int index = 0; index < StartOfRound.Instance.mapScreen.radarTargets.Count; ++index)
			{
				TransformAndName transAndName = StartOfRound.Instance.mapScreen.radarTargets[index];

				if (transAndName.transform != null)
				{
					LoggerUtil.LogInfo($"Name: {transAndName.name} index: {index} isNonPlayer: {transAndName.isNonPlayer}");
					AddTargetLabel(transAndName.transform.gameObject, index, transAndName.name);
				}
			}
		}

		internal static void AddTargetLabel(GameObject target, int index, string targetName) // TODO: Fix the absolute mess that is this function and make radar booster label optional
		{
			bool isCurrentTarget = StartOfRound.Instance.mapScreen.targetTransformIndex == index;
			bool isDead = false;

			Color labelColour = isCurrentTarget ? ConfigUtil.TargetLabelColour.Value : ConfigUtil.OtherLabelColour.Value;

			// Alive players
			GameObject labelParent = null; // target.transform.Find("Misc")?.Find(MapLabelUtil.MAP_DOT_NAME)?.gameObject;

			PlayerControllerB playerController = target.GetComponent<PlayerControllerB>();

			if (playerController != null && playerController.isPlayerDead)
			{
				LoggerUtil.LogInfo($"{target.gameObject.name} is dead");

				if (playerController.redirectToEnemy != null)
				{
					labelParent = playerController.redirectToEnemy.transform.Find("Misc")?.Find(MapLabelUtil.MAP_DOT_NAME)?.gameObject;
				}
				else if (playerController.deadBody != null)
				{
					LoggerUtil.LogInfo("Has body");
					labelParent = playerController.deadBody.transform.Find(MapLabelUtil.MAP_DOT_NAME)?.gameObject;
				}

				labelColour = ConfigUtil.DeadLabelColour.Value;
				isDead      = true;
			}
			else
			{
				LoggerUtil.LogInfo("Not Dead");
				labelParent = target.transform.Find("Misc")?.Find(MapLabelUtil.MAP_DOT_NAME)?.gameObject;
			}

			// Radar boosters
			if (labelParent == null)
			{
				LoggerUtil.LogInfo("Maybe Radar Booster");
				labelParent = target.transform.Find("RadarBoosterDot")?.gameObject;

				if (!isCurrentTarget)
				{
					labelColour = ConfigUtil.RadarBoosterLabelColour.Value;
				}
			}

			if (labelParent == null)
			{
				LoggerUtil.LogWarning("No parent findable for this radar target");
				return;
			}

			GameObject labelObject = labelParent.transform.Find(MapLabelUtil.LABEL_OBJECT_NAME)?.gameObject;
			TMP_Text labelComponent;

			if (labelObject == null)
			{
				labelComponent = MapLabelUtil.AddLabelObject(labelParent);
			}
			else
			{
				labelComponent = labelObject.GetComponent<TextMeshPro>();
			}

			labelComponent.color = labelColour;
			labelComponent.text  = GetLabelString(targetName, index, isCurrentTarget, isDead);
		}

		private static string GetLabelString(string targetName, int index, bool isTarget, bool isDead)
		{
			if (isDead)
			{
				if (ConfigUtil.ForceDeadPlayerLabel.Value)
				{
					return GetPlayerNameString(targetName, index, true);
				}

				if (ConfigUtil.HideDeadLabels.Value)
				{
					return string.Empty;
				}
			}

			if (ConfigUtil.HideNormalLabels.Value)
			{
				return string.Empty;
			}

			if (isTarget)
			{
				if (!ConfigUtil.ShowLabelOnTarget.Value)
				{
					return string.Empty;
				}
			}

			return GetPlayerNameString(targetName, index, isDead);
		}

		private static string GetPlayerNameString(string targetName, int index, bool isDead = false)
		{
			if (isDead)
			{
				string customName = ConfigUtil.CustomDeadName.Value;

				if (customName != string.Empty)
				{
					targetName = customName;
				}
			}

			int length = Mathf.Min(targetName.Length, ConfigUtil.MaximumNameLength.Value);
			targetName = targetName.Substring(0, length);

			return string.Format(ConfigUtil.PlayerLabelStringFormat.Value, targetName, index);
		}
	}
}