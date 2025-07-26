using HospitalAPI.Exceptions;
using HospitalAPI.Hospital.Application.DTO.PatientDTO;
using HospitalAPI.Hospital.Domain;
using HospitalAPI.Hospital.Domain.Models;
using HospitalAPI.Hospital.Infrastructure;
using HospitalAPI.Hospital.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HospitalAPI.Hospital.Application
{
    public class PatientsService : IPatientsService
    {
        private readonly HospitalContex hospitalContex;
        private readonly IWebHostEnvironment webHostEnvironment;
        PatientsRepository patientsRepository;
        private readonly UserManager<UserApplication> userManager;
        private readonly RoleManager<RoleApplication> roleManager;
        public PatientsService(HospitalContex hospitalContex, IWebHostEnvironment webHostEnvironment, UserManager<UserApplication> userManager, RoleManager<RoleApplication> roleManager)
        {
            this.hospitalContex = hospitalContex;
            this.webHostEnvironment = webHostEnvironment;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<AppointmentCreateDTO> CreateAppointmentAsync(AppointmentCreateDTO appointmentDto)
        {
            var existing = await hospitalContex.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a =>
                    a.Doctor.Email == appointmentDto.DoctorEmail &&
                    a.Patient.Email == appointmentDto.PatientEmail &&
                    a.AppointmentDate == appointmentDto.AppointmentDate);


            if (existing != null)
                throw new Exception("Appointment already exists");

            var doctor = await hospitalContex.Doctors
                .FirstOrDefaultAsync(d => d.Email == appointmentDto.DoctorEmail);
            if (doctor == null)
                throw new NotFoundException("Doctor not found");

            var patient = await hospitalContex.Patients
                .FirstOrDefaultAsync(p => p.Email == appointmentDto.PatientEmail);
            if (patient == null)
                throw new NotFoundException("Patient not found");

            var appointment = new Appointments
            {
                PatientID = patient.ID,
                DoctorID = doctor.ID,
                AppointmentDate = appointmentDto.AppointmentDate,
                Status = appointmentDto.Status,
            };

            await hospitalContex.Appointments.AddAsync(appointment);
            await hospitalContex.SaveChangesAsync();

            return appointmentDto;
        }

        //public async Task<PatientCreateDTO> CreatePatientAsync(PatientCreateDTO patient)
        //{
        //    string imagePath = null;
        //    if (patient.Image != null && patient.Image.Length > 0)
        //    {
        //        var UploaderFolder = Path.Combine(webHostEnvironment.WebRootPath, "Image");
        //        Directory.CreateDirectory(UploaderFolder);
        //        var uniqueFileName = Guid.NewGuid().ToString() + "_" + patient.Image.FileName;
        //        var filePath = Path.Combine(UploaderFolder, uniqueFileName);
        //        using (var fileStream = new FileStream(filePath, FileMode.Create))
        //        {
        //            await patient.Image.CopyToAsync(fileStream);
        //        }
        //        imagePath = Path.Combine(UploaderFolder, uniqueFileName);
        //    }
        //    var Patient = new Patients
        //    {
        //        Name = patient.Name,
        //        Age = patient.Age,
        //        ImagePath = imagePath,
        //        Phone = patient.Phone,
        //        Address = patient.Address,
        //        Gander = patient.Gender,
        //        Email = patient.Email,
        //        //Password = patient.Password

        //    };
        //    await hospitalContex.Patients.AddAsync(Patient);
        //    await hospitalContex.SaveChangesAsync();
        //    return patient;
        //}
        public async Task<bool> DeletePatientAsync(string email)
        {
            var patient = await hospitalContex.Patients
                .Include(p => p.ApplicationUser) 
                .FirstOrDefaultAsync(p => p.Email == email);

            if (patient == null)
                throw new NotFoundException("User Not Found");

            var user = await userManager.FindByIdAsync(patient.ApplicationUserId);
            if (user != null)
            {
                var roles = await userManager.GetRolesAsync(user);
                if (roles.Any())
                {
                    var roleRemovalResult = await userManager.RemoveFromRolesAsync(user, roles);
                    if (!roleRemovalResult.Succeeded)
                        throw new Exception("Failed to remove user roles.");
                }

                var userDeletionResult = await userManager.DeleteAsync(user);
                if (!userDeletionResult.Succeeded)
                    throw new Exception("Failed to delete associated user identity.");
            }

            hospitalContex.Patients.Remove(patient);
            await hospitalContex.SaveChangesAsync();

            return true;
        }



        public async Task<List<AppointmentsDTO>> GetAppointmentsAsync(string Email)
        {

            var appointments = await hospitalContex.Appointments.Include(a=>a.Patient)
             .Where(a => a.Patient.Email == Email)
          .Select(a => new AppointmentsDTO
          {
              AppointmentID = a.ID,
              PatientID = a.PatientID,
              DoctorID = a.DoctorID,
              DoctorName = a.Doctor.Name,
              Name = a.Patient.Name,
              AppointmentDate = a.AppointmentDate,
              Status = a.Status
          })
                  .ToListAsync();
            return appointments;
        }
        public async Task<List<Billingdto>> GetBillingHistoryAsync(string Email)
        {

            var Billing = await hospitalContex.Billing.Include(A=>A.Patient)
             .Where(a => a.Patient.Email == Email)
          .Select(a => new Billingdto
          {
              ID = a.PatientId,
              Name = a.Patient.Name,
              TotalPaid = a.TotalAmount,
              Status = a.Status,
          })
                  .ToListAsync();
            return Billing;
        }
        public async Task<UpdateAppoinmentDTO> UpdateAppoinment(UpdateAppoinmentDTO appointmentDto, int AppID)
        {
            var existingAppointment = await hospitalContex.Appointments.FindAsync(AppID);
            if (existingAppointment == null)
                throw new NotFoundException("Appointment not found.");

            var doctor = await hospitalContex.Doctors.FirstOrDefaultAsync(a=>a.Email == appointmentDto.EmailDoctor);
            if (doctor == null)
                throw new NotFoundException("Doctor not found.");

            var patient = await hospitalContex.Patients.FirstOrDefaultAsync(a => a.Email == appointmentDto.EmailPatient);
            if (patient == null)
                throw new NotFoundException("Patient not found.");

            existingAppointment.AppointmentDate = appointmentDto.AppointmentDate;
            existingAppointment.DoctorID = doctor.ID;
            existingAppointment.PatientID = patient.ID;
            existingAppointment.Status = appointmentDto.Status;

            await hospitalContex.SaveChangesAsync();

            return new UpdateAppoinmentDTO
            {
                EmailDoctor = doctor.Email,
                EmailPatient = patient.Email,
                AppointmentDate = existingAppointment.AppointmentDate,

                Status = existingAppointment.Status
            };
        }

        public async Task<List<LaboratoryDTO>> GetLabResultsAsync(string Email)
        {

            var Laboratory = await hospitalContex.Laboratory.Include(A=>A.Patient)
             .Where(a => a.Patient.Email == Email)
          .Select(a => new LaboratoryDTO
          {
              ID = a.PatientId,
              Name = a.Patient.Name,
              DoctorID = a.DoctorId,
              DoctorName = a.doctor.Name,
              LabTestType = a.TestType,
              LabStatus = a.Status.ToString(),

              LabDate = a.RequestDate,

          })
                  .ToListAsync();
            return Laboratory;
        }

        public async Task<PatientCreateDTO> GetProfileAsync(string Email)
        {
            var Patient = await hospitalContex.Patients.FirstOrDefaultAsync( e=>e.Email == Email );
            if (Patient == null) throw new NotFoundException("Not Found");
            PatientCreateDTO patientCreate = new PatientCreateDTO
            {
                Name = Patient.Name,
                ImagePath = Patient.ImagePath,
                Phone = Patient.Phone,
                Email = Patient.Email,
                Gender = Patient.Gander,
                Address = Patient.Address,
                Age = Patient.Age,
            };
            return patientCreate;
        }

        public async Task<List<SearchDoctorsDTO>> SearchDoctorsAsync(string? name = null, string? specialization = null)
        {
            var query = hospitalContex.Doctors.AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(d => d.Name.Contains(name));
            }

            if (!string.IsNullOrEmpty(specialization))
            {
                query = query.Where(d => d.specialization.Contains(specialization));
            }

            var result = await query.Select(d => new SearchDoctorsDTO
            {
                DoctorID = d.ID,
                Name = d.Name,
                Specialization = d.specialization,
                PhoneNumber = d.Phone,
                Email = d.Email,
                Gender = d.Gander,
                YearsOfExperience = d.Experience,
                AvailabilityStatus = d.IsAvailable,


            }).ToListAsync();

            return result;
        }

        public async Task<PatientCreateDTO> UpdatePatientAsync(PatientCreateDTO patient,string Email)
        {
            var Patient = await hospitalContex.Patients.FirstOrDefaultAsync(I=>I.Email == Email);
            if (patient is null)throw new NotFoundException("User Not Found");
            Patient.Name = patient.Name;
            Patient.Email = patient.Email;
            Patient.Age = patient.Age;
            Patient.Address = patient.Address;
            Patient.Phone = patient.Phone;
            Patient.Gander = patient.Gender;
            Patient.ImagePath = patient.ImagePath;
            await patientsRepository.Save();
            return patient;
        }


    }
}
