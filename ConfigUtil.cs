using BepInEx.Configuration;
using UnityEngine;

namespace MonitorLabels;

public static class ConfigUtil
{
	// GENERAL
	public static ConfigEntry<bool> EnableLogger;
	public static ConfigEntry<bool> ForceDeadPlayerLabel;
	public static ConfigEntry<int> MaximumNameLength;
	public static ConfigEntry<bool> ShowLabelOnTarget;
	public static ConfigEntry<string> CustomDeadName;
	public static ConfigEntry<bool> HideNormalLabels;
	public static ConfigEntry<bool> HideDeadLabels;

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
	public static ConfigEntry<string> DogLabel;
	public static ConfigEntry<string> NutCrackerLabel;
	public static ConfigEntry<string> SporeLizardLabel;
	public static ConfigEntry<string> SpiderLabel;
	public static ConfigEntry<string> SandWormLabel;
	public static ConfigEntry<string> CoilHeadLabel;

	// COLOURS
	public static ConfigEntry<Color> TargetLabelColour;
	public static ConfigEntry<Color> OtherLabelColour;
	public static ConfigEntry<Color> DeadLabelColour;
	public static ConfigEntry<Color> RadarBoosterLabelColour;
	public static ConfigEntry<Color> EnemyLabelColour;
	public static ConfigEntry<Color> DeadEnemyLabelColour;

	// ADVANCED
	public static ConfigEntry<string> StringFormat;


	private static ConfigFile config;

	internal static void Initialize(ConfigFile configFile)
	{
		config = configFile;
	}

	public static void ReadConfig()
	{
		// GENERAL
		EnableLogger         = config.Bind("0. General", "enableLogger", true, "Should the plugin log to the console");
		ForceDeadPlayerLabel = config.Bind("0. General", "forceDeadPlayerLabel", true, "Should the label of a dead player always be visible?");
		MaximumNameLength    = config.Bind("0. General", "maxNameLength", 5, "The maximum length of the name that will be shown on the terminal");
		ShowLabelOnTarget    = config.Bind("0. General", "targetLabelEnabled", true, "Should the currently targeted player also show a label");
		CustomDeadName       = config.Bind("0. General", "customDeadLabel", string.Empty, "A custom label to show if someone is dead, leave empty to use their name instead");
		HideNormalLabels     = config.Bind("0. General", "hideNormalLabels", false, "Don't show any labels except for 'forceDeadPlayerLabel'");
		HideDeadLabels       = config.Bind("0. General", "hideDeadLabels", false, "Don't show the labels of dead players");

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
		DogLabel         = config.Bind("1.2 Enemy Labels", "mouthDogLabel", "Dog", "The label of the MouthDog enemy");
		NutCrackerLabel  = config.Bind("1.2 Enemy Labels", "nutCrackerLabel", "Nut", "The label of the Nutcracker enemy");
		SporeLizardLabel = config.Bind("1.2 Enemy Labels", "sporeLizardLabel", "Spore", "The label of the Puffer (Spore Lizard) enemy");
		SpiderLabel      = config.Bind("1.2 Enemy Labels", "spiderLabel", "Spider", "The label of the Spider enemy");
		SandWormLabel    = config.Bind("1.2 Enemy Labels", "sandWormLabel", string.Empty, "The label of the SandWorm enemy");
		CoilHeadLabel    = config.Bind("1.2 Enemy Labels", "coilheadLabel", "Coil", "The label of the SpringMan enemy");

		// COLOURS
		TargetLabelColour       = config.Bind("2. Colours", "targetLabelColour", Color.green, "The colour of the label of the currently viewed player");
		OtherLabelColour        = config.Bind("2. Colours", "otherLabelColour", Color.white, "The default colour of a label");
		DeadLabelColour         = config.Bind("2. Colours", "deadLabelColour", Color.red, "The colour of a label of a player that is dead");
		RadarBoosterLabelColour = config.Bind("2. Colours", "radarBoosterLabelColour", Color.blue, "The colour of a label of a radar booster");
		EnemyLabelColour        = config.Bind("2. Colours", "enemyLabelColour", new Color(1, .5f, .2f, 1.0f), "The colour of a label of an enemy");
		DeadEnemyLabelColour    = config.Bind("2. Colours", "deadEnemyLabelColour", Color.red, "The colour of a label of an enemy that is dead");

		
		// ADVANCED
		StringFormat = config.Bind("3. Advanced", "labelFormat", "{0}", "The string that will be shown above a player dot\n{0} = Name\n{1} = playerIndex");
	}
}