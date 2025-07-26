using HospitalAPI.Hospital.Domain.Models;
using HospitalAPI.Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Hospital.Infrastructure.Repositories.Staff
{
    public class StaffRepository : IStaffRepository
    {
        private readonly HospitalContex hospitalContex;

        public StaffRepository(HospitalContex hospitalContex)
        {
            this.hospitalContex = hospitalContex;
        }
        public async Task AddAsync(Staff_Management staff)
        {
            hospitalContex.Staff_Managements.Add(staff);
            await Save();
        }

        public async Task DeleteAsync(int id)
        {
            var Staff_Managements = await hospitalContex.Staff_Managements.FindAsync(id);
            if (Staff_Managements != null)
            {
                hospitalContex.Staff_Managements.Remove(Staff_Managements);
                await Save();
            }
        }

        public async Task<List<Staff_Management>> GetAllAsync()
        {
            return await hospitalContex.Staff_Managements.

                 //Include(a => a.Accountant).
                 Include(a => a.Department).
                 Include(a => a.Doctors).
                 Include(a => a.UserApplication).
                 Include(a => a.RoleApplication).
                 ToListAsync();

        }

        public async Task<List<Staff_Management>> GetByDepartmentIdAsync(int departmentId)
        {
            return await hospitalContex.Staff_Managements.

                //Include(a => a.Accountant).
                Include(a => a.Department).
                Include(a => a.Doctors).
                Include(a => a.UserApplication).
                Include(a => a.RoleApplication).
        Where(a => a.DepartmentId == departmentId).ToListAsync();
        }

        public async Task<List<Staff_Management>> GetByDepartmentNameAsync(string departmentName)
        {
            if (string.IsNullOrWhiteSpace(departmentName))
                return new List<Staff_Management>();

            return await hospitalContex.Staff_Managements

                //.Include(a => a.Accountant)
                .Include(a => a.Department)
                .Include(a => a.Doctors)
                .Include(a => a.UserApplication)
                .Include(a => a.RoleApplication)
                .Where(a => a.Department != null && a.Department.Name == departmentName)
                .ToListAsync();
        }

        public async Task<Staff_Management?> GetByIdAsync(int id)
        {
            var Staff = await hospitalContex.Staff_Managements.FindAsync(id);
            if (Staff == null) { return null; }
            return await hospitalContex.Staff_Managements

        //.Include(a => a.Accountant)
        .Include(a => a.Department)
        .Include(a => a.Doctors)
        .Include(a => a.UserApplication)
        .Include(a => a.RoleApplication)
        .FirstOrDefaultAsync(a => a.Id == id);

        }

        public async Task<List<Staff_Management>> GetByRoleAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                return new List<Staff_Management>();

            return await hospitalContex.Staff_Managements
                .Include(s => s.RoleApplication)
                .Include(s => s.UserApplication)
                .Include(s => s.Department)
                .Where(s => s.RoleApplication != null && s.RoleApplication.Name == roleName)
                .ToListAsync();
        }

        public async Task<List<Staff_Management>> SearchAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return new List<Staff_Management>();

            keyword = keyword.ToLower();
            return await hospitalContex.Staff_Managements
                .Include(s => s.UserApplication)
                .Include(s => s.RoleApplication)
                .Include(s => s.Department)
                .Where(s =>
                    s.UserApplication != null && s.UserApplication.UserName.ToLower().Contains(keyword) ||
                    s.RoleApplication != null && s.RoleApplication.Name.ToLower().Contains(keyword) ||
                    s.Department != null && s.Department.Name.ToLower().Contains(keyword)
                )
                .ToListAsync();
        }

        public async Task UpdateAsync(int id, Staff_Management staff)
        {
            var existingStaff = await hospitalContex.Staff_Managements.FindAsync(id);
            if (existingStaff == null)
                throw new KeyNotFoundException("Staff not found");

            existingStaff.UserId = staff.UserId;
            existingStaff.RoleId = staff.RoleId;
            existingStaff.DepartmentId = staff.DepartmentId;
            existingStaff.FullName = staff.FullName;
            existingStaff.Email = staff.Email;
            existingStaff.Phone = staff.Phone;
            existingStaff.Salary = staff.Salary;
            existingStaff.DateHired = staff.DateHired;
            hospitalContex.Staff_Managements.Update(existingStaff);
            await Save();
        }

        public async Task<bool> UpdateDepartmentAsync(int staffId, int newDepartmentId)
        {
            var staff = await hospitalContex.Staff_Managements.FindAsync(staffId);
            if (staff == null)
                return false;

            staff.DepartmentId = newDepartmentId;
            await Save();
            return true;
        }

        public async Task<bool> UpdateRoleAsync(int staffId, string newRoleId)
        {
            var staff = await hospitalContex.Staff_Managements.FindAsync(staffId);
            if (staff == null)
                return false;

            staff.RoleId = newRoleId;
            await Save();
            return true;
        }

        public async Task Save()
        {
            await hospitalContex.SaveChangesAsync();
        }
    }
}
