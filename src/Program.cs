using ProjectStatus.Models;
using System;
using System.Threading.Tasks;

namespace ProjectStatus
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var factory = new Factory();

            var builds = await factory.Builds.GetStatusAsync();

            foreach (var build in builds)
            {
                build.WriteToConsole();
            }

        }
    }
}
