using MeBay.Models;
using System.Security.Claims;

namespace MeBay.Interfaces
{
    public interface IJWTService
    {
        string CreateToken(int userId, string name, string userrole);
        TokenDto DecodeToken(IEnumerable<Claim> claims);
    }
}
