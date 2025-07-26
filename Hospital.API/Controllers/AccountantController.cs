using HospitalAPI.Hospital.Application;
using HospitalAPI.Hospital.Application.DTO.AccountantDTO;
using HospitalAPI.Hospital.Application.Services.Accountant;
using HospitalAPI.Hospital.Domain.Models;
using HospitalAPI.Hospital.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class AccountantController : ControllerBase
    {
        private readonly HospitalContex contex;
        private readonly IAccountantService accountant;
        private readonly UserManager<UserApplication> userManager;
        private readonly RoleManager<RoleApplication> roleManager;

        public AccountantController(HospitalContex contex,
            IAccountantService accountant,
            UserManager<UserApplication> userManager,
            RoleManager<RoleApplication> roleManager)
        {
            this.contex = contex;
            this.accountant = accountant;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] CreateAccountantdto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto == null)
                return BadRequest();

            var acc = await accountant.CreateAccountanAsync(dto);

            return Ok(acc);
        }

        [HttpPost("GetProfile")]
        [Authorize(Roles = "Admin,Accountant")]
        public async Task<IActionResult> GetProfileAsync([FromBody] string email)
        {
            var dto = await accountant.GetProfileAsync(email);
            if (dto == null)
            {
                return NotFound($"Accountant with Email {email} not found.");
            }
            return Ok(dto);
        }
        [HttpDelete("Delete")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAccountanAsync([FromForm] string email)
        {
            // 1. Get accountant from App DB
            var accountantEntity = await contex.Accountants.FirstOrDefaultAsync(a => a.Email == email);
            if (accountantEntity == null)
                return NotFound("Accountant Not Found");

            // 2. Get user from Identity
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return NotFound("User not found in Identity");

            // 3. Remove roles
            var roles = await userManager.GetRolesAsync(user);
            var removeRolesResult = await userManager.RemoveFromRolesAsync(user, roles);
            if (!removeRolesResult.Succeeded)
                return BadRequest("Failed to remove user roles");

            // 4. Remove accountant from App DB
            contex.Accountants.Remove(accountantEntity);
            await contex.SaveChangesAsync(); // مهم: نحفظ الأول قبل حذف الـUser

            // 5. Delete user from Identity
            var deleteResult = await userManager.DeleteAsync(user);
            if (!deleteResult.Succeeded)
                return BadRequest("Failed to delete user");

            return Ok("Deleted Successfully");
        }




        [HttpPut("UpdateAccountant")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAccountantAsync([FromBody] CreateAccountantdto updateDto, [FromQuery] string email)
        {
            var AccountExists = await contex.Accountants.FirstOrDefaultAsync(a => a.Email == email);
            if (AccountExists == null)
                return NotFound("Account not found");

            var updated = await accountant.UpdateAccountanAsync(updateDto, email);
            if (updated == null)
                return BadRequest("Update failed");

            return Ok(updated);
        }


        [HttpPost("GetAllBilling")]
        [Authorize(Roles = "Admin,Accountant")]
        public async Task<IActionResult> GetAllBilling([FromBody] string email)
        {
            var Accountant = await contex.Accountants.FirstOrDefaultAsync(a => a.Email == email);
            if (Accountant == null) return NotFound("User Not Found");

            var bills = await accountant.GetAllBilling(email);
            if (bills == null || bills.Count == 0)
                return NotFound("No billing records found");

            return Ok(bills);
        }

    }
}
