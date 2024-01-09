using UnityEngine;

namespace MonitorLabels
{
	/// <summary>
	/// Sets the rotation of this object to North in LateUpdate() to make sure it is always rotated right
	/// </summary>
	/// <seealso cref="FacingNorthRotation"/>
	public class ForceNorthRotation : MonoBehaviour
	{
		public static readonly Vector3 NorthDirection = new Vector3(-1, 0, 1);
		
		/// <summary>
		/// A quaternion that represents an Euler rotation of (90, -45, 0)
		/// </summary>
		public static readonly Quaternion FacingNorthRotation = Quaternion.Euler(90, -45, 0);

		private Transform cachedTransform;

		private void Start()
		{
			cachedTransform = transform;
		}

		public void LateUpdate()
		{
			// Lock to north
			cachedTransform.rotation = Quaternion.Euler(90, -45, 0);
		}
	}
}