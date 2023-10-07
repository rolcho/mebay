using MeBay.Models;

public class ItemDto
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string? Picture { get; set; }
    public int Price { get; set; }
    public int InstantPrice { get; set; }
    public int UserId { get; set; }
    public UserItemDto? User { get; set; }
}
