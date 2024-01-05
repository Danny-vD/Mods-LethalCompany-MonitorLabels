# What does this mod do?
  
This mod adds a label to the icons on the monitor for players, radar boosters and enemies

The mod is fully customisable to your liking, like turning off certain features.

It also exposes an API for other mods to add their own labels (in case they add new enemies)

# What is configurable?
  
• How many characters a player label will have  
• Whether the currently focused player should have a label too  
• Whether enemies should have a label  
• The label of a player that is dead  
• All enemy labels can have a custom label  
• All colours  

# Examples
[![Player labels example](/Examples/ManyPlayer.png)](https://github.com/Danny-vD/Mods-LethalCompany-MonitorLabels)

[![Enemy labels example](/Examples/EnemyLabels.png)](https://github.com/Danny-vD/Mods-LethalCompany-MonitorLabels)

# For Developers
### Custom Labels
`MonitorLabels.AIMapLabelManager.CustomAINames` is a `Dictionary<Type, CustomAILabelData>` where you can add your own types to, to give them custom labels.  
`MonitorLabels.AIMapLabelManager.TryAddNewAI(Type, CustomAILabelData)` and `MonitorLabels.AIMapLabelManager.RemoveAI(Type)` are helper functions for this purpose.  

The `CustomAILabelData.ShowLabel` boolean is used if you want to hide the label of that AI.

#### Example
`MonitorLabels.AIMapLabelManager.TryAddNewAI(typeof(MyAI), new CustomAILabelData("MyLabel"));`

### Configuration
The Configuration files can be publicly accessed from the `MonitorLabels.ConfigUtil` class if for whatever reason you want to modify something.  

### General Info
The GUID, PLUGIN_NAME and PLUGIN_VERSION can be accessed from their respective fields in the `MonitorLabels.MonitorLabelsPlugin` class.
