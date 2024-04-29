using GameNetcodeStuff;
using MonitorLabels.Constants;
using MonitorLabels.Utils;
using MonitorLabels.Utils.ModUtils;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MonitorLabels
{
	internal static class RadarTargetLabelManager
	{
		internal static void UpdateLabel(Transform radarTargetTransform)
		{
			if (radarTargetTransform == null)
			{
				return;
			}

			TransformAndName transformAndName = RadarTargetUtils.GetMatchingRadarTarget(radarTargetTransform, out int radarIndex, out bool currentRadarTarget);

			if (transformAndName == null || transformAndName.transform == null)
			{
				LoggerUtil.LogWarning("Tried to update an invalid transform!\nUpdating all radar targets to make sure everything is correct!");
				UpdateLabels();
				return;
			}

			AddTargetLabel(transformAndName, radarIndex, currentRadarTarget);
		}

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

				if (transformAndName == null)
				{
					continue;
				}

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
					else
					{
						// Dead without a body
						return;
					}
				}
				else
				{
					labelParent = FindRadarDotOfPlayer(playerControllerB.transform);
				}
			}
			else // No playerController, it is probably a radar booster
			{
				labelParent = MapLabelUtil.GetRadarBoosterMapDot(transformAndName.transform);
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
					// Only happens if something deliberately removes the TMP_Text component
					LoggerUtil.LogError("The LabelObject exists but the TMP_Text component does not, this should never happen!\nDestroying the object and reinstantiating it...");
					Object.Destroy(labelObject.gameObject);
				}

				labelComponent = MapLabelUtil.AddLabelObject(labelParent.gameObject, ConfigUtil.RadarTargetLabelOffset.Value, true);
			}

			labelComponent.text  = GetLabelString(transformAndName, index, isCurrentTarget, isDead, transformAndName.isNonPlayer, playerControllerB, out Color labelColour);
			labelComponent.color = labelColour;
		}

		private static string GetLabelString(TransformAndName targetName, int index, bool isTarget, bool isDead, bool isRadarBooster, PlayerControllerB playerControllerB, out Color labelColour)
		{
			if (isDead)
			{
				labelColour = ConfigUtil.DeadPlayerLabelColour.Value;

				if (ConfigUtil.ForceDeadPlayerLabel.Value)
				{
					return GetRadarTargetNameString(targetName, index, true);
				}

				if (ConfigUtil.HideDeadPlayerLabels.Value || ConfigUtil.HidePlayerLabels.Value)
				{
					return string.Empty;
				}
			}
			else if (!isRadarBooster && ConfigUtil.HidePlayerLabels.Value)
			{
				labelColour = Color.white;
				return string.Empty;
			}
			else if (isTarget)
			{
				if (isRadarBooster)
				{
					labelColour = ConfigUtil.TargetRadarBoosterLabelColour.Value;
				}
				else
				{
					if (ConfigUtil.UseColorsToShowPlayerHealth.Value)
					{
						labelColour = ColorCalculator.GetColorDependingOnHealth(playerControllerB, true);
					}
					else
					{
						labelColour = playerControllerB.playerSteamId == SteamIDs.MY_ID ? Colors.DevColor : ConfigUtil.TargetPlayerLabelColour.Value;
					}
				}

				if (!ConfigUtil.ShowLabelOnTarget.Value)
				{
					return string.Empty;
				}
			}
			else if (isRadarBooster)
			{
				labelColour = ConfigUtil.RadarBoosterLabelColour.Value;

				if (ConfigUtil.HideRadarBoosterLabels.Value)
				{
					return string.Empty;
				}
			}
			else
			{
				if (ConfigUtil.UseColorsToShowPlayerHealth.Value)
				{
					labelColour = ColorCalculator.GetColorDependingOnHealth(playerControllerB, false);
				}
				else
				{
					labelColour = playerControllerB.playerSteamId == SteamIDs.MY_ID ? Colors.DevColor : ConfigUtil.DefaultPlayerLabelColour.Value;
				}
			}

			return GetRadarTargetNameString(targetName, index, isDead);
		}

		private static string GetRadarTargetNameString(TransformAndName targetTransformAndName, int index, bool isDead = false)
		{
			string targetName = targetTransformAndName.name;

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

			string carriedValue = string.Empty;

			if (!targetTransformAndName.isNonPlayer && !isDead)
			{
				PlayerControllerB playerControllerB = targetTransformAndName.transform.GetComponentInParent<PlayerControllerB>();
				int valueCarrying = ScrapUtil.GetTotalValueCarrying(playerControllerB, out int currentSlotValue);

				if (valueCarrying > 0)
				{
					carriedValue = string.Format(ConfigUtil.PlayerCarriedScrapValueStringFormat.Value, valueCarrying, currentSlotValue);
				}
			}

			return string.Format(ConfigUtil.PlayerLabelStringFormat.Value, targetName, index, carriedValue);
		}

		/// <summary>
		/// Searches the children for Misc and then searches again for the MapDot
		/// </summary>
		private static Transform FindRadarDotOfPlayer(Transform parent)
		{
			Transform misc = parent.Find("Misc");

			Transform mapDot = misc.Find(MapLabelUtil.MAP_DOT_NAME);

			return mapDot != null ? mapDot : MapLabelUtil.GetMapDot(misc);
		}
	}
}