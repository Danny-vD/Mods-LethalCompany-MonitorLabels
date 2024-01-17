// ReSharper disable UnusedMember.Global // False positive, HarmonyX uses these to patch
// ReSharper disable InconsistentNaming // While true, Harmony wants these specific names

using GameNetcodeStuff;
using HarmonyLib;
using MonitorLabels.Components;
using MonitorLabels.Utils;
using Unity.Netcode;

namespace MonitorLabels
{
	internal static class Patches
	{
		//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//
		//         PLAYERS & RADARBOOSTER
		//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//

		[HarmonyPatch(typeof(ManualCameraRenderer), nameof(ManualCameraRenderer.Awake)), HarmonyPostfix]
		internal static void ManualCameraRendererAwakePatch(ManualCameraRenderer __instance)
		{
			LoggerUtil.LogDebug($"{nameof(ManualCameraRenderer)}.{nameof(ManualCameraRenderer.Awake)} patch run");

			NetworkManager networkManager = __instance.NetworkManager;

			if (networkManager == null || !networkManager.IsListening)
			{
				return;
			}

			RadarTargetLabelManager.UpdateLabels();

			__instance.mapCamera.gameObject.AddComponent<MapCameraRotationObserver>();
		}

		[HarmonyPatch(typeof(ManualCameraRenderer), nameof(ManualCameraRenderer.AddTransformAsTargetToRadar)), HarmonyPostfix]
		internal static void ManualCameraRendererAddTransformAsTargetToRadarPatch()
		{
			LoggerUtil.LogDebug($"{nameof(ManualCameraRenderer)}.{nameof(ManualCameraRenderer.AddTransformAsTargetToRadar)} patch run");

			RadarTargetLabelManager.UpdateLabels();
		}

		[HarmonyPatch(typeof(ManualCameraRenderer), nameof(ManualCameraRenderer.updateMapTarget)), HarmonyPostfix]
		internal static void ManualCameraRendererUpdateMapTargetPatch(int setRadarTargetIndex, bool calledFromRPC = true)
		{
			if (!calledFromRPC) // updateMapTarget calls itself with calledFromRPC = true, so we can ignore the first call where it's still false
			{
				return;
			}

			LoggerUtil.LogDebug($"{nameof(ManualCameraRenderer)}.{nameof(ManualCameraRenderer.updateMapTarget)} patch run"); // Reduce logging by logging under the if-statement

			RadarTargetLabelManager.UpdateLabels(setRadarTargetIndex);
		}

		[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.SendNewPlayerValuesClientRpc)), HarmonyPostfix]
		internal static void PlayerControllerBSendNewPlayerValuesClientRpcPatch()
		{
			LoggerUtil.LogDebug($"{nameof(PlayerControllerB)}.{nameof(PlayerControllerB.SendNewPlayerValuesClientRpc)} patch run");
			RadarTargetLabelManager.UpdateLabels();
		}

		[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.SendNewPlayerValuesServerRpc)), HarmonyPostfix]
		internal static void PlayerControllerBSendNewPlayerValuesServerRpcPatch()
		{
			LoggerUtil.LogDebug($"{nameof(PlayerControllerB)}.{nameof(PlayerControllerB.SendNewPlayerValuesServerRpc)} patch run");

			RadarTargetLabelManager.UpdateLabels();
		}

		[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.SpawnDeadBody)), HarmonyPostfix]
		internal static void PlayerControllerBSpawnDeadBodyPatch()
		{
			LoggerUtil.LogDebug($"{nameof(PlayerControllerB)}.{nameof(PlayerControllerB.SpawnDeadBody)} patch run");
			RadarTargetLabelManager.UpdateLabels();
		}

		[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.KillPlayerServerRpc)), HarmonyPostfix]
		internal static void PlayerControllerBKillPlayerServerRpcPatch()
		{
			LoggerUtil.LogDebug($"{nameof(PlayerControllerB)}.{nameof(PlayerControllerB.KillPlayerServerRpc)} patch run");
			RadarTargetLabelManager.UpdateLabels();
		}

		[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.KillPlayerClientRpc)), HarmonyPostfix]
		internal static void PlayerControllerBKillPlayerClientRpcPatch()
		{
			LoggerUtil.LogDebug($"{nameof(PlayerControllerB)}.{nameof(PlayerControllerB.KillPlayerClientRpc)} patch run");
			RadarTargetLabelManager.UpdateLabels();
		}

		//\\//\\//\\//\\//\\//\\//\\//\\
		//         AI
		//\\//\\//\\//\\//\\//\\//\\//\\

		[HarmonyPatch(typeof(EnemyAI), nameof(EnemyAI.Start)), HarmonyPostfix]
		internal static void EnemyAIStartPatch(EnemyAI __instance)
		{
			LoggerUtil.LogDebug($"{nameof(EnemyAI)}.{nameof(EnemyAI.Start)} patch run");
			AIMapLabelManager.AddLabelToAI(__instance);
		}

		[HarmonyPatch(typeof(MaskedPlayerEnemy), nameof(MaskedPlayerEnemy.Start)), HarmonyPostfix] // MaskedPlayerEnemy does not call base.Start() so it has to be individually patched
		internal static void MaskedPlayerEnemyStartPatch(MaskedPlayerEnemy __instance)
		{
			LoggerUtil.LogDebug($"{nameof(MaskedPlayerEnemy)}.{nameof(MaskedPlayerEnemy.Start)} patch run");

			AIMapLabelManager.AddLabelToAI(__instance);
		}

		[HarmonyPatch(typeof(EnemyAI), nameof(EnemyAI.KillEnemy)), HarmonyPostfix]
		internal static void EnemyAIKillEnemyPatch(EnemyAI __instance, bool destroy = false)
		{
			LoggerUtil.LogDebug($"{nameof(EnemyAI)}.{nameof(EnemyAI.KillEnemy)} patch run");

			if (destroy || !ConfigUtil.ShowLabelOnEnemies.Value || __instance == null)
			{
				return;
			}

			AIMapLabelManager.UpdateAILabel(__instance);
		}
		
		//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
		//         AI	-----	NUTCRACKER
		//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\
		
		[HarmonyPatch(typeof(NutcrackerEnemyAI), nameof(NutcrackerEnemyAI.GrabGun)), HarmonyPostfix]
		internal static void NutcrackerEnemyAIGrabGunPatch(NutcrackerEnemyAI __instance)
		{
			LoggerUtil.LogDebug($"{nameof(NutcrackerEnemyAI)}.{nameof(NutcrackerEnemyAI.GrabGun)} patch run");

			if (!ConfigUtil.ShowLabelOnScrap.Value || __instance.gun == null)
			{
				return;
			}

			ScrapLabelManager.UpdateScrapLabel(__instance.gun);
		}
		
		[HarmonyPatch(typeof(NutcrackerEnemyAI), nameof(NutcrackerEnemyAI.DropGun)), HarmonyPostfix]
		internal static void NutcrackerEnemyAIDropGunPatch(NutcrackerEnemyAI __instance)
		{
			LoggerUtil.LogDebug($"{nameof(NutcrackerEnemyAI)}.{nameof(NutcrackerEnemyAI.DropGun)} patch run");

			if (!ConfigUtil.ShowLabelOnScrap.Value || __instance.gun == null)
			{
				return;
			}

			ScrapLabelManager.UpdateScrapLabel(__instance.gun);
		}

		//\\//\\//\\//\\//\\//\\//\\//\\
		//         SCRAP
		//\\//\\//\\//\\//\\//\\//\\//\\

		[HarmonyPatch(typeof(GrabbableObject), nameof(GrabbableObject.Start)), HarmonyPostfix]
		internal static void GrabbableObjectStartPatch(GrabbableObject __instance)
		{
			LoggerUtil.LogDebug($"{nameof(GrabbableObject)}.{nameof(GrabbableObject.Start)} patch run");

			if (!ConfigUtil.ShowLabelOnScrap.Value)
			{
				return;
			}

			ScrapLabelManager.TryAddLabelToScrap(__instance);
		}

		[HarmonyPatch(typeof(GrabbableObject), nameof(GrabbableObject.SetScrapValue)), HarmonyPostfix]
		internal static void GrabbableObjectSetScrapValuePatch(GrabbableObject __instance)
		{
			LoggerUtil.LogDebug($"{nameof(GrabbableObject)}.{nameof(GrabbableObject.SetScrapValue)} patch run");

			if (!ConfigUtil.ShowLabelOnScrap.Value)
			{
				return;
			}

			ScrapLabelManager.UpdateScrapLabel(__instance);
		}

		[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.SetItemInElevator)), HarmonyPostfix]
		internal static void PlayerControllerBSetItemInElevatorPatch(GrabbableObject gObject)
		{
			LoggerUtil.LogDebug($"{nameof(PlayerControllerB)}.{nameof(PlayerControllerB.SetItemInElevator)} patch run");

			if (!ConfigUtil.ShowLabelOnScrap.Value || gObject == null || !gObject.itemProperties.isScrap)
			{
				return;
			}

			ScrapLabelManager.UpdateScrapLabel(gObject);
		}

		[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.SetObjectAsNoLongerHeld)), HarmonyPostfix]
		internal static void PlayerControllerBSetObjectAsNoLongerHeldPatch(GrabbableObject dropObject)
		{
			LoggerUtil.LogDebug($"{nameof(PlayerControllerB)}.{nameof(PlayerControllerB.SetObjectAsNoLongerHeld)} patch run");

			if (!ConfigUtil.ShowLabelOnScrap.Value || dropObject == null || !dropObject.itemProperties.isScrap)
			{
				return;
			}

			ScrapLabelManager.UpdateScrapLabel(dropObject);
		}

		[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.GrabObjectClientRpc)), HarmonyPostfix]
		internal static void PlayerControllerBGrabObjectClientRpcPatch(NetworkObjectReference grabbedObject)
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