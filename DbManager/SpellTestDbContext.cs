using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DbManager.Models;

public partial class SpellTestDbContext : DbContext
{
    private readonly IConfiguration _configuration;

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<QuestionImage> QuestionImages { get; set; }

    public virtual DbSet<User> Users { get; set; }


    public SpellTestDbContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public SpellTestDbContext(DbContextOptions<SpellTestDbContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = _configuration.GetConnectionString("DefaultConnection"); 
        optionsBuilder.UseSqlServer(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Modules__3214EC076248246B");

            entity.HasIndex(e => e.UserId, "IX_Modules_User_Id");

            entity.Property(e => e.Description).HasMaxLength(512);
            entity.Property(e => e.Name).HasMaxLength(512);
            entity.Property(e => e.UserId).HasColumnName("User_Id");

            entity.HasOne(d => d.User).WithMany(p => p.Modules)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user_Modules");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3214EC07A7EA3024");

            entity.HasIndex(e => e.ModuleId, "IX_Questions_Module_Id");

            entity.Property(e => e.Answer).HasMaxLength(512);
            entity.Property(e => e.ModuleId).HasColumnName("Module_Id");
            entity.Property(e => e.Question1)
                .HasMaxLength(512)
                .HasColumnName("Question");

            entity.HasOne(d => d.Module).WithMany(p => p.Questions)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Modul_Questions");
        });

        modelBuilder.Entity<QuestionImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3214EC07C7C715FB");

            entity.ToTable("Question_Images");

            entity.HasIndex(e => e.QuestionId, "IX_QuestionImages_Question_Id");

            entity.Property(e => e.AnswerImage)
                .HasMaxLength(1024)
                .IsUnicode(false)
                .HasColumnName("Answer_Image");
            entity.Property(e => e.QuestionId).HasColumnName("Question_Id");
            entity.Property(e => e.QuestionImage1)
                .HasMaxLength(1024)
                .IsUnicode(false)
                .HasColumnName("Question_Image");

            entity.HasOne(d => d.Question).WithMany(p => p.QuestionImages)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Question_Images");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC0725CF1A1F");

            entity.HasIndex(e => e.Number, "UQ__Users__78A1A19DD6ED7FF4").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D1053413E89444").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(254);
            entity.Property(e => e.Number).HasMaxLength(25);
            entity.Property(e => e.Password).HasMaxLength(256);
            entity.Property(e => e.Username).HasMaxLength(32);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
