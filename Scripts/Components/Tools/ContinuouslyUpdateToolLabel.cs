using System.Collections;
using MonitorLabels.BaseClasses;
using MonitorLabels.Utils;
using TMPro;
using UnityEngine;

namespace MonitorLabels.Components.Tools
{
	internal class ContinuouslyUpdateToolLabel : BetterMonoBehaviour
	{
		internal bool IsUpdating { get; private set; }

		private TMP_Text label;
		private GrabbableObject item;

		internal void Initialize(GrabbableObject grabbableObject, TMP_Text radarLabel)
		{
			item  = grabbableObject;
			label = radarLabel;

			StartCoroutine(UpdateLabelRoutine());
		}

		private void UpdateLabel()
		{
			//NOTE: By default the battery charge is only synced when the item is dropped, maybe sync it ourselves?
			item.SyncBatteryServerRpc(item.insertedBattery.charge * 100);
			
			PlayerItemSlotsUtil.GetFirstToolAndFirstToolInUse(item.playerHeldBy, out GrabbableObject firstTool, out GrabbableObject firstToolInUse);
			ObjectLabelManager.SetScrapLabel(item, label, firstTool, firstToolInUse);
		}

		private IEnumerator UpdateLabelRoutine()
		{
			while (true)
			{
				IsUpdating = true;

				while (item.isBeingUsed)
				{
					yield return new WaitForEndOfFrame();

					UpdateLabel();
				}

				IsUpdating = false;

				yield return new WaitUntil(() => item.isBeingUsed);
			}
		}
	}
}