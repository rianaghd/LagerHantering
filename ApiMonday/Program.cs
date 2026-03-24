using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using ApiMonday.Data;
using ApiMonday.Data.Seeder;
using ApiMonday.Mapping;
using ApiMonday.Services;
using ApiMonday.Services.Interfaces;
using ApiMonday.Validators;

var builder = WebApplication.CreateBuilder(args);

// Databas
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Services (kopplar interface till implementation)
builder.Services.AddScoped<IItemService,     ItemService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserService,     UserService>();

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateItemValidator>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Kör migration + seeder automatiskt vid start
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    await BogusSeeder.SeedAsync(db);
}

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.Run();