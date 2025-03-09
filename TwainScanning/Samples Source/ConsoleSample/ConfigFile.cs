using System.IO;

namespace ConsoleSample
{
    public class ConfigFile
    {
        private const string CONFIG_FILE = "twainscanningdemo.ini";
        private const string COMPANY_KEY = "COMPANY=";
        private const string LICENSE_KEY = "LICENSE_KEY=";

        public string Company { get; set; }
        public string LicenseKey { get; set; }

        public ConfigFile()
        {
            Company = "Demo Company";
            LicenseKey = "";
        }

        public static void SetConfigFromIni(ConfigFile configFile)
        {

            if (!File.Exists(CONFIG_FILE))
                return;

            var lines = File.ReadAllLines(CONFIG_FILE);
            foreach (var line in lines)
            {
                if (line.StartsWith(COMPANY_KEY))
                    configFile.Company = line.Replace(COMPANY_KEY, "");
                if (line.StartsWith(LICENSE_KEY))
                    configFile.LicenseKey = line.Replace(LICENSE_KEY, "");
            }
        }
    }
}
