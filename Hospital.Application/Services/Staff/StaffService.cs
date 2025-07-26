using HospitalAPI.Hospital.Application.DTO.Staff;
using HospitalAPI.Hospital.Domain.Models;
using HospitalAPI.Hospital.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HospitalAPI.Hospital.Application.Services.Staff
{
    public class StaffService : IStaffService
    {
        private readonly HospitalContex contex;
        private readonly UserManager<UserApplication> userManager;
        private readonly RoleManager<RoleApplication> roleManager;

        public StaffService(HospitalContex contex , UserManager<UserApplication> userManager ,RoleManager<RoleApplication> roleManager )
        {
            this.contex = contex;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<CreateStaffDTO> CreateStaffAsync(CreateStaffDTO dto)
        {
            if (dto == null) return null;

            var role = await contex.Roles.FirstOrDefaultAsync(r => r.Name.ToLower() == dto.RoleName.ToLower());
            if (role == null)
                throw new Exception("Invalid Role Name");

            var dept = await contex.Departments.FirstOrDefaultAsync(i => i.Name == dto.DepartmentName);
            if (dept == null) return null;

            var existingUser = await userManager.FindByEmailAsync(dto.Email);
            if (existingUser == null)
            {
                var newUser = new UserApplication
                {
                    Email = dto.Email,
                    UserName = dto.Email,
                    PhoneNumber = dto.PhoneNumber
                };
                var createResult = await userManager.CreateAsync(newUser, dto.Password); 
                if (!createResult.Succeeded)
                    throw new Exception("Failed to create user.");

                await userManager.AddToRoleAsync(newUser, dto.RoleName);
            }

            Staff_Management staff_management = new Staff_Management()
            {
                DateHired = dto.HireDate,
                DepartmentId = dept.Id,
                Salary = dto.Salary,
                Email = dto.Email,
                FullName = dto.Name,
                RoleId = role.Id,
                Phone = dto.PhoneNumber
            };

            await contex.Staff_Managements.AddAsync(staff_management);
            await contex.SaveChangesAsync();

            return dto;
        }


        public async Task<bool> DeleteStaffAsync(string Email)
        {
            var staff = await contex.Staff_Managements.FirstOrDefaultAsync(a => a.Email == Email);
            if (staff == null) return false;

            var user = await userManager.FindByEmailAsync(Email);
            if (user != null)
            {
                var deleteResult = await userManager.DeleteAsync(user);
                if (!deleteResult.Succeeded)
                {
                    throw new Exception("Failed to delete user from identity.");
                }
            }

            contex.Staff_Managements.Remove(staff);
            await contex.SaveChangesAsync();
            return true;
        }


        public async Task<List<GetAllStaffDTO>> FilterStaffAsync(StaffFilterDTO filter)
        {
            var query = contex.Staff_Managements
                              .Include(s => s.Department)
                              .Include(s => s.RoleApplication)
                              .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.DepartmentName))
            {
                query = query.Where(s => s.Department.Name.Contains(filter.DepartmentName));
            }

            if (!string.IsNullOrWhiteSpace(filter.RoleName))
            {
                var role = await contex.Roles
                    .FirstOrDefaultAsync(r => r.Name.ToLower() == filter.RoleName.ToLower());

                if (role == null)
                    throw new Exception("Invalid Role Name");
                query = query.Where(s => s.RoleId == role.Id);
            }

            return await query.Select(s => new GetAllStaffDTO
            {
                Id = s.Id,
                Name = s.FullName,
                Role = s.RoleApplication.Name,
                Department = s.Department.Name
            }).ToListAsync();
        }

        public async Task<List<GetAllStaffDTO>> GetAllStaffAsync()
        {
            return await contex.Staff_Managements.
                //Include(a => a.Accountant).
                Include(a => a.Doctors).
                Include(a => a.Department).
                Select(a => new GetAllStaffDTO
                {
                    Id = a.Id,
                    Name = a.FullName,
                    Role = a.RoleApplication.Name,
                    Department = a.Department.Name

                }).ToListAsync();
        }

        public async Task<GetAllStaffDTO> GetStaffByIdAsync(string Email)
        {

            return await contex.Staff_Managements.
                //Include(a => a.Accountant).
                Include(a => a.Doctors).
                Include(a => a.Department)
                .Where(i => i.Email == Email ).
                Select(a => new GetAllStaffDTO
                {
                    Id = a.Id,

                    Name = a.FullName,
                    Role = a.RoleApplication.Name,
                    Department = a.Department.Name


                }).FirstOrDefaultAsync();
        }

        public Task<int> GetStaffCountAsync()
        {
            return contex.Staff_Managements.CountAsync();
        }

        public async Task<bool> UpdateStaffAsync(string Email, UpdateStaffDTO dto)
        {
            var staff = await contex.Staff_Managements.FirstOrDefaultAsync(a => a.Email == Email);
            if (staff == null)
                return false;

            var dept = await contex.Departments.FirstOrDefaultAsync(i => i.Name == dto.DepartmentName);
            if (dept == null) return false;

            var role = await contex.Roles.FirstOrDefaultAsync(i => i.Name == dto.RoleName);
            if (role == null) return false;

            // تحديث جدول Staff_Management
            staff.FullName = dto.Name;
            staff.Phone = dto.PhoneNumber;
            staff.Email = dto.Email;
            staff.Salary = dto.Salary;
            staff.DepartmentId = dept.Id;
            staff.RoleId = role.Id;
            staff.DateHired = dto.HireDate;

            // تحديث جدول UserApplication
            var user = await userManager.FindByEmailAsync(Email);
            if (user != null)
            {
                user.Email = dto.Email;
                user.UserName = dto.Email;
                user.PhoneNumber = dto.PhoneNumber;

                var updateResult = await userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    throw new Exception("Failed to update user identity data.");
                }
            }

            await contex.SaveChangesAsync();
            return true;
        }

    }
}
