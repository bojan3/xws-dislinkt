using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;
using System;
using System.Linq;
using System.Threading.Tasks;
using Play.Catalog.Service.Entities;
using Play.Common;

namespace Play.Catalog.Service.Controller
{
    [ApiController]
    // https://localhost:5002/items
    [Route("account")]
    public class AccountController : ControllerBase
    {
        // readonly it's for editing only while constructing item
        // private static readonly List<ItemDto> items = new()
        // {
        //     new ItemDto(Guid.NewGuid(), "Potion", "Restores small amount of HP", 5, System.DateTimeOffset.UtcNow),
        //     new ItemDto(Guid.NewGuid(), "Antidote", "CuresPotion", 7, System.DateTimeOffset.UtcNow),
        //     new ItemDto(Guid.NewGuid(), "Bronze sword", "Deals a small amount of damage", 20, System.DateTimeOffset.UtcNow),
        // };

        private readonly IRepository<Account> accountsRepository;

        public AccountController(IRepository<Account> accountsRepository)
        {
            this.accountsRepository = accountsRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<AccountDto>> GetPublicAsync()
        {

            var accounts = (await accountsRepository.GetAllAsync())
            //.Where(account => account.IsPublic)
            .Select(account => account.AsDto());

            return accounts;
        }
        // GET /items/123
        [HttpGet("{id}")]
        //public ItemDto GetById(Guid id)
        // ActionsResult is just so we can return NotFound or ItemDto
        public async Task<ActionResult<AccountDto>> GetByIdAsync(Guid id)
        {
            // signleOrDefault = return item of null

            var account = (await accountsRepository.GetAsync(id)).AsDto();

            if (account == null)
            {
                return NotFound();
            }

            return account;
        }

        // actionResults is for returning some sort of type
        [HttpPost]
        public async Task<ActionResult<AccountDto>> PostAsync(CreateAccountDto createAccountDto)
        {
            var accounts = (await accountsRepository.GetAllAsync());

            foreach (var acc in accounts)
            {

                if (acc.Username == createAccountDto.Username)
                {
                    return BadRequest(ModelState);
                }
            }
            var account = new Account
            {
                Username = createAccountDto.Username,
                Email = createAccountDto.Email,
                Password = createAccountDto.Password,
                PhoneNumber = createAccountDto.PhoneNumber,
                Gender = createAccountDto.Gender,
                Biography = createAccountDto.Biography,
                IsPublic = createAccountDto.IsPublic
            };

            await accountsRepository.CreateAsync(account);

            // this return route of item when it's stored
            return CreatedAtAction(nameof(GetByIdAsync), new { id = account.Id }, account);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdatedAccountDto updateAccountDto)
        {
            var existingAccount = await accountsRepository.GetAsync(id);

            var accounts = (await accountsRepository.GetAllAsync()).Where(account => account.Id != id);

            foreach (var acc in accounts)
            {

                if (acc.Username == updateAccountDto.Username)
                {
                    return BadRequest(ModelState);
                }
            }

            if (existingAccount == null)
            {
                return NotFound();
            }

            existingAccount.Username = updateAccountDto.Username;
            existingAccount.Password = updateAccountDto.Password;
            existingAccount.Email = updateAccountDto.Email;
            existingAccount.PhoneNumber = updateAccountDto.PhoneNumber;
            existingAccount.Biography = updateAccountDto.Biography;
            existingAccount.IsPublic = updateAccountDto.IsPublic;

            await accountsRepository.UpdateAsync(existingAccount);

            // // with create copy with what you had and what you changed
            // var updatedItem = existingItem with
            // {
            //     Name = updateItemDto.Name,
            //     Description = updateItemDto.Description,
            //     Price = updateItemDto.Price
            // };

            // var index = items.FindIndex(existingItem => existingItem.Id == id);
            // items[index] = updatedItem;

            return NoContent();
        }

        [HttpGet("{Username}, {Password}")]
        public async Task<ActionResult<AccountDto>> Login(string Username, string Password)
        {
            var accounts = (await accountsRepository.GetAllAsync());

            foreach (var account in accounts)
            {

                if (account.Username == Username && account.Password == Password)
                {
                    return account.AsDto();
                }
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            //var index = items.FindIndex(existingItem => existingItem.Id == id);

            var existingAccount = await accountsRepository.GetAsync(id);

            if (existingAccount == null)
            {
                return NotFound();
            }

            await accountsRepository.RemoveAsync(id);

            // if (index < 0)
            // {
            //     return NotFound();
            // }

            // items.RemoveAt(index);

            return NoContent();
        }
    }

}