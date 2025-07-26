using HospitalAPI.Hospital.Domain.Models;
using HospitalAPI.Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Hospital.Infrastructure
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly HospitalContex hospitalContex;

        public DoctorRepository(HospitalContex hospitalContex)
        {
            this.hospitalContex = hospitalContex;
        }
        public async Task Add(Doctors doctors)
        {
            hospitalContex.Doctors.Add(doctors);
            await Save();
        }

        public async Task CreateLab(int doctorId, int patientId, Laboratory labRequest)
        {
            labRequest.DoctorId = doctorId;
            labRequest.PatientId = patientId;
            labRequest.RequestDate = DateTime.Now;
            await hospitalContex.Laboratory.AddAsync(labRequest);
            await Save();
        }

        public async Task Delete(int id)
        {
            var Doctor = await hospitalContex.Doctors.FindAsync(id);
            if (Doctor != null)
            {
                hospitalContex.Doctors.Remove(Doctor);
                await Save();
            }
        }

        public async Task<Appointments?> GetAppointmentById(int appointmentId)
        {
            return await hospitalContex.Appointments
    .Include(a => a.Doctor)
    .Include(a => a.Patient)
    .FirstOrDefaultAsync(a => a.ID == appointmentId);
        }

        public async Task<List<Appointments>> GetAppointments(int DoctorsId)
        {
            var appointments = await hospitalContex.Appointments
            .Where(a => a.DoctorID == DoctorsId).Include(a => a.Doctor)
                    .ToListAsync();
            return appointments;

        }

        public async Task<Doctors?> GetProfile(int DoctorsId)
        {
            return await hospitalContex.Doctors.
                Include(D => D.laboratories).
                Include(D => D.Appointments).
                Include(D => D.Inpatient_Admission).FirstOrDefaultAsync(i => i.ID == DoctorsId);
        }


        public async Task Update(Doctors doctors, int DoctorId)
        {
            Doctors doctors1 = await hospitalContex.Doctors.FindAsync(DoctorId);
            if (doctors1 is not null)
            {
                doctors1.Name = doctors.Name;
                doctors1.specialization = doctors.specialization;
                doctors1.WorkingHours = doctors.WorkingHours;
                doctors1.Email = doctors.Email;
                doctors1.DateOfBirth = doctors.DateOfBirth;
                doctors1.IsAvailable = doctors.IsAvailable;
                doctors1.Experience = doctors.Experience;
                doctors1.Gander = doctors.Gander;
                doctors1.JoiningDate = doctors.JoiningDate;
                doctors1.DepartmentId = doctors.DepartmentId;
                //doctors1.StaffMemberID = doctors.StaffMemberID;
                doctors1.ApplicationUser = doctors.ApplicationUser;
                await Save();

            }
        }

        public async Task<bool> UpdateAppointmentStatusAsync(int appointmentId, string newStatus)
        {
            Appointments appointments = await hospitalContex.Appointments.FindAsync(appointmentId);
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
