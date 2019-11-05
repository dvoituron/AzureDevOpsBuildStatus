using ProjectStatus.Internals;
using ProjectStatus.Models;
using System;
using System.IO;
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

        public async Task<BuildStatus> GetStatus(string projectName, int buildDefinitionId)
        {
            string collection = _factory.Configuration.Collection;

            using (var client = _factory.GetHttpClient())
            {
                using (var response = await client.GetAsync($"{collection}/{projectName}/_apis/build/builds?definitions={buildDefinitionId}&maxBuildsPerDefinition=1"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        File.WriteAllText(@"C:\_Temp\test.json", responseBody);

                        var build = JsonSerializer.Deserialize<BuildDefinition>(responseBody);
                        return new BuildStatus()
                        {
                            Version = build.value[0].buildNumber,
                            Status = build.value[0].status,
                            Result = build.value[0].result,
                            Author = build.value[0].requestedFor.displayName,
                            StartTime = build.value[0].startTime,
                            EndTime = build.value[0].finishTime,
                        };
                    }
                    else
                    {
                        return new BuildStatus();
                    }
                }
            }
        }
    }
}
