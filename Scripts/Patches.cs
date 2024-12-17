// ReSharper disable UnusedMember.Global // False positive, HarmonyX uses these to patch
// ReSharper disable InconsistentNaming // While true, Harmony wants these specific names

using GameNetcodeStuff;
using HarmonyLib;
using MonitorLabels.Components;
using MonitorLabels.Utils;
using MonitorLabels.Utils.ModUtils;
using MonitorLabels.VanillaImprovements;
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
		private static void ManualCameraRendererAwakePatch(ManualCameraRenderer __instance)
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
		private static void ManualCameraRendererAddTransformAsTargetToRadarPatch()
		{
			LoggerUtil.LogDebug($"{nameof(ManualCameraRenderer)}.{nameof(ManualCameraRenderer.AddTransformAsTargetToRadar)} patch run");

			RadarTargetLabelManager.UpdateLabels();
		}

		[HarmonyPatch(typeof(ManualCameraRenderer), nameof(ManualCameraRenderer.updateMapTarget)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		private static void ManualCameraRendererUpdateMapTargetPatch(int setRadarTargetIndex, bool calledFromRPC = true)
		{
			if (!calledFromRPC) // updateMapTarget calls itself with calledFromRPC = true, so we can ignore the first call where it's still false
			{
				return;
			}

			LoggerUtil.LogDebug($"{nameof(ManualCameraRenderer)}.{nameof(ManualCameraRenderer.updateMapTarget)} patch run"); // Reduce logging by logging under the if-statement

			RadarTargetLabelManager.UpdateLabels(setRadarTargetIndex);
		}

		[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.SendNewPlayerValuesClientRpc)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		private static void PlayerControllerBSendNewPlayerValuesClientRpcPatch(PlayerControllerB __instance)
		{
			LoggerUtil.LogDebug($"[{__instance.playerUsername}] {nameof(PlayerControllerB)}.{nameof(PlayerControllerB.SendNewPlayerValuesClientRpc)} patch run");
			RadarTargetLabelManager.UpdateLabels();
		}
		
		[HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.SetShipReadyToLand)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		private static void StartOfRoundSetShipReadyToLandPatch()
		{
			LoggerUtil.LogDebug($"{nameof(StartOfRound)}.{nameof(StartOfRound.SetShipReadyToLand)} patch run");
			RadarTargetLabelManager.UpdateLabels();
		}

		[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.DamagePlayerClientRpc)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		private static void PlayerControllerBDamagePlayerClientRpcPatch(PlayerControllerB __instance)
		{
			LoggerUtil.LogDebug($"[{__instance.playerUsername}] {nameof(PlayerControllerB)}.{nameof(PlayerControllerB.DamagePlayerClientRpc)} patch run");
			RadarTargetLabelManager.UpdateLabel(__instance.transform);
		}

		[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.KillPlayerClientRpc)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		private static void PlayerControllerBKillPlayerClientRpcPatch(PlayerControllerB __instance)
		{
			LoggerUtil.LogDebug($"[{__instance.playerUsername}] {nameof(PlayerControllerB)}.{nameof(PlayerControllerB.KillPlayerClientRpc)} patch run");
			RadarTargetLabelManager.UpdateLabel(__instance.transform);
		}

		//\\//\\//\\//\\//\\//\\//\\//\\
		//         AI
		//\\//\\//\\//\\//\\//\\//\\//\\

		[HarmonyPatch(typeof(EnemyAI), nameof(EnemyAI.Start)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		private static void EnemyAIStartPatch(EnemyAI __instance)
		{
			LoggerUtil.LogDebug($"{nameof(EnemyAI)}.{nameof(EnemyAI.Start)} patch run");

			if (!ConfigUtil.ShowLabelOnEnemies.Value)
			{
				return;
			}

			AIMapLabelManager.AddLabelToAI(__instance, __instance.transform, false);
		}

		[HarmonyPatch(typeof(CaveDwellerAI), nameof(CaveDwellerAI.becomeAdultAnimation)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		private static void becomeAdultAnimationPatch(CaveDwellerAI __instance)
		{
			LoggerUtil.LogDebug($"{nameof(CaveDwellerAI)}.{nameof(CaveDwellerAI.becomeAdultAnimation)} patch run");
			
			if (!ConfigUtil.ShowLabelOnEnemies.Value)
			{
				return;
			}

			__instance.growthMeter = 1;
			
			AIMapLabelManager.AddLabelToAI(__instance, __instance.adultContainer.transform, true);
		}

		[HarmonyPatch(typeof(MaskedPlayerEnemy), nameof(MaskedPlayerEnemy.Start)), HarmonyPostfix, HarmonyPriority(Priority.Low)] // MaskedPlayerEnemy does not call base.Start() so it has to be individually patched
		private static void MaskedPlayerEnemyStartPatch(MaskedPlayerEnemy __instance)
		{
			LoggerUtil.LogDebug($"{nameof(MaskedPlayerEnemy)}.{nameof(MaskedPlayerEnemy.Start)} patch run");

			if (!ConfigUtil.ShowLabelOnEnemies.Value)
			{
				return;
			}

			AIMapLabelManager.AddLabelToAI(__instance, __instance.transform, false);
		}

		[HarmonyPatch(typeof(EnemyAI), nameof(EnemyAI.KillEnemy)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		private static void EnemyAIKillEnemyPatch(EnemyAI __instance, bool destroy = false)
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
		private static void NutcrackerEnemyAIGrabGunPatch(NutcrackerEnemyAI __instance)
		{
			LoggerUtil.LogDebug($"{nameof(NutcrackerEnemyAI)}.{nameof(NutcrackerEnemyAI.GrabGun)} patch run");

			if (!ConfigUtil.ShowLabelOnScrap.Value || __instance.gun == null)
			{
				return;
			}

			ObjectLabelManager.UpdateScrapLabel(__instance.gun);
		}

		[HarmonyPatch(typeof(NutcrackerEnemyAI), nameof(NutcrackerEnemyAI.DropGun)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		private static void NutcrackerEnemyAIDropGunPatch(NutcrackerEnemyAI __instance)
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
		private static void GrabbableObjectStartPatch(GrabbableObject __instance)
		{
			LoggerUtil.LogDebug($"{nameof(GrabbableObject)}.{nameof(GrabbableObject.Start)} patch run");

			__instance.gameObject.AddComponent<DestroyRadarIconOnDestroy>();

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

		[HarmonyPatch(typeof(GrabbableObject), nameof(GrabbableObject.OnBroughtToShip)), HarmonyPrefix, HarmonyPriority(Priority.Low)]
		private static bool GrabbableObjectOnBroughtToShipPatch(GrabbableObject __instance)
		{
			LoggerUtil.LogDebug($"{nameof(GrabbableObject)}.{nameof(GrabbableObject.OnBroughtToShip)} patch run");

			return __instance.itemProperties.isScrap; // Destroys the radarIcon by default, only do this if it is scrap (never destroy icon for tools)
		}

		[HarmonyPatch(typeof(GrabbableObject), nameof(GrabbableObject.SetScrapValue)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		private static void GrabbableObjectSetScrapValuePatch(GrabbableObject __instance)
		{
			LoggerUtil.LogDebug($"{nameof(GrabbableObject)}.{nameof(GrabbableObject.SetScrapValue)} patch run");

			if (!ConfigUtil.ShowLabelOnScrap.Value)
			{
				return;
			}

			PlayerControllerB holdingPlayer = __instance.playerHeldBy;

			if (holdingPlayer != null)
			{
				RadarTargetLabelManager.UpdateLabel(holdingPlayer.transform);

				PlayerItemSlotsUtil.GetFirstToolAndFirstToolInUse(holdingPlayer, out GrabbableObject firstTool, out GrabbableObject firstToolInUse);
				ObjectLabelManager.UpdateItemSlotLabel(__instance, firstTool, firstToolInUse);
			}
			else
			{
				ObjectLabelManager.UpdateScrapLabel(__instance);
			}
		}

		[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.SwitchToItemSlot)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		private static void PlayerControllerBSwitchToItemSlotPatch(PlayerControllerB __instance)
		{
			LoggerUtil.LogDebug($"[{__instance.playerUsername}] {nameof(PlayerControllerB)}.{nameof(PlayerControllerB.SwitchToItemSlot)} patch run");

			PlayerItemSlotsUtil.UpdateLabelsOfItemSlots(__instance);
		}

		[HarmonyPatch(typeof(GrabbableObject), nameof(GrabbableObject.UseItemOnClient)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		private static void GrabbableObjectUseItemOnClientPatch(GrabbableObject __instance)
		{
			LoggerUtil.LogDebug($"{nameof(GrabbableObject)}.{nameof(GrabbableObject.UseItemOnClient)} patch run");

			PlayerControllerB holdingPlayer = __instance.playerHeldBy;

			if (!ReferenceEquals(holdingPlayer, null))
			{
				// Update all item slots because multiple labels are affected by using a tool etc.
				PlayerItemSlotsUtil.UpdateLabelsOfItemSlots(holdingPlayer);
			}
			else
			{
				if (__instance.itemProperties.isScrap)
				{
					return;
				}

				if (!ConfigUtil.ShowLabelOnTools.Value)
				{
					return;
				}

				// Not used by any player so only update this label
				ObjectLabelManager.UpdateScrapLabel(__instance);
			}
		}

		[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.SetItemInElevator)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		private static void PlayerControllerBSetItemInElevatorPatch(PlayerControllerB __instance, GrabbableObject gObject)
		{
			LoggerUtil.LogDebug($"[{__instance.playerUsername}] {nameof(PlayerControllerB)}.{nameof(PlayerControllerB.SetItemInElevator)} patch run");

			RadarTargetLabelManager.UpdateLabel(__instance.transform);

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

		[HarmonyPatch(typeof(GrabbableObject), nameof(GrabbableObject.DiscardItemOnClient)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		private static void GrabbableObjectDiscardItemOnClientPatch(GrabbableObject __instance)
		{
			LoggerUtil.LogDebug($"{nameof(GrabbableObject)}.{nameof(GrabbableObject.DiscardItemOnClient)} patch run");

			ObjectLabelManager.UpdateScrapLabel(__instance);
		}

		//[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.DiscardHeldObject)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		//private static void PlayerControllerBDiscardHeldObjectPatch(PlayerControllerB __instance)
		//{
		//	LoggerUtil.LogDebug($"[{__instance.playerUsername}] {nameof(PlayerControllerB)}.{nameof(PlayerControllerB.DiscardHeldObject)} patch run");
		//
		//	RadarTargetLabelManager.UpdateLabel(__instance.transform);
		//	PlayerItemSlotsUtil.UpdateLabelsOfItemSlots(__instance);
		//}

		[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.PlaceObjectClientRpc)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		private static void PlayerControllerBPlaceObjectClientRpcPatch(PlayerControllerB __instance, ref NetworkObjectReference grabbedObject)
		{
			LoggerUtil.LogDebug($"[{__instance.playerUsername}] {nameof(PlayerControllerB)}.{nameof(PlayerControllerB.PlaceObjectClientRpc)} patch run");

			RadarTargetLabelManager.UpdateLabel(__instance.transform);
			PlayerItemSlotsUtil.UpdateLabelsOfItemSlots(__instance);

			if (grabbedObject.TryGet(out NetworkObject networkObject))
			{
				ObjectLabelManager.UpdateScrapLabel(networkObject.GetComponent<GrabbableObject>());
			}
		}

		[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.ThrowObjectClientRpc)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		private static void PlayerControllerBThrowObjectClientRpcPatch(PlayerControllerB __instance, ref NetworkObjectReference grabbedObject)
		{
			LoggerUtil.LogDebug($"[{__instance.playerUsername}] {nameof(PlayerControllerB)}.{nameof(PlayerControllerB.ThrowObjectClientRpc)} patch run");

			RadarTargetLabelManager.UpdateLabel(__instance.transform);
			PlayerItemSlotsUtil.UpdateLabelsOfItemSlots(__instance);

			if (grabbedObject.TryGet(out NetworkObject networkObject))
			{
				ObjectLabelManager.UpdateScrapLabel(networkObject.GetComponent<GrabbableObject>());
			}
		}

		[HarmonyPatch(typeof(PlayerControllerB), nameof(PlayerControllerB.GrabObjectClientRpc)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		private static void PlayerControllerBGrabObjectClientRpcPatch(PlayerControllerB __instance, NetworkObjectReference grabbedObject)
		{
			LoggerUtil.LogDebug($"[{__instance.playerUsername}] {nameof(PlayerControllerB)}.{nameof(PlayerControllerB.GrabObjectClientRpc)} patch run");

			RadarTargetLabelManager.UpdateLabel(__instance.transform);

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

				ObjectLabelManager.UpdateScrapLabel(item);
			}
			else
			{
				if (!ConfigUtil.ShowLabelOnTools.Value)
				{
					return;
				}

				PlayerItemSlotsUtil.UpdateLabelsOfItemSlots(__instance); // If we grabbed a tool, update all tools in our item slots
			}
		}

		//\\//\\//\\//\\//\\//\\//\\//\\
		//         VANILLA LABELS
		//\\//\\//\\//\\//\\//\\//\\//\\

		[HarmonyPatch(typeof(Landmine), nameof(Landmine.Detonate)), HarmonyPostfix, HarmonyPriority(Priority.Low)]
		private static void LandMineDetonatePatch(Landmine __instance)
		{
			LoggerUtil.LogDebug($"{nameof(Landmine)}.{nameof(Landmine.Detonate)} patch run");

			if (!ConfigUtil.RemoveDetonatedMineLabel.Value)
			{
				return;
			}

			TerminalAccessibleObject terminalObject = __instance.GetComponent<TerminalAccessibleObject>();

			if (terminalObject is not { mapRadarText: not null })
			{
				return;
			}

			terminalObject.mapRadarText.gameObject.SetActive(false);
		}
	}
}