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
        //public DbSet<RefreshToken> RefreshTokens { get; set; }
    }
}
