using HospitalAPI.Hospital.Application.DTO.Staff;

namespace HospitalAPI.Hospital.Application.Services.Staff
{
    public interface IStaffService
    {
        Task<CreateStaffDTO> CreateStaffAsync(CreateStaffDTO dto);

        Task<bool> UpdateStaffAsync(string Email, UpdateStaffDTO dto);

        Task<bool> DeleteStaffAsync(string Email);

        Task<GetAllStaffDTO> GetStaffByIdAsync(string Email);

        Task<List<GetAllStaffDTO>> GetAllStaffAsync();

        Task<List<GetAllStaffDTO>> FilterStaffAsync(StaffFilterDTO filter);

        Task<int> GetStaffCountAsync();
    }
}
