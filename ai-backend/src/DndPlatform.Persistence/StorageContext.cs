#nullable enable
using System;
using System.Collections.Generic;
using DndPlatform.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace DndPlatform.Persistence;

public partial class StorageContext : DbContext
{
    public StorageContext(DbContextOptions<StorageContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Character> Characters { get; set; }

    public virtual DbSet<CharacterTemplate> CharacterTemplates { get; set; }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<FavoriteTemplate> FavoriteTemplates { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GameStatus> GameStatuses { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Master> Masters { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<SenderType> SenderTypes { get; set; }

    public virtual DbSet<Template> Templates { get; set; }

    public virtual DbSet<TemplatesGenre> TemplatesGenres { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UsersGame> UsersGames { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Character>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Characters_pkey");

            entity.HasIndex(e => new { e.UserId, e.GameId }, "IX_Characters_UserId_GameId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Game).WithMany(p => p.Characters)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Characters_GameId_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Characters)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Characters_UserId_fkey");
        });

        modelBuilder.Entity<CharacterTemplate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("CharacterTemplates_pkey");

            entity.HasIndex(e => e.GenderId, "IX_CharacterTemplates_GenderId");

            entity.HasIndex(e => e.UserId, "IX_CharacterTemplates_UserId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Gender).WithMany(p => p.CharacterTemplates)
                .HasForeignKey(d => d.GenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CharacterTemplates_GenderId_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.CharacterTemplates)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("CharacterTemplates_UserId_fkey");
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Chats_pkey");

            entity.HasIndex(e => e.GameId, "IX_Chats_GameId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Game).WithMany(p => p.Chats)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Chats_GameId_fkey");
        });

        modelBuilder.Entity<FavoriteTemplate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("FavoriteTemplates_pkey");

            entity.HasIndex(e => new { e.UserId, e.TemplateId }, "IX_FavoriteTemplates_UserId_TemplateId").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Template).WithMany(p => p.FavoriteTemplates)
                .HasForeignKey(d => d.TemplateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FavoriteTemplates_TemplateId_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.FavoriteTemplates)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FavoriteTemplates_UserId_fkey");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Games_pkey");

            entity.HasIndex(e => e.GameStatusId, "IX_Games_GameStatusId");

            entity.HasIndex(e => e.MasterId, "IX_Games_MasterId");

            entity.HasIndex(e => e.TemplateId, "IX_Games_TemplateId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.GameStatus).WithMany(p => p.Games)
                .HasForeignKey(d => d.GameStatusId)
                .HasConstraintName("Games_GameStatusId_fkey");

            entity.HasOne(d => d.Master).WithMany(p => p.Games)
                .HasForeignKey(d => d.MasterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Games_MasterId_fkey");

            entity.HasOne(d => d.Template).WithMany(p => p.Games)
                .HasForeignKey(d => d.TemplateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Games_TemplateId_fkey");
        });

        modelBuilder.Entity<GameStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("GameStatuses_pkey");

            entity.HasIndex(e => e.Code, "GameStatuses_Code_key").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Genders_pkey");

            entity.HasIndex(e => e.Code, "Genders_Code_key").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Genres_pkey");

            entity.HasIndex(e => e.Code, "Genres_Code_key").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Master>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Masters_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Messages_pkey");

            entity.HasIndex(e => e.ChatId, "IX_Messages_ChatId");

            entity.HasIndex(e => e.SenderTypeId, "IX_Messages_SenderTypeId");

            entity.HasIndex(e => e.UserId, "IX_Messages_UserId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Chat).WithMany(p => p.Messages)
                .HasForeignKey(d => d.ChatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Messages_ChatId_fkey");

            entity.HasOne(d => d.SenderType).WithMany(p => p.Messages)
                .HasForeignKey(d => d.SenderTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Messages_SenderTypeId_fkey");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("News_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<SenderType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SenderTypes_pkey");

            entity.HasIndex(e => e.Code, "SenderTypes_Code_key").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Template>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Templates_pkey");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<TemplatesGenre>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("TemplatesGenres_pkey");

            entity.HasIndex(e => e.GenreId, "IX_GameGenres_GenreId");

            entity.HasIndex(e => e.TemplateId, "IX_GameGenres_TemplateId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Genre).WithMany(p => p.TemplatesGenres)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TemplatesGenres_GenreId_fkey");

            entity.HasOne(d => d.Template).WithMany(p => p.TemplatesGenres)
                .HasForeignKey(d => d.TemplateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("TemplatesGenres_TemplateId_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Users_pkey");

            entity.HasIndex(e => e.Email, "Users_Email_key").IsUnique();

            entity.HasIndex(e => e.Login, "Users_Login_key").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<UsersGame>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("UsersGames_pkey");

            entity.HasIndex(e => e.GameId, "IX_UsersGames_GameId");

            entity.HasIndex(e => e.UserId, "IX_UsersGames_UserId");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Game).WithMany(p => p.UsersGames)
                .HasForeignKey(d => d.GameId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UsersGames_GameId_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.UsersGames)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UsersGames_UserId_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
