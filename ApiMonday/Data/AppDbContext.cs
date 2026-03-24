using Microsoft.EntityFrameworkCore;
using ApiMonday.Models;

namespace ApiMonday.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Item>     Items      => Set<Item>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<User>     Users      => Set<User>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        // Item -> Category (radera inte kategorin om items finns)
        mb.Entity<Item>()
            .HasOne(i => i.Category)
            .WithMany(c => c.Items)
            .HasForeignKey(i => i.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Item -> User (radera items om användaren raderas)
        mb.Entity<Item>()
            .HasOne(i => i.User)
            .WithMany(u => u.Items)
            .HasForeignKey(i => i.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}