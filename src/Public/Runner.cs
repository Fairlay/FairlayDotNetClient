namespace FairlayDotNetClient.Public
{
	public class Runner
	{
		public Runner(string name, int visibleDelay, decimal volMatched)
		{
			VisDelay = visibleDelay;
			Name = name;
			VolMatched = volMatched;
		}

		public string Name { get; set; }
		public int VisDelay { get; set; }
		public decimal VolMatched { get; set; }
		public override string ToString() => Name;
	}
}