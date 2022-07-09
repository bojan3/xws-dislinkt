using System.Text.Json.Serialization;
using System;
using Play.Common;
using System.Collections.Generic;
using Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Entities
{
    public class AuthenticateResponse : IEntity
    {
        public Guid Id { get; set; }
        public String Username { get; set; }
        public String Email { get; set; }
        public String Token{get;set;}  
        public bool IsPublic { get; set; }

        public AuthenticateResponse(AccountDto account, String token)
        {
            this.Id = account.Id;
            this.Username = account.Username;
            this.Email = account.Email;
            this.IsPublic = account.IsPublic;
            this.Token = token;
        }
    }

}