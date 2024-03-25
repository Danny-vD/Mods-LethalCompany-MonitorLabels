using MonitorLabels.Components;
using MonitorLabels.Components.Tools;
using MonitorLabels.ExtensionMethods;
using MonitorLabels.Utils;
using MonitorLabels.Utils.ModUtils;
using TMPro;
using UnityEngine;

namespace MonitorLabels
{
	internal static class ObjectLabelManager
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
			ContinuouslyUpdateToolLabel toolLabelUpdater = item.GetComponent<ContinuouslyUpdateToolLabel>();

			if (toolLabelUpdater != null && toolLabelUpdater.IsUpdating)
			{
				return; // It is already being updated by something else
			}

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

			SetScrapLabel(item, radarLabel);
		}

		internal static void SetScrapLabel(GrabbableObject item, TMP_Text radarLabel)
		{
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

			SetScrapLabel(item, label);

			if (item.itemProperties.requiresBattery && ConfigUtil.ShowBatteryChargeOnLabel.Value)
			{
				item.gameObject.AddComponent<ContinuouslyUpdateToolLabel>().Initialize(item, label);
			}
		}

		private static string GetScrapLabelString(GrabbableObject item, out Color labelColour)
		{
			bool isTool = !item.itemProperties.isScrap;

			int scrapValue = item.scrapValue;

			if (item.isHeld) // If an item is pocketed, it is always being held
			{
				if (isTool)
				{
					labelColour = ConfigUtil.CarriedToolLabelColour.Value;

					if (item.isPocketed && ConfigUtil.HideToolLabelIfPocketed.Value)
					{
						bool currentlyHoldingATool = item.playerHeldBy.currentlyHeldObjectServer != null && !item.playerHeldBy.currentlyHeldObjectServer.itemProperties.isScrap;
						
						if (item.isBeingUsed && ConfigUtil.ShowToolIfInUseAndNoOtherToolHeld.Value && !currentlyHoldingATool) // reads easier
						{
						}
						else
						{
							return string.Empty;
						}
					}

					if (ConfigUtil.HideToolLabelIfInHand.Value)
					{
						return string.Empty;
					}
				}
				else
				{
					labelColour = ConfigUtil.CarriedScrapLabelColour.Value;

					if (ConfigUtil.HideScrapLabelIfCarried.Value)
					{
						return string.Empty;
					}
				}
			}
			else if (item.isInShipRoom)
			{
				if (isTool)
				{
					labelColour = ConfigUtil.InShipToolLabelColour.Value;

					if (ConfigUtil.HideToolLabelIfOnShip.Value)
					{
						return string.Empty;
					}
				}
				else
				{
					labelColour = ConfigUtil.InShipScrapLabelColour.Value;

					if (ConfigUtil.HideScrapLabelIfOnShip.Value)
					{
						return string.Empty;
					}
				}
			}
			else
			{
				if (isTool)
				{
					labelColour = ConfigUtil.ToolLabelColour.Value;
				}
				else
				{
					labelColour = scrapValue >= ConfigUtil.HighValueScrapThreshold.Value ? ConfigUtil.HighValueScrapLabelColour.Value : ConfigUtil.ScrapLabelColour.Value;
				}
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
				string batteryString = string.Empty;

				if (item.itemProperties.requiresBattery && ConfigUtil.ShowBatteryChargeOnLabel.Value && item.insertedBattery != null)
				{
					float charge = item.insertedBattery.empty ? 0 : item.insertedBattery.charge;

					batteryString = string.Format(ConfigUtil.ToolBatteryStringFormat.Value, charge);
				}

				return string.Format(ConfigUtil.ToolLabelStringFormat.Value, scrapName, batteryString);
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