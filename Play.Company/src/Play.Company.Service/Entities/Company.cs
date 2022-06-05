using System;
using System.Collections.Generic;
using Play.Common;

namespace Play.Company.Service.Entities
{
    public class Company : IEntity
    {
        public Guid Id {get; set;}
        public Guid OwnerId {get; set;}
        public String Name {get; set;}
        public String Contact {get; set;}
        public String Description {get; set;}
        public bool IsApproved {get; set;}

    }
}

