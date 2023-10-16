// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Net.Sockets;

var port = 9876;
//Task.Run(() => WaitForConnection(port));

var tcpClient = ConnectToServer("localhost", port);

Console.ReadLine();

static TcpClient WaitForConnection(int port)
{
    // Set up a TCP listener on the specified port
    var listener = new TcpListener(IPAddress.Any, port);
    listener.Start();

    // Wait for a client to connect
    Console.WriteLine($"Waiting for a connection on port {port}...");
    TcpClient client = listener.AcceptTcpClient();
    Console.WriteLine($"Client connected. {client.Client.RemoteEndPoint}");

    // Stop listening for new clients
    listener.Stop();

    return client;
}

static TcpClient ConnectToServer(string serverIp, int port)
{
    // Set up a TCP client and connect to the server
    var client = new TcpClient();
    client.Connect(serverIp, port);
    Console.WriteLine($"Connected to server at {serverIp}:{port}.");

    return client;
}
