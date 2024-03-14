using MonitorLabels.Utils.ModUtils;
using UnityEngine;

namespace MonitorLabels.Utils
{
	public static class ToolIconSpawner
	{
		private static readonly GameObject iconPrefab;

		static ToolIconSpawner()
		{
			AssetBundleUtil.TryLoadAsset("terminalmarker.lem", "Assets/Mods/TerminalMarker/Plane.prefab", out iconPrefab);
		}
		
		public static void SpawnIcon(GrabbableObject grabbableObject)
		{
			grabbableObject.radarIcon = Object.Instantiate(iconPrefab, RoundManager.Instance.mapPropsContainer.transform).transform;
		}
	}
}