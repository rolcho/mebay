using MeBay.Models;
using Microsoft.AspNetCore.Mvc;

public interface IItemRepository
{
    Task<IActionResult> GetAllItems();
    Task<IActionResult> AddItem(ItemSellDto item, HttpContext httpContext);
    Task<IActionResult> BuyItem(int itemId, HttpContext httpContext);
    Task<IActionResult> DeleteItem(int itemId, HttpContext httpContext);
    Task<IActionResult> GetAllToBuy(HttpContext httpContext);
    Task<IActionResult> GetAllForSale(HttpContext httpContext);
    Task<IActionResult> GetAllBought(HttpContext httpContext);
    Task<IActionResult> GetAllSold(HttpContext httpContext);
}
