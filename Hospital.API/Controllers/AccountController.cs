using HospitalAPI.Hospital.Application;
using HospitalAPI.Hospital.Application.DTO.Account;
using HospitalAPI.Hospital.Domain.Models;
using HospitalAPI.Hospital.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HospitalAPI.Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : ControllerBase
    {
        private readonly HospitalContex contex;
        private readonly UserManager<UserApplication> userManager;
        private readonly IConfiguration Cofing;
        private readonly RoleManager<RoleApplication> roleManager;

        public AccountController(HospitalContex contex,
            UserManager<UserApplication> userManager,
            IConfiguration configuration,
            RoleManager<RoleApplication> roleManager)
        {

            this.contex = contex;
            this.userManager = userManager;
            Cofing = configuration;
            this.roleManager = roleManager;
        }

        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn([FromForm] SignInAccountDTO account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var adminRole = await contex.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
            if (adminRole == null)
                return BadRequest("Admin role not found.");
            #region Create Admin
            bool isFirstAdmin = await contex.Staff_Managements.AnyAsync(A => A.RoleId == adminRole.Id);
            if (!isFirstAdmin)
            {
                UserApplication AdminUser = new UserApplication()
                {
                    UserName = account.UserName,
                    PhoneNumber = account.Phone,
                    Email = account.Email,
                };

                var Result = await userManager.CreateAsync(AdminUser, account.Password);
                if (!Result.Succeeded)
                {
                    foreach (var error in Result.Errors)
                        ModelState.AddModelError("", error.Description);

                    return BadRequest(ModelState);
                }
                var Staff = new Staff_Management()
                {
                    FullName = account.UserName,
                    Email = account.Email,
                    Phone = account.Phone,
                    RoleId = adminRole.Id,
                    UserId = AdminUser.Id,
                };

                await contex.Staff_Managements.AddAsync(Staff);
                await contex.SaveChangesAsync();
                await userManager.AddToRoleAsync(AdminUser, "Admin");
                return Ok("Admin Created");
            }

            var normalizedName = account.Email.Trim().ToUpper();
            var existingUser = await contex.Users.FirstOrDefaultAsync(u => u.Email == normalizedName);
            if (existingUser != null)
                return BadRequest("Email Is Exists");

            UserApplication USER = new UserApplication()
            {
                UserName = account.UserName,
                PhoneNumber = account.Phone,
                Email = account.Email,
            };

            var Result2 = await userManager.CreateAsync(USER, account.Password);
            if (!Result2.Succeeded)
            {
                foreach (var error in Result2.Errors)
                    ModelState.AddModelError("", error.Description);

                return BadRequest(ModelState);
            }
            #endregion
            #region Create Patient
            string imagePath = "Images/default.jpg";
            if (account.Image != null && account.Image.Length > 0)
            {
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(account.Image.FileName);
                string fullPath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await account.Image.CopyToAsync(stream);
                }

                imagePath = "Images/" + fileName;
            }

            Patients patients = new Patients()
            {
                Address = account.Address,
                Age = (int)account.Age,
                Name = account.UserName,
                Gander = account.Gender,
                Email = account.Email,
                Phone = account.Phone,
                ImagePath = imagePath,
                ApplicationUserId = USER.Id,
            };

            try
            {
                await contex.Patients.AddAsync(patients);
                await contex.SaveChangesAsync();
                await userManager.AddToRoleAsync(USER, "Patient");
                return Ok("Patient Created");
            }
            catch (Exception ex)
            {
                return BadRequest("Error saving patient: " + ex.Message);
            }
            #endregion

        }

        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn([FromBody] LoginDTO login)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var userNameFromLogin = await userManager.FindByNameAsync(login.UserName);
            if (userNameFromLogin == null) return Unauthorized("Invalid username or password.");

            var PasswordFromLogin = await userManager.CheckPasswordAsync(userNameFromLogin, login.Password);
            if (!PasswordFromLogin)
                return Unauthorized("Invalid username or password.");

            bool isValidRole = await roleManager.RoleExistsAsync(login.RoleName);
            if (!isValidRole)
            {
                return BadRequest("This role does not exist.");
            }

            var RoleFromUser = await userManager.GetRolesAsync(userNameFromLogin);
            //if (!RoleFromUser.Contains(login.RoleName))
            //    return Unauthorized("User does not have the required role.");

            var Claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, login.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti , Guid.NewGuid().ToString() ),
                        new Claim(ClaimTypes.Email,login.Email),
                        new Claim("UserID",userNameFromLogin.Id),
                    };
            foreach (var role in RoleFromUser)
            {
                Claims.Add(new Claim(ClaimTypes.Role, role));
                Claims.Add(new Claim("role", role));

            }
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Cofing["JWT:SecritKey"]));
            var SignCre = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var Token = new JwtSecurityToken(
                issuer: Cofing["JWT:IssuerIP"],
                audience: Cofing["JWT:AudienceIP"],
                expires: DateTime.Now.AddHours(2),
                claims: Claims,
                signingCredentials: SignCre
                );
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(Token),
                expiration = DateTime.Now.AddHours(2)
            });


        }
        [HttpDelete("Delete Account/{Email}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAccount(string Email)
        {
            var UserInStaff = await contex.Staff_Managements.FirstOrDefaultAsync(x => x.Email == Email);
            if (UserInStaff == null) return NotFound("User not found");
            var user = await userManager.FindByEmailAsync(Email);
            if (user == null) return NotFound("User not found");

            var staff = await contex.Staff_Managements.FirstOrDefaultAsync(s => s.UserId == user.Id);
            if (staff != null)
            {
                contex.Staff_Managements.Remove(staff);
                await contex.SaveChangesAsync();
            }

            var result = await userManager.DeleteAsync(user);
            if (!result.Succeeded) return BadRequest(result.Errors);

            return Ok("User deleted");
        }
        [Authorize]
        [HttpPost("Logout")]
        public async Task<IActionResult> LogOut()
        {
            var UserID = User.FindFirstValue("UserID");
            var user = await userManager.FindByIdAsync(UserID);
            if (user == null) return Unauthorized("User not found");
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();
            var token2 = handler.ReadJwtToken(token);
            var jti = token2.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            if (string.IsNullOrEmpty(jti)) return BadRequest("Invali Token");
            contex.revokedTokens.Add(new RevokedToken
            {
                JWT = jti,

            });
            await contex.SaveChangesAsync();
            return Ok("User logged out");
        }
        [HttpPost("Forget Password")]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto forget)
        {
            var User = await userManager.FindByEmailAsync(forget.Email);
            if (User == null) return Ok("If this email exists, a reset link has been sent.");
            var token = await userManager.GeneratePasswordResetTokenAsync(User);
            return Ok(new { token, email = User.Email });
        }

        [HttpPost("Reset Password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO resetPassword)
        {
            var user = await userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null) return NotFound("User not found");
            var result = await userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.NewPassword);
            if (result.Succeeded)
                return Ok("Password has been reset successfully");
            return BadRequest(result.Errors);
        }
        [Authorize]
        [HttpPost("Change Password")]
        public async Task<IActionResult> ChangePassword(UpdatePasswordDTO updatePassword)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return Unauthorized();
            var result = await userManager.ChangePasswordAsync(user, updatePassword.CurrentPassword, updatePassword.NewPassword);
            if (result.Succeeded)
                return Ok("Password changed successfully");
            return BadRequest(result.Errors);
        }

    }

}
