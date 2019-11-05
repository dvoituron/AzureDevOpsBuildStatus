using System;
using System.IO;
using System.Linq;
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

        public string[] Builds { get; set; }

        public BuildDefinition[] BuildDefinitionsDetails => Builds?.Select(i => new BuildDefinition(i))?.ToArray();

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
                    ReadCommentHandling = System.Text.Json.JsonCommentHandling.Skip,
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

        public class BuildDefinition
        {
            public BuildDefinition(string value)
            {
                var details = value.Split("#");

                this.ProjectName = details[0];

                if (details.Length > 1)
                    this.BuildDefinitionId = Convert.ToInt32(details[1]);
                else
                    this.BuildDefinitionId = null;
            }

            public BuildDefinition(string projectName, int buildDefinitionId)
            {
                this.ProjectName = projectName;
                this.BuildDefinitionId = buildDefinitionId;
            }

            public string ProjectName { get; set; }
            public int? BuildDefinitionId { get; set; }
        }
    }
}
