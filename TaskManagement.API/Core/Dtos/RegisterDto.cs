using System.ComponentModel.DataAnnotations;
using TaskManagement.API.Core.Entities;

namespace TaskManagement.API.Core.Dtos
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "FirstName is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [EnumDataType(typeof(UserRole))]
        public string UserRole { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
