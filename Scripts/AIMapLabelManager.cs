using System;
using System.Collections.Generic;
using MonitorLabels.ExtensionMethods;
using MonitorLabels.Structs;
using MonitorLabels.Utils;
using MonitorLabels.Utils.ModUtils;
using TMPro;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MonitorLabels
{
	public static class AIMapLabelManager
	{
		public static readonly Dictionary<Type, CustomLabelData> CustomAINames = new Dictionary<Type, CustomLabelData>();

		public static bool TryAddNewAI(Type type, CustomLabelData labelData)
		{
			return CustomAINames.TryAdd(type, labelData);
		}

		public static bool TryAddNewAI(Type type, string label, bool showLabel = true)
		{
			return CustomAINames.TryAdd(type, new CustomLabelData(label, showLabel));
		}

		public static void SetAI(Type type, CustomLabelData customLabelData)
		{
			CustomAINames[type] = customLabelData;
		}

		public static void SetAI(Type type, string label, bool showLabel = true)
		{
			SetAI(type, new CustomLabelData(label, showLabel));
		}

		public static void RemoveAI(Type type)
		{
			CustomAINames.Remove(type);
		}

		public static void AddLabelToAI(EnemyAI enemyAI, Transform mapDotParent, bool checkDisabledObjectsForMapDot)
		{
			string aiLabel = GetAILabel(enemyAI, out bool showLabel);

			if (!showLabel)
			{
				return;
			}

			// The MapDot for enemies are inconsisently named and are not in the same position for all enemies
			// So we check all children to find an object that contains 'MapDot' in the name
			Transform mapDot = MapLabelUtil.GetMapDot(mapDotParent, checkDisabledObjectsForMapDot);

			if (ReferenceEquals(mapDot, null))
			{
				LoggerUtil.LogWarning($"Child {MapLabelUtil.MAP_DOT_NAME} cannot be found for enemy: {enemyAI.gameObject.name}");
				return;
			}

			switch (enemyAI)
			{
				// The sand spider is weirdly scaled which causes the label to appear rotated when it crawls on a wall
				case SandSpiderAI:
				{
					// Prevent non-uniform scaling in the parent
					Vector3 parentScale = mapDot.localScale;
					float highestScale = Mathf.Max(parentScale.x, parentScale.y, parentScale.z);

					mapDot.localScale = new Vector3(highestScale, highestScale, highestScale);
					break;
				}

				//
				// // The label for the Maneater is not visible in the smaller state, so we have to scale it up
				// case CaveDwellerAI:
				// 	break; // TODO
			}

			TMP_Text label = MapLabelUtil.AddLabelObject(mapDot.gameObject, ConfigUtil.EnemyLabelOffset.Value);

			label.color = ConfigUtil.EnemyLabelColour.Value;
			label.text  = aiLabel;
		}

		public static void UpdateAILabel(EnemyAI enemyAI)
		{
			Transform mapDot = MapLabelUtil.GetMapDot(enemyAI.transform);

			if (ReferenceEquals(mapDot, null))
			{
				return;
			}

			_ = MapLabelUtil.GetRadarLabel(mapDot, out TMP_Text mapLabel);

			if (ReferenceEquals(mapLabel, null)) // This enemy does not have a label, it was most likely skipped as a result of ConfigUtil.HideLabelOnCertainEnemies
			{
				return;
			}

			if (!ConfigUtil.ShowLabelOnDeadEnemies.Value)
			{
				Object.Destroy(mapLabel.gameObject);
				return;
			}

			mapLabel.color = ConfigUtil.DeadEnemyLabelColour.Value;
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

				case RedLocustBees:
				case DocileLocustBeesAI:
					if (true) //ConfigUtil.HideLabelOnCertainEnemies.Value) // NOTE: Bees/Locusts do not have a mapdot
					{
						showLabel = false;
					}

					return "Bees";

				case DoublewingAI:
					if (ConfigUtil.HideLabelOnSomeEnemies.Value)
					{
						showLabel = false;
					}

					return ConfigUtil.ManticoilLabel.Value;

				case DressGirlAI:
					if (true) //ConfigUtil.HideLabelOnCertainEnemies.Value) // NOTE: DressGirl does not have a mapdot
					{
						showLabel = false;
					}

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
					if (ConfigUtil.HideLabelOnSomeEnemies.Value)
					{
						showLabel = false;
					}

					return ConfigUtil.SandWormLabel.Value;

				case SpringManAI:
					return ConfigUtil.CoilHeadLabel.Value;

				case TestEnemy:
					return "Obunga";

				case ButlerEnemyAI:
					return ConfigUtil.ButlerLabel.Value;

				case RadMechAI:
					return ConfigUtil.RadMechLabel.Value;

				case FlowerSnakeEnemy:
					if (ConfigUtil.HideLabelOnSomeEnemies.Value)
					{
						showLabel = false;
					}

					return ConfigUtil.FlowerSnakeLabel.Value;

				case BushWolfEnemy:
					return ConfigUtil.BushWolfLabel.Value;

				case CaveDwellerAI caveDwellerAI:
					
					return ManeaterUtil.HasTransformed(caveDwellerAI) ? ConfigUtil.ManeaterLabel.Value : ConfigUtil.BabyManeaterLabel.Value;

				case ClaySurgeonAI:
					return ConfigUtil.ClaySurgeonLabel.Value;

				default:
					return GetUnknownAILabel(enemyAI, out showLabel);
			}
		}

		private static string GetUnknownAILabel(EnemyAI enemyAISubclassInstance, out bool showLabel)
		{
			foreach (KeyValuePair<Type, CustomLabelData> pair in CustomAINames)
			{
				if (pair.Key.IsInstanceOfType(enemyAISubclassInstance))
				{
					showLabel = pair.Value.ShowLabel;
					return pair.Value.Label;
				}
			}

			showLabel = true;

			string label = ConfigUtil.UnknownLabel.Value;
			return label.Equals(string.Empty) ? MapLabelUtil.RemoveCloneFromString(enemyAISubclassInstance.gameObject.name).InsertSpaceBeforeCapitals() : label;
		}
	}
}