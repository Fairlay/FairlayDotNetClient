namespace FairlayDotNetClient.Public
{
	public class Runner
	{
		public Runner(string name, int visibleDelay)
		{
			VisDelay = visibleDelay;
			Name = name;
		}

		public string Name { get; set; }
		public int VisDelay { get; set; }
		public override string ToString() => Name;
	}
}