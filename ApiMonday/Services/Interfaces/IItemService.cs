using ApiMonday.DTOs.Item;

namespace ApiMonday.Services.Interfaces;

public interface IItemService
{
    Task<IEnumerable<ItemDto>> GetAllAsync();
    Task<ItemDto> GetByIdAsync(int id);
    Task<ItemDto> CreateAsync(CreateItemDto dto);
    Task<ItemDto> UpdateAsync(int id, UpdateItemDto dto);
    Task DeleteAsync(int id);
    Task<IEnumerable<ItemDto>> GetExpiringSoonAsync(int days);
    Task<IEnumerable<ItemDto>> GetByCategoryAsync(int categoryId);
    Task<int> DeleteExpiredAsync();
}