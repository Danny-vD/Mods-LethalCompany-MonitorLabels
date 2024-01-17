using MonitorLabels.BaseClasses;

namespace MonitorLabels.Components
{
	/// <summary>
	/// Sets the rotation of this object to the mapCamera when the camera rotates to make sure it is always rotated right
	/// </summary>
	public class RotateWithMapCamera : BetterMonoBehaviour
	{
		private void Awake()
		{
			MapCameraRotationObserver.OnMapCameraRotated += RotateWithCamera;
		}

		private void RotateWithCamera()
		{
			// Set to map camera rotation
			CachedTransform.rotation = MapCameraRotationObserver.MapCameraRotation;
		}

		private void OnDestroy()
		{
			MapCameraRotationObserver.OnMapCameraRotated -= RotateWithCamera;
		}
	}

	/// <summary>
	/// Sets the rotation of this object to the mapCamera in LateUpdate() to make sure it is always rotated right
	/// </summary>
	/// <details>This is useful for when the parent rotates a lot (e.g. the enemies)</details>
	public class RotateWithMapCameraContinuously : BetterMonoBehaviour
	{
		public void LateUpdate()
		{
			// Lock to map camera rotation
			CachedTransform.rotation = MapCameraRotationObserver.MapCameraRotation;
		}
	}
}