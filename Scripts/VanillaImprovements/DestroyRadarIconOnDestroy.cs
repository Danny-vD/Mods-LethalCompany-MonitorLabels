using System;
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
			Transform radarIcon = grabbableObject.radarIcon;

			if (ReferenceEquals(radarIcon, null))
			{
				return;
			}
			
			Destroy(radarIcon.gameObject);
			grabbableObject = null;
		}
	}
}