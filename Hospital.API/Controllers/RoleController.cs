using HospitalAPI.Hospital.Application.DTO.Account;
using HospitalAPI.Hospital.Domain.Models;
using HospitalAPI.Hospital.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace HospitalAPI.Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly HospitalContex contex;
        private readonly UserManager<UserApplication> userManager;
        private readonly IConfiguration Cofing;
        private readonly RoleManager<RoleApplication> roleManager;

        public RoleController(HospitalContex contex,
            UserManager<UserApplication> userManager,
            IConfiguration configuration,
            RoleManager<RoleApplication> roleManager)
        {

            this.contex = contex;
            this.userManager = userManager;
            Cofing = configuration;
            this.roleManager = roleManager;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Add Role")]
        public async Task<IActionResult> AddRole([FromBody] AddRole roleName)
        {
            var isfound = await roleManager.FindByNameAsync(roleName.RoleName);
            if (isfound is not null) return BadRequest($"{roleName.RoleName} already exists");

            //var AddName = new RoleApplication
            //{
            //    Name = roleName.RoleName,
            //};
            //await contex.Roles.AddAsync(AddName);
            //await contex.SaveChangesAsync();
            var result = await roleManager.CreateAsync(new RoleApplication { Name = roleName.RoleName });


            if (result.Succeeded)
                return Ok("Role added successfully");

            return BadRequest(result.Errors);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete Role")]
        public async Task<IActionResult> DeleteRole(string RoleName)
        {
            var isfound = await roleManager.FindByNameAsync(RoleName);
            if (isfound is null) return NotFound($"{RoleName} does not exist");

            var result = await roleManager.DeleteAsync(isfound);
            if (result.Succeeded)
                return Ok("Role deleted successfully");

            return BadRequest(result.Errors);


        }
        [Authorize(Roles = "Admin")]
        [HttpPut("Edit Role")]
        public async Task<IActionResult> EditRole(EditRoleDTO dto)
        {
            var role = await roleManager.FindByNameAsync(dto.OldRoleName);
            if (role == null)
                return NotFound($"{dto.OldRoleName} does not exist");

            var roleExists = await roleManager.FindByNameAsync(dto.NewRoleName);
            if (roleExists != null)
                return BadRequest("New role name already exists");


            role.Name = dto.NewRoleName;
            role.NormalizedName = dto.NewRoleName.ToUpper();

            var result = await roleManager.UpdateAsync(role);

            if (result.Succeeded)
                return Ok("Role updated successfully");

            return BadRequest(result.Errors);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("Get All Roles")]
        public IActionResult GetAllRoles()
        {
            var roles = roleManager.Roles.Select(r => r.Name).ToList();
            return Ok(roles);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Assign Role")]
        public async Task<IActionResult> AssignRole(string email, string roleName)
        {
            var EmailIsFound = await userManager.FindByEmailAsync(email);
            if (EmailIsFound is null) return NotFound("User not found");
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists) return NotFound("RoleName not found");
            var result = await userManager.AddToRoleAsync(EmailIsFound, roleName);
            if (result.Succeeded)
                return Ok("Role assigned successfully");

            return BadRequest(result.Errors);

        }
        [Authorize(Roles = "Admin")]
        [HttpPost("Remove Role from User")]
        public async Task<IActionResult> RemoverolefromUser(string email, string roleName)
        {
            var EmailIsFound = await userManager.FindByEmailAsync(email);
            if (EmailIsFound is null) return NotFound("User not found");
            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (!roleExists) return NotFound("RoleName not found");
            var result = await userManager.RemoveFromRoleAsync(EmailIsFound, roleName);
            if (result.Succeeded)
                return Ok("Role assigned successfully");

            return BadRequest(result.Errors);

        }
        [Authorize(Roles = "Admin")]
        [HttpGet("Get User Roles")]
        public async Task<IActionResult> GetUserRoles(string email)
        {
            var EmailIsFound = await userManager.FindByEmailAsync(email);
            if (EmailIsFound is null) return NotFound("User not found");

            var result = await userManager.GetRolesAsync(EmailIsFound);
            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("Users In Role")]
        public async Task<IActionResult> GetUsersInRole(string roleName)
        {
            var users = await userManager.GetUsersInRoleAsync(roleName);
            var result = users.Select(u => new { u.Id, u.UserName, u.Email });
            return Ok(result);
        }


    }
}
