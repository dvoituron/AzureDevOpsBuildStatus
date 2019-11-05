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
            var builds = await factory.Builds.GetStatusAsync();

            foreach (var build in builds.OrderBy(i => $"{i.Project}-{i.Name}"))
            {
                build.WriteToConsole();
            }

        }
    }
}
