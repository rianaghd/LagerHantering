namespace ApiMonday.DTOs.Item;

public class CreateItemDto
{
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public DateTime ExpiryDate { get; set; }
    public int CategoryId { get; set; }
    public int UserId { get; set; }
}