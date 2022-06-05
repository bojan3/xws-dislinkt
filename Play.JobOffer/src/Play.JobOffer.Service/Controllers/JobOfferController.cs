using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.JobOffer.Service.Clients;
using Play.JobOffer.Service.Entities;
using Play.JobOffer.Service.Dtos;
using Play.JobOffer.Service.Entities;

namespace Play.JobOffer.Service.Controllers
{
    [ApiController]
    [Route("JobOffer")]
    public class JobOfferController: ControllerBase
    {
        private readonly IRepository<JobOffers> jobsRepository;
        private readonly AccountClient accountClinet;

        public JobOfferController(IRepository<JobOffers> jobsRepository, AccountClient accountClient)
        {
            this.jobsRepository = jobsRepository;
            this.accountClinet = accountClient;
        }

        [HttpGet]
        public async Task<IEnumerable<JobOfferDto>> GetAsync()
        {
            var jobs = (await jobsRepository.GetAllAsync()).Select(jobs => jobs.AsDto());
            return jobs;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<JobOfferDto>> GetByIdAsync(Guid id)
        {
            var job = (await jobsRepository.GetAsync(id)).AsDto();

            if (job == null)
            {
                return NotFound();
            }

            return job;
        }

        [HttpPost]
        public async Task<ActionResult<JobOfferDto>> CreateJobOfferAsync(CreateJobOfferDto jobOfferDto)
        {
            var job = new Play.JobOffer.Service.Entities.JobOffers
            {
                Id = Guid.NewGuid(),
                CompanyId = jobOfferDto.CompanyId,
                Name = jobOfferDto.Name,
                Description = jobOfferDto.Description,
                Requirement = jobOfferDto.Requirement,
                CreatedDate = DateTime.UtcNow
            };

            await jobsRepository.CreateAsync(job);

            // this return route of item when it's stored
            return CreatedAtAction(nameof(GetByIdAsync), new { id = job.Id }, job);
        }

   
    }
}