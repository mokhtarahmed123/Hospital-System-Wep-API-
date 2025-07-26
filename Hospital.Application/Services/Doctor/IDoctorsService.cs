using HospitalAPI.Hospital.Application.DTO.DoctorDTo;

namespace HospitalAPI.Hospital.Application
{
    public interface IDoctorsService
    {
        Task<CreateDoctorDTO> CreateDoctorAsync(CreateDoctorDTO dto);
        Task<CreateDoctorDTO> UpdateDoctorAsync(CreateDoctorDTO Doctor, string Email);
        Task<bool> DeleteDoctorAsync(string Email);
        Task<CreateDoctorDTO> GetProfileAsync(string Email);
        Task<List<DoctorAppointmentsDTO>> GetAppointmentsAsync(string Email);
        Task<LabTestRequestDTO> RequestLabAsync(LabTestRequestDTO laboratoryDTO);


    }
}
