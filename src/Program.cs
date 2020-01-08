using ProjectStatus.Helpers;
using ProjectStatus.Models;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectStatus
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var watcher = System.Diagnostics.Stopwatch.StartNew();
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
                    const bool IN_PARALLEL = true;

                    var projects = await factory.Projects.GetAllProjectsAsync();
                    BuildStatus.WriteHeaders();

                    if (IN_PARALLEL)
                    {
                        var tasks = new ConcurrentDictionary<Project, Task<IEnumerable<BuildStatus>>>();
                        Parallel.ForEach(projects, (project) =>
                        {
                            tasks.TryAdd(project, factory.Builds.GetStatusAsync(project.Name, buildDefinitionId: null));
                        });

                        foreach (var task in tasks)
                        {
                            task.Value.Wait();
                            task.Value.GetAwaiter().GetResult().WriteToConsole();
                        }
                    }

                    else
                    {
                        foreach (var project in projects)
                        {
                            var builds = await factory.Builds.GetStatusAsync(project.Name, buildDefinitionId: null);
                            builds.WriteToConsole();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ERROR: {ex.Message}");
                    Console.ResetColor();
                }

                Console.WriteLine($"Finished in {watcher.ElapsedMilliseconds} ms.");

            }
        }
    }
}
