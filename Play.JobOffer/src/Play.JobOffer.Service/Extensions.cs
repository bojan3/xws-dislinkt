using Play.JobOffer.Service.Dtos;
using Play.JobOffer.Service.Entities;

namespace Play.JobOffer.Service
{
    public static class Extensions
    {
        

        public static JobOfferDto AsDto(this Play.JobOffer.Service.Entities.JobOffers jobOffers)
        {
            return new JobOfferDto(jobOffers.Id ,jobOffers.CompanyId, jobOffers.Name , jobOffers.Description, jobOffers.Requirement , jobOffers.CreatedDate);
        }

    }
}