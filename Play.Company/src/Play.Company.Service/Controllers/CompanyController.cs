using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Play.Common;
using Play.Company.Service.Clients;
using Play.Company.Service.Dtos;
using Play.Company.Service.Entities;

namespace Play.Company.Service.Controllers
{
    [ApiController]
    [Route("Company")]

    public class CompanyController : ControllerBase
    {
        private readonly IRepository<Play.Company.Service.Entities.Company> companiesRepository;
        private readonly AccountClient accountClinet;

        public CompanyController(IRepository<Play.Company.Service.Entities.Company> companiesRepository, AccountClient accountClient)
        {
            this.companiesRepository = companiesRepository;
            this.accountClinet = accountClient;
        }

        [HttpGet]
        public async Task<IEnumerable<CompanyDto>> GetAsync()
        {
            var companies = (await companiesRepository.GetAllAsync()).Select(companies => companies.AsDto());
            return companies;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CompanyDto>> GetByIdAsync(Guid id)
        {
            var company = (await companiesRepository.GetAsync(id)).AsDto();

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        [HttpPost]
        public async Task<ActionResult<CompanyDto>> CreateCompanyAsync(CreateCompanyDto companyDto)
        {
            var company = new Play.Company.Service.Entities.Company
            {
                Id = Guid.NewGuid(),
                OwnerId = companyDto.OwnerId,
                Name = companyDto.Name,
                Contact = companyDto.Contact,
                Description = companyDto.Description,
                IsApproved = false
            };

            await companiesRepository.CreateAsync(company);

            // this return route of item when it's stored
            return CreatedAtAction(nameof(GetByIdAsync), new { id = company.Id }, company);
        }

        [HttpPut("Approve/{Id}")]
        public async Task<IActionResult> ApproveCompany(Guid Id)
        {
            
             var company = await companiesRepository.GetAsync(Id);
            if(company != null) {
                company.IsApproved = true;
                await companiesRepository.UpdateAsync(company);
            }
                return NotFound();
        
        }

    }
}
