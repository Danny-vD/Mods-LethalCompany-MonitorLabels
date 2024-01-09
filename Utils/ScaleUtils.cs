using UnityEngine;

namespace MonitorLabels.Utils
{
	internal static class ScaleUtils
	{
		/// <summary>
		/// Returns a modified scale that ensures it does not skew because of non-uniform scaling from the parent (by using its highest scale component)
		/// </summary>
		public static Vector3 GetScaleToNonSkewedParent(Vector3 parentScale, Vector3 scale)
		{
			float highestScale = Mathf.Max(parentScale.x, parentScale.y, parentScale.z);

			scale.x *= GetScaleFactor(parentScale.x, highestScale, scale.x);
			scale.y *= GetScaleFactor(parentScale.y, highestScale, scale.y);
			scale.z *= GetScaleFactor(parentScale.z, highestScale, scale.z);

			return scale;
		}

		private static float GetScaleFactor(float parentScale, float desiredParentScale, float currentScale)
		{
			if (currentScale == 0)
			{
				return 1;
			}

			if (!Mathf.Approximately(parentScale, desiredParentScale))
			{
				float desiredScale = desiredParentScale * currentScale;
				float actualScale = parentScale * currentScale;

				if (actualScale == 0)
				{
					return 1;
				}

				return desiredScale / actualScale;
			}

			return 1;
		}
	}
}