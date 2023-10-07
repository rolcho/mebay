using Microsoft.AspNetCore.Mvc;
using MeBay.Models;
using AutoMapper;
using MeBay.Data;
using Microsoft.EntityFrameworkCore;
using Mebay.Models;
using MeBay.Interfaces;

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
                .Include(i => i.User)
                .OrderByDescending(i => i.ListingDate)
                .ToListAsync();

            return new OkObjectResult(_mapper.Map<List<ItemDto>>(items));
        }

        public async Task<IActionResult> AddItem(ItemSellDto item, HttpContext httpContext)
        {
            var authenticatedUser = httpContext.User;
            var claims = authenticatedUser.Claims;
            var tokenData = _jwtService.DecodeToken(claims);

            var userById = await _dbContext.Users.FindAsync(tokenData.UserId);

            if (userById == null)
            {
                return new NotFoundResult();
            }

            var newItem = _mapper.Map<Item>(item);
            newItem.UserId = (int)tokenData.UserId!;
            newItem.ListingDate = DateTime.UtcNow;

            await _dbContext.Items.AddAsync(newItem);
            await _dbContext.SaveChangesAsync();

            return new CreatedResult("", new { });
        }
    }
}
