using System.Linq;
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
			// Item slots are not applicable because this is not a tool
			UpdateItemSlotLabel(item, null, null);
		}
		
		internal static void UpdateItemSlotLabel(GrabbableObject item, GrabbableObject firstToolInItemSlots, GrabbableObject firstToolInUseInItemSlots)
		{
			Transform radarIcon = item.radarIcon;

			if (radarIcon == null) // No radar icon, this can happen because some items (e.g. flashlights and keys) do not have a radar icon by default
			{
				return;
			}

			ContinuouslyUpdateToolLabel toolLabelUpdater = item.GetComponent<ContinuouslyUpdateToolLabel>();

			if (toolLabelUpdater != null && toolLabelUpdater.IsUpdating)
			{
				return; // It is already being updated by something else
			}

			_ = MapLabelUtil.GetRadarLabel(radarIcon, out TMP_Text radarLabel);

			if (ReferenceEquals(radarLabel, null)) // No map label
			{
				return;
			}

			SetScrapLabel(item, radarLabel, firstToolInItemSlots, firstToolInUseInItemSlots);
		}

		internal static void SetScrapLabel(GrabbableObject item, TMP_Text radarLabel, GrabbableObject firstToolInItemSlots = null, GrabbableObject firstToolInUseInItemSlots = null)
		{
			radarLabel.text  = GetScrapLabelString(item, out Color labelColour, firstToolInItemSlots, firstToolInUseInItemSlots);
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

		private static string GetScrapLabelString(GrabbableObject item, out Color labelColour, GrabbableObject firstToolInItemSlots, GrabbableObject firstToolInUseInItemSlots)
		{
			if (item == null)
			{
				labelColour = Color.white;
				return string.Empty;
			}

			bool isTool = !item.itemProperties.isScrap;

			int scrapValue = item.scrapValue;

			if (item.isHeld) // If an item is pocketed, it is always being held
			{
				if (isTool && item.playerHeldBy != null)
				{
					labelColour = ConfigUtil.CarriedToolLabelColour.Value;

					GrabbableObject currentlyHeldObject = item.playerHeldBy.currentlyHeldObjectServer;
					bool currentlyHoldingATool = currentlyHeldObject != null && !currentlyHeldObject.itemProperties.isScrap;

					if (item.isPocketed && ConfigUtil.HideToolLabelIfPocketed.Value)
					{
						if (item.isBeingUsed && ConfigUtil.ShowToolIfInUseAndNoOtherToolHeld.Value && !currentlyHoldingATool) // Show a pocketed tool in use when not holding another tool
						{
							if (ConfigUtil.OnlyShow1PocketedLabel.Value) // Prevent showing multiple pocketed tools in use
							{
								GrabbableObject firstItemInUse = item.playerHeldBy.ItemSlots.FirstOrDefault(grabbableObject => grabbableObject != null && grabbableObject.isBeingUsed);

								if (item != firstItemInUse)
								{
									return string.Empty;
								}
							}
						}
						else
						{
							return string.Empty;
						}
					}
					else if (item.isPocketed && ConfigUtil.OnlyShow1PocketedLabel.Value) // Show pocketed labels, but we only want to show one. We do this by preferring the first tool or the first tool in use
					{
						if (currentlyHoldingATool && !ConfigUtil.HideToolLabelIfInHand.Value) // If we are currently holding a tool with a visible label, we don't have to check anything else
						{
							return string.Empty;
						}

						if (ConfigUtil.ShowToolIfInUseAndNoOtherToolHeld.Value) // Prefer showing a tool that is in use
						{
							if (firstToolInUseInItemSlots != null)
							{
								if (firstToolInUseInItemSlots != item)
								{
									return string.Empty;
								}
							}
							else if (firstToolInItemSlots != item)
							{
								return string.Empty;
							}
						}
						else // Show the first tool
						{
							if (item != firstToolInItemSlots)
							{
								return string.Empty;
							}
						}
					}
					else if (!item.isPocketed && ConfigUtil.HideToolLabelIfInHand.Value) // If item is not pocketed and we should hide in hand, hide the label
					{
						return string.Empty;
					}
				}
				else if (isTool)
				{
					labelColour = ConfigUtil.CarriedToolLabelColour.Value;

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

		// ReSharper disable Unity.PerformanceAnalysis | REASON: Most of the time the expensive invocation is not done, only in the rare case that itemName is an empty string
		private static string GetScrapName(GrabbableObject item)
		{
			Item itemProperties = item.itemProperties;

			if (itemProperties && !string.IsNullOrEmpty(itemProperties.itemName))
			{
				return itemProperties.itemName;
			}

			ScanNodeProperties scanNodeProperties = item.GetComponentInChildren<ScanNodeProperties>();

			if (scanNodeProperties) // If scannode is available, take the name from that
			{
				return scanNodeProperties.headerText;
			}

			return MapLabelUtil.RemoveCloneFromString(item.gameObject.name).InsertSpaceBeforeCapitals();
		}
	}
}