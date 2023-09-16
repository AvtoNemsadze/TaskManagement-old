using TaskManagement.API.Core.Entities;
using TaskManagement.API.Core.Enums;
using TaskManagement.API.Core.OtherObjects;

namespace TaskManagement.API.Core.DataAccess
{
    public static class RoleSeedData
    {
        public static readonly Role[] Roles =
        {
            // basic roles
            new Role { RoleName = UserRoles.Tester.ToString(), Description = "Tester Role" },
            new Role { RoleName = UserRoles.Developer.ToString(), Description = "Developer Role" },
            new Role { RoleName = UserRoles.TeamLead.ToString(), Description = "TeamLead Role" },
            new Role { RoleName = UserRoles.Designer.ToString(), Description = "Designer Role" },
            new Role { RoleName = UserRoles.ProductManager.ToString(), Description = "ProductManager Role" },

            // System roles
            new Role { RoleName = SystemRoles.ADMIN },
            new Role { RoleName = SystemRoles.SUPERADMIN }
        };
    }
}
