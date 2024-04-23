using System.Collections;
using MonitorLabels.BaseClasses;
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
			ObjectLabelManager.SetScrapLabel(item, label);
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