namespace MonitorLabels.Structs;

public struct CustomAILabelData
{
	public string Label;
	public bool ShowLabel;

	public CustomAILabelData(string label, bool showLabel = true)
	{
		Label     = label;
		ShowLabel = showLabel;
	}
}