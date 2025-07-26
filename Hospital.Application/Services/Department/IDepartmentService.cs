using HospitalAPI.Hospital.Application.DTO.DepartmentDTO;

namespace HospitalAPI.Hospital.Application
{
    public interface IDepartmentService
    {
        Task<CreateDepartmentDTO> CreateDepartmentAsync(CreateDepartmentDTO dto);
        Task<CreateDepartmentDTO> UpdateDepartmentAsync(CreateDepartmentDTO Doctor, string NewName);
        Task<bool> DeleteDepartmentAsync(string Name);
        Task<CreateDepartmentDTO> GetDepartmentAsync(int id);
        Task<DepartmentWithDoctorsCountDTO> GetDepartmentsWithDoctorsCountAsync(string Name);
        Task<DepartmentWithDoctorsNamesDTO> GetDepartmentsWithDoctorsNamestAsync(string Name);



    }
}
