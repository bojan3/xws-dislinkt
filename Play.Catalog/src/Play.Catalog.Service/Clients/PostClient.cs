using System.Net.Http;

namespace Play.Catalog.Service.Clients
{
    public class PostClient
    {
        private readonly HttpClient httpClient;

        public PostClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }
    }
}