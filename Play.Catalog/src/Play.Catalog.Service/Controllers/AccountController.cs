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
        public async Task<IEnumerable<AccountDto>> GetAsync()
        {

            var accounts = (await accountsRepository.GetAllAsync()).Select(account => account.AsDto());
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
            var account = new Account
            {
                Username = createAccountDto.Username,
                Email = createAccountDto.Email,
                Password = createAccountDto.Password,
                PhoneNumber = createAccountDto.PhoneNumber,
                Gender = createAccountDto.Gender,
                Biography = createAccountDto.Biography
            };

            await accountsRepository.CreateAsync(account);

            // this return route of item when it's stored
            return CreatedAtAction(nameof(GetByIdAsync), new { id = account.Id }, account);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdatedAccountDto updateAccountDto)
        {
            var existingAccount = await accountsRepository.GetAsync(id);

            if (existingAccount == null)
            {
                return NotFound();
            }

            existingAccount.Username = updateAccountDto.Username;
            existingAccount.Password = updateAccountDto.Password;
            existingAccount.Email = updateAccountDto.Email;

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