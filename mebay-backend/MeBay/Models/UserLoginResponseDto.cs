namespace MeBay.Models
{
    public class UserLoginResponseDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Role { get; set; }
        public string? Token { get; set; }

        public int? Credits { get; set; }
    }
}
