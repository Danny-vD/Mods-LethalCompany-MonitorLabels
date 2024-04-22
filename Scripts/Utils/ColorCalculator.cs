using GameNetcodeStuff;
using UnityEngine;

namespace MonitorLabels.Utils
{
	public static class ColorCalculator
	{
		private const int fullHealth = 100;
		private const int halfHealth = fullHealth / 2;
		private const int criticalHealth = 10;
		
		public static Color GetColorDependingOnHealth(PlayerControllerB playerController, Color fullHealthColour, Color halfHealthColour, Color criticalHealthColour)
		{
			if (ReferenceEquals(playerController, null))
			{
				return fullHealthColour;
			}

			float lerpValue = 0;
			
			if (playerController.health >= halfHealth)
			{
				lerpValue = Mathf.InverseLerp(halfHealth, fullHealth, playerController.health);

				return Color.Lerp(halfHealthColour, fullHealthColour, lerpValue);
			}
			else
			{
				lerpValue = Mathf.InverseLerp(criticalHealth, halfHealth, playerController.health);

				return Color.Lerp(halfHealthColour, fullHealthColour, lerpValue);
			}
		}
	}
}