using System;
using System.IO;
using System.Web;

namespace WpfClickOnceStudy;

public class MainWindowViewModel
{
    public string Message
    {
        get
        {
            string query;
            if (Environment.GetEnvironmentVariable("ClickOnce_IsNetworkDeployed")?.ToLower() == "true")
            {
                string value = Environment.GetEnvironmentVariable("ClickOnce_ActivationUri")!;
                Uri activationUri = new Uri(value);
                query = activationUri.Query;

            }
            else
            {
                query = "ApplicationDeployment.IsNetworkDeployed is false";
            }


            File.WriteAllText("Message.txt", $"Hello, Click Once! from File. {DateTime.Now} {query}");
            return File.ReadAllText("Message.txt");
        }
    }
}