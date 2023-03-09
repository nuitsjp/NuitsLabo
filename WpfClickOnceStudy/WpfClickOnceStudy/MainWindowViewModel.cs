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
            try
            {
                string query;
#if NET
                if (Environment.GetEnvironmentVariable("ClickOnce_IsNetworkDeployed")?.ToLower() == "true")
                {
                    string? value = Environment.GetEnvironmentVariable("ClickOnce_ActivationUri");
                    Uri? activationUri = string.IsNullOrEmpty(value) ? null : new Uri(value);
                    query = activationUri != null
                        ? activationUri.Query
                        : Environment.GetEnvironmentVariable("ClickOnce_CurrentVersion")!;

                }
                else
                {
                    query = "ApplicationDeployment.IsNetworkDeployed is false";
                }
#else
#endif


                File.WriteAllText("Message.txt", $"Hello, Click Once! from File. {DateTime.Now} {query}");
                return File.ReadAllText("Message.txt");
            }
            catch (Exception e)
            {
                return e.StackTrace!;
            }
        }
    }
}