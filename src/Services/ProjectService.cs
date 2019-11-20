using ProjectStatus.Internals.Projects;
using ProjectStatus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjectStatus.Services
{
    public class ProjectService
    {
        private Factory _factory;

        public ProjectService(Factory factory)
        {
            _factory = factory;
        }

        /// <summary>
        /// Get all projects in the organization that the authenticated user has access to.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            string collection = _factory.Configuration.Collection;

            using (var client = _factory.GetHttpClient())
            {
                using (var response = await client.GetAsync($"{collection}/_apis/projects"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();

                        var projects = JsonSerializer.Deserialize<ProjectsList>(responseBody);
                        return projects.value
                                       .OrderBy(i => i.name)
                                       .Select(i => new Project() { Name = i.name });
                    }
                    else
                    {
                        return Array.Empty<Project>();
                    }
                }
            }
        }
    }
}
