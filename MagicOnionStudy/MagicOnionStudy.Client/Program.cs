using Grpc.Net.Client;
using MagicOnion.Client;
using MagicOnionStudy;
using System.Security.Cryptography.X509Certificates;

//RSACryptoServiceProviderオブジェクトの作成
System.Security.Cryptography.RSACryptoServiceProvider rsa =
    new System.Security.Cryptography.RSACryptoServiceProvider();

//公開鍵をXML形式で取得
File.WriteAllText("public.xml",  rsa.ToXmlString(false));
//秘密鍵をXML形式で取得
File.WriteAllText("private.xml",rsa.ToXmlString(true));

HttpClient httpClient = new(new HttpClientHandler()
{
    UseDefaultCredentials = true
});
var token = await httpClient.GetAsync("https://localhost:5001/WeatherForecast");

var channel = GrpcChannel.ForAddress("https://localhost:5001");
var client = MagicOnionClient.Create<IMyFirstService>(
    channel,
    new IClientFilter[]
    {
        new AppendHeaderFilter()
    });

var result = await client.SumAsync(123, 456);
Console.WriteLine($"Result: {result.Result}");