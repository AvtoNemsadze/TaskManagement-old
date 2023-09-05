using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;
using System.Text.Json.Serialization;
using TaskManagement.API.Core.Enums;

namespace TaskManagement.API.Core.Entities
{
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
        public RoleEntity Role { get; set; }
        public int RoleId { get; set; }

        [JsonIgnore]
        public ICollection<TaskEntity>? Tasks { get; set; }
    }
}

// one - to - many relationship between RoleEntity and UserEntity.
// Each role can be associated with multiple users and each user can have only one role.