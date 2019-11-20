using ProjectStatus.Helpers;
using ProjectStatus.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStatus
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine($"Status of Azure DevOps Builds and Releases at {DateTime.Now.ToShortTimeString()}");

            var factory = new Factory();
            var projects = await factory.Projects.GetAllProjectsAsync();

            foreach (var project in projects)
            {
                var builds = await factory.Builds.GetStatusAsync(project.Name, buildDefinitionId: null);
                builds.WriteToConsole();
            }

            //var builds = await factory.Builds.GetStatusAsync(projects);

            //foreach (var build in builds.OrderBy(i => $"{i.Project}-{i.Name}"))
            //{
            //    build.WriteToConsole();
            //}

        }
    }
}
