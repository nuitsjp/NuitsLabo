using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace HelloDependencyInjection
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (var stream = File.OpenRead("README.txt"))
            using (var reader = new StreamReader(stream))
            {
                Console.WriteLine(reader.ReadToEnd());
            }

            using (var httpClient = new HttpClient())
            using (var stream = await httpClient.GetStreamAsync("https://www.google.com/"))
            using (var reader = new StreamReader(stream))
            {
                Console.WriteLine(reader.ReadToEnd());
            }
        }
    }
}