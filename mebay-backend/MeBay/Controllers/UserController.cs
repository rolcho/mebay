using Microsoft.AspNetCore.Mvc;
using MeBay.Models;
using Microsoft.AspNetCore.Authorization;

namespace MeBay.Controllers.UserController
{
    [ApiController]
    [Route("/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto user)
        {
            return await _userRepository.Login(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Registartion([FromBody] UserRegisterDto user)
        {
            return await _userRepository.Registartion(user);
        }

        [HttpGet("list")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            return await _userRepository.GetAllUsers(HttpContext);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            return await _userRepository.GetUser(id, HttpContext);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> UpdateUser([FromRoute] int id, [FromBody] User user)
        {
            return await _userRepository.UpdateUser(id, user, HttpContext);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            return await _userRepository.DeleteUser(id, HttpContext);
        }

        [HttpPost("credit")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> ChangeCredit([FromBody] CreditTransfer creditTransfer)
        {
            return await _userRepository.CreditTransfer(creditTransfer, HttpContext);
        }
    }
}
