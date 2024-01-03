using UnityEngine;

namespace PlayerMapName.Components;

/// <summary>
/// Something changes the layer back to the 'enemies' layer, this component enforces the correct layer
/// </summary>
public class MapLabelLayerEnforcer : MonoBehaviour
{
	private int mapRadarLayer = -1;
	private string warningMessage = "Changed Layer to ";

	public void Initialize(int radarLayer)
	{
		mapRadarLayer  =  radarLayer;
		warningMessage += mapRadarLayer;
	}

	private void LateUpdate()
	{
		if (gameObject.layer != mapRadarLayer)
		{
			gameObject.layer = mapRadarLayer;
			LoggerUtil.LogWarning(warningMessage);
		}
	}
}