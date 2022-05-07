using System;
using Play.Common;

namespace Play.Catalog.Service.Entities
{

    public enum Gender
    {
        male,
        female
    }

    public class Account : IEntity
    {
        public Guid Id { get; set; }

        public String Username { get; set; }

        public String Password { get; set; }

        public String Email { get; set; }

        public String PhoneNumber { get; set; }

        public Gender Gender { get; set; }

        public String Biography { get; set; }

        public Boolean isPublic { get; set; }
    }
}