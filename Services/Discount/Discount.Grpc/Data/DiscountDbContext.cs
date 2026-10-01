using Discount.Grpc.Models;
using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Data;

public class DiscountDbContext: DbContext
{
    public DiscountDbContext(DbContextOptions<DiscountDbContext> options) : base(options) {}
    public DbSet<Coupon> Coupons { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Coupon>().HasData(
            new Coupon { Id = 1, ProductName = "Iphone X", Description = "Iphone Description", Amount = 5 },
            new Coupon { Id = 2, ProductName = "Samsung 10", Description = "Samsung Description", Amount = 5 }
            );
    }
}