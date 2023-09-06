using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Core.Entities;

namespace TaskManagement.API.Core.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<TaskEntity> Tasks { get; set; } 
        public DbSet<ApplicationUser> Users { get; set; } 
        public DbSet<RoleEntity> Roles { get; set; } 
        public DbSet<UserRoleEntity> UserRoles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommentEntity>()
              .HasOne(c => c.User)
              .WithMany(u => u.Comments)
              .HasForeignKey(c => c.UserId)
              .OnDelete(DeleteBehavior.Restrict); // Cascade delete when a user is deleted

            modelBuilder.Entity<CommentEntity>()
                .HasOne(c => c.Task)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Restrict); // Cascade delete when a task is deleted

            base.OnModelCreating(modelBuilder);
        }
    }
}
