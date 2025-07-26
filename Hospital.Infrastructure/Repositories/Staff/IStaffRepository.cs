using HospitalAPI.Hospital.Domain.Models;

namespace HospitalAPI.Hospital.Infrastructure.Repositories.Staff
{
    public interface IStaffRepository
    {
        Task<List<Staff_Management>> GetAllAsync();
        Task<Staff_Management?> GetByIdAsync(int id);
        Task AddAsync(Staff_Management staff);
        Task UpdateAsync(int id, Staff_Management staff);
        Task DeleteAsync(int id);
        Task<List<Staff_Management>> GetByRoleAsync(string roleName);
        Task<List<Staff_Management>> GetByDepartmentIdAsync(int departmentId);
        Task<List<Staff_Management>> GetByDepartmentNameAsync(string departmentName);
        Task<List<Staff_Management>> SearchAsync(string keyword);
        Task<bool> UpdateDepartmentAsync(int staffId, int newDepartmentId);
        Task<bool> UpdateRoleAsync(int staffId, string newRoleId);

    }
}
