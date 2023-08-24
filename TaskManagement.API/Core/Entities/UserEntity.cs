using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.Core.Entities
{
    public class UserEntity
    {
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

        [Required]
        [EnumDataType(typeof(UserRole))]
        public string Role { get; set; }

        [Required]
        public string Id { get; set; } 

        [Required]
        public string PasswordHash { get; set; }

        public string? PhoneNumber { get; set; }
        public DateTime? CreatedDate { get; set; } 
    }
}
