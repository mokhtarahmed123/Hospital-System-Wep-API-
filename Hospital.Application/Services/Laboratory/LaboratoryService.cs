using HospitalAPI.Hospital.Application.DTO.LaboratoryDTO;
using HospitalAPI.Hospital.Application.Services.Laboratory;
using HospitalAPI.Hospital.Infrastructure.Data;

using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Hospital.Application
{
    public class LaboratoryService : ILaboratoryService
    {
        private readonly HospitalContex contex;

        public LaboratoryService(HospitalContex contex)
        {
            this.contex = contex;
        }
        public async Task<bool> Confirm(int id, string Status)
        {
            var ISFOUND = await contex.Laboratory.FindAsync(id);
            if (ISFOUND == null) return false;
            ISFOUND.Status = Status;
            await contex.SaveChangesAsync();
            return true;

        }

        public async Task<bool> Delete(int id)
        {
            var ISFOUND = await contex.Laboratory.FindAsync(id);
            if (ISFOUND == null) return false;
            contex.Laboratory.Remove(ISFOUND);
            await contex.SaveChangesAsync();
            return true;

        }

        public async Task<List<GetAllLabRequests>> GetAllLabRequests()
        {
            return await contex.Laboratory.Include(a => a.doctor).
                Include(a => a.Patient).Select(a => new GetAllLabRequests
                {
                    DoctorId = a.DoctorId,
                    PatientId = a.PatientId,
                    DoctorName = a.doctor.Name,
                    PatientName = a.Patient.Name,
                    Status = a.Status,
                    RequestDate = a.RequestDate,
                    TestType = a.TestType,

                }).ToListAsync();
        }

        public async Task<List<GetAllLabRequests>> GetByDoctorIdAsync(string DoctorEmail)
        {
            return await contex.Laboratory.Include(a => a.doctor).
           Include(a => a.Patient).Where(a => a.doctor.Email == DoctorEmail).Select(a => new GetAllLabRequests
           {
               DoctorId = a.DoctorId,
               PatientId = a.PatientId,
               DoctorName = a.doctor.Name,
               PatientName = a.Patient.Name,
               Status = a.Status,
               RequestDate = a.RequestDate,
               TestType = a.TestType,

           }).ToListAsync();

        }

        public async Task<List<GetAllLabRequests>> GetByID(int id)
        {
            return await contex.Laboratory.Include(a => a.doctor).
           Include(a => a.Patient).Where(a => a.Id == id).Select(a => new GetAllLabRequests
           {
               DoctorId = a.DoctorId,
               PatientId = a.PatientId,
               DoctorName = a.doctor.Name,
               PatientName = a.Patient.Name,
               Status = a.Status,
               RequestDate = a.RequestDate,
               TestType = a.TestType,

           }).ToListAsync();
        }

        public async Task<List<GetAllLabRequests>> GetByPatientIdAsync(string PatientEmail)
        {
            return await contex.Laboratory.Include(a => a.doctor).
           Include(a => a.Patient).Where(a => a.Patient.Email == PatientEmail).Select(a => new GetAllLabRequests
           {
               DoctorId = a.DoctorId,
               PatientId = a.PatientId,
               DoctorName = a.doctor.Name,
               PatientName = a.Patient.Name,
               Status = a.Status,
               RequestDate = a.RequestDate,
               TestType = a.TestType,

           }).ToListAsync();
        }

        public async Task<List<GetAllLabRequests>> GetByStatusAsync(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return new List<GetAllLabRequests>();
            return await contex.Laboratory.Include(a => a.doctor).
           Include(a => a.Patient).
           Where(a => a.Status.ToLower() == status.ToLower()).
           Select(a => new GetAllLabRequests
           {
               DoctorId = a.DoctorId,
               PatientId = a.PatientId,
               DoctorName = a.doctor.Name,
               PatientName = a.Patient.Name,
               Status = a.Status,
               RequestDate = a.RequestDate,
               TestType = a.TestType,

           }).ToListAsync();
        }

        public async Task<List<GetAllLabRequests>> GetByTestTypeAsync(string testType)
        {
            if (string.IsNullOrWhiteSpace(testType))
                return new List<GetAllLabRequests>();
            return await contex.Laboratory.Include(a => a.doctor).
           Include(a => a.Patient).
           Where(a => a.TestType.ToLower() == testType.ToLower()).
           Select(a => new GetAllLabRequests
           {
               DoctorId = a.DoctorId,
               PatientId = a.PatientId,
               DoctorName = a.doctor.Name,
               PatientName = a.Patient.Name,
               Status = a.Status,
               RequestDate = a.RequestDate,
               TestType = a.TestType,

           }).ToListAsync();
        }

        public async Task<bool> Update(int id, UpdateLabDTO update)
        {
            var ISFOUND = await contex.Laboratory.FindAsync(id);
            if (ISFOUND == null) return false;
            ISFOUND.Status = update.Status;
            ISFOUND.TestType = update.TestType;
           await contex.SaveChangesAsync();
            return true;



        }
    }
}
