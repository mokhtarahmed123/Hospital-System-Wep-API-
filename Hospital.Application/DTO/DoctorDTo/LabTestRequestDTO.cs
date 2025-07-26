using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Hospital.Application.DTO.DoctorDTo
{
    public class LabTestRequestDTO
    {
        [Required]
        public string PatientEmail { get; set; }

        [Required]
        public string DoctorEmail { get; set; }

        [Required(ErrorMessage = "Test name is required")]
        [StringLength(100)]
        public string TestType { get; set; }
        public DateTime RequestedDate { get; set; } = DateTime.Now;

        public string? Status { get; set; } = "Pending";
    }
}
