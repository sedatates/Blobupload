using Microsoft.EntityFrameworkCore;
using fileupload.Models;
using File = fileupload.Models.File;

namespace fileupload.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<File> Files { get; set; }

    // Diğer DbSet'ler burada tanımlanabilir...

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Veritabanı modeli ve ilişkileri tanımlanabilir...
        
    }
}