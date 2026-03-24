using AutoMapper;
using ApiMonday.DTOs.Category;
using ApiMonday.DTOs.Item;
using ApiMonday.DTOs.User;
using ApiMonday.Models;

namespace ApiMonday.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Item
        CreateMap<Item, ItemDto>()
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category.Name))
            .ForMember(d => d.Username,     o => o.MapFrom(s => s.User.Username));
        CreateMap<CreateItemDto, Item>();
        CreateMap<UpdateItemDto, Item>()
            .ForAllMembers(o => o.Condition((src, dest, val) => val != null));

        // Category
        CreateMap<Category, CategoryDto>()
            .ForMember(d => d.ItemCount, o => o.MapFrom(s => s.Items.Count));
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdateCategoryDto, Category>()
            .ForAllMembers(o => o.Condition((src, dest, val) => val != null));

        // User
        CreateMap<User, UserDto>()
            .ForMember(d => d.ItemCount, o => o.MapFrom(s => s.Items.Count));
        CreateMap<CreateUserDto, User>();
        CreateMap<UpdateUserDto, User>()
            .ForAllMembers(o => o.Condition((src, dest, val) => val != null));
    }
}