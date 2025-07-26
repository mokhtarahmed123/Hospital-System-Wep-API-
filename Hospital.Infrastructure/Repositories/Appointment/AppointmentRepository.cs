using HospitalAPI.Hospital.Domain.Models;
using HospitalAPI.Hospital.Infrastructure.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace HospitalAPI.Hospital.Infrastructure
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly HospitalContex hospitalContex;

        public AppointmentRepository(HospitalContex hospitalContex)
        {
            this.hospitalContex = hospitalContex;
        }

        public async Task<Appointments> CreateAppointment(int patientId, int doctorId, Appointments appointment)
        {
            var doctorExists = await hospitalContex.Doctors.FindAsync(doctorId);
            var patientExists = await hospitalContex.Patients.FindAsync(patientId);
            if (doctorExists is not null && patientExists is not null)
            {
                appointment.DoctorID = doctorId;
                appointment.PatientID = patientId;
                appointment.Status = "Pending";
                hospitalContex.Appointments.Add(appointment);
                await Save();
                return appointment;
            }
            return null;
        }

        public async Task<List<Appointments>> Appointments()
        {
            return await hospitalContex.Appointments.Include(a => a.Doctor).Include(p => p.Patient).ToListAsync();
        }

        public async Task<Appointments?> AppointmentsByID(int id)
        {
            return await hospitalContex.Appointments.Include(a => a.Doctor).Include(p => p.Patient).
                FirstOrDefaultAsync(a => a.ID == id);
        }

        public async Task Delete(int id)
        {
            var app = await hospitalContex.Appointments.FindAsync(id);
            if (app == null)
                throw new KeyNotFoundException($"Appointment with ID {id} not found");

            hospitalContex.Appointments.Remove(app);
            await Save();
        }

        public async Task<List<Appointments>> GetByDoctorAsync(int doctorId)
        {
            return await hospitalContex.Appointments.
                 Include(a => a.Doctor).Where(i => i.DoctorID == doctorId).ToListAsync();
        }

        public async Task<List<Appointments>> GetByPatientAsync(int patientId)
        {
            return await hospitalContex.Appointments.
      Include(a => a.Patient).Where(i => i.PatientID == patientId).ToListAsync();
        }

        public async Task<List<Appointments>> SearchAppointmentsAsync(string? status, DateTime? date)
        {

            return await hospitalContex.Appointments.Where(d =>
            (string.IsNullOrEmpty(status) || d.Status.Contains(status)) &&
            (!date.HasValue || d.AppointmentDate.Date == date.Value.Date)
        ).ToListAsync();

        }

        public async Task<List<Doctors>> SearchByNameAsyncDoctors(string keyword)
        {
            return await hospitalContex.Doctors.Where(a => a.Name.Contains(keyword)).ToListAsync();

        }

        public async Task<List<Patients>> SearchByNameAsyncPatients(string keyword)
        {
            return await hospitalContex.Patients.Where(a => a.Name.Contains(keyword)).ToListAsync();

        }

        public async Task UpdateAsync(int id, Appointments appointments)
        {
            Appointments app = await hospitalContex.Appointments.FindAsync(id);
            if (app != null)
            {
                app.AppointmentDate = appointments.AppointmentDate;
                app.Status = appointments.Status;
                app.DoctorID = appointments.DoctorID;
                app.PatientID = appointments.PatientID;
                await Save();

            }
        }

        public async Task<bool> UpdateStatusAsync(int id, string newStatus)
        {
            Appointments appointments = await hospitalContex.Appointments.FindAsync(id);
            if (appointments is not null)
            {
                appointments.Status = newStatus;
                await Save();
                return true;
            }
            return false;
        }
        public async Task Save()
        {
            await hospitalContex.SaveChangesAsync();
        }
    }
}
