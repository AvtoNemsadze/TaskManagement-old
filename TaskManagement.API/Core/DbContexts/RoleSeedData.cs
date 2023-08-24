using TaskManagement.API.Core.Entities;
using TaskManagement.API.Core.Enums;
using TaskManagement.API.Core.OtherObjects;

namespace TaskManagement.API.Core.DbContexts
{
    public static class RoleSeedData
    {
        public static readonly RoleEntity[] Roles =
        {
            // basic roles
            new RoleEntity { RoleName = UserRoles.Tester.ToString(), Description = "Tester Role" },
            new RoleEntity { RoleName = UserRoles.Developer.ToString(), Description = "Developer Role" },
            new RoleEntity { RoleName = UserRoles.TeamLead.ToString(), Description = "TeamLead Role" },
            new RoleEntity { RoleName = UserRoles.Designer.ToString(), Description = "Designer Role" },
            new RoleEntity { RoleName = UserRoles.ProductManager.ToString(), Description = "ProductManager Role" },

            // System roles
            new RoleEntity { RoleName = SystemRoles.ADMIN, Description = "Administrator Role" },
            new RoleEntity { RoleName = SystemRoles.SUPERADMIN, Description = "Super Administrator Role" }
        };
    }

}
