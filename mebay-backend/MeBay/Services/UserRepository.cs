using Microsoft.AspNetCore.Mvc;
using MeBay.Models;
using AutoMapper;
using MeBay.Data;
using Microsoft.EntityFrameworkCore;
using Mebay.Models;
using MeBay.Interfaces;

namespace MeBay.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly IMapper _mapper;
        private readonly MeBayDbContext _dbContext;
        private readonly IJWTService _jwtService;

        public UserRepository(IMapper mapper, MeBayDbContext dbContext, IJWTService jwtService)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _jwtService = jwtService;
        }

        public async Task<IActionResult> Login(UserLoginDto user)
        {
            var userByEmail = await _dbContext.Users.FirstOrDefaultAsync(
                u => u.Email == user.Email
            );
            if (userByEmail == null)
            {
                return new NotFoundResult();
            }
            if (!BCrypt.Net.BCrypt.Verify(user.Password, userByEmail.Password))
            {
                return new UnauthorizedResult();
            }
            var userData = _mapper.Map<UserLoginResponseDto>(userByEmail);
            userData.Token = _jwtService.CreateToken(
                userByEmail.Id,
                userByEmail.Name,
                userByEmail.Role
            );
            return new OkObjectResult(userData);
        }

        public async Task<IActionResult> Registartion(UserRegisterDto user)
        {
            var userByEmail = await _dbContext.Users.FirstOrDefaultAsync(
                u => u.Email == user.Email
            );
            if (userByEmail != null)
            {
                return new ConflictResult();
            }
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);

            var newUser = _mapper.Map<User>(user);
            newUser.Password = passwordHash;
            newUser.Role = !_dbContext.Users.Any() ? "Admin" : "User";
            newUser.CreationDate = DateTime.UtcNow;
            newUser.ModificationDate = DateTime.UtcNow;

            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();

            return new CreatedResult("", new { newUser.Email });
        }

        public async Task<IActionResult> GetAllUsers(HttpContext httpContext)
        {
            var users = await _dbContext.Users.ToListAsync();
            return new OkObjectResult(_mapper.Map<List<UserResponseDto>>(users));
        }

        public async Task<IActionResult> GetUser(int id, HttpContext httpContext)
        {
            var authenticatedUser = httpContext.User;
            var claims = authenticatedUser.Claims;
            var tokenData = _jwtService.DecodeToken(claims);

            var userById = await _dbContext.Users.FindAsync(id);
            if (userById == null)
            {
                return new NotFoundResult();
            }
            if (id != tokenData.UserId && tokenData.Role != "Admin")
            {
                return new ForbidResult();
            }
            return new OkObjectResult(_mapper.Map<UserResponseDto>(userById));
        }

        public async Task<IActionResult> UpdateUser(int id, User user, HttpContext httpContext)
        {
            var authenticatedUser = httpContext.User;
            var claims = authenticatedUser.Claims;
            var tokenData = _jwtService.DecodeToken(claims);

            var userById = await _dbContext.Users.FindAsync(id);
            if (userById == null)
            {
                return new NotFoundResult();
            }
            if (id != tokenData.UserId && tokenData.Role != "Admin")
            {
                return new ForbidResult();
            }
            var userByEmail = await _dbContext.Users.FirstOrDefaultAsync(
                u => u.Email == user.Email
            );
            if (userByEmail != null && userByEmail.Id != id)
            {
                return new ConflictResult();
            }

            var passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            userById.ModificationDate = DateTime.UtcNow;
            userById.Name = user.Name;
            userById.Email = user.Email;
            userById.Password = passwordHash;

            if (tokenData.Role == "Admin")
            {
                userById.Role = user.Role;
            }

            await _dbContext.SaveChangesAsync();
            return new OkResult();
        }

        public async Task<IActionResult> DeleteUser(int id, HttpContext httpContext)
        {
            var authenticatedUser = httpContext.User;
            var claims = authenticatedUser.Claims;
            var tokenData = _jwtService.DecodeToken(claims);

            var userById = await _dbContext.Users.FindAsync(id);
            if (userById == null)
            {
                return new NotFoundResult();
            }
            if (id != tokenData.UserId && tokenData.Role != "Admin")
            {
                return new ForbidResult();
            }
            _dbContext.Remove(userById);
            await _dbContext.SaveChangesAsync();
            return new NoContentResult();
        }

        public async Task<IActionResult> CreditTransfer(
            CreditTransfer creditTransfer,
            HttpContext httpContext
        )
        {
            var authenticatedUser = httpContext.User;
            var claims = authenticatedUser.Claims;
            var tokenData = _jwtService.DecodeToken(claims);

            var userById = await _dbContext.Users.FindAsync(creditTransfer.Id);
            if (userById == null)
            {
                return new NotFoundResult();
            }
            if (creditTransfer.Id != tokenData.UserId && tokenData.Role != "Admin")
            {
                return new ForbidResult();
            }
            if (userById.Credits + creditTransfer.Credits < 0)
            {
                return new BadRequestResult();
            }
            userById.Credits += creditTransfer.Credits;
            await _dbContext.SaveChangesAsync();
            return new OkObjectResult(new { Credits = userById.Credits });
        }
    }
}
