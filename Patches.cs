using GameNetcodeStuff;
using HarmonyLib;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace PlayerMapName;

[HarmonyPatch(typeof(ManualCameraRenderer), nameof(ManualCameraRenderer.Awake))]
public static class ManualCameraRendererAwakePatch
{
	public static void Postfix(ManualCameraRenderer __instance)
	{
		LoggerUtil.LogInfo($"{nameof(ManualCameraRenderer)} ({nameof(ManualCameraRenderer.Awake)}) patch run");

		NetworkManager networkManager = __instance.NetworkManager;

		if (networkManager == null || !networkManager.IsListening)
		{
			return;
		}

		MonitorLabelsPlugin.UpdateLabels();
	}
}

[HarmonyPatch(typeof(ManualCameraRenderer), nameof(ManualCameraRenderer.RemoveTargetFromRadar))]
public static class ManualCameraRendererRemoveTargetFromRadarPatch
{
	public static void Postfix()
	{
		LoggerUtil.LogInfo($"{nameof(ManualCameraRenderer)} ({nameof(ManualCameraRenderer.RemoveTargetFromRadar)}) patch run");
		MonitorLabelsPlugin.UpdateLabels();
	}
}

[HarmonyPatch(typeof(ManualCameraRenderer), nameof(ManualCameraRenderer.AddTransformAsTargetToRadar))]
public static class ManualCameraRendererAddTransformAsTargetToRadarPatch
{
	public static void Postfix()
	{
		LoggerUtil.LogInfo($"{nameof(ManualCameraRenderer)} ({nameof(ManualCameraRenderer.AddTransformAsTargetToRadar)}) patch run");
		
		MonitorLabelsPlugin.UpdateLabels();
	}
}

[HarmonyPatch(typeof(ManualCameraRenderer), nameof(ManualCameraRenderer.SwitchRadarTargetServerRpc))]
public static class ManualCameraRendererSwitchRadarTargetServerRpcPatch
{
	public static void Postfix()
	{
		LoggerUtil.LogInfo($"{nameof(ManualCameraRenderer)} ({nameof(ManualCameraRenderer.SwitchRadarTargetServerRpc)}) patch run");
		MonitorLabelsPlugin.UpdateLabels();
	}
}

[HarmonyPatch(typeof(ManualCameraRenderer), nameof(ManualCameraRenderer.SwitchRadarTargetClientRpc))]
public static class ManualCameraRendererSwitchRadarTargetClientRpcPatch
{
	public static void Postfix()
	{
		LoggerUtil.LogInfo($"{nameof(ManualCameraRenderer)} ({nameof(ManualCameraRenderer.SwitchRadarTargetClientRpc)}) patch run");
		MonitorLabelsPlugin.UpdateLabels();
	}
}

[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.SendNewPlayerValuesClientRpc))]
public static class SendNewPlayerValuesClientRpcPatch
{
	public static void Postfix(PlayerControllerB __instance)
	{
		LoggerUtil.LogInfo($"{nameof(PlayerControllerB)} ({nameof(PlayerControllerB.SendNewPlayerValuesClientRpc)}) patch run");
		MonitorLabelsPlugin.UpdateLabels();
	}
}

[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.SendNewPlayerValuesServerRpc))]
public static class SendNewPlayerValuesServerRpcPatch
{
	public static void Postfix(PlayerControllerB __instance)
	{
		LoggerUtil.LogInfo($"{nameof(PlayerControllerB)} ({nameof(PlayerControllerB.SendNewPlayerValuesServerRpc)}) patch run");
		MonitorLabelsPlugin.UpdateLabels();
	}
}

[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.SpawnDeadBody))]
public static class SpawnDeadBodyPatch
{
	public static void Postfix(PlayerControllerB __instance)
	{
		LoggerUtil.LogInfo($"{nameof(PlayerControllerB)} ({nameof(PlayerControllerB.SpawnDeadBody)}) patch run");
		MonitorLabelsPlugin.UpdateLabels();
	}
}

[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.KillPlayerServerRpc))]
public static class KillPlayerServerRpcPatch
{
	public static void Postfix(PlayerControllerB __instance)
	{
		LoggerUtil.LogInfo($"{nameof(PlayerControllerB)} ({nameof(PlayerControllerB.KillPlayerServerRpc)}) patch run");
		MonitorLabelsPlugin.UpdateLabels();
	}
}

[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.KillPlayerClientRpc))]
public static class KillPlayerClientRpcPatch
{
	public static void Postfix(PlayerControllerB __instance)
	{
		LoggerUtil.LogInfo($"{nameof(PlayerControllerB)} ({nameof(PlayerControllerB.KillPlayerClientRpc)}) patch run");
		MonitorLabelsPlugin.UpdateLabels();
	}
}

[HarmonyPatch(typeof(EnemyAI), nameof(EnemyAI.Start))]
public static class EnemyAIStartPatch
{
	public static void Postfix(EnemyAI __instance)
	{
		LoggerUtil.LogInfo($"{nameof(EnemyAI)} ({nameof(EnemyAI.Start)}) patch run");
		EnemyMapLabelManager.AddLabelToEnemy(__instance);
	}
}

[HarmonyPatch(typeof(EnemyAI), nameof(EnemyAI.KillEnemy))]
public static class KillEnemyPatch
{
	public static void Postfix(EnemyAI __instance, bool destroy = false)
	{
		LoggerUtil.LogInfo($"{nameof(EnemyAI)} ({nameof(EnemyAI.KillEnemy)}) patch run");

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