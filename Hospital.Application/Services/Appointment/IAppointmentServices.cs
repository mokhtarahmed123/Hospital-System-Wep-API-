using HospitalAPI.Hospital.Application.DTO.AppointmentDTO;

namespace HospitalAPI.Hospital.Application.Services.Appointment
{
    public interface IAppointmentServices
    {
        Task<List<GetAllAppointmentsDTO>> GetAllAppointmentsAsync();
        Task<bool> Delete(int id);
        Task<EditAppointment> EditAppointment(int id, EditAppointment edit);
        Task<bool> ConfirmAppointment(int id, ConfirmAppointment confirm);
        Task<List<GetAllAppointmentsDTO>> SearchDoctorsAsync(FilterAppointment filter);
        Task<GetAllAppointmentsDTO> GetAppointmentByIdAsync(int id);
        Task<List<GetAllAppointmentsDTO>> GetAppointmentsByPatientIdAsync(string PatientEmail);


    }
}
