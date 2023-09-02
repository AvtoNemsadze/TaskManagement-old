using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.Core.Dtos
{
    public class UpdateUserDto
    {
        public string? FirstName { get; set; } 
        public string? LastName { get; set; }
        public string? UserName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
        public int RoleId { get; set; } 
    }
}
