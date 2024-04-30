using GameNetcodeStuff;
using MonitorLabels.Utils.ModUtils;

namespace MonitorLabels.Utils
{
	public static class PlayerItemSlotsUtil
	{
		public static void GetFirstToolAndFirstToolInUse(PlayerControllerB player, out GrabbableObject firstTool, out GrabbableObject firstToolInUse)
		{
			firstTool      = null;
			firstToolInUse = null;

			if (ReferenceEquals(player, null))
			{
				return;
			}

			foreach (GrabbableObject item in player.ItemSlots)
			{
				if (item == null || item.itemProperties.isScrap)
				{
					continue;
				}

				if (ReferenceEquals(firstTool, null))
				{
					firstTool = item;
				}

				if (item.isBeingUsed && ReferenceEquals(firstToolInUse, null))
				{
					firstToolInUse = item;
					return; // We set both firstTool and firstToolInUse, so we can quit here
				}
			}
		}

		public static void UpdateLabelsOfItemSlots(PlayerControllerB player)
		{
			if (!ConfigUtil.ShowLabelOnTools.Value)
			{
				return; // Don't bother updating anything if tools have no label
			}

			GetFirstToolAndFirstToolInUse(player, out GrabbableObject firstTool, out GrabbableObject firstToolInUse);

			for (int i = 0; i < player.ItemSlots.Length; i++) // Update all item slots because multiple labels are affected by holding a tool, using a tool etc.
			{
				GrabbableObject item = player.ItemSlots[i];

				if (item == null)
				{
					continue;
				}

				if (item.itemProperties.isScrap)
				{
					continue;
				}

				ObjectLabelManager.UpdateItemSlotLabel(item, firstTool, firstToolInUse);
			}
		}
	}
}