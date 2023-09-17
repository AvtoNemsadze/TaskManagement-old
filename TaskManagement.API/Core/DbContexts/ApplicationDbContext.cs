using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Core.Entities;

namespace TaskManagement.API.Core.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<TaskEntity> Tasks { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Team> Teams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
              .HasOne(c => c.User)
              .WithMany(u => u.Comments)
              .HasForeignKey(c => c.UserId)
              .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Task)
                .WithMany(t => t.Comments)
                .HasForeignKey(c => c.TaskId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Team>()
                .HasMany(t => t.Members)
                .WithOne(u => u.Team)
                .HasForeignKey(u => u.TeamId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure the one-to-many relationship between ApplicationUser and TaskEntity
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.CreatedTasks)
                .WithOne(t => t.CreatedByUser)
                .HasForeignKey(t => t.CreatedByUserId);

            base.OnModelCreating(modelBuilder);
        }

    }
}
