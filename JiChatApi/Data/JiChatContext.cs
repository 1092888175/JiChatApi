using JiChatApi.Entity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace JiChatApi.Data
{
    public class JiChatContext:IdentityUserContext<JiChatUser,long>
    {
        public JiChatContext() { }
        public JiChatContext(DbContextOptions<JiChatContext> options) : base(options) { }
        public DbSet<JiChatUser> JiChatUser { get; set; }
        public DbSet<JiChatUserDetail> JiChatUserDetail { get; set; }
        public DbSet<JiChatGroup> JiChatGroup { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<JiChatUser>()
                .HasOne(e => e.UserDetail)
                .WithOne()
                .HasForeignKey<JiChatUserDetail>(e => e.UserId)
                .IsRequired();
            modelBuilder.Entity<JiChatUser>()
                .HasMany(e => e.Friends)
                .WithOne()
                .HasForeignKey(e=>e.UserId)
                .IsRequired();
            modelBuilder.Entity<JiChatFriend>()
                .HasOne<JiChatUser>()
                .WithMany()
                .HasForeignKey(e=>e.FriendId)
                .IsRequired();
            modelBuilder.Entity<JiChatUser>()
                .HasMany(e => e.Groups)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired();
            modelBuilder.Entity<JiChatGroup>()
                .HasMany<JiChatGroupInfo>()
                .WithOne()
                .HasForeignKey(e => e.GroupId)
                .IsRequired();
            modelBuilder.Entity<JiChatGroup>()
                .HasMany(e => e.Member)
                .WithMany()
                .UsingEntity<JiChatGroupInfo>();
            modelBuilder.Entity<JiChatGroup>()
                .HasOne(e => e.Owner)
                .WithMany();

        }
    }
}
