using System;
using System.Collections.Generic;
using MessageMicroservice.Models;
using Microsoft.EntityFrameworkCore;

namespace MessageMicroservice.Context;

public partial class ApplicationContext : DbContext
{
    public ApplicationContext()
    {
        /*Database.EnsureDeleted();
        Database.EnsureCreated();*/
    }

    public ApplicationContext(DbContextOptions<ApplicationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Message> Messages { get; set; }

    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.MessageId).HasName("Message_pkey");

            entity.ToTable("Message");

            entity.HasIndex(e => new { e.ChatId, e.SenderId }, "unique_chat_sender").IsUnique();

            entity.Property(e => e.MessageId)
                .ValueGeneratedNever()
                .HasColumnName("message_id");
            entity.Property(e => e.ChatId).HasColumnName("chat_id");
            entity.Property(e => e.Context).HasColumnName("context");
            entity.Property(e => e.CreatedAt)
                .HasPrecision(0)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("created_at");
            entity.Property(e => e.EditedAt)
                .HasPrecision(0)
                .HasColumnName("edited_at");
            entity.Property(e => e.IsDeleted)
                .HasDefaultValue(false)
                .HasColumnName("is_deleted");
            entity.Property(e => e.IsEdited)
                .HasDefaultValue(false)
                .HasColumnName("is_edited");
            entity.Property(e => e.SenderId).HasColumnName("sender_id");
        });

    }
}
