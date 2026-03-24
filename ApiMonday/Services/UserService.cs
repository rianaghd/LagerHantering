using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ApiMonday.Data;
using ApiMonday.DTOs.User;
using ApiMonday.Exceptions;
using ApiMonday.Models;
using ApiMonday.Services.Interfaces;

namespace ApiMonday.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _db;
    private readonly IMapper _mapper;

    public UserService(AppDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _db.Users.Include(u => u.Items).ToListAsync();
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }

    public async Task<UserDto> GetByIdAsync(int id)
    {
        var user = await _db.Users.Include(u => u.Items)
            .FirstOrDefaultAsync(u => u.Id == id)
            ?? throw new NotFoundException($"Användare med id {id} hittades inte.");
        return _mapper.Map<UserDto>(user);
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        var user = _mapper.Map<User>(dto);
        _db.Users.Add(user);
        await _db.SaveChangesAsync();
        return await GetByIdAsync(user.Id);
    }

    public async Task<UserDto> UpdateAsync(int id, UpdateUserDto dto)
    {
        var user = await _db.Users.FindAsync(id)
            ?? throw new NotFoundException($"Användare med id {id} hittades inte.");
        _mapper.Map(dto, user);
        await _db.SaveChangesAsync();
        return await GetByIdAsync(id);
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _db.Users.FindAsync(id)
            ?? throw new NotFoundException($"Användare med id {id} hittades inte.");
        _db.Users.Remove(user);
        await _db.SaveChangesAsync();
    }
}