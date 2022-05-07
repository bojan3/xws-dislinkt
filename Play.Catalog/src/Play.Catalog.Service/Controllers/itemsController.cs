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
    [Route("items")]
    public class ItemController : ControllerBase
    {
        // readonly it's for editing only while constructing item
        // private static readonly List<ItemDto> items = new()
        // {
        //     new ItemDto(Guid.NewGuid(), "Potion", "Restores small amount of HP", 5, System.DateTimeOffset.UtcNow),
        //     new ItemDto(Guid.NewGuid(), "Antidote", "CuresPotion", 7, System.DateTimeOffset.UtcNow),
        //     new ItemDto(Guid.NewGuid(), "Bronze sword", "Deals a small amount of damage", 20, System.DateTimeOffset.UtcNow),
        // };

        private readonly IRepository<Item> itemsRepository;

        public ItemController(IRepository<Item> itemsRepository)
        {
            this.itemsRepository = itemsRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetAsync()
        {
            var items = (await itemsRepository.GetAllAsync()).Select(item => item.AsDto());
            return items;
        }
        // GET /items/123
        [HttpGet("{id}")]
        //public ItemDto GetById(Guid id)
        // ActionsResult is just so we can return NotFound or ItemDto
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            // signleOrDefault = return item of null

            var item = (await itemsRepository.GetAsync(id)).AsDto();

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // actionResults is for returning some sort of type
        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostAsync(CreateItemDto createItemDto)
        {
            var item = new Item
            {
                Name = createItemDto.Name,
                Description = createItemDto.Description,
                Price = createItemDto.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await itemsRepository.CreateAsync(item);

            // this return route of item when it's stored
            return CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, UpdateItemDto updateItemDto)
        {
            var existingItem = await itemsRepository.GetAsync(id);

            if (existingItem == null)
            {
                return NotFound();
            }

            existingItem.Name = updateItemDto.Name;
            existingItem.Description = updateItemDto.Description;
            existingItem.Price = updateItemDto.Price;

            await itemsRepository.UpdateAsync(existingItem);

            // //  with create copy with what you had and what you changed
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

            var existingItem = await itemsRepository.GetAsync(id);

            if (existingItem == null)
            {
                return NotFound();
            }

            await itemsRepository.RemoveAsync(id);

            // if (index < 0)
            // {
            //     return NotFound();
            // }

            // items.RemoveAt(index);

            return NoContent();
        }
    }

}