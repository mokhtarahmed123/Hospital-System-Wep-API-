using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Hospital.Application.DTO.DoctorDTo
{
    public class PatientAdmissionDTO
    {
        [Required]
        public int Patient { get; set; }
        [Required]
        public int DepartmentId { get; set; }
        [Required]
        public int RoomtId { get; set; }
        [Required]
        public int DoctortId { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime AdmissionDate { get; set; } = DateTime.Now;
        public DateTime DischargeDate { get; set; }

    }
}
