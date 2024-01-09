using MonitorLabels.Utils;
using UnityEngine;

namespace MonitorLabels
{
	internal static class RadarTargetLabelManager
	{
		internal static void AddTargetLabel(GameObject target, int index, string targetName)
		{
			
		}

		internal static void UpdateLabels() //TODO: Use the index provided by the function
		{
			if (StartOfRound.Instance == null || StartOfRound.Instance.mapScreen == null)
			{
				return;
			}

			for (int index = 0; index < StartOfRound.Instance.mapScreen.radarTargets.Count; ++index)
			{
				bool isCurrentTarget = StartOfRound.Instance.mapScreen.targetTransformIndex == index;
				TransformAndName transAndName = StartOfRound.Instance.mapScreen.radarTargets[index];

				if (transAndName.transform != null)
				{
					AddTargetLabel(transAndName.transform.gameObject, index, transAndName.name);
				}
			}
		}

		private static string GetLabelString(string targetName, int index, bool isTarget, bool isDead)
		{
			if (isDead)
			{
				if (ConfigUtil.ForceDeadPlayerLabel.Value)
				{
					return GetPlayerNameString(targetName, index, true);
				}

				if (ConfigUtil.HideDeadPlayerLabels.Value)
				{
					return string.Empty;
				}
			}

			if (ConfigUtil.HideNormalPlayerLabels.Value)
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