using ProjectStatus.Models;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;

namespace ProjectStatus
{
    public class Factory
    {
        public Factory(string[] args)
        {
            Configuration = new Configuration().FillWithArgs(args);
        }

        public Configuration Configuration { get; }

        public Services.BuildService Builds => new Services.BuildService(this);

        public Services.ProjectService Projects => new Services.ProjectService(this);

        public HttpClient GetHttpClient()
        {
            var client = new HttpClient();

            client.BaseAddress = Configuration.BaseUrl;
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Configuration.PatAsBase64);

            return client;
        }
    }
}
