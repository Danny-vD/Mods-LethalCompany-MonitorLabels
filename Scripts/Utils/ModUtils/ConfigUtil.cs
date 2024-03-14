﻿using BepInEx.Configuration;
using BepInEx.Logging;
using UnityEngine;

namespace MonitorLabels.Utils.ModUtils
{
	public static class ConfigUtil
	{
		// GENERAL
		public static ConfigEntry<LogLevel> LoggingLevel;
		public static ConfigEntry<bool> ForceDeadPlayerLabel;
		public static ConfigEntry<int> MaximumNameLength;
		public static ConfigEntry<bool> ShowLabelOnTarget;
		public static ConfigEntry<string> CustomDeadName;
		public static ConfigEntry<bool> HideNormalPlayerLabels;
		public static ConfigEntry<bool> HideDeadPlayerLabels;
		public static ConfigEntry<bool> HideRadarBoosterLabels;

		// SCRAP
		public static ConfigEntry<bool> ShowIconOnTools;
		public static ConfigEntry<bool> ShowLabelOnScrap;
		public static ConfigEntry<float> ScrapLabelScaleFactor;
		public static ConfigEntry<bool> HideScrapLabelIfOnShip;
		public static ConfigEntry<bool> HideScrapLabelIfCarried;
		public static ConfigEntry<int> HighValueScrapThreshold;
		public static ConfigEntry<bool> HideScrapLabelOnNutcracker;

		// ENEMIES
		public static ConfigEntry<bool> ShowLabelOnEnemies;
		public static ConfigEntry<bool> ShowLabelOnDeadEnemies;
		public static ConfigEntry<bool> HideLabelOnCertainEnemies;

		//     ENEMY LABELS
		public static ConfigEntry<string> UnknownLabel;
		public static ConfigEntry<string> BaboonHawkLabel;
		public static ConfigEntry<string> BlobLabel;
		public static ConfigEntry<string> CentipedeLabel;
		public static ConfigEntry<string> CrawlerLabel;
		public static ConfigEntry<string> ManticoilLabel;
		public static ConfigEntry<string> BrackenLabel;
		public static ConfigEntry<string> ForestGiantLabel;
		public static ConfigEntry<string> HoarderBugLabel;
		public static ConfigEntry<string> JesterLabel;
		public static ConfigEntry<string> MaskedLabel;
		public static ConfigEntry<string> DogLabel;
		public static ConfigEntry<string> NutCrackerLabel;
		public static ConfigEntry<string> SporeLizardLabel;
		public static ConfigEntry<string> SpiderLabel;
		public static ConfigEntry<string> SandWormLabel;
		public static ConfigEntry<string> CoilHeadLabel;

		// COLOURS // TODO: Create sub sections for players, radar booster, enemy and scrap
		public static ConfigEntry<Color> TargetLabelColour;
		public static ConfigEntry<Color> DefaultPlayerLabelColour;
		public static ConfigEntry<Color> DeadPlayerLabelColour;

		public static ConfigEntry<Color> RadarBoosterLabelColour;

		public static ConfigEntry<Color> EnemyLabelColour;
		public static ConfigEntry<Color> DeadEnemyLabelColour;

		public static ConfigEntry<Color> ScrapLabelColour;
		public static ConfigEntry<Color> HighValueScrapLabelColour;
		public static ConfigEntry<Color> CarriedScrapLabelColour;
		public static ConfigEntry<Color> InShipScrapLabelColour;
	
		// LABEL OFFSETS
		public static ConfigEntry<Vector2> RadarTargetLabelOffset;
		public static ConfigEntry<Vector2> EnemyLabelOffset;
		public static ConfigEntry<Vector2> ScrapLabelOffset;

		// ADVANCED
		public static ConfigEntry<string> PlayerLabelStringFormat;
		public static ConfigEntry<string> ScrapLabelStringFormat;


		private static ConfigFile config;

		internal static void Initialize(ConfigFile configFile)
		{
			config = configFile;
		}

		public static void ReadConfig()
		{
			// GENERAL
			LoggingLevel = config.Bind("0. General", "logLevel", LogLevel.Fatal | LogLevel.Error | LogLevel.Warning,
				"What should be logged?\nYou can seperate the options by a ',' to enable multiple\nValid options:\nNone, Fatal, Error, Warning, Message, Info, Debug, All");

			//		Player Label						//TODO: move to 0.1 (breaks backwards compatibility)
			ForceDeadPlayerLabel   = config.Bind("0. General", "forceDeadPlayerLabel", true, "Should the label of a dead player always be visible?");
			MaximumNameLength      = config.Bind("0. General", "maxNameLength", 5, "The maximum length of the name that will be shown on the terminal");
			ShowLabelOnTarget      = config.Bind("0. General", "targetLabelEnabled", true, "Should the currently targeted player also show a label");
			CustomDeadName         = config.Bind("0. General", "customDeadLabel", string.Empty, "A custom label to show if someone is dead, leave empty to use their name instead");
			HideNormalPlayerLabels = config.Bind("0. General", "hideNormalLabels", false, "Don't show any player labels except for 'forceDeadPlayerLabel'");
			HideDeadPlayerLabels   = config.Bind("0. General", "hideDeadLabels", false, "Don't show the labels of dead players");

			//		Radar Booster Label
			HideRadarBoosterLabels = config.Bind("0.2 General", "hideRadarBoosterLabels", false, "Don't show the labels of radar boosters");

			// SCRAP
			ShowIconOnTools       = config.Bind("0.3 Scrap", "showScrapIconOnTools", true, "If true, adds an icon to tools that don't have an icon by default (e.g. Keys, flashlights, shovels)");
			ShowLabelOnScrap           = config.Bind("0.3 Scrap", "showLabelOnScrap", true, "Should scrap also have a label?");
			ScrapLabelScaleFactor      = config.Bind("0.3 Scrap", "scrapLabelScaleFactor", 3.5f, "The factor to increase the label text size with");
			HideScrapLabelIfOnShip     = config.Bind("0.3 Scrap", "hideScrapLabelOnShip", true, "Hide the label if the scrap is on the ship");
			HideScrapLabelIfCarried    = config.Bind("0.3 Scrap", "hideScrapLabelIfCarried", true, "Hide the label if the scrap is being carried");
			HighValueScrapThreshold    = config.Bind("0.3 Scrap", "highValueScrapThreshold", 80, "The threshold above which the scrap will be considered 'high-value'");
			HideScrapLabelOnNutcracker = config.Bind("0.3 Scrap", "hideScrapLabelOnNutcracker", true, "Hide the shotgun label if it is held by the nutcracker");

			// ENEMIES
			ShowLabelOnEnemies        = config.Bind("1. Enemies", "showLabelOnEnemies", true, "Should enemies have labels?");
			ShowLabelOnDeadEnemies    = config.Bind("1. Enemies", "showLabelOnDeadEnemies", true, "Should the label stay on a dead enemy?");
			HideLabelOnCertainEnemies = config.Bind("1. Enemies", "hideLabelOnSomeEnemies", false, "Don't show a label for the following enemies:\nBirds\nBees\nWorm");

			//    ENEMY LABELS
			UnknownLabel     = config.Bind("1.2 Enemy Labels", "unknownLabel", string.Empty, "The label of an unidentified enemy, leave empty to use the name");
			BaboonHawkLabel  = config.Bind("1.2 Enemy Labels", "baboonHawkLabel", "Hawk", "The label of the BaboonBird enemy");
			BlobLabel        = config.Bind("1.2 Enemy Labels", "blobLabel", "Blob", "The label of the Blob enemy");
			CentipedeLabel   = config.Bind("1.2 Enemy Labels", "snareFleaLabel", "Snare", "The label of the Centipede (Snare Flea) enemy");
			CrawlerLabel     = config.Bind("1.2 Enemy Labels", "crawlerLabel", "Half", "The label of the Crawler (Thumper) enemy");
			ManticoilLabel   = config.Bind("1.2 Enemy Labels", "manticoilLabel", "Bird", "The label of the Doublewing (Manticoil) enemy");
			BrackenLabel     = config.Bind("1.2 Enemy Labels", "brackenLabel", "Bracken", "The label of the FlowerMan enemy");
			ForestGiantLabel = config.Bind("1.2 Enemy Labels", "forestGiantLabel", "Giant", "The label of the ForestGiant enemy");
			HoarderBugLabel  = config.Bind("1.2 Enemy Labels", "hoarderBugLabel", "Bug", "The label of the HoarderBug enemy");
			JesterLabel      = config.Bind("1.2 Enemy Labels", "jesterLabel", "Jester", "The label of the Jester enemy");
			MaskedLabel      = config.Bind("1.2 Enemy Labels", "maskedPlayerLabel", "X", "The label of the MaskedPlayer enemy");
			DogLabel         = config.Bind("1.2 Enemy Labels", "mouthDogLabel", "Dog", "The label of the MouthDog enemy");
			NutCrackerLabel  = config.Bind("1.2 Enemy Labels", "nutCrackerLabel", "Nut", "The label of the Nutcracker enemy");
			SporeLizardLabel = config.Bind("1.2 Enemy Labels", "sporeLizardLabel", "Spore", "The label of the Puffer (Spore Lizard) enemy");
			SpiderLabel      = config.Bind("1.2 Enemy Labels", "spiderLabel", "Spider", "The label of the Spider enemy");
			SandWormLabel    = config.Bind("1.2 Enemy Labels", "sandWormLabel", string.Empty, "The label of the SandWorm enemy");
			CoilHeadLabel    = config.Bind("1.2 Enemy Labels", "coilheadLabel", "Coil", "The label of the SpringMan enemy");

			// COLOURS
			TargetLabelColour        = config.Bind("2. Colours", "targetLabelColour", Color.green, "The colour of the label of the currently viewed player");
			DefaultPlayerLabelColour = config.Bind("2. Colours", "otherLabelColour", Color.white, "The default colour of a player label"); // TODO: Rename to defaultPlayerLabelColour (this breaks backward compatibility)
			DeadPlayerLabelColour    = config.Bind("2. Colours", "deadLabelColour", Color.red, "The colour of a label of a player that is dead");

			RadarBoosterLabelColour = config.Bind("2. Colours", "radarBoosterLabelColour", Color.magenta, "The colour of a label of a radar booster");

			EnemyLabelColour     = config.Bind("2. Colours", "enemyLabelColour", new Color(1, .5f, .2f, 1.0f), "The colour of a label of an enemy");
			DeadEnemyLabelColour = config.Bind("2. Colours", "deadEnemyLabelColour", Color.red, "The colour of a label of an enemy that is dead");

			ScrapLabelColour          = config.Bind("2. Colours", "scrapLabelColour", Color.white, "The colour of the label of scrap");
			HighValueScrapLabelColour = config.Bind("2. Colours", "highValueScrapLabelColour", new Color(1, .5f, .2f, 1.0f), "The colour of a label of scrap that is worth more than the highValueScrapThreshold");
			CarriedScrapLabelColour   = config.Bind("2. Colours", "carriedScrapLabelColour", Color.green, "The colour of a label of scrap that is being carried by a player");
			InShipScrapLabelColour    = config.Bind("2. Colours", "inShipScrapLabelColour", Color.blue, "The colour of a label of scrap that is stored in the ship");

			// LABEL OFFSETS
			RadarTargetLabelOffset = config.Bind("4. Label Offsets", "radarTargetLabelOffset", Vector2.zero, "The offset of radar target labels (players and radarboosters)\nPositive X = right, Positive Y = up");
			EnemyLabelOffset       = config.Bind("4. Label Offsets", "enemyLabelOffset", Vector2.down * 0.15f, "The offset of AI labels\nPositive X = right, Positive Y = up");
			ScrapLabelOffset       = config.Bind("4. Label Offsets", "scrapLabelOffset", Vector2.up * 1.5f, "The offset of scrap labels\nPositive X = right, Positive Y = up");
		
			// ADVANCED //TODO: Make this section a high number to allow room for more (e.g. scrap will be section 3) (this breaks backward compatibility)
			PlayerLabelStringFormat = config.Bind("3. Advanced", "labelFormat", "{0}", "The string that will be shown on a player label\n{0} = Name\n{1} = playerIndex");
			ScrapLabelStringFormat  = config.Bind("3. Advanced", "scrapLabelFormat", "{0} [{1}]", "The string that will be shown on a scrap label\n{0} = Name\n{1} = Value");
		}
	}
}