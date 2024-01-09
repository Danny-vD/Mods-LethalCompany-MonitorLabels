using UnityEngine;

namespace MonitorLabels
{
	public class ForceNorthRotation : MonoBehaviour
	{
		public static readonly Vector3 NorthDirection = new Vector3(-1, 0, 1);
		public static readonly Quaternion FacingNorthRotation = Quaternion.Euler(90, -45, 0);
		
		public void LateUpdate()
		{
			// Lock to north
			transform.rotation = FacingNorthRotation;
		}
	}
}