using HospitalAPI.Hospital.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HospitalAPI.Hospital.Application.DTO.PatientDTO
{
    public class AppointmentCreateDTO
    {
        //[Required]
        public string? PatientEmail { get; set; }
        [Required]
        public string DoctorEmail { get; set; }
        public string specialization { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime AppointmentDate { get; set; }
        //[JsonIgnore]
        //public List<Doctors>? Doctors { get; set; } = new();



    }
}
