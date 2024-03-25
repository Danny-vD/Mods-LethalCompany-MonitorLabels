namespace MonitorLabels.Structs
{
	public struct CustomLabelData
	{
		public string Label;
		public bool ShowLabel;

		public CustomLabelData(string label, bool showLabel = true)
		{
			Label     = label;
			ShowLabel = showLabel;
		}
	}
}