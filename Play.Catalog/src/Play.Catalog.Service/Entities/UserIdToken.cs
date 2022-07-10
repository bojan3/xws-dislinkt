using System;
using Play.Common;
using Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Entities
{
    public class UserIdToken : IEntity
    {
        public Guid Id { get; set; }
        public String Token{get;set;}  

        public UserIdToken(Guid accountId, String token)
        {
            this.Id = accountId;
            this.Token = token;
        }
    }

}