using Play.Company.Service.Dtos;
using Play.Company.Service.Entities;

namespace Play.Company.Service
{
    public static class Extensions
    {
        public static CompanyDto AsDto(this Play.Company.Service.Entities.Company company)
        {
            return new CompanyDto(company.Id,  company.OwnerId, company.Name, company.Contact, company.Description, company.IsApproved);
        }
    }
}