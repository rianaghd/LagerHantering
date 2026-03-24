using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ApiMonday.Data;
using ApiMonday.DTOs.Item;
using ApiMonday.Exceptions;
using ApiMonday.Models;
using ApiMonday.Services.Interfaces;

namespace ApiMonday.Services;

public class ItemService : IItemService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public ItemService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    private IQueryable<Item> ItemsWithRelations() =>
        _db.Items.Include(i => i.Category).Include(i => i.User);

    public async Task<IEnumerable<ItemDto>> GetAllAsync()
    {
        var items = await ItemsWithRelations().ToListAsync();
        return _mapper.Map<IEnumerable<ItemDto>>(items);
    }

    public async Task<ItemDto> GetByIdAsync(int id)
    {
        var item = await ItemsWithRelations().FirstOrDefaultAsync(i => i.Id == id)
            ?? throw new NotFoundException($"Item med id {id} hittades inte.");
        return _mapper.Map<ItemDto>(item);
    }

    public async Task<ItemDto> CreateAsync(CreateItemDto dto)
    {
        var item = _mapper.Map<Item>(dto);
        _db.Items.Add(item);
        await _db.SaveChangesAsync();
        return await GetByIdAsync(item.Id);
    }

    public async Task<ItemDto> UpdateAsync(int id, UpdateItemDto dto)
    {
        var item = await _db.Items.FindAsync(id)
            ?? throw new NotFoundException($"Item med id {id} hittades inte.");
        _mapper.Map(dto, item);
        await _db.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task DeleteAsync(int id)
    {
        var item = await _db.Items.FindAsync(id)
            ?? throw new NotFoundException($"Item med id {id} hittades inte.");
        _db.Items.Remove(item);
        await _db.SaveChangesAsync();
    }

    public async Task<IEnumerable<ItemDto>> GetExpiringSoonAsync(int days)
    {
        var cutoff = DateTime.UtcNow.AddDays(days);
        var items = await ItemsWithRelations()
            .Where(i => i.ExpiryDate >= DateTime.UtcNow && i.ExpiryDate <= cutoff)
            .OrderBy(i => i.ExpiryDate)
            .ToListAsync();
        return _mapper.Map<IEnumerable<ItemDto>>(items);
    }

    public async Task<IEnumerable<ItemDto>> GetByCategoryAsync(int categoryId)
    {
        var items = await ItemsWithRelations()
            .Where(i => i.CategoryId == categoryId)
            .ToListAsync();
        return _mapper.Map<IEnumerable<ItemDto>>(items);
    }

    public async Task<int> DeleteExpiredAsync()
    {
        var expired = await _db.Items
            .Where(i => i.ExpiryDate < DateTime.UtcNow)
            .ToListAsync();
        _db.Items.RemoveRange(expired);
        await _db.SaveChangesAsync();
        return expired.Count;
    }
}