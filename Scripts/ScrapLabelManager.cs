using MonitorLabels.ExtensionMethods;
using MonitorLabels.Utils;
using TMPro;
using UnityEngine;

namespace MonitorLabels
{
	internal static class ScrapLabelManager
	{
		internal static void TryAddLabelToScrap(GrabbableObject item)
		{
			Transform radarIcon = item.radarIcon;

			if (radarIcon == null) // No radar icon, this can happen because some items (e.g. flashlights and keys) do not have a radar icon
			{
				return;
			}

			AddLabelToScrap(item, radarIcon.gameObject);
		}

		internal static void UpdateScrapLabel(GrabbableObject item)
		{
			Transform radarIcon = item.radarIcon;

			if (radarIcon == null) // No radar icon, this can happen because some items (e.g. flashlights and keys) do not have a radar icon
			{
				return;
			}
			
			_ = MapLabelUtil.GetRadarLabel(radarIcon, out TMP_Text radarLabel);

			if (ReferenceEquals(radarLabel, null)) // No map label
			{
				return;
			}

			radarLabel.text  = GetScrapLabelString(item, out Color labelColour);
			radarLabel.color = labelColour;
		}

		private static void AddLabelToScrap(GrabbableObject item, GameObject radarParent)
		{
			TMP_Text label = MapLabelUtil.AddLabelObject(radarParent, ConfigUtil.ScrapLabelOffset.Value, false);

			label.fontSize *= ConfigUtil.ScrapLabelScaleFactor.Value;

			label.text  = GetScrapLabelString(item, out Color labelColor);
			label.color = labelColor;
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

			if (TryGetCustomLabel(item, out string label))
			{
				return label;
			}

			return GetFormattedScrapLabel(GetScrapName(item), scrapValue);
		}

		/// <summary>
		/// Used for special cases where the label of a specific object depends on something specific to that object
		/// </summary>
		private static bool TryGetCustomLabel(GrabbableObject item, out string label)
		{
			label = string.Empty;
			
			if (ConfigUtil.HideScrapLabelOnNutcracker.Value && item is ShotgunItem)
			{
				if (item.isHeldByEnemy)
				{
					return true;
				}
			}

			return false;
		}
		
		private static string GetFormattedScrapLabel(string scrapName, int scrapValue)
		{
			return string.Format(ConfigUtil.ScrapLabelStringFormat.Value, scrapName, scrapValue);
		}
		
		private static string GetScrapName(GrabbableObject item)
		{
			ScanNodeProperties scanNodeProperties = item.GetComponentInChildren<ScanNodeProperties>();
			
			// If scannode is available, take the name from that
			return ReferenceEquals(scanNodeProperties, null) ? MapLabelUtil.RemoveCloneFromString(item.gameObject.name).InsertSpaceBeforeCapitals() : scanNodeProperties.headerText;
		}
	}
}