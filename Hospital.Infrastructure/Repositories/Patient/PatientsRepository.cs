using HospitalAPI.Hospital.Domain.Models;
using HospitalAPI.Hospital.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace HospitalAPI.Hospital.Infrastructure
{
    public class PatientsRepository : IPatientsRepository
    {

        private readonly HospitalContex hospitalContex;


        public PatientsRepository(HospitalContex hospitalContex)
        {
            this.hospitalContex = hospitalContex;
        }
        public async Task Add(Patients patient)
        {
            hospitalContex.Patients.Add(patient);
            await Save();
        }

        public async Task Delete(int id)
        {
            var patient = await hospitalContex.Patients.FindAsync(id);
            if (patient != null)
            {
                hospitalContex.Patients.Remove(patient);
                await Save();
            }
        }

        public async Task<List<Appointments>> GetAppointments(int patientId)
        {
            var appointments = await hospitalContex.Appointments
           .Where(a => a.PatientID == patientId).Include(a => a.Patient)
           .ToListAsync();
            return appointments;
        }

        public async Task<Patients?> GetById(int id)
        {
            return await hospitalContex.Patients.FirstOrDefaultAsync(i => i.ID == id);
        }

        public async Task<List<Laboratory>> GetLabs(int patientId)
        {
            var lab = await hospitalContex.Laboratory.Where(i => i.PatientId == patientId)
                .Include(a => a.Patient).ToListAsync();
            return lab;
        }

        public async Task<Patients?> GetProfile(int patientId)
        {
            return await hospitalContex.Patients
           .Include(p => p.billings)
           .Include(p => p.Inpatients)
           .Include(p => p.appointments)
           .Include(p => p.laboratory)
           .FirstOrDefaultAsync(p => p.ID == patientId);
        }

        public async Task<List<Patients>> SearchByNameAsyncPatients(string keyword)
        {
            return await hospitalContex.Patients.Where(a => a.Name.Contains(keyword)).ToListAsync();
        }

        public Task<List<Doctors>> SearchDoctorsAsync(string? specialization, string? name)
        {
            return hospitalContex.Doctors.
                Where(d =>
            (string.IsNullOrEmpty(name) || d.Name.Contains(name)) &&
            (string.IsNullOrEmpty(specialization) || d.specialization.Contains(specialization))
        ).ToListAsync();
        }

        public async Task Update(Patients updatedPatient, int id)
        {
            Patients patients = hospitalContex.Patients.FirstOrDefault(p => p.ID == id);
            if (patients != null)
            {
                patients.Name = updatedPatient.Name;
                patients.Address = updatedPatient.Address;
                patients.Age = updatedPatient.Age;
                patients.Email = updatedPatient.Email;
                patients.Phone = updatedPatient.Phone;
                patients.Gander = updatedPatient.Gander;
                patients.ImagePath = updatedPatient.ImagePath;
                patients.ApplicationUser = updatedPatient.ApplicationUser;
                await Save();



            }
        }
        public async Task Save()
        {
            await hospitalContex.SaveChangesAsync();
        }
    }
}
