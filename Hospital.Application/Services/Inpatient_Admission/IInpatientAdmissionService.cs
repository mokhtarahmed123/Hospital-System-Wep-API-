using HospitalAPI.Hospital.Application.DTO.Inpatient_Admission;

namespace HospitalAPI.Hospital.Application.Services.Inpatient_Admission
{
    public interface IInpatientAdmissionService
    {
        Task<CreateInpatientAdmissionDTO> CreateAdmissionAsync(CreateInpatientAdmissionDTO dto);
        Task<bool> UpdateAdmissionAsync(int admissionId, UpdateInpatientAdmissionDTO dto);
        Task<bool> DeleteAdmissionAsync(int admissionId);
        Task<GetInpatientAdmissionByIdDTO> GetAdmissionByIdAsync(int admissionId);
        Task<List<GetAllInpatientAdmissionsDTO>> GetAllAdmissionsAsync();
        Task<List<GetAllInpatientAdmissionsDTO>> FilterAdmissionsAsync(InpatientAdmissionFilterDTO filter);
        Task<List<GetAllInpatientAdmissionsDTO>> GetCurrentAdmissionsAsync();
        Task<List<GetAllInpatientAdmissionsDTO>> GetAdmissionsByPatientIdAsync(string Email);

    }
}
