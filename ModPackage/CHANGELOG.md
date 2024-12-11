# Version 2.1.0
Support for V68!

Fix the labels of a dropped item not correctly updating  

# Version 2.0.0

Supports all new enemies from V50!  
Add support for the [Lethal Config Mod](https://thunderstore.io/c/lethal-company/p/AinaVT/LethalConfig/) by adding a 'reload config' button that immediately applies changed config settings  

New mod icon!  

Restructured the config file  

Add extra options in the config for non-scrap item labels (keys, flashlights, shovels etc.)  
Add an option to show the value that a player is carrying (total value and value in their current item slot or both)  
Add an option to tint the player label depending on their health  
Add an option to remove the label of a landmine after it detonates  

### Disclaimer
Some config options were renamed, while keeping these options in the config has no effect, for cleanliness it is best to delete the entire config and let it generate again  

#### For Developers
CustomAILabelData was renamed to CustomLabelData  

# Version 1.4.0

Add an icon to the radar for useful items like keys, shotgun shells and items bought from the store

# Version 1.3.1

Fix an incompatibility issue with MoreCompany

Fix the config entry for enemy labels not having any effect

# Version 1.3.0

Fix the rotation of the labels being wrong if the radar camera is rotated

Add config entries to set the offset of the labels for players(/RadarBoosters), enemies and scrap

# Version 1.2.0

Fix the label for the shotgun of the nutcracker not always disappearing when the appropriate config option is set

Add a config option for the label text size of scrap

# Version 1.1.1

Clarify the available options for the 'logLevel' config option

Update the description in the manifest

Add the correct .dll to the modpackage (Oops!)

# Version 1.1.0

Add an option to show a label on scrap (name or value or both, your choice)!

Fix the labels for the MaskedPlayerEnemy (mimics)!

Fix the target colour not working correctly if another player changes the radar focus

Fix the label for a spider appearing rotated when the spider is crawling on the wall/ceiling

Change 'enableLogger' to a 'logLevel' in the config, so you can now modify what will be logged

### Disclaimer
Some config options were deprecated, while keeping these options in the config has no effect, for cleanliness it is best to delete the entire config and let it generate again  

# Version 1.0.0

Add labels for Players, Radar boosters and enemies

Add configurable options for all labels