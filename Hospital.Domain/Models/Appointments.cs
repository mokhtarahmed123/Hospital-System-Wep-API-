using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace HospitalAPI.Hospital.Domain.Models
{
    public class Appointments
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [RegularExpression("Pending | Completed | Cancelled",
            ErrorMessage = "Status must be: Pending, Completed, or Cancelled")]
        public string Status { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime AppointmentDate { get; set; }

        [Required]
        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public Patients Patient { get; set; }

        [Required]
        [ForeignKey("Doctor")]
        public int DoctorID { get; set; }
        public Doctors Doctor { get; set; }


    }
}