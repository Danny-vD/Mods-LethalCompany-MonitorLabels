using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MonitorLabels;

public static class MapLabelUtil
{
	public const string MAP_DOT_NAME = "MapDot";
	public const string LABEL_OBJECT_NAME = "MapLabel";

	private static readonly Vector3 labelPosition = new Vector3(0, 0.5f, 0);
	private static readonly Quaternion labelRotation = Quaternion.Euler(new Vector3(90, 0, 0));
	private static readonly Vector3 labelScale = new Vector3(0.5f, 0.5f, 0.5f);

	/// <summary>
	/// Adds a child to this parent with a <see cref="TMP_Text"/> component
	/// </summary>
	public static TMP_Text AddLabelObject(GameObject parent)
	{
		GameObject labelObject = new GameObject(LABEL_OBJECT_NAME);
		labelObject.transform.SetParent(parent.transform, false);

		labelObject.transform.localPosition = labelPosition;
		labelObject.transform.rotation      = labelRotation;
		labelObject.transform.localScale    = labelScale;

		labelObject.layer = parent.layer; // 14 == MapRadar
		labelObject.tag   = parent.tag;

		labelObject.AddComponent<ForceNorthRotation>();

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
	public static Transform GetMapDot(Transform parent)
	{
		// A breadth-first queue to search through all the children before looking at their children
		Queue<Transform> queue = new Queue<Transform>();

		foreach (Transform child in parent)
		{
			queue.Enqueue(child);
		}

		while (queue.Count > 0)
		{
			Transform current = queue.Dequeue();

			if (current.gameObject.name.Contains(MapLabelUtil.MAP_DOT_NAME))
			{
				return current;
			}

			foreach (Transform child in current)
			{
				queue.Enqueue(child);
			}
		}

		return null;
	}
}