using UnityEngine;

namespace MonitorLabels.Utils
{
	public static class RadarTargetUtils
	{
		public static TransformAndName GetMatchingRadarTarget(Transform transform, out int index, out bool isCurrentRadarTarget)
		{
			if (ReferenceEquals(transform, null))
			{
				index                = -1;
				isCurrentRadarTarget = false;

				return null;
			}

			ManualCameraRenderer mapScreen = StartOfRound.Instance.mapScreen;
			int targetIndex = mapScreen.targetTransformIndex;

			for (index = 0; index < mapScreen.radarTargets.Count; ++index)
			{
				isCurrentRadarTarget = targetIndex == index;
				TransformAndName transformAndName = mapScreen.radarTargets[index];

				if (transformAndName == null)
				{
					continue;
				}

				Transform radarTargetTransform = transformAndName.transform;

				if (ReferenceEquals(transform, radarTargetTransform))
				{
					return transformAndName;
				}
			}

			isCurrentRadarTarget = false;
			
			return null;
		}
	}
}