using Microsoft.AspNetCore.Mvc;
using MeBay.Models;
using MeBay.Services;
using AutoMapper;
using MeBay.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Mebay.Models;

namespace MeBay.UserController
{
    [ApiController]
    [Route("/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly MeBayDbContext _dbContext;
        private readonly JWTService _jwtService;

        public UserController(IMapper mapper, MeBayDbContext dbContext, JWTService jwtService)
        {
            _mapper = mapper;
            _dbContext = dbContext;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto user)
        {
            Console.WriteLine(user);
            var userByEmail = await _dbContext.Users.FirstOrDefaultAsync(
                u => u.Email == user.Email
            );
            if (userByEmail == null)
            {
                return NotFound();
            }
            if (!BCrypt.Net.BCrypt.Verify(user.Password, userByEmail.Password))
            {
                return Unauthorized();
            }
            var jwt = new AccessToken();
            jwt.Token = _jwtService.CreateToken(userByEmail.Id, userByEmail.Name, userByEmail.Role);
            return Ok(jwt);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Registartion([FromBody] UserRegisterDto user)
        {
            var userByEmail = await _dbContext.Users.FirstOrDefaultAsync(
                u => u.Email == user.Email
            );
            if (userByEmail != null)
            {
                return Conflict();
            }
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);

            var newUser = _mapper.Map<User>(user);
            newUser.Password = passwordHash;
            newUser.Role = !_dbContext.Users.Any() ? "Admin" : "User";
            newUser.CreationDate = DateTime.UtcNow;
            newUser.ModificationDate = DateTime.UtcNow;

            await _dbContext.Users.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();

            return Created("", new { newUser.Email });
        }

        [HttpGet("list")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _dbContext.Users.ToListAsync();
            return Ok(_mapper.Map<List<UserResponseDto>>(users));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            var authenticatedUser = HttpContext.User;
            var claims = authenticatedUser.Claims;
            var tokenData = _jwtService.DecodeToken(claims);

            var list = _dbContext.Users.ToListAsync();
            var userById = await _dbContext.Users.FindAsync(id);
            if (userById == null)
            {
                return NotFound();
            }
            if (id != tokenData.UserId && tokenData.Role != "Admin")
            {
                return Forbid();
            }
            return Ok(_mapper.Map<UserResponseDto>(userById));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] User user)
        {
            var authenticatedUser = HttpContext.User;
            var claims = authenticatedUser.Claims;
            var tokenData = _jwtService.DecodeToken(claims);

            var userById = await _dbContext.Users.FindAsync(id);
            if (userById == null)
            {
                return NotFound();
            }
            if (id != tokenData.UserId && tokenData.Role != "Admin")
            {
                return Forbid();
            }
            var userByEmail = await _dbContext.Users.FirstOrDefaultAsync(
                u => u.Email == user.Email
            );
            if (userByEmail != null && userByEmail.Id != id)
            {
                return Conflict();
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
            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            var authenticatedUser = HttpContext.User;
            var claims = authenticatedUser.Claims;
            var tokenData = _jwtService.DecodeToken(claims);

            var userById = await _dbContext.Users.FindAsync(id);
            if (userById == null)
            {
                return NotFound();
            }
            if (id != tokenData.UserId && tokenData.Role != "Admin")
            {
                return Forbid();
            }
            _dbContext.Remove(userById);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
