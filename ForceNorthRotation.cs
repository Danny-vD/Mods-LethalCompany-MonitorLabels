using UnityEngine;

namespace PlayerMapName
{
	public class ForceNorthRotation : MonoBehaviour
	{
		public static Quaternion FacingNorthRotation = Quaternion.Euler(90, -45, 0); 
		
		public void LateUpdate()
		{
			// Lock to north
			gameObject.transform.rotation = FacingNorthRotation;
		}
	}
}