using BepInEx.Configuration;
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
		public static ConfigEntry<bool> HidePlayerLabels;
		public static ConfigEntry<bool> HideDeadPlayerLabels;
		public static ConfigEntry<bool> HideRadarBoosterLabels;
		public static ConfigEntry<bool> UseColorsToShowPlayerHealth;

		// OBJECTS
		//     TOOLS
		public static ConfigEntry<bool> ShowIconOnTools;
		public static ConfigEntry<bool> ShowLabelOnTools;
		public static ConfigEntry<bool> ShowBatteryChargeOnLabel;
		public static ConfigEntry<float> ToolLabelFontSize;
		public static ConfigEntry<bool> HideToolLabelIfOnShip;
		public static ConfigEntry<bool> HideToolLabelIfInHand;
		public static ConfigEntry<bool> HideToolLabelIfPocketed;
		public static ConfigEntry<bool> OnlyShow1PocketedLabel;
		public static ConfigEntry<bool> ShowToolIfInUseAndNoOtherToolHeld;

		//     SCRAP
		public static ConfigEntry<bool> ShowLabelOnScrap;
		public static ConfigEntry<float> ScrapLabelScaleFactor;
		public static ConfigEntry<bool> HideScrapLabelIfOnShip;
		public static ConfigEntry<bool> HideScrapLabelIfCarried;
		public static ConfigEntry<int> HighValueScrapThreshold;
		public static ConfigEntry<bool> HideScrapLabelOnNutcracker;

		// ENEMIES
		public static ConfigEntry<bool> ShowLabelOnEnemies;
		public static ConfigEntry<bool> ShowLabelOnDeadEnemies;
		public static ConfigEntry<bool> HideLabelOnSomeEnemies;

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
		public static ConfigEntry<string> ButlerLabel;
		public static ConfigEntry<string> RadMechLabel;
		public static ConfigEntry<string> FlowerSnakeLabel;

		// COLOURS
		//     PLAYERS
		public static ConfigEntry<Color> DeadPlayerLabelColour;

		public static ConfigEntry<Color> TargetPlayerLabelColour;
		public static ConfigEntry<Color> TargetPlayerHalfHealthColour;
		public static ConfigEntry<Color> TargetPlayerCriticalHealthColour;

		public static ConfigEntry<Color> DefaultPlayerLabelColour;
		public static ConfigEntry<Color> DefaultPlayerHalfHealthColour;
		public static ConfigEntry<Color> DefaultPlayerCriticalHealthColour;

		//     RADAR BOOSTERS
		public static ConfigEntry<Color> TargetRadarBoosterLabelColour;
		public static ConfigEntry<Color> RadarBoosterLabelColour;


		//     ENEMIES
		public static ConfigEntry<Color> EnemyLabelColour;
		public static ConfigEntry<Color> DeadEnemyLabelColour;


		//     TOOLS
		public static ConfigEntry<Color> ToolLabelColour;
		public static ConfigEntry<Color> CarriedToolLabelColour;
		public static ConfigEntry<Color> InShipToolLabelColour;


		//     SCRAP
		public static ConfigEntry<Color> ScrapLabelColour;
		public static ConfigEntry<Color> HighValueScrapLabelColour;
		public static ConfigEntry<Color> CarriedScrapLabelColour;
		public static ConfigEntry<Color> InShipScrapLabelColour;

		// LABEL OFFSETS
		public static ConfigEntry<Vector2> RadarTargetLabelOffset;
		public static ConfigEntry<Vector2> EnemyLabelOffset;
		public static ConfigEntry<Vector2> ToolLabelOffset;
		public static ConfigEntry<Vector2> ScrapLabelOffset;

		// MISC
		public static ConfigEntry<bool> RemoveDetonatedMineLabel;

		// ADVANCED
		public static ConfigEntry<string> PlayerLabelStringFormat;
		public static ConfigEntry<string> PlayerCarriedScrapValueStringFormat;

		public static ConfigEntry<string> ScrapLabelStringFormat;

		public static ConfigEntry<string> ToolLabelStringFormat;
		public static ConfigEntry<string> ToolBatteryStringFormat;


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

			// RADAR TARGETS
			//    Player Label
			MaximumNameLength = config.Bind("1.1 RadarTarget/Player", "maximumNameLength", 5, "The maximum length of the name that will be shown on the terminal");
			ShowLabelOnTarget = config.Bind("1.1 RadarTarget/Player", "showLabelOnTarget", true, "Should the currently targeted player also show a label");
			ForceDeadPlayerLabel = config.Bind("1.1 RadarTarget/Player", "forceDeadPlayerLabel", true,
				"Should the label of a dead player always be visible?\nThis is to show dead labels if 'hideNormalLabels' is disabled");
			CustomDeadName       = config.Bind("1.1 RadarTarget/Player", "customDeadLabel", string.Empty, "A custom label to show if someone is dead, leave empty to use their name instead");
			HidePlayerLabels     = config.Bind("1.1 RadarTarget/Player", "hidePlayerLabels", false, "Don't use any player labels except for 'forceDeadPlayerLabel'");
			HideDeadPlayerLabels = config.Bind("1.1 RadarTarget/Player", "hideDeadPlayerLabels", false, "Don't use labels for dead players");
			UseColorsToShowPlayerHealth = config.Bind("1.1 RadarTarget/Player", "useColorsToShowPlayerHealth", true,
				"If true the player label will be coloured depending on their health\nGradient between full and half health and a gradient betwen half and critical health");

			//    Radar Booster Label
			HideRadarBoosterLabels = config.Bind("1.2 RadarTarget/RadarBooster", "hideRadarBoosterLabels", false, "Don't use labels for radar boosters");

			// ENEMIES
			ShowLabelOnEnemies     = config.Bind("2.1 Enemies", "showLabelOnEnemies", true, "Should enemies have labels?");
			ShowLabelOnDeadEnemies = config.Bind("2.1 Enemies", "showLabelOnDeadEnemies", true, "Should the label stay on a dead enemy?");
			HideLabelOnSomeEnemies = config.Bind("2.1 Enemies", "hideLabelOnSomeEnemies", false, "Don't show a label for the following enemies:\nBirds\nBees\nWorm");

			//    ENEMY LABELS
			UnknownLabel     = config.Bind("2.2 Enemy Labels", "unknownLabel", string.Empty, "The label of an unidentified enemy, leave empty to use the name");
			BaboonHawkLabel  = config.Bind("2.2 Enemy Labels", "baboonHawkLabel", "Hawk", "The label of the BaboonBird enemy");
			BlobLabel        = config.Bind("2.2 Enemy Labels", "blobLabel", "Blob", "The label of the Blob enemy");
			CentipedeLabel   = config.Bind("2.2 Enemy Labels", "snareFleaLabel", "Snare", "The label of the Centipede (Snare Flea) enemy");
			CrawlerLabel     = config.Bind("2.2 Enemy Labels", "crawlerLabel", "Half", "The label of the Crawler (Thumper) enemy");
			ManticoilLabel   = config.Bind("2.2 Enemy Labels", "manticoilLabel", "Bird", "The label of the Doublewing (Manticoil) enemy");
			BrackenLabel     = config.Bind("2.2 Enemy Labels", "brackenLabel", "Bracken", "The label of the FlowerMan (Bracken) enemy");
			ForestGiantLabel = config.Bind("2.2 Enemy Labels", "forestGiantLabel", "Giant", "The label of the ForestGiant enemy");
			HoarderBugLabel  = config.Bind("2.2 Enemy Labels", "hoarderBugLabel", "Bug", "The label of the HoarderBug enemy");
			JesterLabel      = config.Bind("2.2 Enemy Labels", "jesterLabel", "Jester", "The label of the Jester enemy");
			MaskedLabel      = config.Bind("2.2 Enemy Labels", "maskedPlayerLabel", "X", "The label of the MaskedPlayer enemy");
			DogLabel         = config.Bind("2.2 Enemy Labels", "mouthDogLabel", "Dog", "The label of the MouthDog enemy");
			NutCrackerLabel  = config.Bind("2.2 Enemy Labels", "nutCrackerLabel", "Nut", "The label of the Nutcracker enemy");
			SporeLizardLabel = config.Bind("2.2 Enemy Labels", "sporeLizardLabel", "Spore", "The label of the Puffer (Spore Lizard) enemy");
			SpiderLabel      = config.Bind("2.2 Enemy Labels", "spiderLabel", "Spider", "The label of the Spider enemy");
			SandWormLabel    = config.Bind("2.2 Enemy Labels", "sandWormLabel", string.Empty, "The label of the SandWorm enemy");
			CoilHeadLabel    = config.Bind("2.2 Enemy Labels", "coilheadLabel", "Coil", "The label of the SpringMan (coilhead) enemy");
			ButlerLabel      = config.Bind("2.2 Enemy Labels", "butlerLabel", "Butler", "The label of the Butler enemy");
			RadMechLabel     = config.Bind("2.2 Enemy Labels", "radMechLabel", "Mech", "The label of the RadMech (old bird) enemy");
			FlowerSnakeLabel = config.Bind("2.2 Enemy Labels", "flowerSnakeLabel", "Snake", "The label of the FlowerSnake (Tulip Snake) enemy");

			// ITEMS
			//    TOOLS
			ShowIconOnTools          = config.Bind("3.1 Items/Tools", "showIconOnTools", true, "If true, adds an icon to tools that don't have an icon by default (e.g. Keys, flashlights, shovels)");
			ShowLabelOnTools         = config.Bind("3.1 Items/Tools", "showLabelOnTools", true, "If true, adds a label to tools (e.g. Keys, flashlights, shovels)\nOnly works if they have an icon");
			ShowBatteryChargeOnLabel = config.Bind("3.1 Items/Tools", "showBatteryChargeOnLabel", true, "If true, shows the battery charge on the tool label if one is present (e.g. Flashlights)");
			ToolLabelFontSize        = config.Bind("3.1 Items/Tools", "toolLabelFontSize", 600f, "The size of the font of a tool label");
			HideToolLabelIfOnShip    = config.Bind("3.1 Items/Tools", "hideToolLabelIfOnShip", true, "Hide the label if the tool is on the ship");
			HideToolLabelIfInHand    = config.Bind("3.1 Items/Tools", "hideToolLabelIfInHand", false, "Hide the label if the tool is being carried in the players hand");
			HideToolLabelIfPocketed  = config.Bind("3.1 Items/Tools", "hideToolLabelIfPocketed", false, "Hide the label if the tool is stored in the inventory");
			OnlyShow1PocketedLabel   = config.Bind("3.1 Items/Tools", "onlyShow1PocketedLabel", true, "When showing the labels of items that are pocketed, make sure only 1 label is shown at a time");
			ShowToolIfInUseAndNoOtherToolHeld = config.Bind("3.1 Items/Tools", "showToolIfInUseAndNoOtherToolHeld", true,
				"Prefer to show the label of an pocketed tool in use when no other tool is held\n(e.g. active flashlight in pocket)\nThis setting overrides hideToolLabelIfCarried if the conditions are met");

			//     SCRAP
			ShowLabelOnScrap           = config.Bind("3.2 Items/Scrap", "showLabelOnScrap", true, "Should scrap also have a label?");
			ScrapLabelScaleFactor      = config.Bind("3.2 Items/Scrap", "scrapLabelScaleFactor", 3.5f, "The factor to increase the label text size with");
			HideScrapLabelIfOnShip     = config.Bind("3.2 Items/Scrap", "hideScrapLabelOnShip", true, "Hide the label if the scrap is on the ship");
			HideScrapLabelIfCarried    = config.Bind("3.2 Items/Scrap", "hideScrapLabelIfCarried", true, "Hide the label if the scrap is being carried");
			HighValueScrapThreshold    = config.Bind("3.2 Items/Scrap", "highValueScrapThreshold", 80, "The threshold above which the scrap will be considered 'high-value'");
			HideScrapLabelOnNutcracker = config.Bind("3.2 Items/Scrap", "hideScrapLabelOnNutcracker", true, "Hide the shotgun label if it is held by the nutcracker");

			// COLOURS
			//     PLAYERS
			DeadPlayerLabelColour = config.Bind("4.1 Colours/Players", "deadPlayerLabelColour", Color.red, "The colour of a label of a player that is dead");

			TargetPlayerLabelColour = config.Bind("4.1 Colours/Players", "targetPlayerLabelColour", Color.green, "The colour of the label of the currently viewed player");
			TargetPlayerHalfHealthColour = config.Bind("4.1 Colours/Players", "targetPlayerHalfHealthColour", new Color(.62f, .547f, 0, 1.0f),
				$"The colour of the label of the currently viewed player at {ColorCalculator.HALF_HEALTH}% health");
			TargetPlayerCriticalHealthColour = config.Bind("4.1 Colours/Players", "targetPlayerCriticalHealthColour", new Color(.1965f, 0, 0f, 1.0f),
				$"The colour of the label of the currently viewed player at {ColorCalculator.CRITICAL_HEALTH}% health");

			DefaultPlayerLabelColour = config.Bind("4.1 Colours/Players", "defaultPlayerLabelColour", Color.white, "The default colour of a player label");
			DefaultPlayerHalfHealthColour = config.Bind("4.1 Colours/Players", "defaultPlayerHalfHealthColour", new Color(.62f, .547f, 0, 1.0f),
				$"The default colour of a player label at {ColorCalculator.HALF_HEALTH}% health");
			DefaultPlayerCriticalHealthColour = config.Bind("4.1 Colours/Players", "defaultPlayerCriticalHealthColour", new Color(.1965f, 0, 0f, 1.0f),
				$"The default colour of a player label at {ColorCalculator.CRITICAL_HEALTH}% health");

			//     RADAR BOOSTERS
			TargetRadarBoosterLabelColour = config.Bind("4.2 Colours/RadarBooster", "targetRadarBoosterLabelColour", Color.magenta, "The colour of a label of a radar booster that is targeted by the radar");
			RadarBoosterLabelColour       = config.Bind("4.2 Colours/RadarBooster", "radarBoosterLabelColour", Color.magenta, "The colour of a label of a radar booster");

			//     ENEMIES
			EnemyLabelColour     = config.Bind("4.3 Colours/Enemies", "enemyLabelColour", new Color(1, .5f, .2f, 1.0f), "The colour of a label of an enemy");
			DeadEnemyLabelColour = config.Bind("4.3 Colours/Enemies", "deadEnemyLabelColour", Color.red, "The colour of a label of an enemy that is dead");

			//     TOOLS
			ToolLabelColour        = config.Bind("4.4 Colours/Tools", "toolLabelColour", new Color(1, .5f, .2f, 1.0f), "The colour of the label of tools");
			CarriedToolLabelColour = config.Bind("4.4 Colours/Tools", "carriedToolLabelColour", new Color(1, .5f, .2f, 1.0f), "The colour of a label of a tool that is being carried by a player");
			InShipToolLabelColour  = config.Bind("4.4 Colours/Tools", "inShipToolLabelColour", new Color(1, .5f, .2f, 1.0f), "The colour of a label of a tool that is stored in the ship");

			//     SCRAP
			ScrapLabelColour          = config.Bind("4.5 Colours/Scrap", "scrapLabelColour", Color.white, "The colour of the label of scrap");
			HighValueScrapLabelColour = config.Bind("4.5 Colours/Scrap", "highValueScrapLabelColour", new Color(1, .5f, .2f, 1.0f), "The colour of a label of scrap that is worth more than the highValueScrapThreshold");
			CarriedScrapLabelColour   = config.Bind("4.5 Colours/Scrap", "carriedScrapLabelColour", Color.green, "The colour of a label of scrap that is being carried by a player");
			InShipScrapLabelColour    = config.Bind("4.5 Colours/Scrap", "inShipScrapLabelColour", Color.blue, "The colour of a label of scrap that is stored in the ship");

			// LABEL OFFSETS
			RadarTargetLabelOffset = config.Bind("5.1 Label Offsets/RadarTarget", "radarTargetLabelOffset", Vector2.zero,
				"The offset of radar target labels (players and radarboosters)\nPositive X = right, Positive Y = up");

			//     ENEMIES
			EnemyLabelOffset = config.Bind("5.2 Label Offsets/Enemies", "enemyLabelOffset", Vector2.down * 0.15f, "The offset of AI labels\nPositive X = right, Positive Y = up");

			//     ITEMS
			ToolLabelOffset  = config.Bind("5.3 Label Offsets/Items", "toolLabelOffset", Vector2.up * 1.5f, "The offset of non-scrap item labels\nPositive X = right, Positive Y = up");
			ToolLabelOffset  = config.Bind("5.3 Label Offsets/Items", "toolLabelOffset", Vector2.up * 1.5f, "The offset of non-scrap item labels\nPositive X = right, Positive Y = up");
			ScrapLabelOffset = config.Bind("5.3 Label Offsets/Items", "scrapLabelOffset", Vector2.up * 1.5f, "The offset of scrap labels\nPositive X = right, Positive Y = up");

			RemoveDetonatedMineLabel = config.Bind("9. Miscellaneous", "removeDetonatedMineLabel", true, "Remove the code-label of a mine after it detonates");

			// ADVANCED
			PlayerLabelStringFormat = config.Bind("99. Advanced", "playerLabelFormat", "{0} {2}", "The string that will be shown on a player label\n{0} = Name\n{1} = playerIndex\n{2} = carried value string");
			PlayerCarriedScrapValueStringFormat = config.Bind("99. Advanced", "playerCarriedScrapValueFormat", "[{0}]",
				"The string that will be shown to display scrap value for a player that is carrying scrap\n{0} = Total Value\n{1} = Value in currently held slot");

			ScrapLabelStringFormat = config.Bind("99. Advanced", "scrapLabelFormat", "{0} [{1}]", "The string that will be shown on a scrap label\n{0} = Name\n{1} = Value");

			ToolLabelStringFormat   = config.Bind("99. Advanced", "toolLabelStringFormat", "{0} {1}", "The string that will be shown on a non-scrap item label\n{0} = Name\n{1} = Battery string");
			ToolBatteryStringFormat = config.Bind("99. Advanced", "toolBatteryStringFormat", "[{0:P0}]", "The string that will be shown for the battery charge\n{0} = Battery charge");
		}
	}
}