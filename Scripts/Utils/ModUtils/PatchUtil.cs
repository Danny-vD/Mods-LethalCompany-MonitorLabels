﻿using System;
using BepInEx.Logging;
using HarmonyLib;

namespace MonitorLabels.Utils.ModUtils
{
	internal static class PatchUtil
	{
		internal static void PatchFunctions()
		{
			Harmony harmonyInstance = new Harmony(MonitorLabelsPlugin.GUID);
			LoggerUtil.LogInfo("Attempting to patch with Harmony!");

			try
			{
				harmonyInstance.PatchAll(typeof(Patches));
				LoggerUtil.Log(LogLevel.Info, "Patching success!");
			}
			catch (Exception ex)
			{
				LoggerUtil.Log(LogLevel.Error, "Failed to patch: " + ex);
			}
		}
	}
}