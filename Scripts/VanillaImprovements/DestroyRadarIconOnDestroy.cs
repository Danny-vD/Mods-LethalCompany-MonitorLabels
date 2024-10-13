using MonitorLabels.BaseClasses;
using UnityEngine;

namespace MonitorLabels.VanillaImprovements
{
	public class DestroyRadarIconOnDestroy : BetterMonoBehaviour
	{
		private GrabbableObject grabbableObject;
		
		private void Awake()
		{
			grabbableObject = GetComponent<GrabbableObject>();
		}

		private void OnDestroy()
		{
			if (grabbableObject == null)
			{
				return;
			}
			
			Transform radarIcon = grabbableObject.radarIcon;

			if (radarIcon == null)
			{
				return;
			}
			
			Destroy(radarIcon.gameObject);
			grabbableObject = null;
		}
	}
}