using MonitorLabels.Utils.ModUtils;
using UnityEngine;

namespace MonitorLabels.Utils
{
	public static class ToolIconSpawner
	{
		private const int mapRadarLayer = 14;

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
			radarIcon.layer = mapRadarLayer;

			grabbableObject.radarIcon = radarIcon.transform;
		}
	}
}