using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;
using System.Text.Json.Serialization;
using TaskManagement.API.Core.Enums;

namespace TaskManagement.API.Core.Entities
{
    [Index(nameof(Email), IsUnique = true)]
    public class ApplicationUser
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

        public DateTime? CreatedDate { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }
        public int RoleId { get; set; }

        [ForeignKey("TeamId")]
        public Team? Team { get; set; }
        public int? TeamId { get; set; }

        [InverseProperty("CreatedByUser")]
        public ICollection<TaskEntity> CreatedTasks { get; set; }

        [JsonIgnore]
        public ICollection<TaskEntity>? Tasks { get; set; }

        [JsonIgnore]
        public ICollection<Comment>? Comments { get; set; } = new List<Comment>();
    }
}

// one - to - many relationship between RoleEntity and UserEntity.
// Each role can be associated with multiple users and each user can have only one role.