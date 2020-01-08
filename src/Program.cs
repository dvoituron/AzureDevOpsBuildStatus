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
            var factory = new Factory(args);

            Console.WriteLine($"Status of Azure DevOps Builds at {DateTime.Now.ToShortTimeString()} for organisation {factory.Configuration.Organization}");
            Console.WriteLine();

            if (factory.Configuration.DisplayHelp)
            {
                Console.WriteLine("Arguments: ");
                Console.WriteLine("--help, -h           Display this help.");
                Console.WriteLine("--url, -u            Set the Azure DevOps URL. By default: https://dev.azure.com.");
                Console.WriteLine("--organization, -o   Set the organisation name.");
                Console.WriteLine("--pat, -p            Set the security PAT key.");
                Console.WriteLine("                     Go to Azure DevOps / Your profile / Security / Personal Access Token");
                Console.WriteLine("                     and create a new token with 'Build Read' and 'Release Read' authorizations.");
            }
            else
            {
                try
                {
                    var projects = await factory.Projects.GetAllProjectsAsync();

                    BuildStatus.WriteHeaders();
                    foreach (var project in projects)
                    {
                        var builds = await factory.Builds.GetStatusAsync(project.Name, buildDefinitionId: null);
                        builds.WriteToConsole();
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR: {ex.Message}");
                    Console.ResetColor();
                }

            }
        }
    }
}
