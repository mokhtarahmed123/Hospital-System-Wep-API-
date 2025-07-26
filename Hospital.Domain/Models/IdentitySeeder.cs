using HospitalAPI.Hospital.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace HospitalAPI.Hospital.Domain
{
    public class IdentitySeeder
    {
        public static async Task SeedRolesAsync(RoleManager<RoleApplication> roleManager)
        {
            string[] roles = { "Admin", "Doctor", "Patient", "Accountant" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new RoleApplication { Name = role });
                }
            }
        }
    }
}
