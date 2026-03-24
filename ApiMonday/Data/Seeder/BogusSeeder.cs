using Bogus;
using ApiMonday.Models;

namespace InventoryAPI.Data.Seeder;

public static class BogusSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (db.Users.Any()) return; // Hoppa över om data redan finns

        // Kategorier
        var categories = new List<Category>
        {
            new() { Name = "Mat",     Description = "Livsmedel och dryck" },
            new() { Name = "Medicin", Description = "Läkemedel och kosttillskott" },
            new() { Name = "Verktyg", Description = "Underhållsmaterial" },
            new() { Name = "Hygien",  Description = "Hygien- och skönhetsprodukter" }
        };
        db.Categories.AddRange(categories);
        await db.SaveChangesAsync();

        // Användare
        var userFaker = new Faker<User>("sv")
            .RuleFor(u => u.Username,  f => f.Internet.UserName())
            .RuleFor(u => u.Email,     f => f.Internet.Email())
            .RuleFor(u => u.CreatedAt, f => f.Date.Past(1));

        var users = userFaker.Generate(5);
        db.Users.AddRange(users);
        await db.SaveChangesAsync();

        // Items (mix av utgångna, snart utgångna, och ok)
        var itemFaker = new Faker<Item>("sv")
            .RuleFor(i => i.Name,       f => f.Commerce.ProductName())
            .RuleFor(i => i.Quantity,   f => f.Random.Int(1, 100))
            .RuleFor(i => i.ExpiryDate, f => f.Date.Between(
                DateTime.UtcNow.AddDays(-10),  // några redan utgångna
                DateTime.UtcNow.AddDays(30)))   // resten inom 30 dagar
            .RuleFor(i => i.CategoryId, f => f.PickRandom(categories).Id)
            .RuleFor(i => i.UserId,     f => f.PickRandom(users).Id);

        db.Items.AddRange(itemFaker.Generate(30));
        await db.SaveChangesAsync();
    }
}