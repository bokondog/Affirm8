using Microsoft.EntityFrameworkCore;
using KindWordsApi.Models;

namespace KindWordsApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // DbSets for our entities
        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Reply> Replies { get; set; }
        public DbSet<MessageReply> MessageReplies { get; set; }
        public DbSet<ReplyLike> ReplyLikes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure User entity
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.NickName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.PasswordHash).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Configure Message entity
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.Category).IsRequired().HasMaxLength(50);
                
                // Relationship: User -> Messages (one-to-many)
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                // Relationship: Message -> Replies (one-to-many)
                entity.HasMany(e => e.Replies)
                      .WithOne(r => r.Message)
                      .HasForeignKey(r => r.MessageId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure Reply entity
            modelBuilder.Entity<Reply>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired().HasMaxLength(500);
                
                // Relationship: User -> Replies (one-to-many)
                entity.HasOne(e => e.User)
                      .WithMany()
                      .HasForeignKey(e => e.UserId)
                      .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
            });

            // Configure MessageReply junction table
            modelBuilder.Entity<MessageReply>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                // Composite unique constraint to prevent duplicate replies from same user
                entity.HasIndex(e => new { e.MessageId, e.UserId }).IsUnique();
                
                // Relationships
                entity.HasOne(e => e.Message)
                      .WithMany(m => m.MessageReplies)
                      .HasForeignKey(e => e.MessageId)
                      .OnDelete(DeleteBehavior.Cascade);
                      
                                        entity.HasOne(e => e.User)
                              .WithMany()
                              .HasForeignKey(e => e.UserId)
                              .OnDelete(DeleteBehavior.NoAction); // Changed to NoAction to avoid cascade conflicts
                    });

                    // Configure ReplyLike entity
                    modelBuilder.Entity<ReplyLike>(entity =>
                    {
                        entity.HasKey(e => e.Id);
                        
                        // One reply can have multiple likes (though currently only message owner)
                        entity.HasOne(e => e.Reply)
                              .WithMany(r => r.Likes)
                              .HasForeignKey(e => e.ReplyId)
                              .OnDelete(DeleteBehavior.Cascade);

                        // One user (message owner) can like multiple replies
                        entity.HasOne(e => e.MessageOwner)
                              .WithMany()
                              .HasForeignKey(e => e.MessageOwnerId)
                              .OnDelete(DeleteBehavior.NoAction);

                        // Prevent duplicate likes from same user on same reply
                        entity.HasIndex(e => new { e.ReplyId, e.MessageOwnerId }).IsUnique();
                    });
                }
            }
        }
