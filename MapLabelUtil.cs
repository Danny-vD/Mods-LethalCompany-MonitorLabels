using PlayerMapName.Components;
using TMPro;
using UnityEngine;

namespace PlayerMapName;

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

		labelObject.transform.SetLocalPositionAndRotation(labelPosition, labelRotation);
		labelObject.transform.localScale = labelScale;

		labelObject.AddComponent<MapLabelLayerEnforcer>().Initialize(parent.layer);
		labelObject.layer = parent.layer; // 14 == MapRadar

		labelObject.AddComponent<ForceNorthRotation>();

		TMP_Text labelComponent = labelObject.AddComponent<TextMeshPro>();

		labelComponent.alignment             = TextAlignmentOptions.Center;
		labelComponent.autoSizeTextContainer = true;

		labelComponent.enableWordWrapping = false;
		labelComponent.overflowMode       = TextOverflowModes.Overflow;

		return labelComponent;
	}
}