using ProjectStatus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectStatus.Helpers
{
    public static class BuildStatusExtensions
    {
        public static void WriteToConsole(this IEnumerable<BuildStatus> builds)
        {
            foreach (var build in builds.OrderBy(i => $"{i.Project}-{i.Name}"))
            {
                build.WriteToConsole();
            }
        }
    }
}
