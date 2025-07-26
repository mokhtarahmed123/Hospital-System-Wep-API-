using HospitalAPI.Hospital.Domain.Models;

namespace HospitalAPI.Hospital.Infrastructure
{
    public interface IDoctorRepository
    {
        Task<Doctors?> GetProfile(int DoctorsId);
        Task Add(Doctors doctors);
        Task Update(Doctors doctors, int DoctorId);
        Task Delete(int id);
        Task<List<Appointments>> GetAppointments(int DoctorsId);
        Task<Appointments?> GetAppointmentById(int appointmentId);
        Task<bool> UpdateAppointmentStatusAsync(int appointmentId, string newStatus);
        Task CreateLab(int doctorId, int patientId, Laboratory labRequest);







    }
}
