using System;
using System.Collections.Generic;
using DbManager.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using File = DbManager.Models.File;
namespace DbManager;

public partial class SpellTestDbContext : DbContext
{
    IConfiguration _configuration = null!;
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

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WordsToLearn> WordsToLearns { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        ConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        _configuration = configurationBuilder.Build();
        optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
    }
        

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DifficultyLevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Difficul__3214EC07BB4B78A0");

            entity.ToTable("Difficulty_Level");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<File>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Files__3214EC0720225BD0");

            entity.Property(e => e.EntityType).HasMaxLength(50);
            entity.Property(e => e.UploadedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime");
        });

        modelBuilder.Entity<Friend>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Friends__3214EC079A2A8020");

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

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3214EC07D48F812E");

            entity.Property(e => e.CorrectVariant)
                .HasMaxLength(1024)
                .HasColumnName("Correct_Variant");
            entity.Property(e => e.Expression).HasMaxLength(1024);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC070A3F4FB3");

            entity.HasIndex(e => e.Number, "UQ__Users__78A1A19D82189C43").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D10534A4E6F381").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("smalldatetime")
                .HasColumnName("Created_At");
            entity.Property(e => e.Email).HasMaxLength(254);
            entity.Property(e => e.Number).HasMaxLength(25);
            entity.Property(e => e.Password).HasMaxLength(256);
            entity.Property(e => e.Username).HasMaxLength(32);
        });

        modelBuilder.Entity<WordsToLearn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Words_To__3214EC0717916050");

            entity.ToTable("Words_To_Learn");

            entity.Property(e => e.Expression).HasMaxLength(256);
            entity.Property(e => e.LearningProgress).HasColumnName("Learning_Progress");
            entity.Property(e => e.Meaning).HasMaxLength(256);
            entity.Property(e => e.UserId).HasColumnName("User_Id");

            entity.HasOne(d => d.DifficultyNavigation).WithMany(p => p.WordsToLearns)
                .HasForeignKey(d => d.Difficulty)
                .HasConstraintName("FK_Difficulty_Difficulty_Level");

            entity.HasOne(d => d.User).WithMany(p => p.WordsToLearns)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Words_To_Learn_User");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
