namespace MonitorLabels.Structs;

public struct CustomAILabelData //TODO: rename to CustomLabelData (breaks backwards compatibility)
{
	public string Label;
	public bool ShowLabel;

	public CustomAILabelData(string label, bool showLabel = true)
	{
		Label     = label;
		ShowLabel = showLabel;
	}
}