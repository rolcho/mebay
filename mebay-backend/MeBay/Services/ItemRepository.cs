using Microsoft.AspNetCore.Mvc;
using MeBay.Models;
using AutoMapper;
using MeBay.Data;
using Microsoft.EntityFrameworkCore;
using Mebay.Models;
using MeBay.Interfaces;
using System.Net;

namespace MeBay.Services
{
    public class ItemRepository : IItemRepository
    {
        private readonly IMapper _mapper;
        private readonly MeBayDbContext _dbContext;
        private readonly IJWTService _jwtService;

        public ItemRepository(IMapper mapper, MeBayDbContext dbContext, IJWTService jwtService)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _jwtService = jwtService;
        }

        public async Task<IActionResult> GetAllItems()
        {
            var items = await _dbContext.Items
                .Include(i => i.Seller)
                .OrderByDescending(i => i.ListingDate)
                .ToListAsync();

            return new OkObjectResult(_mapper.Map<List<ItemDto>>(items));
        }

        public async Task<IActionResult> GetAllForSale(HttpContext httpContext)
        {
            var userId = getUserId(httpContext);

            var items = await _dbContext.Items
                .Include(i => i.Seller)
                .Include(i => i.Buyer)
                .Where(i => (i.SellerId == userId) && (i.BuyerId == null))
                .OrderByDescending(i => i.ListingDate)
                .ToListAsync();

            return new OkObjectResult(_mapper.Map<List<ItemDto>>(items));
        }

        public async Task<IActionResult> GetAllToBuy(HttpContext httpContext)
        {
            var userId = getUserId(httpContext);

            var items = await _dbContext.Items
                .Include(i => i.Seller)
                .Include(i => i.Buyer)
                .Where(i => (i.SellerId != userId) && (i.BuyerId == null))
                .OrderByDescending(i => i.ListingDate)
                .ToListAsync();

            return new OkObjectResult(_mapper.Map<List<ItemDto>>(items));
        }

        public async Task<IActionResult> GetAllSold(HttpContext httpContext)
        {
            var userId = getUserId(httpContext);

            var items = await _dbContext.Items
                .Include(i => i.Seller)
                .Include(i => i.Buyer)
                .Where(i => (i.SellerId == userId) && (i.BuyerId != null))
                .OrderByDescending(i => i.ListingDate)
                .ToListAsync();

            return new OkObjectResult(_mapper.Map<List<ItemDto>>(items));
        }

        public async Task<IActionResult> GetAllBought(HttpContext httpContext)
        {
            var authenticatedUser = httpContext.User;
            var claims = authenticatedUser.Claims;
            var tokenData = _jwtService.DecodeToken(claims);

            var items = await _dbContext.Items
                .Include(i => i.Seller)
                .Include(i => i.Buyer)
                .Where(i => i.BuyerId == tokenData.UserId)
                .OrderByDescending(i => i.ListingDate)
                .ToListAsync();

            return new OkObjectResult(_mapper.Map<List<ItemDto>>(items));
        }

        public async Task<IActionResult> AddItem(ItemSellDto item, HttpContext httpContext)
        {
            var userId = getUserId(httpContext);

            var userById = await _dbContext.Users.FindAsync(userId);

            if (userById == null)
            {
                return new NotFoundResult();
            }

            if (item.Picture == null)
            {
                item.Picture = "https://www.freeiconspng.com/uploads/no-image-icon-11.PNG";
            }

            if (Uri.TryCreate(item.Picture, UriKind.Absolute, out var imageUri))
            {
                using (var httpClient = new HttpClient())
                {
                    try
                    {
                        var response = await httpClient.GetAsync(imageUri);

                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            item.Picture =
                                "https://www.freeiconspng.com/uploads/no-image-icon-11.PNG";
                        }
                    }
                    catch (HttpRequestException)
                    {
                        item.Picture = "https://www.freeiconspng.com/uploads/no-image-icon-11.PNG";
                    }
                }
            }
            else
            {
                item.Picture = "https://www.freeiconspng.com/uploads/no-image-icon-11.PNG";
            }

            var newItem = _mapper.Map<Item>(item);
            newItem.SellerId = userId;
            newItem.ListingDate = DateTime.UtcNow;

            await _dbContext.Items.AddAsync(newItem);
            await _dbContext.SaveChangesAsync();

            return new CreatedResult("", new { });
        }

        public async Task<IActionResult> BuyItem(int itemId, HttpContext httpContext)
        {
            var buyerId = getUserId(httpContext);

            var buyerById = await _dbContext.Users.FindAsync(buyerId);
            if (buyerById == null)
            {
                return new NotFoundResult();
            }

            var itemById = await _dbContext.Items.FindAsync(itemId);
            if (itemById == null)
            {
                return new NotFoundResult();
            }

            var sellerById = await _dbContext.Users.FindAsync(itemById.SellerId);
            if (sellerById == null)
            {
                return new NotFoundResult();
            }

            if (buyerById.Credits < itemById.InstantPrice)
            {
                return new BadRequestResult();
            }

            buyerById.Credits -= itemById.InstantPrice;
            sellerById.Credits += itemById.InstantPrice;
            itemById.BuyerId = buyerById.Id;
            itemById.Buyer = buyerById;

            await _dbContext.SaveChangesAsync();

            return new OkResult();
        }

        public async Task<IActionResult> DeleteItem(int itemId, HttpContext httpContext)
        {
            var sellerId = getUserId(httpContext);

            var itemById = await _dbContext.Items.FindAsync(itemId);
            if (itemById == null)
            {
                return new NotFoundResult();
            }

            if (itemById.SellerId != sellerId)
            {
                return new ForbidResult();
            }

            _dbContext.Remove(itemById);
            await _dbContext.SaveChangesAsync();

            return new NoContentResult();
        }

        private int getUserId(HttpContext httpContext)
        {
            var authenticatedUser = httpContext.User;
            var claims = authenticatedUser.Claims;
            var tokenData = _jwtService.DecodeToken(claims);

            return (int)tokenData.UserId!;
        }
    }
}
