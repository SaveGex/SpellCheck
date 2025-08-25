using System;
using System.Collections.Generic;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using File = Infrastructure.Models.File;
namespace Infrastructure;

public partial class SpellTestDbContext : DbContext
{
    private IConfiguration _configuration = null!;
    public SpellTestDbContext()
    {
    }

    public SpellTestDbContext(DbContextOptions<SpellTestDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<DifficultyLevel> DifficultyLevels { get; set; }

    public virtual DbSet<File> Files { get; set; }

    public virtual DbSet<Friend> Friends { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<Role> Roles{ get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Word> Words { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddJsonFile("Configuration\\appsettings.json", optional: false, reloadOnChange: true);
        _configuration = configurationBuilder.Build();
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DifficultyLevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Difficul__3214EC071CD4BFD4");

            entity.ToTable("Difficulty_Level");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<File>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Files__3214EC0755E9AEE8");

            entity.Property(e => e.EntityType).HasMaxLength(50);
            entity.Property(e => e.UploadedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Friend>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Friends__3214EC0787ADD9A2");

            entity.HasIndex(e => new { e.FromIndividualId, e.ToIndividualId }, "UQ_from_individual_to_individual_ids").IsUnique();

            entity.Property(e => e.FromIndividualId).HasColumnName("from_individual_id");
            entity.Property(e => e.ToIndividualId).HasColumnName("to_individual_id");

            entity.HasOne(d => d.FromIndividual).WithMany(p => p.FriendFromIndividuals)
                .HasForeignKey(d => d.FromIndividualId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_from_individual_Friend_id_Users");

            entity.HasOne(d => d.ToIndividual).WithMany(p => p.FriendToIndividuals)
                .HasForeignKey(d => d.ToIndividualId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_to_individual_Friend_id_Users");
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Modules__3214EC079E5E094A");

            entity.HasIndex(e => e.IdentifierName, "IX_Modules_Identifier_Name_NotNull")
                .IsUnique()
                .HasFilter("([Identifier_Name] IS NOT NULL)");

            entity.Property(e => e.AuthorId).HasColumnName("User_Id");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime")
                .HasColumnName("Created_At");

            entity.Property(e => e.Identifier)
                .HasDefaultValueSql("(newid())")
                .IsRequired()
                .HasMaxLength(256);

            entity.Property(e => e.IdentifierName)
                .HasMaxLength(256)
                .HasColumnName("Identifier_Name");

            entity.HasOne(d => d.Author).WithMany(p => p.CreatedModules)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Modules_Users");

            entity.HasIndex(e => e.AuthorId, "IX_Modules_User_Id");

            entity.HasIndex(e => e.Identifier, "IX_Modules_Identifier")
                .IsUnique();
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("Id");

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnName("Name");

            entity.HasIndex(e => e.Name, "IX_Roles_Name")
                .IsUnique()
                .HasDatabaseName("UQ_Roles_Name");
        });

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = RoleNames.Admin },
            new Role { Id = 2, Name = RoleNames.Manager },
            new Role { Id = 3, Name = RoleNames.User }
        );

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07EF14166B");

            entity.HasIndex(e => e.Email, "IX_Users_Email_NotNull")
                .IsUnique()
                .HasFilter("([Email] IS NOT NULL)");

            entity.HasIndex(e => e.Number, "IX_Users_Number_NotNull")
                .IsUnique()
                .HasFilter("([Number] IS NOT NULL)");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime")
                .HasColumnName("Created_At");

            entity.Property(e => e.DeletedAt)
                .HasColumnType("smalldatetime")
                .HasColumnName("Deleted_At");

            entity.HasMany(u => u.Roles)
                .WithMany(r => r.Users);

            entity.HasMany(u => u.UserModules)
                .WithMany(m => m.Users);

            entity.Property(e => e.Email).HasMaxLength(254);
            entity.Property(e => e.Number).HasMaxLength(25);
            entity.Property(e => e.Password).HasMaxLength(256);
            entity.Property(e => e.Username).HasMaxLength(32);
        });

        modelBuilder.Entity<Word>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Words__3214EC07AEB353A1");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime")
                .HasColumnName("Created_At");
            entity.Property(e => e.Expression).HasMaxLength(256);
            entity.Property(e => e.Meaning).HasMaxLength(256);
            entity.Property(e => e.ModuleId).HasColumnName("Module_Id");
            entity.Property(e => e.AuthorId).HasColumnName("Author_Id");

            entity.HasOne(d => d.DifficultyNavigation).WithMany(p => p.Words)
                .HasForeignKey(d => d.DifficultyId)
                .HasConstraintName("FK_Difficulty_Difficulty_Level");

            entity.HasOne(d => d.Module).WithMany(p => p.Words)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Words_Modules");

            entity.HasOne(d => d.User).WithMany(p => p.Words)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Words_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
