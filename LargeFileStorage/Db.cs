using LargeFileStorage.Models;
using Microsoft.EntityFrameworkCore;

namespace LargeFileStorage;

public class Db: DbContext
{
    public DbSet<Image> Images => Set<Image>();
    public DbSet<ContentTypes> ContentTypes => Set<ContentTypes>();

    
    
    public Db(DbContextOptions<Db> options) 
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Image>()
            .HasOne(i => i.ContentType)
            .WithMany(ct => ct.Images)
            .HasForeignKey(i => i.ContentTypeId);
    }
}
