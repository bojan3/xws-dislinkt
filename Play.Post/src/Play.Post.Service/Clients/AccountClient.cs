using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Play.Post.Service.Clients
{
    public class AccountClient
    {
        private readonly HttpClient httpClient;

        public AccountClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<ActionResult<bool>> GetIsPublic(Guid id)
        {
            return await httpClient.GetFromJsonAsync<bool>($"account/isPublic/{id}");
        }
    }
}