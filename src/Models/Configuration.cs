using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace ProjectStatus.Models
{
    public class Configuration
    {
        public Uri BaseUrl { get; set; }

        public string Collection { get; set; }

        public string Pat { get; set; }

        public string PatAsBase64 => Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", this.Pat)));

        public static Configuration ReadAppSettings()
        {
            // Get Execurting Assembly path
            var uri = new UriBuilder(Assembly.GetExecutingAssembly().CodeBase);
            var file = Uri.UnescapeDataString(uri.Path);
            var path = Path.GetDirectoryName(file);

            // Settings filename
            var jsonFilename = Path.Combine(path, "AppSettings.json");

            if (File.Exists(jsonFilename))
            {
                var jsonContent = File.ReadAllText(jsonFilename);
                var config = System.Text.Json.JsonSerializer.Deserialize<Configuration>(jsonContent, new System.Text.Json.JsonSerializerOptions()
                {
                    AllowTrailingCommas = true,
                    IgnoreNullValues = true,
                    PropertyNameCaseInsensitive = true,
                });
                return config;
            }
            else
            {
                Console.WriteLine("ERROR: Can not find AppSettings.json file.");
                return null;
            }
        }

    }
}
