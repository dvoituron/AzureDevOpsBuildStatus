using ProjectStatus.Internals.Builds;
using ProjectStatus.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectStatus
{
    public class Builds
    {
        private Factory _factory;

        public Builds(Factory factory)
        {
            _factory = factory;
        }

        public async Task<IEnumerable<BuildStatus>> GetStatusAsync()
        {
            var listOfBuildStatus = new List<BuildStatus>();

            foreach (var item in _factory.Configuration.BuildDefinitionsDetails)
            {
                listOfBuildStatus.AddRange(await GetStatusAsync(item.ProjectName, item.BuildDefinitionId));
            }

            return listOfBuildStatus;
        }

        // https://dev.azure.com/{organization}/{project}/_apis/build/builds
        public async Task<IEnumerable<BuildStatus>> GetStatusAsync(string projectName, int? buildDefinitionId)
        {
            string collection = _factory.Configuration.Collection;
            string queryString = buildDefinitionId.HasValue
                                    ? $"?maxBuildsPerDefinition=1&definitions={buildDefinitionId}&maxBuildsPerDefinition=1"
                                    : $"?maxBuildsPerDefinition=1";

            using (var client = _factory.GetHttpClient())
            {
                using (var response = await client.GetAsync($"{collection}/{projectName}/_apis/build/builds{queryString}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        File.WriteAllText(@"C:\_Temp\test.json", responseBody);

                        var build = JsonSerializer.Deserialize<BuildDefinition>(responseBody);
                        return build.value.Select(i => new BuildStatus(i));
                    }
                    else
                    {
                        return Array.Empty<BuildStatus>();
                    }
                }
            }
        }
    }
}
