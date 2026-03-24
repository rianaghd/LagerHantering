namespace ApiMonday.DTOs.Item;

public class UpdateItemDto
{
    public string? Name { get; set; }
    public int? Quantity { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public int? CategoryId { get; set; }
}