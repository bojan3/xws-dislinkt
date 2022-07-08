using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Play.Catalog.Service.Entities;
using Play.Common;
using System;
using System.Threading.Tasks;

namespace Play.Catalog.Service.Services
{
    public class IsPublicService: IsPublic.IsPublicBase
    {
        private readonly ILogger<IsPublicService> _logger;
        private readonly IRepository<Account> accountsRepository;

        public IsPublicService(ILogger<IsPublicService> logger, IRepository<Account> accountsRepository)
        {
            _logger = logger;
            this.accountsRepository = accountsRepository;
        }
        public override async Task<Response> isAccountPublic(AccountId id, ServerCallContext context)
        {
            var account = (await accountsRepository.GetAsync(new Guid(id.Id))).AsDto();
            return new Response { IsPublic = account.IsPublic };
        }

        /*public override async Task isAccountPublic(AccountId request, IServerStreamWriter<Response> responseStream, ServerCallContext context)
        {
            var account = (await accountsRepository.GetAsync(new Guid())).AsDto();
            //return new Response { IsPublic = false };
            await responseStream.WriteAsync(new Response {IsPublic = account.IsPublic});
        }*/

    }
}