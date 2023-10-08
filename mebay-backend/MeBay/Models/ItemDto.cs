using MeBay.Models;

public class ItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string? Picture { get; set; }
    public int Price { get; set; }
    public int InstantPrice { get; set; }
    public int SellerId { get; set; }
    public UserItemDto? Seller { get; set; }
    public int BuyerId { get; set; }
    public UserItemDto? Buyer { get; set; }
}
