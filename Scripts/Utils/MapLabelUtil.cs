﻿using System.Collections.Generic;
using MonitorLabels.Components;
using TMPro;
using UnityEngine;

namespace MonitorLabels.Utils
{
	public static class MapLabelUtil
	{
		public const string MAP_DOT_NAME = "MapDot";
		public const string RADAR_BOOSTER_DOT_NAME = "RadarBoosterDot";

		public const string LABEL_OBJECT_NAME = "MapLabel";

		private static readonly Vector3 labelPosition = new Vector3(0, 0.5f, 0);
		private static readonly Vector3 labelScale = new Vector3(0.5f, 0.5f, 0.5f);

		/// <summary>
		/// Adds a child to this parent with a <see cref="TMP_Text"/> component
		/// </summary>
		/// <param name="parent">A transform that can be seen on the radar</param>
		/// <param name="labelOffset">The offset for the label with respect to the mapCamera</param>
		/// <param name="continuouslyUpdateRotationAndOffset">Should the rotation and offset be updated every frame? (necessary when the parent rotates, like enemies)</param>
		public static TMP_Text AddLabelObject(GameObject parent, Vector2 labelOffset, bool continuouslyUpdateRotationAndOffset = true)
		{
			GameObject labelObject = new GameObject(LABEL_OBJECT_NAME);
			Transform labelObjectTransform = labelObject.transform;

			labelObjectTransform.SetParent(parent.transform, false);

			labelObjectTransform.localPosition = labelPosition;
			labelObjectTransform.rotation      = MapCameraRotationObserver.MapCameraRotation;
			labelObjectTransform.localScale    = labelScale;

			labelObject.layer = parent.layer;
			labelObject.tag   = parent.tag;

			LabelOffsetManager labelOffsetManager = null;

			if (continuouslyUpdateRotationAndOffset)
			{
				labelObject.AddComponent<RotateWithMapCameraContinuously>();
				labelOffsetManager = labelObject.AddComponent<LabelOffsetManagerContinuously>();
			}
			else
			{
				labelObject.AddComponent<RotateWithMapCamera>();
				labelOffsetManager = labelObject.AddComponent<LabelOffsetManagerEventHandler>();
			}

			labelOffsetManager.Offset = labelOffset;

			TMP_Text labelComponent = labelObject.AddComponent<TextMeshPro>();

			labelComponent.alignment             = TextAlignmentOptions.Center;
			labelComponent.autoSizeTextContainer = true;

			labelComponent.enableWordWrapping = false;
			labelComponent.overflowMode       = TextOverflowModes.Overflow;

			return labelComponent;
		}

		/// <summary>
		/// A breadth-first search for a child that has 'MapDot' in its name
		/// </summary>
		public static Transform GetMapDot(Transform parent, bool checkDisabledObjects = false)
		{
			// A breadth-first queue to search through all the children before looking at their children
			Queue<Transform> queue = new Queue<Transform>();

			foreach (Transform child in parent)
			{
				if (checkDisabledObjects || child.gameObject.activeInHierarchy)
				{
					queue.Enqueue(child);
				}
			}

			while (queue.Count > 0)
			{
				Transform current = queue.Dequeue();

				if (current.gameObject.name.Contains(MAP_DOT_NAME))
				{
					return current;
				}

				foreach (Transform child in current)
				{
					if (checkDisabledObjects || child.gameObject.activeInHierarchy)
					{
						queue.Enqueue(child);
					}
				}
			}

			return null;
		}

		public static Transform GetRadarLabel(Transform radarParent, out TMP_Text label)
		{
			Transform labelObject = radarParent.Find(LABEL_OBJECT_NAME);

			label = labelObject != null ? labelObject.GetComponent<TMP_Text>() : null;

			return labelObject;
		}

		public static Transform GetRadarBoosterMapDot(Transform radarParent)
		{
			return radarParent.Find(RADAR_BOOSTER_DOT_NAME);
		}

		/// <summary>
		/// <para>Returns the same string without the (clone) part</para>
		/// <para>It does this by removing anything after the first '('</para>
		/// </summary>
		public static string RemoveCloneFromString(string name)
		{
			int removeStartIndex = name.IndexOf('(');

			return removeStartIndex == -1 ? name : name[..removeStartIndex];
		}
	}
}