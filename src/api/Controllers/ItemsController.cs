using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using r7.Models;
using r7.Services;

namespace r7
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemsController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Item>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetItems()
        {
            try
            {
                var returnedItems = await _itemService.GetItems();
                return Ok(returnedItems);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e);
            }
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(Item), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [Route("{itemId}")]
        public async Task<ActionResult> GetItemByItemId(long itemId)
        {
            try
            {
                var returnedItem = await _itemService.GetItemByItemId(itemId);
                return Ok(returnedItem);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e);
            }
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(Item), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddItem([FromBody] NewItemRequest item)
        {
            try
            {
                var newItem = await _itemService.AddItem(item);

                return Created($"{newItem.Id}", newItem);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e);
            }
        }
        
        [HttpPut]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> EditItem([FromBody] Item item)
        {
            try
            {
                await _itemService.EditItem(item);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e);
            }
        }
    }
}