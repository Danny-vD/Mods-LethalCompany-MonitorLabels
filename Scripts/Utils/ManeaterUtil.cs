using UnityEngine;

namespace MonitorLabels.Utils
{
	public static class ManeaterUtil
	{
		public static bool HasTransformed(CaveDwellerAI caveDwellerAI)
		{
			return caveDwellerAI.growthMeter >= 1.0;
		}
	}
}