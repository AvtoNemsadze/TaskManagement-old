using System.ComponentModel.DataAnnotations;

namespace TaskManagement.API.Core.Dtos
{
    public class UserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Id { get; set; }
        //public List<string> Roles { get; set; }
    }

}
