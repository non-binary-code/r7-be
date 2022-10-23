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
        [ProducesResponseType(typeof(IEnumerable<ReuseItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [Route("reuse")]
        public async Task<ActionResult> GetItems([FromQuery] ReuseQueryParameters queryParameters)
        {
            try
            {
                var returnedItems = await _itemService.GetItems(queryParameters);
                return Ok(returnedItems);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RecycleItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [Route("recycle")]
        public async Task<ActionResult> GetItems([FromQuery] RecycleQueryParameters queryParameters)
        {
            try
            {
                var returnedItems = await _itemService.GetItems(queryParameters);
                return Ok(returnedItems);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RepairItem>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [Route("repair")]
        public async Task<ActionResult> GetItems([FromQuery] RepairQueryParameters queryParameters)
        {
            try
            {
                var returnedItems = await _itemService.GetItems(queryParameters);
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
        [Route("reuse")]
        public async Task<ActionResult> AddItem([FromBody] NewReuseItemRequest item)
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

        [HttpPost]
        [ProducesResponseType(typeof(Item), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [Route("recycle")]
        public async Task<ActionResult> AddItem([FromBody] NewRecycleItemRequest item)
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

        [HttpPost]
        [ProducesResponseType(typeof(Item), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [Route("repair")]
        public async Task<ActionResult> AddItem([FromBody] NewRepairItemRequest item)
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
        [Route("{itemId}")]
        public async Task<ActionResult> EditItem(long itemId, [FromBody] EditItemRequest editItemRequest)
        {
            try
            {
                await _itemService.EditItem(itemId, editItemRequest);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        [Route("{itemId}/archive")]
        public async Task<ActionResult> ArchiveItem(long itemId, [FromBody] ArchiveItemRequest archiveItemRequest)
        {
            try
            {
                var found = await _itemService.ArchiveItem(itemId, archiveItemRequest);
                return found ? Ok() : NotFound();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e);
            }
        }
    }

    public class ReuseQueryParameters
    {
        public int? CategoryTypeId { get; set; }
        public int? ConditionTypeId { get; set; }
        public bool Delivery { get; set; }
        public bool Collection { get; set; }
        public bool Postage { get; set; }
        public bool Recover { get; set; }
        public bool IncludeArchived { get; set; }
    }

    public class RecycleQueryParameters
    {
        public bool? Compostable { get; set; }
        public bool IncludeArchived { get; set; }
    }

    public class RepairQueryParameters
    {
        public bool Delivery { get; set; }
        public bool Collection { get; set; }
        public bool Postage { get; set; }
        public bool Recover { get; set; }
        public bool IncludeArchived { get; set; }
    }
}