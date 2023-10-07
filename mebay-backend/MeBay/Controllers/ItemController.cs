using Microsoft.AspNetCore.Mvc;
using MeBay.Models;
using Microsoft.AspNetCore.Authorization;

namespace MeBay.Controllers.ItemController
{
    [ApiController]
    [Route("/[controller]")]
    public class ItemController : ControllerBase
    {
        private readonly IItemRepository _itemRepository;

        public ItemController(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllItems()
        {
            return await _itemRepository.GetAllItems();
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> AddItem([FromBody] ItemSellDto item)
        {
            return await _itemRepository.AddItem(item, HttpContext);
        }
    }
}
