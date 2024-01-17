using MonitorLabels.BaseClasses;
using UnityEngine;

namespace MonitorLabels.Components
{
	/// <summary>
	/// Makes sure a label is always correctly offset with respect to the map camera
	/// </summary>
	public class LabelOffset : BetterMonoBehaviour
	{
		public const float LABEL_HEIGHT = 0.5f;
	
		public Vector2 Offset { get; set; }

		private void Start()
		{
			SetOffset();
		}

		private void OnEnable()
		{
			MapCameraRotationObserver.OnMapCameraRotated += SetOffset;
		}

		private void OnDisable()
		{
			MapCameraRotationObserver.OnMapCameraRotated -= SetOffset;
		}

		private void SetOffset()
		{
			Vector3 newPosition = transform.parent.position;
			newPosition += MapCameraRotationObserver.MapCameraUp * Offset.y;
			newPosition += MapCameraRotationObserver.MapCameraRight * Offset.x;
			newPosition += Vector3.up * LABEL_HEIGHT;

			transform.position = newPosition;
		}
	}
}