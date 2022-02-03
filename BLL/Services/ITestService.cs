using System.Collections.Generic;
using System.Threading.Tasks;
using DLL.Model;
using Microsoft.AspNetCore.Identity;

namespace BLL.Services
{
    public interface ITestService
    {
        Task AddNewRoles();
        Task AddNewUser();
    }

    public class TestService : ITestService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public TestService(RoleManager<AppRole> roleManager,
            UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
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
                new AppUser()
                {
                    UserName = "Tapos",
                    Email = "Tapos.aa@gmail.com"
                },
                new AppUser()
                {
                    UserName = "Sanjib",
                    Email = "Sanjib.aa@gmail.com"
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
                        await _userManager.AddToRoleAsync(user, "admin");
                    }
                }
            }
        }
    }
}