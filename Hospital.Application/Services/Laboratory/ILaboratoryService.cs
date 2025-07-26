using HospitalAPI.Hospital.Application.DTO.LaboratoryDTO;

namespace HospitalAPI.Hospital.Application.Services.Laboratory
{
    public interface ILaboratoryService
    {
        Task<List<GetAllLabRequests>> GetAllLabRequests();
        Task<List<GetAllLabRequests>> GetByID(int id);
        Task<bool> Update(int id, UpdateLabDTO update);
        Task<bool> Confirm(int id, string Status);
        Task<bool> Delete(int id);
        Task<List<GetAllLabRequests>> GetByTestTypeAsync(string testType);
        Task<List<GetAllLabRequests>> GetByStatusAsync(string status);
        Task<List<GetAllLabRequests>> GetByPatientIdAsync(string PatientEmail);
        Task<List<GetAllLabRequests>> GetByDoctorIdAsync(string doctorEmail);


    }
}
