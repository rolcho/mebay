public class ItemSellDto
{
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string? Picture { get; set; }
    public int Price { get; set; }
    public int InstantPrice { get; set; }
    public DateTime? SellingDate { get; set; }
}
