using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Hospital.Domain.Models
{
    public class Inpatient_Admission
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime AdmissionDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DischargeDate { get; set; }
        [Required]
        [ForeignKey("Patient")]
        public int PatientID { get; set; }
        public Patients Patient { get; set; }

        [Required]
        [ForeignKey("Doctor")]
        public int DoctorID { get; set; }
        public Doctors Doctor { get; set; }
        [Required]
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
        [Required]
        [ForeignKey("Rooms")]
        public int RoomId { get; set; }
        public Rooms Rooms { get; set; }
    }
}