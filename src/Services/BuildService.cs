using ProjectStatus.Internals.Builds;
using ProjectStatus.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectStatus.Services
{
    public class BuildService
    {
        private Factory _factory;

        public BuildService(Factory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Gets the Build Status for all specified projects names.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<BuildStatus>> GetStatusAsync(IEnumerable<Models.Project> projects)
        {
            var listOfBuildStatus = new List<BuildStatus>();

            foreach (var project in projects)
            {
                listOfBuildStatus.AddRange(await GetStatusAsync(project.Name, buildDefinitionId: null));
            }

            return listOfBuildStatus;
        }

        /// <summary>
        /// Gets the Build Status for the specified project name.
        /// If the <paramref name="buildDefinitionId"/> is missing, all Build Definition Status for this project will be returns.
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="buildDefinitionId"></param>
        /// <returns></returns>
        /// <remarks>
        /// https://dev.azure.com/{organization}/{project}/_apis/build/builds
        /// </remarks>
        public async Task<IEnumerable<BuildStatus>> GetStatusAsync(string projectName, int? buildDefinitionId)
        {
            string organization = _factory.Configuration.Organization;
            string queryString = buildDefinitionId.HasValue
                                    ? $"?maxBuildsPerDefinition=1&definitions={buildDefinitionId}&maxBuildsPerDefinition=1"
                                    : $"?maxBuildsPerDefinition=1";

            using (var client = _factory.GetHttpClient())
            {
                using (var response = await client.GetAsync($"{organization}/{projectName}/_apis/build/builds{queryString}"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

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
