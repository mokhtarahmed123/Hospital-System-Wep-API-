using HospitalAPI.Hospital.Domain.Models;

namespace HospitalAPI.Hospital.Application.DTO.PatientDTO
{
    public class PatientMedicalReportDTO
    {
        public int PatientID { get; set; }
        public string Name { get; set; }
        public List<Laboratory>? LabResults { get; set; } = new();
        public List<Appointments>? Appointments { get; set; } = new();
        public List<Billing>? Billings { get; set; } = new();
        public DateTime GeneratedDate { get; set; } = DateTime.Now;
    }
}
