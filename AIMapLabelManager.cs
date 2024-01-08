using System;
using System.Collections.Generic;
using MonitorLabels.Structs;
using TMPro;
using UnityEngine;

namespace MonitorLabels
{
	public static class AIMapLabelManager
	{
		public static readonly Dictionary<Type, CustomAILabelData> CustomAINames = new Dictionary<Type, CustomAILabelData>();

		public static bool TryAddNewAI(Type type, CustomAILabelData labelData)
		{
			return CustomAINames.TryAdd(type, labelData);
		}

		public static bool TryAddNewAI(Type type, string label, bool showLabel = true)
		{
			return CustomAINames.TryAdd(type, new CustomAILabelData(label, showLabel));
		}

		public static void RemoveAI(Type type)
		{
			CustomAINames.Remove(type);
		}

		internal static void AddLabelToAI(EnemyAI enemyAI)
		{
			string aiLabel = GetAILabel(enemyAI, out bool showLabel);

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
			label.text  = aiLabel;
		}

		private static string GetAILabel(EnemyAI enemyAI, out bool showLabel)
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
					if (true) //ConfigUtil.HideLabelOnCertainEnemies.Value) // NOTE: DocileLocustBees do not have a mapdot
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

				case MaskedPlayerEnemy:
					return ConfigUtil.MaskedLabel.Value;

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
					return GetUnknownAILabel(enemyAI, out showLabel);
			}
		}

		private static string GetUnknownAILabel(EnemyAI enemyAI, out bool showLabel)
		{
			foreach (KeyValuePair<Type, CustomAILabelData> pair in CustomAINames)
			{
				if (pair.Key.IsInstanceOfType(enemyAI))
				{
					showLabel = pair.Value.ShowLabel;
					return pair.Value.Label;
				}
			}

			showLabel = true;

			string label = ConfigUtil.UnknownLabel.Value;
			return label.Equals(string.Empty) ? MapLabelUtil.RemoveCloneFromString(enemyAI.gameObject.name) : label;
		}
	}
}