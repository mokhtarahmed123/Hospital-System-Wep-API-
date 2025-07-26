using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Hospital.Application.DTO.DoctorDTo
{
    public class DoctorAppointmentsDTO
    {
        [Key]
        public int DoctorID { get; set; }
        public int AppointmentId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public string PatientPhone { get; set; }
    }
}
