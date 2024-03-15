using MonitorLabels.ExtensionMethods;
using MonitorLabels.Utils;
using MonitorLabels.Utils.ModUtils;
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
			bool isScrap = item.itemProperties.isScrap;
			
			TMP_Text label = MapLabelUtil.AddLabelObject(radarParent, isScrap ? ConfigUtil.ScrapLabelOffset.Value : ConfigUtil.ToolLabelOffset.Value, false);

			if (isScrap)
			{
				label.fontSize *= ConfigUtil.ScrapLabelScaleFactor.Value;
			}
			else
			{
				label.fontSize = ConfigUtil.ToolLabelFontSize.Value;
			}

			label.text  = GetScrapLabelString(item, out Color labelColor);
			label.color = labelColor;
		}

		private static string GetScrapLabelString(GrabbableObject item, out Color labelColour)
		{
			bool isTool = !item.itemProperties.isScrap;

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

			if (isTool)
			{
				labelColour = ConfigUtil.ToolLabelColour.Value;
			}

			if (TryGetCustomLabel(item, out string label))
			{
				return label;
			}

			return GetFormattedScrapLabel(item, scrapValue, isTool);
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

		private static string GetFormattedScrapLabel(GrabbableObject item, int scrapValue, bool isTool)
		{
			string scrapName = GetScrapName(item);

			if (isTool)
			{
				return string.Format(ConfigUtil.ToolLabelStringFormat.Value, scrapName);
			}

			return string.Format(ConfigUtil.ScrapLabelStringFormat.Value, scrapName, scrapValue);
		}

		private static string GetScrapName(GrabbableObject item)
		{
			ScanNodeProperties scanNodeProperties = item.GetComponentInChildren<ScanNodeProperties>();

			if (scanNodeProperties) // If scannode is available, take the name from that
			{
				return scanNodeProperties.headerText;
			}

			Item itemProperties = item.itemProperties;

			if (itemProperties && !string.IsNullOrEmpty(itemProperties.itemName))
			{
				return itemProperties.itemName;
			}

			return MapLabelUtil.RemoveCloneFromString(item.gameObject.name).InsertSpaceBeforeCapitals();
		}
	}
}