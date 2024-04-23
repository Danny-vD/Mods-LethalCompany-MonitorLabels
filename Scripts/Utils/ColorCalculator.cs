using GameNetcodeStuff;
using UnityEngine;

namespace MonitorLabels.Utils
{
	public static class ColorCalculator
	{
		public const int FULL_HEALTH = 100;
		public const int HALF_HEALTH = FULL_HEALTH / 2;
		public const int CRITICAL_HEALTH = 10;
		
		public static Color GetColorDependingOnHealth(PlayerControllerB playerController, Color fullHealthColour, Color halfHealthColour, Color criticalHealthColour)
		{
			if (ReferenceEquals(playerController, null))
			{
				return fullHealthColour;
			}

			float lerpValue = 0;
			
			if (playerController.health >= HALF_HEALTH)
			{
				lerpValue = Mathf.InverseLerp(HALF_HEALTH, FULL_HEALTH, playerController.health);

				return Color.Lerp(halfHealthColour, fullHealthColour, lerpValue);
			}
			else
			{
				lerpValue = Mathf.InverseLerp(CRITICAL_HEALTH, HALF_HEALTH, playerController.health);

				return Color.Lerp(criticalHealthColour, halfHealthColour, lerpValue);
			}
		}
	}
}