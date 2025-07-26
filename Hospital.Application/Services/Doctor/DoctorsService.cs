using HospitalAPI.Exceptions;
using HospitalAPI.Hospital.Application.DTO.DoctorDTo;
using HospitalAPI.Hospital.Domain;
using HospitalAPI.Hospital.Domain.Models;
using HospitalAPI.Hospital.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Runtime.Intrinsics.Arm;

namespace HospitalAPI.Hospital.Application
{
    public class DoctorsService : IDoctorsService
    {
        private readonly HospitalContex contex;
        private readonly RoleManager<RoleApplication> roleManager;
        private readonly UserManager<UserApplication> userManager;


        public DoctorsService(HospitalContex contex, UserManager<UserApplication> userManager, RoleManager<RoleApplication> roleManager)
        {
            this.contex = contex;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        public async Task<CreateDoctorDTO> CreateDoctorAsync(CreateDoctorDTO dto)
        {
            var department = await contex.Departments
                .FirstOrDefaultAsync(d => d.Name == dto.DepartmentName);

            if (department == null)
                throw new NotFoundException("Department does not exist.");

            if (await userManager.FindByEmailAsync(dto.Email) != null)
                throw new BadRequestException("Email already exists.");

            if (await userManager.FindByNameAsync(dto.Name) != null)
                throw new BadRequestException("Username already exists.");

            var user = new UserApplication
            {
                UserName = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.Phone
            };

            var result = await userManager.CreateAsync(user, dto.Password);
            if (!result.Succeeded)
                throw new Exception("Failed to create user: " + string.Join(", ", result.Errors.Select(e => e.Description)));

            if (!await userManager.IsInRoleAsync(user, "Doctor"))
            {
                await userManager.AddToRoleAsync(user, "Doctor");
            }

            var doctor = new Doctors
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Gander = dto.Gender,
                specialization = dto.Specialization,
                DateOfBirth = dto.DateOfBirth,
                Experience = dto.Experience,
                WorkingHours = dto.WorkingHours,
                IsAvailable = dto.IsAvailable,
                JoiningDate = dto.JoiningDate ?? DateTime.Now,
                DepartmentId = department.Id,
                ApplicationUserId = user.Id
            };

            await contex.Doctors.AddAsync(doctor);

            try
            {
                await contex.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await userManager.DeleteAsync(user); 
                throw new Exception("Error saving doctor: " + (ex.InnerException?.Message ?? ex.Message));
            }

            return dto;
        }




        public async Task<bool> DeleteDoctorAsync(string email)
        {
            var doctor = await contex.Doctors.Include(d => d.ApplicationUser)
                                             .FirstOrDefaultAsync(d => d.Email == email);

            if (doctor == null) return false;

            var user = await userManager.FindByIdAsync(doctor.ApplicationUserId);
            if (user != null)
            {
                // 1. Remove user roles first
                var roles = await userManager.GetRolesAsync(user);
                if (roles.Any())
                {
                    var roleRemovalResult = await userManager.RemoveFromRolesAsync(user, roles);
                    if (!roleRemovalResult.Succeeded)
                    {
                        throw new Exception("Failed to remove user roles: " + string.Join(", ", roleRemovalResult.Errors.Select(e => e.Description)));
                    }
                }

                // 2. Delete user identity
                var deleteResult = await userManager.DeleteAsync(user);
                if (!deleteResult.Succeeded)
                {
                    throw new Exception("Failed to delete user: " + string.Join(", ", deleteResult.Errors.Select(e => e.Description)));
                }
            }

            // 3. Delete doctor entity
            contex.Doctors.Remove(doctor);
            await contex.SaveChangesAsync();

            return true;
        }



        public async Task<List<DoctorAppointmentsDTO>> GetAppointmentsAsync(string Email)
        {
            var appointments = await contex.Appointments
                .Include(a => a.Doctor)
                .Include(a => a.Patient)
                .Where(a => a.Doctor.Email == Email)
                .Select(a => new DoctorAppointmentsDTO
                {
                    AppointmentId = a.ID,
                    PatientId = a.PatientID,
                    DoctorID = a.DoctorID,
                    AppointmentDate = a.AppointmentDate,
                    PatientName = a.Patient.Name,
                    PatientPhone = a.Patient.Phone,
                    Status = a.Status
                })
                .ToListAsync();

            return appointments;
        }
        public async Task<CreateDoctorDTO> GetProfileAsync(string Email)
        {
            var DoctorFromDatabase = await contex.Doctors
                .Include(a => a.laboratories)
                .Include(a => a.Appointments)
                .FirstOrDefaultAsync(a=>a.Email == Email);
            var Dept = await contex.Departments.Include(a=>a.Doctor).FirstOrDefaultAsync(A=>A.Id == DoctorFromDatabase.DepartmentId);
            if (Dept == null) return null;
            if (DoctorFromDatabase == null ) return null;

            return new CreateDoctorDTO()
            {
                Name = DoctorFromDatabase.Name,
                DateOfBirth = DoctorFromDatabase.DateOfBirth,
                Email = DoctorFromDatabase.Email,
                Specialization = DoctorFromDatabase.specialization,
                Experience = DoctorFromDatabase.Experience,
                Gender = DoctorFromDatabase.Gander,
                Phone = DoctorFromDatabase.Phone,
                IsAvailable = DoctorFromDatabase.IsAvailable,
                WorkingHours = DoctorFromDatabase.WorkingHours,
                JoiningDate = DoctorFromDatabase.JoiningDate,
                DepartmentName = Dept.Name
            };
        }

        public async Task<LabTestRequestDTO> RequestLabAsync(LabTestRequestDTO laboratoryDTO)
        {
            if (laboratoryDTO == null)
                throw new ArgumentNullException(nameof(laboratoryDTO));

            // Get Patient ID from Email
            var patient = await contex.Patients
                .Include(p => p.ApplicationUser)
                .FirstOrDefaultAsync(p => p.ApplicationUser.Email == laboratoryDTO.PatientEmail);

            if (patient == null)
                return null;

            // Get Doctor ID from Email
            var doctor = await contex.Doctors
                .Include(d => d.ApplicationUser)
                .FirstOrDefaultAsync(d => d.ApplicationUser.Email == laboratoryDTO.DoctorEmail);

            if (doctor == null)
                return null;

            // Check for existing request
            var existingRequest = await contex.Laboratory
                .FirstOrDefaultAsync(i => i.PatientId == patient.ID && i.DoctorId == doctor.ID && i.TestType == laboratoryDTO.TestType);

            if (existingRequest != null)
                return null;

            // Create new lab request
            var newLab = new Laboratory
            {
                PatientId = patient.ID,
                DoctorId = doctor.ID,
                RequestDate = DateTime.Now,
                Status = laboratoryDTO.Status ?? "Pending",
                TestType = laboratoryDTO.TestType
            };

            await contex.Laboratory.AddAsync(newLab);
            await contex.SaveChangesAsync();

            return new LabTestRequestDTO
            {
                PatientEmail = laboratoryDTO.PatientEmail,
                DoctorEmail = laboratoryDTO.DoctorEmail,
                RequestedDate = newLab.RequestDate,
                Status = newLab.Status,
                TestType = newLab.TestType
            };
        }


        public async Task<CreateDoctorDTO> UpdateDoctorAsync(CreateDoctorDTO Doctor, string email)
        {
            var OldDoctor = await contex.Doctors.FirstOrDefaultAsync(d => d.Email == email);
            if (OldDoctor == null) return null;

            var Dept = await contex.Departments
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.Id == OldDoctor.DepartmentId);
            if (Dept == null) return null;

            // تحديث بيانات جدول الأطباء
            OldDoctor.Name = Doctor.Name;
            OldDoctor.Phone = Doctor.Phone;
            OldDoctor.DateOfBirth = Doctor.DateOfBirth;
            OldDoctor.JoiningDate = Doctor.JoiningDate;
            OldDoctor.Email = Doctor.Email;
            OldDoctor.Experience = Doctor.Experience;
            OldDoctor.IsAvailable = Doctor.IsAvailable;
            OldDoctor.specialization = Doctor.Specialization;
            OldDoctor.WorkingHours = Doctor.WorkingHours;
            OldDoctor.DepartmentId = Dept.Id;

            // تحديث AspNetUsers
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                user.UserName = Doctor.Name;
                user.Email = Doctor.Email;
                user.PhoneNumber = Doctor.Phone;

                var updateResult = await userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    throw new Exception("Failed to update user identity data");
                }
            }

            await contex.SaveChangesAsync();
            return Doctor;
        }



    }
}
