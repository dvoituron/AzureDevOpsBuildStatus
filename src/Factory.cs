using ProjectStatus.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace ProjectStatus
{
    public class Factory
    {
        public Factory()
        {
            Configuration = Models.Configuration.ReadAppSettings();
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
