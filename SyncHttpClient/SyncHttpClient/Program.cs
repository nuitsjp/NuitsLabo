// See https://aka.ms/new-console-template for more information

using System.Net.Http;

Console.WriteLine(GetDataSync("https://www.google.com/"));

static string GetDataSync(string url)
{
    using var client = new HttpClient();
    // GetAsync を同期的に待つ
    var response = client.GetAsync(url).Result;
    response.EnsureSuccessStatusCode();

    string content = response.Content.ReadAsStringAsync().Result;
    return content;
}
