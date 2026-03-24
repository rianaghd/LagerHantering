using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ApiMonday.Data;
using ApiMonday.DTOs.Category;
using ApiMonday.Exceptions;
using ApiMonday.Models;
using ApiMonday.Services.Interfaces;

namespace ApiMonday.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public CategoryService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        var cats = await _db.Categories.Include(c => c.Items).ToListAsync();
        return _mapper.Map<IEnumerable<CategoryDto>>(cats);
    }

    public async Task<CategoryDto> GetByIdAsync(int id)
    {
        var cat = await _db.Categories.Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new NotFoundException($"Kategori med id {id} hittades inte.");
        return _mapper.Map<CategoryDto>(cat);
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryDto dto)
    {
        var cat = _mapper.Map<Category>(dto);
        _db.Categories.Add(cat);
        await _db.SaveChangesAsync();
        return await GetByIdAsync(cat.Id);
    }

    public async Task<CategoryDto> UpdateAsync(int id, UpdateCategoryDto dto)
    {
        var cat = await _db.Categories.FindAsync(id)
            ?? throw new NotFoundException($"Kategori med id {id} hittades inte.");
        _mapper.Map(dto, cat);
        await _db.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task DeleteAsync(int id)
    {
        var cat = await _db.Categories.FindAsync(id)
            ?? throw new NotFoundException($"Kategori med id {id} hittades inte.");
        _db.Categories.Remove(cat);
        await _db.SaveChangesAsync();
    }
}