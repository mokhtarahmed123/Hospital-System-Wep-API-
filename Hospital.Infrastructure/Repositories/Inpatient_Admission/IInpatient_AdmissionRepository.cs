using HospitalAPI.Hospital.Domain.Models;
using HospitalAPI.Hospital.Domain.Models;
namespace HospitalAPI.Hospital.Infrastructure
{
    public interface IInpatient_AdmissionRepository
    {
        Task<List<Inpatient_Admission>> GetAll();
        Task<Inpatient_Admission?> GetById(int id);
        Task Add(Inpatient_Admission admission);
        Task Update(Inpatient_Admission admission, int id);
        Task Delete(int id);
        Task<List<Inpatient_Admission>> GetByPatientId(int patientId);
        Task<List<Inpatient_Admission>> GetByDoctorId(int doctorId);
        Task<List<Inpatient_Admission>> GetByDepartmentId(int departmentId);
        Task<List<Inpatient_Admission>> GetByRoomId(int roomId);




    }
}
