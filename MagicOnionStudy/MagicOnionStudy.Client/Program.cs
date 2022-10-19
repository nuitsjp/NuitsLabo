using Grpc.Net.Client;
using MagicOnion.Client;
using MagicOnionStudy;

// Connect to the server using gRPC channel.
var channel = GrpcChannel.ForAddress("https://localhost:7064");

// NOTE: If your project targets non-.NET Standard 2.1, use `Grpc.Core.Channel` class instead.
// var channel = new Channel("localhost", 5001, new SslCredentials());

// Create a proxy to call the server transparently.
var client = MagicOnionClient.Create<IMyFirstService>(channel);

// Call the server-side method using the proxy.
var result = await client.SumAsync(123, 456);
Console.WriteLine($"Result: {result}");