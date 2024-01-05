using TMPro;
using UnityEngine;

namespace PlayerMapName
{
	public static class EnemyMapLabelManager
	{
		internal static void AddLabelToEnemy(EnemyAI enemyAI)
		{
			string enemyLabel = GetEnemyLabel(enemyAI, out bool showLabel);

			if (!showLabel)
			{
				return;
			}

			// The MapDot for enemies are inconsisently named and are not in the same position for all enemies
			// So we check all children to find an object that contains 'MapDot' in the name
			Transform mapDot = MapLabelUtil.GetMapDot(enemyAI.transform);

			if (ReferenceEquals(mapDot, null))
			{
				LoggerUtil.LogError($"Child {MapLabelUtil.MAP_DOT_NAME} cannot be found for enemy: {enemyAI.gameObject.name}");
				return;
			}
			
			TMP_Text label = MapLabelUtil.AddLabelObject(mapDot.gameObject);
			
			label.color = ConfigUtil.EnemyLabelColour.Value;
			label.text  = enemyLabel;
		}

		private static string GetEnemyLabel(EnemyAI enemyAI, out bool showLabel)
		{
			showLabel = true;

			switch (enemyAI)
			{
				case BaboonBirdAI:
					return ConfigUtil.BaboonHawkLabel.Value;

				case BlobAI:
					return ConfigUtil.BlobLabel.Value;

				case CentipedeAI:
					return ConfigUtil.CentipedeLabel.Value;

				case CrawlerAI:
					return ConfigUtil.CrawlerLabel.Value;

				case DocileLocustBeesAI:
					if (ConfigUtil.HideLabelOnCertainEnemies.Value)
					{
						showLabel = false;
					}

					return "Bees";

				case DoublewingAI:
					if (ConfigUtil.HideLabelOnCertainEnemies.Value)
					{
						showLabel = false;
					}

					return ConfigUtil.ManticoilLabel.Value;

				case DressGirlAI:
					return "Girl";

				case FlowermanAI:
					return ConfigUtil.BrackenLabel.Value;

				case ForestGiantAI:
					return ConfigUtil.ForestGiantLabel.Value;

				case HoarderBugAI:
					return ConfigUtil.HoarderBugLabel.Value;

				case JesterAI:
					return ConfigUtil.JesterLabel.Value;

				case LassoManAI:
					return "Lasso";

				case MouthDogAI:
					return ConfigUtil.DogLabel.Value;

				case NutcrackerEnemyAI:
					return ConfigUtil.NutCrackerLabel.Value;

				case PufferAI:
					return ConfigUtil.SporeLizardLabel.Value;

				case SandSpiderAI:
					return ConfigUtil.SpiderLabel.Value;

				case SandWormAI:
					if (ConfigUtil.HideLabelOnCertainEnemies.Value)
					{
						showLabel = false;
					}

					return ConfigUtil.SandWormLabel.Value;

				case SpringManAI:
					return ConfigUtil.CoilHeadLabel.Value;

				default:
					string label = ConfigUtil.UnknownLabel.Value;
					return label.Equals(string.Empty) ? enemyAI.gameObject.name : label;
			}
		}
	}
}