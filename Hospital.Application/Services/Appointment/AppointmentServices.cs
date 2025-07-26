using HospitalAPI.Hospital.Application.DTO.AppointmentDTO;
using HospitalAPI.Hospital.Application.Services.Appointment;
using HospitalAPI.Hospital.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Hospital.Application
{
    public class AppointmentServices : IAppointmentServices
    {
        private readonly HospitalContex contex;

        public AppointmentServices(HospitalContex contex)
        {
            this.contex = contex;
        }
        public async Task<bool> ConfirmAppointment(int id, ConfirmAppointment confirm)
        {
            var IsFound = await contex.Appointments.FindAsync(id);
            if (IsFound == null) return false;
            IsFound.Status = confirm.Status;
            await contex.SaveChangesAsync();
            return true;
        }
        public async Task<bool> Delete(int id)
        {
            var IsFound = await contex.Appointments.FindAsync(id);
            if (IsFound == null) return false;
            contex.Appointments.Remove(IsFound);
            await contex.SaveChangesAsync();
            return true;
        }

        public async Task<EditAppointment?> EditAppointment(int id, EditAppointment edit)
        {
            var appointment = await contex.Appointments
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(i => i.ID == id);

            if (appointment == null) return null;

            appointment.Status = edit.Status;
            appointment.AppointmentDate = edit.AppointmentDate;

            if (!string.IsNullOrEmpty(edit.DoctorEmail))
            {
                var newDoctor = await contex.Doctors.FirstOrDefaultAsync(d => d.Email == edit.DoctorEmail);
                if (newDoctor == null) return null; 
                appointment.DoctorID = newDoctor.ID;
            }

            await contex.SaveChangesAsync();
            return edit;
        }


        public async Task<List<GetAllAppointmentsDTO>> GetAllAppointmentsAsync()
        {
            return await contex.Appointments.
                Include(a => a.Patient).
                Include(a => a.Doctor).
                Select(a => new GetAllAppointmentsDTO
                {
                    DoctorName = a.Doctor.Name,
                    PatientName = a.Patient.Name,
                    AppointmentDate = a.AppointmentDate,
                    Status = a.Status,
                    specialization = a.Doctor.specialization,
                    DoctorID = a.Doctor.ID,
                    PatientID = a.Patient.ID,
                }).ToListAsync();

        }

        public async Task<GetAllAppointmentsDTO> GetAppointmentByIdAsync(int id)
        {
            var App = await contex.Appointments
        .Include(a => a.Doctor)
        .Include(a => a.Patient)
        .FirstOrDefaultAsync(a => a.ID == id);

            if (App == null) return null;
            return new GetAllAppointmentsDTO
            {
                PatientName = App.Patient.Name,
                AppointmentDate = App.AppointmentDate,
                Status = App.Status,
                specialization = App.Doctor.specialization,
                DoctorID = App.Doctor.ID,
                DoctorName = App.Doctor.Name,
                PatientID = App.Patient.ID,
            };

        }

        public async Task<List<GetAllAppointmentsDTO>> GetAppointmentsByPatientIdAsync(string EmailOfPatient)
        {
            return await contex.Appointments.
              Include(a => a.Patient).
          Include(a => a.Doctor).Where(i => i.Patient.Email == EmailOfPatient).
          Select(a => new GetAllAppointmentsDTO
          {
              DoctorName = a.Doctor.Name,
              PatientName = a.Patient.Name,
              AppointmentDate = a.AppointmentDate,
              Status = a.Status,
              specialization = a.Doctor.specialization,
              DoctorID = a.Doctor.ID,
              PatientID = a.Patient.ID,
          }).ToListAsync();

        }

        public async Task<List<GetAllAppointmentsDTO>> SearchDoctorsAsync(FilterAppointment filter)
        {
            var query = contex.Appointments
          .Include(d => d.Patient)
            .Include(d => d.Doctor)
              .AsQueryable();

            if (!string.IsNullOrEmpty(filter.Status))
            {
                query = query.Where(d => d.Status.Contains(filter.Status));
            }
            if (!string.IsNullOrEmpty(filter.specialization))
            {
                query = query.Where(d => d.Doctor.specialization.Contains(filter.specialization));
            }

            //if (filter.PatientID.HasValue)
            //{
            //    query = query.Where(d => d.PatientID == filter.PatientID.Value);
            //}

            //if (filter.DoctorID.HasValue)
            //{
            //    query = query.Where(d => d.DoctorID == filter.DoctorID.Value);
            //}
            return await query.Select(d => new GetAllAppointmentsDTO
            {
                DoctorName = d.Doctor.Name,
                PatientID = d.Patient.ID,
                PatientName = d.Patient.Name,
                specialization = d.Doctor.specialization,
                AppointmentDate = d.AppointmentDate,
                Status = d.Status,
                DoctorID = d.Doctor.ID,

            }).ToListAsync();

        }
    }
}
