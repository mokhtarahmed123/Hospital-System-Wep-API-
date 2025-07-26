using HospitalAPI.Hospital.Domain.Models;

namespace HospitalAPI.Hospital.Application.DTO.PatientDTO
{
    public class LaboratoryDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? DoctorID { get; set; }
        public string? DoctorName { get; set; }
        public string? LabStatus { get; set; }
        public string? LabResult { get; set; }
        public DateTime? LabDate { get; set; }
        public string? LabTestType { get; set; }
        public List<Laboratory>? LabResults { get; set; } = new();


    }
}
