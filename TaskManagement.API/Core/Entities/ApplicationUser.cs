using Microsoft.AspNetCore.Identity;
using TaskManagement.API.Core.Dtos;

namespace TaskManagement.API.Core.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        //internal object Select(Func<object, UserDto> value)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
