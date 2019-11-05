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
            Builds = new Builds(this);
        }

        public Configuration Configuration { get; }

        public Builds Builds { get; }

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
