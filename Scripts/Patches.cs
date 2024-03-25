// ReSharper disable UnusedMember.Global // False positive, HarmonyX uses these to patch
// ReSharper disable InconsistentNaming // While true, Harmony wants these specific names

using GameNetcodeStuff;
using HarmonyLib;
using MonitorLabels.Components;
using MonitorLabels.Utils;
using MonitorLabels.Utils.ModUtils;
using Unity.Netcode;

namespace MonitorLabels
{
	internal static class Patches
	{
		//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//
		//         PLAYERS & RADARBOOSTER
		//\\//\\//\\//\\//\\//\\//\\//\\//\\//\\//

		// Cannot patch Start, it causes incompatibility with MoreCompany
		[HarmonyPatch(typeof(ManualCameraRenderer), nameof(ManualCameraRenderer.Awake)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		internal static void ManualCameraRendererAwakePatch(ManualCameraRenderer __instance)
		{
			LoggerUtil.LogDebug($"{nameof(ManualCameraRenderer)}.{nameof(ManualCameraRenderer.Awake)} patch run");

			NetworkManager networkManager = __instance.NetworkManager;

			if (networkManager == null || !networkManager.IsListening)
			{
				return;
			}

			RadarTargetLabelManager.UpdateLabels();

			if (!ReferenceEquals(__instance.mapCamera, null))
			{
				__instance.mapCamera.gameObject.AddComponent<MapCameraRotationObserver>();
			}
		}

		[HarmonyPatch(typeof(ManualCameraRenderer), nameof(ManualCameraRenderer.AddTransformAsTargetToRadar)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		internal static void ManualCameraRendererAddTransformAsTargetToRadarPatch()
		{
			LoggerUtil.LogDebug($"{nameof(ManualCameraRenderer)}.{nameof(ManualCameraRenderer.AddTransformAsTargetToRadar)} patch run");

			RadarTargetLabelManager.UpdateLabels();
		}

		[HarmonyPatch(typeof(ManualCameraRenderer), nameof(ManualCameraRenderer.updateMapTarget)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		internal static void ManualCameraRendererUpdateMapTargetPatch(int setRadarTargetIndex, bool calledFromRPC = true)
		{
			if (!calledFromRPC) // updateMapTarget calls itself with calledFromRPC = true, so we can ignore the first call where it's still false
			{
				return;
			}

			LoggerUtil.LogDebug($"{nameof(ManualCameraRenderer)}.{nameof(ManualCameraRenderer.updateMapTarget)} patch run"); // Reduce logging by logging under the if-statement

			RadarTargetLabelManager.UpdateLabels(setRadarTargetIndex);
		}

		[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.SendNewPlayerValuesClientRpc)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		internal static void PlayerControllerBSendNewPlayerValuesClientRpcPatch()
		{
			LoggerUtil.LogDebug($"{nameof(PlayerControllerB)}.{nameof(PlayerControllerB.SendNewPlayerValuesClientRpc)} patch run");
			RadarTargetLabelManager.UpdateLabels();
		}

		[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.SendNewPlayerValuesServerRpc)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		internal static void PlayerControllerBSendNewPlayerValuesServerRpcPatch()
		{
			LoggerUtil.LogDebug($"{nameof(PlayerControllerB)}.{nameof(PlayerControllerB.SendNewPlayerValuesServerRpc)} patch run");

			RadarTargetLabelManager.UpdateLabels();
		}

		[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.SpawnDeadBody)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		internal static void PlayerControllerBSpawnDeadBodyPatch()
		{
			LoggerUtil.LogDebug($"{nameof(PlayerControllerB)}.{nameof(PlayerControllerB.SpawnDeadBody)} patch run");
			RadarTargetLabelManager.UpdateLabels();
		}

		[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.KillPlayerServerRpc)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		internal static void PlayerControllerBKillPlayerServerRpcPatch()
		{
			LoggerUtil.LogDebug($"{nameof(PlayerControllerB)}.{nameof(PlayerControllerB.KillPlayerServerRpc)} patch run");
			RadarTargetLabelManager.UpdateLabels();
		}

		[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.KillPlayerClientRpc)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		internal static void PlayerControllerBKillPlayerClientRpcPatch()
		{
			LoggerUtil.LogDebug($"{nameof(PlayerControllerB)}.{nameof(PlayerControllerB.KillPlayerClientRpc)} patch run");
			RadarTargetLabelManager.UpdateLabels();
		}

		//\\//\\//\\//\\//\\//\\//\\//\\
		//         AI
		//\\//\\//\\//\\//\\//\\//\\//\\

		[HarmonyPatch(typeof(EnemyAI), nameof(EnemyAI.Start)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		internal static void EnemyAIStartPatch(EnemyAI __instance)
		{
			LoggerUtil.LogDebug($"{nameof(EnemyAI)}.{nameof(EnemyAI.Start)} patch run");

			if (!ConfigUtil.ShowLabelOnEnemies.Value)
			{
				return;
			}

			AIMapLabelManager.AddLabelToAI(__instance);
		}

		[HarmonyPatch(typeof(MaskedPlayerEnemy), nameof(MaskedPlayerEnemy.Start)), HarmonyPostfix, HarmonyPriority(Priority.Low)] // MaskedPlayerEnemy does not call base.Start() so it has to be individually patched
		internal static void MaskedPlayerEnemyStartPatch(MaskedPlayerEnemy __instance)
		{
			LoggerUtil.LogDebug($"{nameof(MaskedPlayerEnemy)}.{nameof(MaskedPlayerEnemy.Start)} patch run");

			if (!ConfigUtil.ShowLabelOnEnemies.Value)
			{
				return;
			}

			AIMapLabelManager.AddLabelToAI(__instance);
		}

		[HarmonyPatch(typeof(EnemyAI), nameof(EnemyAI.KillEnemy)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
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

		[HarmonyPatch(typeof(NutcrackerEnemyAI), nameof(NutcrackerEnemyAI.GrabGun)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		internal static void NutcrackerEnemyAIGrabGunPatch(NutcrackerEnemyAI __instance)
		{
			LoggerUtil.LogDebug($"{nameof(NutcrackerEnemyAI)}.{nameof(NutcrackerEnemyAI.GrabGun)} patch run");

			if (!ConfigUtil.ShowLabelOnScrap.Value || __instance.gun == null)
			{
				return;
			}

			ObjectLabelManager.UpdateScrapLabel(__instance.gun);
		}

		[HarmonyPatch(typeof(NutcrackerEnemyAI), nameof(NutcrackerEnemyAI.DropGun)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		internal static void NutcrackerEnemyAIDropGunPatch(NutcrackerEnemyAI __instance)
		{
			LoggerUtil.LogDebug($"{nameof(NutcrackerEnemyAI)}.{nameof(NutcrackerEnemyAI.DropGun)} patch run");

			if (!ConfigUtil.ShowLabelOnScrap.Value || __instance.gun == null)
			{
				return;
			}

			ObjectLabelManager.UpdateScrapLabel(__instance.gun);
		}

		//\\//\\//\\//\\//\\//\\//\\//\\
		//         SCRAP
		//\\//\\//\\//\\//\\//\\//\\//\\

		[HarmonyPatch(typeof(GrabbableObject), nameof(GrabbableObject.Start)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		internal static void GrabbableObjectStartPatch(GrabbableObject __instance)
		{
			LoggerUtil.LogDebug($"{nameof(GrabbableObject)}.{nameof(GrabbableObject.Start)} patch run");

			if (!__instance.itemProperties.isScrap)
			{
				if (ConfigUtil.ShowIconOnTools.Value)
				{
					if (__instance.radarIcon == null)
					{
						ToolIconSpawner.SpawnIcon(__instance);
					}
				}

				if (ConfigUtil.ShowLabelOnTools.Value)
				{
					// Only works if radarIcon is not null
					ObjectLabelManager.TryAddLabelToScrap(__instance);
				}

				return;
			}

			if (!ConfigUtil.ShowLabelOnScrap.Value)
			{
				return;
			}

			ObjectLabelManager.TryAddLabelToScrap(__instance);
		}

		[HarmonyPatch(typeof(GrabbableObject), nameof(GrabbableObject.SetScrapValue)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		internal static void GrabbableObjectSetScrapValuePatch(GrabbableObject __instance)
		{
			LoggerUtil.LogDebug($"{nameof(GrabbableObject)}.{nameof(GrabbableObject.SetScrapValue)} patch run");

			if (!ConfigUtil.ShowLabelOnScrap.Value)
			{
				return;
			}

			ObjectLabelManager.UpdateScrapLabel(__instance);
		}

		[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.SwitchToItemSlot)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		internal static void PlayerControllerBSwitchToItemSlotPatch(PlayerControllerB __instance)
		{
			LoggerUtil.LogDebug($"{nameof(GrabbableObject)}.{nameof(GrabbableObject.PocketItem)} patch run");

			for (int i = 0; i < __instance.ItemSlots.Length; i++)
			{
				GrabbableObject item = __instance.ItemSlots[i];

				if (item == null)
				{
					continue;
				}

				if (item.itemProperties.isScrap)
				{
					if (!ConfigUtil.ShowLabelOnScrap.Value)
					{
						continue;
					}
				}
				else
				{
					if (!ConfigUtil.ShowLabelOnTools.Value)
					{
						continue;
					}
				}

				ObjectLabelManager.UpdateScrapLabel(item);
			}
		}

		[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.SetItemInElevator)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		internal static void PlayerControllerBSetItemInElevatorPatch(GrabbableObject gObject)
		{
			LoggerUtil.LogDebug($"{nameof(PlayerControllerB)}.{nameof(PlayerControllerB.SetItemInElevator)} patch run");

			if (gObject == null)
			{
				return;
			}

			if (gObject.itemProperties.isScrap)
			{
				if (!ConfigUtil.ShowLabelOnScrap.Value)
				{
					return;
				}
			}
			else
			{
				if (!ConfigUtil.ShowLabelOnTools.Value)
				{
					return;
				}
			}

			ObjectLabelManager.UpdateScrapLabel(gObject);
		}

		[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.SetObjectAsNoLongerHeld)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		internal static void PlayerControllerBSetObjectAsNoLongerHeldPatch(GrabbableObject dropObject)
		{
			LoggerUtil.LogDebug($"{nameof(PlayerControllerB)}.{nameof(PlayerControllerB.SetObjectAsNoLongerHeld)} patch run");

			if (dropObject == null)
			{
				return;
			}

			if (dropObject.itemProperties.isScrap)
			{
				if (!ConfigUtil.ShowLabelOnScrap.Value)
				{
					return;
				}
			}
			else
			{
				if (!ConfigUtil.ShowLabelOnTools.Value)
				{
					return;
				}
			}

			ObjectLabelManager.UpdateScrapLabel(dropObject);
		}

		[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.GrabObjectClientRpc)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		internal static void PlayerControllerBGrabObjectClientRpcPatch(NetworkObjectReference grabbedObject)
		{
			LoggerUtil.LogDebug($"{nameof(PlayerControllerB)}.{nameof(PlayerControllerB.GrabObjectClientRpc)} patch run");

			if (!ConfigUtil.ShowLabelOnScrap.Value)
			{
				return;
			}

			NetworkObject networkObject = grabbedObject;
			GrabbableObject item = networkObject.GetComponentInChildren<GrabbableObject>();

			if (item == null)
			{
				return;
			}

			if (item.itemProperties.isScrap)
			{
				if (!ConfigUtil.ShowLabelOnScrap.Value)
				{
					return;
				}
			}
			else
			{
				if (!ConfigUtil.ShowLabelOnTools.Value)
				{
					return;
				}
			}

			ObjectLabelManager.UpdateScrapLabel(item);
		}
	}
}