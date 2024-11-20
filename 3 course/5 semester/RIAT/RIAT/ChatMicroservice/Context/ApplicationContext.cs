using System;
using System.Collections.Generic;
using ChatMicroservice.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatMicroservice.Context;

public class ApplicationContext : DbContext
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

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<ChatMember> ChatMembers { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Chat>(entity =>
        {
            entity.HasKey(e => e.ChatId).HasName("Chat_pkey");

            entity.ToTable("Chat");

            entity.Property(e => e.ChatId)
                .ValueGeneratedNever()
                .HasColumnName("chat_id");
            entity.Property(e => e.ChatName)
                .HasMaxLength(255)
                .HasColumnName("chat_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(0) with time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.IsGroup)
                .HasDefaultValue(false)
                .HasColumnName("is_group");
        });

        modelBuilder.Entity<ChatMember>(entity =>
        {
            entity.HasKey(e => e.ChatMemberId).HasName("ChatMember_pkey");

            entity.ToTable("ChatMember");

            entity.Property(e => e.ChatMemberId)
                .ValueGeneratedNever()
                .HasColumnName("chat_member_id");
            entity.Property(e => e.ChatId).HasColumnName("chat_id");
            entity.Property(e => e.JoinedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(0) with time zone")
                .HasColumnName("joined_at");
            entity.Property(e => e.NotificationsEnabled)
                .HasDefaultValue(true)
                .HasColumnName("notifications_enabled");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Chat).WithMany(p => p.ChatMembers)
                .HasForeignKey(d => d.ChatId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("chatmember_chat_id_foreign");
        });

    }
}
