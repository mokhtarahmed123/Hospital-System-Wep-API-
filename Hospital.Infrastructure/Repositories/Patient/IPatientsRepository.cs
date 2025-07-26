using HospitalAPI.Hospital.Domain.Models;

namespace HospitalAPI.Hospital.Infrastructure
{
    public interface IPatientsRepository
    {
        Task<Patients?> GetById(int id);
        Task Add(Patients patient);
        Task Update(Patients updatedPatient, int id);
        Task Delete(int id);
        Task<List<Appointments>> GetAppointments(int patientId);
        Task<List<Laboratory>> GetLabs(int patientId);
        Task<Patients?> GetProfile(int patientId);
        Task<List<Patients>> SearchByNameAsyncPatients(string keyword);
        Task<List<Doctors>> SearchDoctorsAsync(string? specialization, string? name);







    }
}
