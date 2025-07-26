using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Hospital.Domain.Models
{
    public class Billing
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int TotalAmount { get; set; }

        [Required]
        [RegularExpression("Pending | Completed | Cancelled",
            ErrorMessage = "Status must be: Pending, Completed, or Cancelled")]
        public string Status { get; set; }
        public DateTime? DateIssued { get; set; } = DateTime.Now;

        [Required]
        [ForeignKey("Patient")]
        public int PatientId { get; set; }
        public Patients Patient { get; set; }
        [ForeignKey("Accountant")]
        public int? AccountantId { get; set; }
        public Accountant? Accountant { get; set; }

    }
}