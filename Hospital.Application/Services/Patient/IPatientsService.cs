using HospitalAPI.Hospital.Application.DTO.PatientDTO;


namespace HospitalAPI.Hospital.Application
{
    public interface IPatientsService
    {
        //Task<PatientCreateDTO> CreatePatientAsync(PatientCreateDTO patient);
        Task<PatientCreateDTO> UpdatePatientAsync(PatientCreateDTO patient, string Email);
        Task<bool> DeletePatientAsync(string Email);
        Task<PatientCreateDTO> GetProfileAsync(string Email);
        Task<AppointmentCreateDTO> CreateAppointmentAsync(AppointmentCreateDTO appointmentDto);
        Task<List<AppointmentsDTO>> GetAppointmentsAsync(string Email);
        Task<List<LaboratoryDTO>> GetLabResultsAsync(string Email);
        Task<List<Billingdto>> GetBillingHistoryAsync(string Email);
        Task<List<SearchDoctorsDTO>> SearchDoctorsAsync(string? name = null, string? specialization = null);
        //Task<byte[]> DownloadMedicalReportAsync(int patientId); 

        Task<UpdateAppoinmentDTO> UpdateAppoinment(UpdateAppoinmentDTO appointmentCreateDTO, int id);



    }
}
