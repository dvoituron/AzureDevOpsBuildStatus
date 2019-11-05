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

            var build = await factory.Builds.GetStatus("DossierUnique", 38);
            build.WriteToConsole();
        }
    }
}
