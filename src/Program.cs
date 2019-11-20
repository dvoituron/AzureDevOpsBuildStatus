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

            var factory = new Factory(args);

            if (factory.Configuration.DisplayHelp)
            {
                Console.WriteLine("Arguments: ");
                Console.WriteLine("--help, -h           Display this help.");
                Console.WriteLine("--url, -u            Set the Azure DevOps URL. By default: https://dev.azure.com.");
                Console.WriteLine("--organization, -o   Set the organisation name.");
                Console.WriteLine("--pat, -p            Set the security PAT key.");
            }
            else
            {
                var projects = await factory.Projects.GetAllProjectsAsync();

                foreach (var project in projects)
                {
                    var builds = await factory.Builds.GetStatusAsync(project.Name, buildDefinitionId: null);
                    builds.WriteToConsole();
                }
            }
        }
    }
}
