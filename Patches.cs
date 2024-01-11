// ReSharper disable UnusedMember.Global // False positive, HarmonyX uses these to patch
// ReSharper disable InconsistentNaming // While true, Harmony wants these specific names

using GameNetcodeStuff;
using HarmonyLib;
using MonitorLabels.Utils;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace MonitorLabels
{
	//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//
	//         PLAYERS & RADARBOOSTER
	//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//

	[HarmonyPatch(typeof(ManualCameraRenderer), nameof(ManualCameraRenderer.Awake))]
	public static class ManualCameraRendererAwakePatch
	{
		public static void Postfix(ManualCameraRenderer __instance)
		{
			LoggerUtil.LogDebug($"{nameof(ManualCameraRenderer)}.{nameof(ManualCameraRenderer.Awake)} patch run");

			NetworkManager networkManager = __instance.NetworkManager;

			if (networkManager == null || !networkManager.IsListening)
			{
				return;
			}

			RadarTargetLabelManager.UpdateLabels();
		}
	}

	[HarmonyPatch(typeof(ManualCameraRenderer), nameof(ManualCameraRenderer.AddTransformAsTargetToRadar))]
	public static class ManualCameraRendererAddTransformAsTargetToRadarPatch
	{
		public static void Postfix()
		{
			LoggerUtil.LogDebug($"{nameof(ManualCameraRenderer)}.{nameof(ManualCameraRenderer.AddTransformAsTargetToRadar)} patch run");

			RadarTargetLabelManager.UpdateLabels();
		}
	}

	[HarmonyPatch(typeof(ManualCameraRenderer), nameof(ManualCameraRenderer.updateMapTarget))]
	public static class ManualCameraRendererUpdateMapTargetPatch
	{
		public static void Postfix(int setRadarTargetIndex, bool calledFromRPC = true)
		{
			if (!calledFromRPC) // updateMapTarget calls itself with calledFromRPC = true, so we can ignore the first call where it's still false
			{
				return;
			}
			
			LoggerUtil.LogDebug($"{nameof(ManualCameraRenderer)}.{nameof(ManualCameraRenderer.updateMapTarget)} patch run"); // Reduce logging by logging under the if-statement

			RadarTargetLabelManager.UpdateLabels(setRadarTargetIndex);
		}
	}

	[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.SendNewPlayerValuesClientRpc))]
	public static class PlayerControllerBSendNewPlayerValuesClientRpcPatch
	{
		public static void Postfix()
		{
			LoggerUtil.LogDebug($"{nameof(PlayerControllerB)}.{nameof(PlayerControllerB.SendNewPlayerValuesClientRpc)} patch run");
			RadarTargetLabelManager.UpdateLabels();
		}
	}

	[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.SendNewPlayerValuesServerRpc))]
	public static class PlayerControllerBSendNewPlayerValuesServerRpcPatch
	{
		public static void Postfix()
		{
			LoggerUtil.LogDebug($"{nameof(PlayerControllerB)}.{nameof(PlayerControllerB.SendNewPlayerValuesServerRpc)} patch run");
			RadarTargetLabelManager.UpdateLabels();
		}
	}

	[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.SpawnDeadBody))]
	public static class PlayerControllerBSpawnDeadBodyPatch
	{
		public static void Postfix()
		{
			LoggerUtil.LogDebug($"{nameof(PlayerControllerB)}.{nameof(PlayerControllerB.SpawnDeadBody)} patch run");
			RadarTargetLabelManager.UpdateLabels();
		}
	}

	[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.KillPlayerServerRpc))]
	public static class PlayerControllerBKillPlayerServerRpcPatch
	{
		public static void Postfix()
		{
			LoggerUtil.LogDebug($"{nameof(PlayerControllerB)}.{nameof(PlayerControllerB.KillPlayerServerRpc)} patch run");
			RadarTargetLabelManager.UpdateLabels();
		}
	}

	[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.KillPlayerClientRpc))]
	public static class PlayerControllerBKillPlayerClientRpcPatch
	{
		public static void Postfix()
		{
			LoggerUtil.LogDebug($"{nameof(PlayerControllerB)}.{nameof(PlayerControllerB.KillPlayerClientRpc)} patch run");
			RadarTargetLabelManager.UpdateLabels();
		}
	}

	//\\//\\//\\//\\//\\//\\//\\//\\
	//         AI
	//\\//\\//\\//\\//\\//\\//\\//\\

	[HarmonyPatch(typeof(EnemyAI), nameof(EnemyAI.Start))]
	public static class EnemyAIStartPatch
	{
		public static void Postfix(EnemyAI __instance)
		{
			LoggerUtil.LogDebug($"{nameof(EnemyAI)}.{nameof(EnemyAI.Start)} patch run");
			AIMapLabelManager.AddLabelToAI(__instance);
		}
	}

	[HarmonyPatch(typeof(MaskedPlayerEnemy), nameof(MaskedPlayerEnemy.Start))] // MaskedPlayerEnemy does not call base.Start() so it has to be individually patched
	public static class MaskedPlayerEnemyStartPatch
	{
		public static void Postfix(MaskedPlayerEnemy __instance)
		{
			LoggerUtil.LogDebug($"{nameof(MaskedPlayerEnemy)}.{nameof(MaskedPlayerEnemy.Start)} patch run");
			AIMapLabelManager.AddLabelToAI(__instance);
		}
	}

	[HarmonyPatch(typeof(EnemyAI), nameof(EnemyAI.KillEnemy))]
	public static class EnemyAIKillEnemyPatch
	{
		public static void Postfix(EnemyAI __instance, bool destroy = false)
		{
			LoggerUtil.LogDebug($"{nameof(EnemyAI)}.{nameof(EnemyAI.KillEnemy)} patch run");

			if (destroy || !ConfigUtil.ShowLabelOnEnemies.Value || __instance == null)
			{
				return;
			}

			Transform mapDot = MapLabelUtil.GetMapDot(__instance.transform);

			if (ReferenceEquals(mapDot, null))
			{
				return;
			}

			_ = MapLabelUtil.GetRadarLabel(mapDot, out TMP_Text mapLabel);

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

	//\\//\\//\\//\\//\\//\\//\\//\\
	//         SCRAP
	//\\//\\//\\//\\//\\//\\//\\//\\

	[HarmonyPatch(typeof(GrabbableObject), nameof(GrabbableObject.Start))]
	public static class GrabbableObjectStartPatch
	{
		public static void Postfix(GrabbableObject __instance)
		{
			LoggerUtil.LogDebug($"{nameof(GrabbableObject)}.{nameof(GrabbableObject.Start)} patch run");

			if (!ConfigUtil.ShowLabelOnScrap.Value)
			{
				return;
			}

			ScrapLabelManager.TryAddLabelToScrap(__instance);
		}
	}

	[HarmonyPatch(typeof(GrabbableObject), nameof(GrabbableObject.SetScrapValue))]
	public static class GrabbableObjectSetScrapValuePatch
	{
		public static void Postfix(GrabbableObject __instance)
		{
			LoggerUtil.LogDebug($"{nameof(GrabbableObject)}.{nameof(GrabbableObject.SetScrapValue)} patch run");

			if (!ConfigUtil.ShowLabelOnScrap.Value)
			{
				return;
			}

			ScrapLabelManager.UpdateScrapLabel(__instance);
		}
	}

	[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.SetItemInElevator))]
	public static class PlayerControllerBSetItemInElevatorPatch
	{
		public static void Postfix(GrabbableObject gObject)
		{
			LoggerUtil.LogDebug($"{nameof(PlayerControllerB)}.{nameof(PlayerControllerB.SetItemInElevator)} patch run");

			if (!ConfigUtil.ShowLabelOnScrap.Value || gObject == null || !gObject.itemProperties.isScrap)
			{
				return;
			}

			ScrapLabelManager.UpdateScrapLabel(gObject);
		}
	}

	[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.SetObjectAsNoLongerHeld))]
	public static class PlayerControllerBSetObjectAsNoLongerHeldPatch
	{
		public static void Postfix(GrabbableObject dropObject)
		{
			LoggerUtil.LogDebug($"{nameof(PlayerControllerB)}.{nameof(PlayerControllerB.SetObjectAsNoLongerHeld)} patch run");

			if (!ConfigUtil.ShowLabelOnScrap.Value || dropObject == null || !dropObject.itemProperties.isScrap)
			{
				return;
			}

			ScrapLabelManager.UpdateScrapLabel(dropObject);
		}
	}

	[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.GrabObjectClientRpc))]
	public static class PlayerControllerBGrabObjectClientRpcPatch
	{
		public static void Postfix(NetworkObjectReference grabbedObject)
		{
			LoggerUtil.LogDebug($"{nameof(PlayerControllerB)}.{nameof(PlayerControllerB.GrabObjectClientRpc)} patch run");

			if (!ConfigUtil.ShowLabelOnScrap.Value)
			{
				return;
			}

			NetworkObject networkObject = grabbedObject;
			GrabbableObject item = networkObject.GetComponentInChildren<GrabbableObject>();

			if (item == null || !item.itemProperties.isScrap)
			{
				return;
			}

			ScrapLabelManager.UpdateScrapLabel(item);
		}
	}
}