var httpClient  = new HttpClient();
var response = await httpClient.GetAsync("https://localhost:7252/WeatherForecast");
var content = await response.Content.ReadAsStringAsync();
Console.WriteLine(content);