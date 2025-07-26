using HospitalAPI.Hospital.Domain.Models;

namespace HospitalAPI.Hospital.Infrastructure;
public interface IAppointmentRepository
{

    Task<List<Appointments>> Appointments();
    Task<Appointments?> AppointmentsByID(int id);
    Task UpdateAsync(int id, Appointments appointments);
    Task Delete(int id);
    Task<bool> UpdateStatusAsync(int id, string newStatus);
    Task<Appointments> CreateAppointment(int patientId, int doctorId, Appointments appointment);
    Task<List<Appointments>> GetByDoctorAsync(int doctorId);
    Task<List<Appointments>> GetByPatientAsync(int patientId);
    Task<List<Patients>> SearchByNameAsyncPatients(string keyword);
    Task<List<Doctors>> SearchByNameAsyncDoctors(string keyword);
    Task<List<Appointments>> SearchAppointmentsAsync(string? status, DateTime? date);



}
