using System.Net.Sockets;
using System.Net;
using System.Text;

public class SimpleSocketClient
{
    private static string _name;

    public static void Start()
    {
        Console.Write("Введите ваше имя: ");
        _name = Console.ReadLine();

        LaunchClient().Wait();
    }

    public static async Task LaunchClient()
    {
        IPAddress address = IPAddress.Parse("127.0.0.1");
        IPEndPoint endPoint = new IPEndPoint(address, 8888);

        Socket clientSocket = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

        await clientSocket.ConnectAsync(endPoint);

        byte[] buffer = new byte[1024];
        int bytesRead;

        bytesRead = await clientSocket.ReceiveAsync(buffer, SocketFlags.None);

        string welcomeMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
        Console.WriteLine(welcomeMessage);

        _ = Task.Run(() => HandleClient(clientSocket));

        while (true)
        {
            string sendMessage = _name + ":" + Console.ReadLine();
            byte[] sendMessageBytes = Encoding.UTF8.GetBytes(sendMessage);

            await clientSocket.SendAsync(sendMessageBytes, SocketFlags.None);
        }
    }
    private static async void HandleClient(Socket clientSocket)
    {
        byte[] buffer = new byte[1024];
        int bytesRead;

        while (true)
        {
            bytesRead = await clientSocket.ReceiveAsync(buffer, SocketFlags.None);
            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            string[] receivedMessages = receivedMessage.Split(":");
            Console.WriteLine($"[{receivedMessages[0]}]: {receivedMessages[1]}");
        }
    }
}