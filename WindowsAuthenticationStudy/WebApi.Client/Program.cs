HttpClientHandler handler = new HttpClientHandler()
{
    UseDefaultCredentials = true
};

HttpClient httpClient = new HttpClient(handler);
var response = await httpClient.GetAsync("https://localhost:7252/WeatherForecast");
var content = await response.Content.ReadAsStringAsync();
Console.WriteLine(content);