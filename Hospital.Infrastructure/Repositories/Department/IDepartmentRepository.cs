using HospitalAPI.Hospital.Domain.Models;

namespace HospitalAPI.Hospital.Infrastructure
{
    public interface IDepartmentRepository
    {
        Task<List<Department>> GetAllDepartment();
        Task<Department> departmentByAsync(int Id);
        Task Add(Department department);
        Task<List<Department>> SearchDepartmentsAsync(string? name);
        Task<bool> DepartmentExistsAsync(string name);
        Task Update(Department department, int DepartmentId);
        Task Delete(int id);





    }
}
