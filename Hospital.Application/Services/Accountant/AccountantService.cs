using HospitalAPI.Hospital.Application.DTO.AccountantDTO;
using HospitalAPI.Hospital.Application.Services.Accountant;
using HospitalAPI.Hospital.Domain.Models;
using HospitalAPI.Hospital.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Hospital.Application
{
    public class AccountantService : IAccountantService
    {
        private readonly HospitalContex contex;
        private readonly UserManager<UserApplication> userManager;
        private readonly RoleManager<RoleApplication> roleManager;

        public AccountantService(HospitalContex contex, UserManager<UserApplication> userManager, RoleManager<RoleApplication> roleManager)
        {
            this.contex = contex;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<CreateAccountantdto> CreateAccountanAsync(CreateAccountantdto dto)
        {
            if (dto == null) return dto;

            var role = await contex.Roles.FirstOrDefaultAsync(r => r.Name.ToLower() == dto.RoleName.ToLower());
            if (role == null)
                throw new Exception("Role not found");
            if (await userManager.FindByEmailAsync(dto.Email) != null)
                throw new Exception("Email already exists");

            if (await userManager.FindByNameAsync(dto.Name) != null)
                throw new Exception("Username already exists");
            UserApplication user = new UserApplication()
            {
                UserName = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.Phone,

            };

            var result = await userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"Failed to create user: {errors}");
            }
            await userManager.AddToRoleAsync(user, "Accountant");


            Accountant accountant = new Accountant()
            {
                Name = dto.Name,
                Address = dto.Address,
                Certification = dto.Certification,
                Gander = dto.Gender,
                Email = dto.Email,
                Phone = dto.Phone,
                RoleID = role.Id,
                UserId = user.Id,
            };

            await contex.Accountants.AddAsync(accountant);
            await contex.SaveChangesAsync();
            return dto;
        }

        public async Task<bool> DeleteAccountanAsync(string Email)
        {
            var Acc = await contex.Accountants.FirstOrDefaultAsync(i=>i.Email == Email);
            if (Acc == null) return false;
            var user = await userManager.FindByEmailAsync(Acc.Email);
            if (user != null)
            {
                await userManager.DeleteAsync(user);
            }
            contex.Accountants.Remove(Acc);
            await contex.SaveChangesAsync();
            return true;
        }

        public async Task<List<GetAccountantDTO>> GetAllBilling(string Email)
        {
            return await contex.Billing
                       .Include(b => b.Patient)
                       .Include(b => b.Accountant).Where(i => i.Accountant.Email == Email)
                       .Select(b => new GetAccountantDTO
                       {
                           PatientName = b.Patient.Name,
                           TotalAmount = b.TotalAmount,
                           Name = b.Accountant.Name,
                           Certification = b.Accountant.Certification,
                           Id = b.Accountant.Id,
                           Status = b.Status,
                           BillingDate = (DateTime)b.DateIssued,

                       }).ToListAsync();
        }

        public async Task<CreateAccountantdto> GetProfileAsync(string Email)
        {

            var Account = await contex.Accountants.
                //Include(a=>a.Staff).
                Include(a => a.Billing).
                FirstOrDefaultAsync(i => i.Email == Email);
            if (Account == null) return null;
            var role = await contex.Roles.FirstOrDefaultAsync(r => r.Id == Account.RoleID);

            CreateAccountantdto create = new CreateAccountantdto()
            {
                Certification = Account.Certification,
                RoleName = role.Name,
                Address = Account.Address,
                Name = Account.Name,
                Gender = Account.Gander,
                Email = Account.Email,
                Phone = Account.Phone,

            };
            return create;

        }

        public async Task<CreateAccountantdto> UpdateAccountanAsync(CreateAccountantdto Accountant, string Email)
        {
            var OldAccountant = await contex.Accountants.FirstOrDefaultAsync(i => i.Email == Email);
            if (OldAccountant == null) return null;

            // تحديث بيانات الـ Accountant
            OldAccountant.Name = Accountant.Name;
            OldAccountant.Phone = Accountant.Phone;
            OldAccountant.Email = Accountant.Email;
            OldAccountant.Address = Accountant.Address;
            OldAccountant.Certification = Accountant.Certification;

            // تحديث بيانات الـ Identity User المرتبط بيه
            var user = await userManager.FindByIdAsync(OldAccountant.UserId);
            if (user != null)
            {
                user.UserName = Accountant.Name;
                user.Email = Accountant.Email;
                user.PhoneNumber = Accountant.Phone;

                var result = await userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to update user: {errors}");
                }
            }

            await contex.SaveChangesAsync();
            return Accountant;
        }

    }
}
