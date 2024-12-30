using Business.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Context
{
    public class ChatDpContext : DbContext
    {
        public ChatDpContext(DbContextOptions<ChatDpContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<ConversationUser> ConversationUsers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // User - ConversationUser Relationship
            builder.Entity<User>()
                .HasMany(u => u.ConversationUsers)
                .WithOne(cu => cu.User)
                .HasForeignKey(cu => cu.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            // User - Notifications Relationship
            builder.Entity<User>()
                .HasMany(u => u.Notifications)
                .WithOne(n => n.User)
                .HasForeignKey(n => n.UserID)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete to avoid cycles

            // Conversation - ConversationUser Relationship
            builder.Entity<Conversation>()
                .HasMany(c => c.ConversationUsers)
                .WithOne(cu => cu.Conversation)
                .HasForeignKey(cu => cu.ConversationID)
                .OnDelete(DeleteBehavior.Cascade);

            // Conversation - Message Relationship
            builder.Entity<Conversation>()
                .HasMany(c => c.Messages)
                .WithOne(m => m.Conversation)
                .HasForeignKey(m => m.ConversationID)
                .OnDelete(DeleteBehavior.Cascade);

            // Message - Media Relationship
            builder.Entity<Message>()
                .HasMany(m => m.Media)
                .WithOne(md => md.Message)
                .HasForeignKey(md => md.MessageID)
                .OnDelete(DeleteBehavior.Cascade);

            // Notification - Message Relationship
            builder.Entity<Notification>()
                .HasOne(n => n.Message)
                .WithMany()
                .HasForeignKey(n => n.MessageID)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete to avoid cycles

            // LastMessageID Relationship in Conversation
            builder.Entity<Conversation>()
                .HasOne(c => c.LastMessage)
                .WithMany()
                .HasForeignKey(c => c.LastMessageID)
                .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete if needed
        }

    }
}
