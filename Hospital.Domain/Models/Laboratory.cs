using Microsoft.Extensions.FileProviders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Hospital.Domain.Models
{
    public class Laboratory
    {
        [Key]
        public int Id { get; set; }
        [Required]

        public string TestType { get; set; }
        [Required]
        [RegularExpression("Pending | Completed | Cancelled",
            ErrorMessage = "Status must be: Pending, Completed, or Cancelled")]
        public string Status { get; set; }
        [NotMapped]
        public IFormFile? ResultFile { get; set; }
        public string? ResultFilePath { get; set; }
        public DateTime RequestDate { get; set; } = DateTime.Now;
        [Required]
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patients Patient { get; set; }
        [Required]
        [ForeignKey("doctor")]
        public int DoctorId { get; set; }
        public Doctors doctor { get; set; }


    }
}