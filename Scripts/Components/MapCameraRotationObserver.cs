using System;
using MonitorLabels.BaseClasses;
using MonitorLabels.Utils;
using UnityEngine;

namespace MonitorLabels.Components
{
	public class MapCameraRotationObserver : BetterMonoBehaviour
	{
		public static event Action OnMapCameraRotated = delegate { };

		public static Vector3 MapCameraUp { get; private set; } = new Vector3(-1, 0, 1);
		public static Vector3 MapCameraRight { get; private set; } = new Vector3(1, 0, 1);
		
		public static Quaternion MapCameraRotation { get; private set; } = Quaternion.Euler(90, -45, 0);

		private void LateUpdate()
		{
			Quaternion currentRotation = CachedTransform.rotation;

			if (!QuaternionMathUtil.IsApproximate(MapCameraRotation, currentRotation))
			{
				MapCameraRotated(CachedTransform);
			}
		}

		private static void MapCameraRotated(Transform transform)
		{
			MapCameraUp    = transform.up;
			MapCameraRight = transform.right;

			MapCameraRotation = transform.rotation;
			
			OnMapCameraRotated.Invoke();
		}
	}
}