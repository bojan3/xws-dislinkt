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
        public override Task<Response> isAccountPublic(AccountId id, ServerCallContext context)
        {
            //var account = (await accountsRepository.GetAsync(new Guid())).AsDto();
            //var account = accountsRepository.GetAsync(new Guid(id.Id)).AsDto();

            //return new Response { IsPublic = account.IsPublic };
            return Task.FromResult(new Response { IsPublic = false });
        }


    }
}
