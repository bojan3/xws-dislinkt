using System;

namespace Play.JobOffer.Service.Dtos
{
        public record JobOfferDto(Guid Id,Guid CompanyId, String Name , String Description , String Requirement, DateTime CreatedDate);
        public record CreateJobOfferDto(Guid CompanyId, String Name , String Description , String Requirement);
}