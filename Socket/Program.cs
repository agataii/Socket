internal class Program
{
	private static void Main(string[] args)
	{
		Console.WriteLine("Выберите позицию: ");

		switch (Console.ReadLine().ToLower())
		{
			case "server":
				SimpleSocketServer.Start();
				break;
			case "client":
				SimpleSocketClient.Start();
				break;
			default:
				break;
		}
	}
}