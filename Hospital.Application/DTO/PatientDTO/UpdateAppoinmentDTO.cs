using HospitalAPI.Hospital.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Hospital.Application.DTO.PatientDTO
{
    public class UpdateAppoinmentDTO
    {

        //[Required]
        public string EmailPatient { get; set; }
        //[Required]
        public string EmailDoctor{ get; set; }
        public string specialization { get; set; }
        public string Status { get; set; } = "Pending";
        public DateTime AppointmentDate { get; set; }
        public List<Doctors>? Doctors { get; set; } = new();

    }
}
