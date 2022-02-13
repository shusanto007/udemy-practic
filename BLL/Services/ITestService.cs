using System.Collections.Generic;
using System.Threading.Tasks;
using DLL.Model;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;

namespace BLL.Services
{
    public interface ITestService
    {
        Task AddNewRoles();
        Task AddNewUser();
        Task CreateAndroidAndWebClient();
    }

    public class TestService : ITestService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IOpenIddictApplicationManager _manager;

        public TestService(RoleManager<AppRole> roleManager,
            UserManager<AppUser> userManager,
            IOpenIddictApplicationManager manager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _manager = manager;
        }

        public async Task AddNewRoles()
        {
            var roleList = new List<string>()
            {
                "admin",
                "manager",
                "supervisor"
            };

            foreach (var role in roleList)
            {
                var roleExits = await _roleManager.FindByNameAsync(role);

                if (roleExits == null)
                {
                    await _roleManager.CreateAsync(new AppRole()
                    {
                        Name = role
                    });
                }
            }
        }

        public async Task AddNewUser()
        {
            var userList = new List<AppUser>()
            {
                new ()
                {
                    UserName = "Tapos",
                    Email = "Tapos.aa@gmail.com",
                    FullName = "Biswanath gosh Tapos"
                    
                },
                new ()
                {
                    UserName = "Sanjib",
                    Email = "Sanjib.aa@gmail.com",
                    FullName = "Sanjib dhar"
                },
                new ()
                {
                    UserName = "keenedy",
                    Email = "keenedy.aa@gmail.com",
                    FullName = "keenedy sarker"
                }
            };

            foreach (var user in userList)
            {
                var userExits = await _userManager.FindByEmailAsync(user.Email);

                if (userExits == null)
                {
                    var insertData = await _userManager.CreateAsync(user,"abc123$..A!");

                    if (insertData.Succeeded)
                    {
                        var myRole = user.Email switch
                        {
                            "Tapos.aa@gmail.com" => "admin",
                            "Sanjib.aa@gmail.com" => "manager",
                            "keenedy.aa@gmail.com" => "supervisor",
                            _ => ""
                        };
                        await _userManager.AddToRoleAsync(user, myRole);
                    }
                }
            }
        }

        public async Task CreateAndroidAndWebClient()
        {
            var listOfClient = new List<OpenIddictApplicationDescriptor>()
            {
                new()
                {
                    ClientId = "udemy_android_application",
                    ClientSecret = "udemy123",
                    DisplayName = "our android client",
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.Endpoints.Logout,
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.GrantTypes.Password,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                        OpenIddictConstants.Permissions.Scopes.Email,
                        OpenIddictConstants.Permissions.Scopes.Profile,
                        OpenIddictConstants.Permissions.Scopes.Roles
                    }
                },

                new()
                {
                    ClientId = "udemy_web_application",
                    ClientSecret = "udemy456",
                    DisplayName = "our web client",
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.Endpoints.Logout,
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.GrantTypes.Password,
                        OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                        OpenIddictConstants.Permissions.Scopes.Email,
                        OpenIddictConstants.Permissions.Scopes.Profile,
                        OpenIddictConstants.Permissions.Scopes.Roles
                    }
                },
            };

            foreach (var application in listOfClient)
            {
                if (application.ClientId != null)
                {
                    var applicationExits = await _manager.FindByClientIdAsync(application.ClientId);

                    if (applicationExits == null)
                    {
                        await _manager.CreateAsync(application);
                    }
                }
            }
        }
    }
}