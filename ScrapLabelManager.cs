using TMPro;
using UnityEngine;

namespace MonitorLabels
{
	internal static class ScrapLabelManager
	{
		internal static void AddLabelToScrap(GrabbableObject item)
		{
			Transform radarIcon = item.radarIcon;

			if (radarIcon == null) //No radar icon
			{
				return;
			}

			TMP_Text label = MapLabelUtil.AddLabelObject(radarIcon.gameObject);

			label.gameObject.transform.localPosition += new Vector3(-1, 0, 1) * 7.5f;

			label.fontSize *= 3;

			label.text  = GetScrapLabelString(item, out Color labelColor);
			label.color = labelColor;
		}

		internal static void UpdateScrapLabel(GrabbableObject item)
		{
			Transform radarIcon = item.radarIcon;

			if (radarIcon == null) // No radar icon
			{
				LoggerUtil.LogError("radarIcon is null for " + item.gameObject.name);
				return;
			}

			TMP_Text radarLabel = MapLabelUtil.GetRadarLabel(radarIcon);

			if (ReferenceEquals(radarLabel, null)) // No map label
			{
				return;
			}

			radarLabel.text  = GetScrapLabelString(item, out Color labelColour);
			radarLabel.color = labelColour;
		}

		private static string GetScrapLabelString(GrabbableObject item, out Color labelColour)
		{
			int scrapValue = item.scrapValue;

			if (item.isInShipRoom)
			{
				labelColour = ConfigUtil.InShipScrapLabelColour.Value;

				if (ConfigUtil.HideScrapLabelIfOnShip.Value)
				{
					return string.Empty;
				}
			}
			else if (item.isHeld || item.isPocketed)
			{
				labelColour = ConfigUtil.CarriedScrapLabelColour.Value;

				if (ConfigUtil.HideScrapLabelIfCarried.Value)
				{
					return string.Empty;
				}
			}
			else
			{
				labelColour = scrapValue >= ConfigUtil.HighValueScrapThreshold.Value ? ConfigUtil.HighValueScrapLabelColour.Value : ConfigUtil.ScrapLabelColour.Value;
			}

			string objectName = MapLabelUtil.RemoveCloneFromString(item.gameObject.name);

			return string.Format(ConfigUtil.ScrapLabelStringFormat.Value, objectName, scrapValue);
		}
	}
}