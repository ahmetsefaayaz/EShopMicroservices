using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Models;

namespace Ordering.Application.Data;

public interface IApplicationDbContext
{
    DbSet<Order> Orders { get; set; }
    DbSet<OrderItem> OrderItems { get; set; }
    DbSet<Product> Products { get; set; }
    DbSet<Customer> Customers { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}