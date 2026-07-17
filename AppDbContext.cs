using Microsoft.EntityFrameworkCore;
using StationeryAPI.Models;

namespace StationeryAPI;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Product> Products => Set<Product>();
}