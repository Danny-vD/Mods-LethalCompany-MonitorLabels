﻿using MonitorLabels.BaseClasses;
using UnityEngine;

namespace MonitorLabels.Components
{
	/// <summary>
	/// Makes sure a label is always correctly offset with respect to the map camera
	/// </summary>
	public class LabelOffsetManager : BetterMonoBehaviour
	{
		public const float LABEL_HEIGHT = 1.15f;

		public Vector2 Offset { get; set; }

		private void Start()
		{
			SetOffset();
		}

		protected void SetOffset()
		{
			Vector3 newPosition = transform.parent.position;
			newPosition += MapCameraRotationObserver.MapCameraUp * Offset.y;
			newPosition += MapCameraRotationObserver.MapCameraRight * Offset.x;
			newPosition += Vector3.up * LABEL_HEIGHT;

			transform.position = newPosition;
		}
	}

	/// <summary>
	/// Sets the label offset whenever the camera rotates to make sure the offset is always correct
	/// </summary>
	public class LabelOffsetManagerEventHandler : LabelOffsetManager
	{
		private void OnEnable()
		{
			MapCameraRotationObserver.OnMapCameraRotated += SetOffset;
		}

		private void OnDisable()
		{
			MapCameraRotationObserver.OnMapCameraRotated -= SetOffset;
		}
	}

	/// <summary>
	/// Sets the label offset in LateUpdate() to make sure the offset is always correct
	/// </summary>
	/// <details>This is useful for when the parent rotates (e.g. the enemies)</details>
	public class LabelOffsetManagerContinuously : LabelOffsetManager
	{
		private void LateUpdate()
		{
			SetOffset();
		}
	}
}