using Microsoft.EntityFrameworkCore;
using TechLibrary.Api.Domain.Entities;


namespace TechLibrary.Api.Infrastructure;

public class TechLibraryDbContext : DbContext
{
    public DbSet<User> Users { get; set; } // Para mostra a IDE que o DbSet não é nulo

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=/home/pabloalan/Documents/Studies/19_csharp/TechLibrary/TechLibraryDb.db");
    }
}