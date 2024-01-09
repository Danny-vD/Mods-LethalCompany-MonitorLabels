using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace MonitorLabels.Utils;

public static class MapLabelUtil
{
	public const string MAP_DOT_NAME = "MapDot";
	public const string RADAR_BOOSTER_DOT_NAME = "RadarBoosterDot";

	public const string LABEL_OBJECT_NAME = "MapLabel";

	private static readonly Vector3 labelPosition = new Vector3(0, 0.5f, 0);
	private static readonly Quaternion labelRotation = Quaternion.Euler(new Vector3(90, -45, 0));
	private static readonly Vector3 labelScale = new Vector3(0.5f, 0.5f, 0.5f);

	/// <summary>
	/// Adds a child to this parent with a <see cref="TMP_Text"/> component
	/// </summary>
	/// <param name="parent">A transform that can be seen on the radar</param>
	/// <param name="setRotationInUpdate">Should a <see cref="ForceNorthRotation"/> component be added to this object?</param>
	/// <returns></returns>
	public static TMP_Text AddLabelObject(GameObject parent, bool setRotationInUpdate = true)
	{
		GameObject labelObject = new GameObject(LABEL_OBJECT_NAME);
		Transform labelObjectTransform = labelObject.transform;

		labelObjectTransform.SetParent(parent.transform, false);

		labelObjectTransform.localPosition = labelPosition;
		labelObjectTransform.rotation      = labelRotation;
		labelObjectTransform.localScale    = labelScale;

		// Prevent non-uniform scaling in the parent
		Vector3 parentScale = parent.transform.localScale;
		float highestScale = Mathf.Max(parentScale.x, parentScale.y, parentScale.z);

		parentScale.x               = highestScale;
		parentScale.y               = highestScale;
		parentScale.z               = highestScale;
		parent.transform.localScale = parentScale;

		labelObject.layer = parent.layer; // 14 == MapRadar
		labelObject.tag   = parent.tag;

		if (setRotationInUpdate)
		{
			labelObject.AddComponent<ForceNorthRotation>();
		}

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

			if (current.gameObject.name.Contains(MAP_DOT_NAME))
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

	public static Transform GetRadarLabel(Transform radarParent, out TMP_Text label)
	{
		Transform labelObject = radarParent.Find(LABEL_OBJECT_NAME);

		if (labelObject != null)
		{
			label = labelObject.GetComponent<TMP_Text>();
		}
		else
		{
			label = null;
		}

		return labelObject;
	}

	public static Transform GetRadarBoosterLabel(Transform radarParent)
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