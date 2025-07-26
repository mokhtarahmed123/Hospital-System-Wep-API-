using HospitalAPI.Hospital.Domain.Models;

namespace HospitalAPI.Hospital.Application.DTO.PatientDTO
{
    public class AppointmentsDTO
    {
        public int PatientID { get; set; }
        public int AppointmentID { get; set; }
        public string Name { get; set; }
        public int? DoctorID { get; set; }
        public string? DoctorName { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public string? Status { get; set; }
        public List<Appointments>? Appointments { get; set; } = new();
    }
}
