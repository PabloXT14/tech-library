using Microsoft.EntityFrameworkCore;
using TechLibrary.Api.Domain.Entities;

namespace TechLibrary.Api.Infrastructure.DataAccess;

public class TechLibraryDbContext : DbContext
{
    public DbSet<User> Users { get; set; } // Obs: o nome do campo deve ser igual ao da tabela no banco de dados
    public  DbSet<Book> Books { get; set; }
    
    public DbSet<Checkout> Checkouts { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=/home/pabloalan/Documents/Studies/19_csharp/TechLibrary/TechLibraryDb.db");
    }
}