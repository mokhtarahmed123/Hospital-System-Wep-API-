using HospitalAPI.Hospital.Application.DTO.Inpatient_Admission;
using HospitalAPI.Hospital.Application.Services.Inpatient_Admission;
using HospitalAPI.Hospital.Domain.Models;
using HospitalAPI.Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Hospital.Application
{
    public class InpatientAdmissionService : IInpatientAdmissionService
    {
        private readonly HospitalContex contex;

        public InpatientAdmissionService(HospitalContex contex)
        {
            this.contex = contex;
        }
        public async Task<CreateInpatientAdmissionDTO> CreateAdmissionAsync(CreateInpatientAdmissionDTO dto)
        {
            if (dto == null) return null;
            var Dept = await contex.Departments.FirstOrDefaultAsync(i=>i.Name == dto.DepartmentName);
            var Doc = await contex.Doctors.FirstOrDefaultAsync(i=>i.Email == dto.DoctorEmail);
            var Pat = await contex.Patients.FirstOrDefaultAsync(i => i.Email == dto.PatientEmail);
            var room = await contex.Rooms.FirstOrDefaultAsync(i=>i.RoomNumber == dto.RoomNumber);
            if (Dept == null || Doc == null || Pat == null || room == null) return null;

         
            Inpatient_Admission inpatient = new Inpatient_Admission()
            {
                AdmissionDate = dto.AdmissionDate,
                DepartmentId = Dept.Id,
                DischargeDate = dto.DischargeDate,
                DoctorID = Doc.ID,
                RoomId = room.Id,
                PatientID = Pat.ID

            };
            await contex.InpatientAdmissions.AddAsync(inpatient);
            await contex.SaveChangesAsync();
            return dto;
        }

        public async Task<bool> DeleteAdmissionAsync(int admissionId)
        {
            var ISFOUND = await contex.InpatientAdmissions.FindAsync(admissionId);
            if (ISFOUND == null) return false;
            contex.InpatientAdmissions.Remove(ISFOUND);
            await contex.SaveChangesAsync();
            return true;
        }

        public async Task<List<GetAllInpatientAdmissionsDTO>> FilterAdmissionsAsync(InpatientAdmissionFilterDTO filter)
        {
            var query = contex.InpatientAdmissions
                    .Include(r => r.Doctor)
                    .Include(r => r.Patient)
                    .Include(r => r.Rooms)
                    .AsQueryable();


            if (filter.RoomNumber.HasValue)
            {
                query = query.Where(r => r.Rooms.RoomNumber == filter.RoomNumber.Value);
            }

            if (!string.IsNullOrEmpty(filter.DoctorEmail))
            {
                query = query.Where(r => r.Doctor != null && r.Doctor.Email == filter.DoctorEmail);
            }


            return await query.Select(r => new GetAllInpatientAdmissionsDTO
            {
                Id = r.ID,
                AdmissionDate = r.AdmissionDate,
                DischargeDate = r.DischargeDate,
                DoctorName = r.Doctor.Name,
                PatientName = r.Patient.Name,
                RoomNumber = r.Rooms.RoomNumber,
            }).ToListAsync();
        }

        public async Task<GetInpatientAdmissionByIdDTO> GetAdmissionByIdAsync(int admissionId)
        {
            var ISFOUND = await contex.InpatientAdmissions.Include(a => a.Rooms).Include(a => a.Patient).Include(a => a.Department).Include(a => a.Doctor).FirstOrDefaultAsync(i => i.ID == admissionId);
            if (ISFOUND == null) return null;
            return new GetInpatientAdmissionByIdDTO
            {

                AdmissionDate = ISFOUND.AdmissionDate,
                DischargeDate = ISFOUND.DischargeDate,
                DoctorName = ISFOUND.Doctor.Name,
                PatientName = ISFOUND.Patient.Name,
                RoomNumber = ISFOUND.Rooms.RoomNumber,
            };

        }

        public async Task<List<GetAllInpatientAdmissionsDTO>> GetAdmissionsByPatientIdAsync(string EmailPatient)
        {
            return await contex.InpatientAdmissions
                .Include(a => a.Patient).Include(a => a.Department).Include(a => a.Doctor).Include(a => a.Rooms).
                Where(i => i.Patient.Email == EmailPatient).
                Select(a => new GetAllInpatientAdmissionsDTO
                {
                    Id = a.ID,
                    AdmissionDate = a.AdmissionDate,
                    DischargeDate = a.DischargeDate,
                    DoctorName = a.Doctor.Name,
                    PatientName = a.Patient.Name,
                    RoomNumber = a.Rooms.RoomNumber,
                }).ToListAsync();
            ;

        }

        public async Task<List<GetAllInpatientAdmissionsDTO>> GetAllAdmissionsAsync()
        {
            return await contex.InpatientAdmissions
                  .Include(a => a.Patient).Include(a => a.Department).Include(a => a.Doctor).Include(a => a.Rooms).
                  Select(a => new GetAllInpatientAdmissionsDTO
                  {
                      Id = a.ID,
                      AdmissionDate = a.AdmissionDate,
                      DischargeDate = a.DischargeDate,
                      DoctorName = a.Doctor.Name,
                      PatientName = a.Patient.Name,
                      RoomNumber = a.Rooms.RoomNumber,
                  }).ToListAsync();
            ;
        }

        public async Task<List<GetAllInpatientAdmissionsDTO>> GetCurrentAdmissionsAsync()
        {
            return await contex.InpatientAdmissions
                  .Include(a => a.Patient).Include(a => a.Department).Include(a => a.Doctor).Include(a => a.Rooms).Where(i => i.DischargeDate == null).
                  Select(a => new GetAllInpatientAdmissionsDTO
                  {
                      AdmissionDate = a.AdmissionDate,
                      DischargeDate = a.DischargeDate,
                      DoctorName = a.Doctor.Name,
                      PatientName = a.Patient.Name,
                      RoomNumber = a.Rooms.RoomNumber,
                  }).ToListAsync();
            ;
        }

        public async Task<bool> UpdateAdmissionAsync(int admissionId, UpdateInpatientAdmissionDTO dto)
        {
            var Dept = await contex.Departments.FirstOrDefaultAsync(i => i.Name == dto.DepartmentName);
            var Doc = await contex.Doctors.FirstOrDefaultAsync(i => i.Email == dto.DoctorEmail);
            //var Pat = await contex.Patients.FirstOrDefaultAsync(i => i.Email == dto.PatientEmail);
            var room = await contex.Rooms.FirstOrDefaultAsync(i => i.RoomNumber == dto.RoomNumber);
            if (Dept == null || Doc == null || room == null) return false;

            var isfound = await contex.InpatientAdmissions.FindAsync(admissionId);
            if (isfound == null) return false;
            isfound.DepartmentId = Dept.Id;
            isfound.DoctorID = Doc.ID;
            isfound.DischargeDate = dto.DischargeDate;
            isfound.RoomId = room.Id;
            await contex.SaveChangesAsync();
            return true;

        }
    }
}
