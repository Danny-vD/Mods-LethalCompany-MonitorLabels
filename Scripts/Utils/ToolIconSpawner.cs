using MonitorLabels.Utils.ModUtils;
using UnityEngine;

namespace MonitorLabels.Utils
{
	public static class ToolIconSpawner
	{
		private const int mapRadarLayer = 14; // Radar sees layer 14 and 8

		private static readonly GameObject iconPrefab;

		static ToolIconSpawner()
		{
			AssetBundleUtil.TryLoadAsset("terminalmarker.lem", "Assets/Mods/TerminalMarker/ItemMarker.prefab", out iconPrefab);
			AssetBundleUtil.TryLoadAsset("terminalmarker.lem", "Assets/Mods/TerminalMarker/MarkerMaterial.mat", out Material iconMaterial);
			
			iconPrefab.GetComponent<Renderer>().sharedMaterial = iconMaterial;
		}

		public static void SpawnIcon(GrabbableObject grabbableObject)
		{
			GameObject radarIcon = Object.Instantiate(iconPrefab, RoundManager.Instance.mapPropsContainer.transform);

			radarIcon.name  = $"Item Marker ({grabbableObject.itemProperties.itemName})";
			radarIcon.layer = mapRadarLayer;

			grabbableObject.radarIcon = radarIcon.transform;
		}
	}
}