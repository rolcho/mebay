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

        [HttpGet("tobuy")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetAllToBuy()
        {
            return await _itemRepository.GetAllToBuy(HttpContext);
        }

        [HttpGet("forsale")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetAllForSale()
        {
            return await _itemRepository.GetAllForSale(HttpContext);
        }

        [HttpGet("bought")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetAllBought()
        {
            return await _itemRepository.GetAllBought(HttpContext);
        }

        [HttpGet("sold")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetAllSold()
        {
            return await _itemRepository.GetAllSold(HttpContext);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> AddItem([FromBody] ItemSellDto item)
        {
            return await _itemRepository.AddItem(item, HttpContext);
        }

        [HttpPatch("{itemId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> BuyItem([FromRoute] int itemId)
        {
            return await _itemRepository.BuyItem(itemId, HttpContext);
        }

        [HttpDelete("{itemId}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DeleteItem([FromRoute] int itemId)
        {
            return await _itemRepository.DeleteItem(itemId, HttpContext);
        }
    }
}
