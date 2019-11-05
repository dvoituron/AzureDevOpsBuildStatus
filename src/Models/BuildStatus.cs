using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectStatus.Models
{
    public class BuildStatus
    {
        public string Version { get; set; }
        public string Status { get; set; }
        public string Result { get; set; }
        public string Author { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public ProcessStatus GetProcessStatus()
        {
            // https://docs.microsoft.com/en-us/rest/api/azure/devops/build/builds/get?view=azure-devops-rest-5.1#buildstatus
            if (Status == "completed" && Result == "succeeded")
            {
                return ProcessStatus.Succeeded;
            }
            else if (Result == "canceled" || Result == "failed" || Result == "partiallySucceeded")
            {
                return ProcessStatus.Failed;
            }
            else
            {
                return ProcessStatus.InProgress;
            }
        }

        public override string ToString()
        {
            return $"{Status} - {Version} - {Author}";
        }

        public void WriteToConsole()
        {
            string fromTo = $"{StartTime.ToShortDateString()} {StartTime.ToShortTimeString()} -> {EndTime.ToShortDateString()} {EndTime.ToShortTimeString()}";

            switch (GetProcessStatus())
            {
                case ProcessStatus.Succeeded:
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("SUCCEEDED  ");
                    break;
                case ProcessStatus.InProgress:
                    Console.BackgroundColor = ConsoleColor.Blue;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("IN PROGRESS");
                    break;
                case ProcessStatus.Failed:
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("FAILED     ");
                    break;
            }
            Console.ResetColor();
            Console.WriteLine($" {fromTo} | BUILD v.{Version} requested by {Author}");
        }

        public enum ProcessStatus
        {
            Succeeded,
            InProgress,
            Failed,
        }
    }
}
