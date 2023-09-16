using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskManagement.API.Core.Enums;

namespace TaskManagement.API.Core.Entities
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [EnumDataType(typeof(UserRoles))]
        public string RoleName { get; set; }
        public string? Description { get; set; }

        // relation to RoleEntity
        public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();
    }
}
