using MeBay.Models;
using Microsoft.AspNetCore.Mvc;

public interface IUserRepository
{
    Task<IActionResult> Login(UserLoginDto user);
    Task<IActionResult> Registartion(UserRegisterDto user);
    Task<IActionResult> GetAllUsers(HttpContext httpContext);
    Task<IActionResult> UpdateUser(int id, User user, HttpContext httpContext);
    Task<IActionResult> DeleteUser(int id, HttpContext httpContext);
    Task<IActionResult> GetUser(int id, HttpContext httpContext);
    Task<IActionResult> CreditTransfer(CreditTransfer changeCredit, HttpContext httpContext);
}
