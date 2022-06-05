using System;
using System.Collections.Generic;
using Play.Common;

namespace Play.JobOffer.Service.Entities
{
    public class JobOffers: IEntity
    {
        public Guid Id {get;set;}
        
        public Guid CompanyId {get;set;}
        public String Name {get;set;}
        public String Description {get;set;}
        public String Requirement {get;set;}
        public DateTime CreatedDate {get;set;}
    }
}