using System;

namespace Play.Company.Service.Dtos
{
    public record CreateCompanyDto(Guid OwnerId, String Name, String Contact, String Description);
    public record CompanyDto(Guid Id,  Guid OwnerId, String Name, String Contact, String Description, bool IsAprowed);
   
}