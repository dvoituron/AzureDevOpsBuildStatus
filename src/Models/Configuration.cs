using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ProjectStatus.Models
{
    public class Configuration
    {
        public Uri BaseUrl { get; set; } = new Uri("https://dev.azure.com");

        public string Organization { get; set; }

        public string Pat { get; set; }

        public string PatAsBase64 => Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", "", this.Pat)));

        public bool IncludeValid { get; set; } = false;

        public bool DisplayHelp { get; set; } = false;

        public Configuration FillWithArgs(string[] args)
        {
            var arguments = args.Select(arg =>
            {
                int index = arg.IndexOfAny(new[] { ':', '=' });
                if (index > 0)
                    return new[] { arg.Substring(0, index), arg.Substring(index + 1) };
                else
                    return new[] { arg, String.Empty };
            });

            foreach (var argument in arguments)
            {
                if (argument.Length == 2)
                {
                    string name = argument[0].ToLower();
                    string value = argument[1];

                    switch (name)
                    {
                        case "--help":
                        case "-h":
                            this.DisplayHelp = true;
                            break;

                        case "--url":
                        case "-u":
                            this.BaseUrl = new Uri(value);
                            break;

                        case "--organization":
                        case "-o":
                            this.Organization = value;
                            break;

                        case "--pat":
                        case "-p":
                            this.Pat = value;
                            break;

                        case "--include-valid":
                        case "-iv":
                            this.IncludeValid = true;
                            break;

                        default:
                            this.DisplayHelp = true;
                            break;
                    }
                }
            }

            return this;
        }
    }
}
