using MeBay.Models;
using Microsoft.AspNetCore.Mvc;

public interface IItemRepository
{
    Task<IActionResult> GetAllItems();
    Task<IActionResult> AddItem(ItemSellDto item, HttpContext httpContext);
    // Task<IActionResult> GetItemForUser(int Id);
}
