using GameNetcodeStuff;
using HarmonyLib;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace PlayerMapName;

[HarmonyPatch(typeof(ManualCameraRenderer), "Awake")]
public static class ManualCameraRendererAwakePatch
{
	public static void Postfix(ManualCameraRenderer __instance)
	{
		LoggerUtil.LogInfo("ManualCameraRendererAwakePatch patch run");
		NetworkManager networkManager = __instance.NetworkManager;
		if (networkManager == null || !networkManager.IsListening)
			return;

		MonitorLabelsPlugin.UpdateLabels();
	}
}

[HarmonyPatch(typeof(ManualCameraRenderer), "RemoveTargetFromRadar")]
public static class ManualCameraRendererRemoveTargetFromRadarPatch
{
	public static void Postfix(ManualCameraRenderer __instance, Transform removeTransform)
	{
		LoggerUtil.LogInfo("ManualCameraRendererRemoveTargetFromRadarPatch patch run");
		MonitorLabelsPlugin.UpdateLabels();
	}
}

[HarmonyPatch(typeof(ManualCameraRenderer), "AddTransformAsTargetToRadar")]
public static class ManualCameraRendererAddTransformAsTargetToRadarPatch
{
	public static void Postfix(ManualCameraRenderer __instance, Transform newTargetTransform, string targetName, bool isNonPlayer)
	{
		LoggerUtil.LogInfo("ManualCameraRendererAddTransformAsTargetToRadarPatch patch run");
		MonitorLabelsPlugin.UpdateLabels();
	}
}

[HarmonyPatch(typeof(ManualCameraRenderer), "SwitchRadarTargetServerRpc")]
public static class ManualCameraRendererSwitchRadarTargetServerRpcPatch
{
	public static void Postfix(ManualCameraRenderer __instance, int targetIndex)
	{
		LoggerUtil.LogInfo("ManualCameraRendererSwitchRadarTargetServerRpcPatch patch run");
		MonitorLabelsPlugin.UpdateLabels();
	}
}

[HarmonyPatch(typeof(ManualCameraRenderer), "SwitchRadarTargetClientRpc")]
public static class ManualCameraRendererSwitchRadarTargetClientRpcPatch
{
	public static void Postfix(ManualCameraRenderer __instance, int switchToIndex)
	{
		LoggerUtil.LogInfo("ManualCameraRendererSwitchRadarTargetClientRpcPatch patch run");
		MonitorLabelsPlugin.UpdateLabels();
	}
}

[HarmonyPatch(typeof(PlayerControllerB), "SendNewPlayerValuesClientRpc")]
public static class SendNewPlayerValuesClientRpcPatch
{
	public static void Postfix(PlayerControllerB __instance, ref ulong[] playerSteamIds)
	{
		LoggerUtil.LogInfo("SendNewPlayerValuesClientRpcPatch patch run");
		MonitorLabelsPlugin.UpdateLabels();
	}
}

[HarmonyPatch(typeof(PlayerControllerB), "SendNewPlayerValuesServerRpc")]
public static class SendNewPlayerValuesServerRpcPatch
{
	public static void Postfix(PlayerControllerB __instance, ulong newPlayerSteamId)
	{
		LoggerUtil.LogInfo("SendNewPlayerValuesServerRpcPatch patch run");
		MonitorLabelsPlugin.UpdateLabels();
	}
}

[HarmonyPatch(typeof(PlayerControllerB), "SpawnDeadBody")]
public static class SpawnDeadBodyPatch
{
	public static void Postfix(PlayerControllerB __instance)
	{
		LoggerUtil.LogInfo("SpawnDeadBodyPatch patch run");
		MonitorLabelsPlugin.UpdateLabels();
	}
}

[HarmonyPatch(typeof(PlayerControllerB), "KillPlayerServerRpc")]
public static class KillPlayerServerRpcPatch
{
	public static void Postfix(PlayerControllerB __instance)
	{
		LoggerUtil.LogInfo("KillPlayerServerRpcPatch patch run");
		MonitorLabelsPlugin.UpdateLabels();
	}
}

[HarmonyPatch(typeof(PlayerControllerB), "KillPlayerClientRpc")]
public static class KillPlayerClientRpcPatch
{
	public static void Postfix(PlayerControllerB __instance)
	{
		LoggerUtil.LogInfo("KillPlayerClientRpcPatch patch run");
		MonitorLabelsPlugin.UpdateLabels();
	}
}

[HarmonyPatch(typeof(RoundManager), "SpawnEnemyGameObject")]
public static class SpawnEnemyGameObjectPatch
{
	public static void Postfix(ref NetworkObjectReference __result)
	{
		LoggerUtil.LogInfo("SpawnEnemyGameObjectPatch patch run");
		
		if (!ConfigUtil.ShowLabelOnEnemies.Value)
		{
			return;
		}

		NetworkObject networkObject = __result; // Implicit conversion

		if (networkObject == null)
		{
			LoggerUtil.LogError("Network object cannot be found for this enemy");
			return;
		}

		EnemyAI enemyAI = networkObject.GetComponentInParent<EnemyAI>();

		EnemyMapLabelManager.AddLabelToEnemy(enemyAI);
	}
}

[HarmonyPatch(typeof(RoundManager), "SpawnRandomDaytimeEnemy")]
public static class SpawnRandomDaytimeEnemyPatch
{
	public static void Postfix(ref GameObject __result)
	{
		LoggerUtil.LogInfo("SpawnRandomDaytimeEnemyPatch patch run");
		
		if (!ConfigUtil.ShowLabelOnEnemies.Value || ReferenceEquals(__result, null))
		{
			LoggerUtil.LogError(ConfigUtil.ShowLabelOnEnemies.Value + "  (Should be true)");
			LoggerUtil.LogError("Result is null");
			
			return;
		}

		EnemyAI enemyAI = __result.GetComponentInParent<EnemyAI>();

		EnemyMapLabelManager.AddLabelToEnemy(enemyAI);
	}
}

[HarmonyPatch(typeof(RoundManager), "SpawnRandomOutsideEnemy")]
public static class SpawnRandomOutsideEnemyPatch
{
	public static void Postfix(ref GameObject __result)
	{
		LoggerUtil.LogInfo("SpawnRandomOutsideEnemyPatch patch run");
		
		if (!ConfigUtil.ShowLabelOnEnemies.Value || ReferenceEquals(__result, null))
		{
			return;
		}

		EnemyAI enemyAI = __result.GetComponentInParent<EnemyAI>();

		EnemyMapLabelManager.AddLabelToEnemy(enemyAI);
	}
}

[HarmonyPatch(typeof(EnemyAI), "KillEnemy")]
public static class KillEnemyPatch
{
	public static void Postfix(EnemyAI __instance, bool destroy = false)
	{
		LoggerUtil.LogInfo("KillEnemyPatch patch run");
		
		if (destroy || !ConfigUtil.ShowLabelOnEnemies.Value || __instance == null)
		{
			return;
		}

		Transform mapDot = MapLabelUtil.GetMapDot(__instance.transform);

		if (ReferenceEquals(mapDot, null))
		{
			return;
		}
		
		TMP_Text mapLabel = mapDot.GetComponentInChildren<TMP_Text>();
		
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
}