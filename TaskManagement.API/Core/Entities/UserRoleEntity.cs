using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.Core.Entities
{
    public class UserRoleEntity
    {
        [Key]
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public string UserName { get; set; }
    }
}
