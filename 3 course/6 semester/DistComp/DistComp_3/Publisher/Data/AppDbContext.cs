using Microsoft.EntityFrameworkCore;
using Publisher.Models;

namespace Publisher.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Story> Stories { get; set; }
    public DbSet<Tag> Tags { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Login)
            .IsUnique();

        modelBuilder.Entity<User>()
            .ToTable("tbl_user");
        
        modelBuilder.Entity<Tag>()
            .ToTable("tbl_tag");
        
        modelBuilder.Entity<Story>()
            .ToTable("tbl_story");
        
        modelBuilder.Entity<Story>()
            .HasIndex(s => s.Title)
            .IsUnique();

        modelBuilder.Entity<Tag>()
            .HasIndex(t => t.Name)
            .IsUnique();
    }
}