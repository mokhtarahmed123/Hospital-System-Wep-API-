using HospitalAPI.Hospital.Domain.Models;
using HospitalAPI.Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Hospital.Infrastructure
{
    public class Inpatient_AdmissionRepository : IInpatient_AdmissionRepository
    {
        private readonly HospitalContex hospitalContex;

        public Inpatient_AdmissionRepository(HospitalContex hospitalContex)
        {
            this.hospitalContex = hospitalContex;
        }
        public async Task Add(Inpatient_Admission admission)
        {
            hospitalContex.InpatientAdmissions.Add(admission);
            await Save();

        }

        public async Task Delete(int id)
        {
            Inpatient_Admission inpatient_Admission = await hospitalContex.InpatientAdmissions.FindAsync(id);
            if (inpatient_Admission == null)
            {
                throw new Exception("Not Found");
            }
            hospitalContex.InpatientAdmissions.Remove(inpatient_Admission);
            await Save();
        }

        public async Task<List<Inpatient_Admission>> GetAll()
        {
            return await hospitalContex.InpatientAdmissions.
                Include(a => a.Rooms)
                .Include(b => b.Department).
                Include(b => b.Patient).
                Include(a => a.Doctor).
                ToListAsync();
        }
        public async Task<List<Inpatient_Admission>> GetByDepartmentId(int departmentId)
        {
            return await hospitalContex.InpatientAdmissions.
          Include(a => a.Rooms).Include(b => b.Department).
             Include(b => b.Patient).Include(a => a.Doctor).
         Where(a => a.DepartmentId == departmentId).ToListAsync();
        }

        public async Task<List<Inpatient_Admission>> GetByDoctorId(int doctorId)
        {
            return await hospitalContex.InpatientAdmissions.
              Include(a => a.Rooms).Include(b => b.Department).
              Include(b => b.Patient).Include(a => a.Doctor).
              Where(a => a.DoctorID == doctorId).ToListAsync();
        }

        public async Task<Inpatient_Admission?> GetById(int id)
        {
            return await hospitalContex.InpatientAdmissions.
         Include(a => a.Rooms).Include(b => b.Department).
        Include(b => b.Patient).Include(a => a.Doctor).FirstOrDefaultAsync(i => i.ID == id);

        }

        public async Task<List<Inpatient_Admission>> GetByPatientId(int patientId)
        {
            return await hospitalContex.InpatientAdmissions.
            Include(a => a.Rooms).Include(b => b.Department).
             Include(b => b.Patient).Include(a => a.Doctor).
             Where(a => a.PatientID == patientId).ToListAsync();
        }

        public async Task<List<Inpatient_Admission>> GetByRoomId(int roomId)
        {
            return await hospitalContex.InpatientAdmissions.
            Include(a => a.Rooms).Include(b => b.Department).
        Include(b => b.Patient).Include(a => a.Doctor).Where(i => i.RoomId == roomId)
        .ToListAsync();
        }

        public async Task Update(Inpatient_Admission admission, int id)
        {
            Inpatient_Admission inpatient_Admission = await hospitalContex.InpatientAdmissions.FindAsync(id);
            if (inpatient_Admission == null)
            {
                throw new Exception("Not Found");
            }
            inpatient_Admission.AdmissionDate = admission.AdmissionDate;
            inpatient_Admission.DoctorID = admission.DoctorID;
            inpatient_Admission.PatientID = admission.PatientID;
            inpatient_Admission.DepartmentId = admission.DepartmentId;
            inpatient_Admission.RoomId = admission.RoomId;
            await Save();
        }
        public async Task Save()
        {
            await hospitalContex.SaveChangesAsync();
        }
    }
}
