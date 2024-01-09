using GameNetcodeStuff;
using MonitorLabels.Utils;
using TMPro;
using UnityEngine;

namespace MonitorLabels
{
	internal static class RadarTargetLabelManager
	{
		internal static void UpdateLabels(int radarTargetIndex = -1)
		{
			if (StartOfRound.Instance == null || StartOfRound.Instance.mapScreen == null)
			{
				return;
			}

			int targetIndex = radarTargetIndex != -1 ? radarTargetIndex : StartOfRound.Instance.mapScreen.targetTransformIndex;

			for (int index = 0; index < StartOfRound.Instance.mapScreen.radarTargets.Count; ++index)
			{
				bool isCurrentTarget = targetIndex == index;
				TransformAndName transformAndName = StartOfRound.Instance.mapScreen.radarTargets[index];

				if (transformAndName.transform != null)
				{
					AddTargetLabel(transformAndName, index, isCurrentTarget);
				}
			}
		}

		internal static void AddTargetLabel(TransformAndName transformAndName, int index, bool isCurrentTarget)
		{
			GameObject radarTarget = transformAndName.transform.gameObject;
			PlayerControllerB playerControllerB = radarTarget.GetComponent<PlayerControllerB>();

			Transform labelParent = null;

			bool isDead = false;

			if (playerControllerB != null)
			{
				if (playerControllerB.isPlayerDead)
				{
					isDead = true;

					if (playerControllerB.redirectToEnemy != null)
					{
						// The only enemy is a MaskedPlayerEnemy, who uses the Misc->MapDot structure
						labelParent = FindRadarDotOfPlayer(playerControllerB.redirectToEnemy.transform);
					}
					else if (playerControllerB.deadBody != null)
					{
						// A dead body has the MapDot as a child
						labelParent = MapLabelUtil.GetMapDot(playerControllerB.deadBody.transform);
					}
				}
				else
				{
					labelParent = FindRadarDotOfPlayer(playerControllerB.transform);
				}
			}
			else // No playerController, it is probably a radar booster
			{
				labelParent = MapLabelUtil.GetRadarBoosterLabel(transformAndName.transform);
			}

			if (labelParent == null)
			{
				LoggerUtil.LogError("Cannot find radar icon for " + transformAndName.name);
				return;
			}

			Transform labelObject = MapLabelUtil.GetRadarLabel(labelParent, out TMP_Text labelComponent);

			if (labelComponent == null)
			{
				if (labelObject != null)
				{
					LoggerUtil.LogError("The LabelObject exists but the TMP_Text component does not, this should never happen!\nDestroying the object and remaking it...");
					Object.Destroy(labelObject);
				}

				labelComponent = MapLabelUtil.AddLabelObject(labelParent.gameObject, true);
			}

			labelComponent.text  = GetLabelString(transformAndName.name, index, isCurrentTarget, isDead, transformAndName.isNonPlayer, out Color labelColour);
			labelComponent.color = labelColour;
		}

		private static string GetLabelString(string targetName, int index, bool isTarget, bool isDead, bool isRadarBooster, out Color labelColour)
		{
			if (isDead)
			{
				labelColour = ConfigUtil.DeadPlayerLabelColour.Value;

				if (ConfigUtil.ForceDeadPlayerLabel.Value)
				{
					return GetPlayerNameString(targetName, index, true);
				}

				if (ConfigUtil.HideDeadPlayerLabels.Value || ConfigUtil.HideNormalPlayerLabels.Value)
				{
					return string.Empty;
				}
			}
			else if (!isRadarBooster && ConfigUtil.HideNormalPlayerLabels.Value)
			{
				labelColour = Color.white;
				return string.Empty;
			}
			else if (isTarget)
			{
				labelColour = ConfigUtil.TargetLabelColour.Value;

				if (!ConfigUtil.ShowLabelOnTarget.Value)
				{
					return string.Empty;
				}
			}
			else if (isRadarBooster)
			{
				labelColour = ConfigUtil.RadarBoosterLabelColour.Value;
			}
			else
			{
				labelColour = ConfigUtil.DefaultPlayerLabelColour.Value;
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

		/// <summary>
		/// Searches the children for Misc and then searches again for the MapDot
		/// </summary>
		private static Transform FindRadarDotOfPlayer(Transform parent)
		{
			Transform misc = parent.Find("Misc");
			return MapLabelUtil.GetMapDot(misc);
		}
	}
}