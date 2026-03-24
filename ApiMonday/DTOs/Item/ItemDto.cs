namespace ApiMonday.DTOs.Item;

public class ItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public bool IsExpired => ExpiryDate < DateTime.UtcNow;
    public bool IsExpiringSoon => ExpiryDate <= DateTime.UtcNow.AddDays(7) && !IsExpired;
}