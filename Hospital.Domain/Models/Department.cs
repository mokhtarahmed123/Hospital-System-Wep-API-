using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace HospitalAPI.Hospital.Domain.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public List<Rooms> rooms { get; set; } = new();
        public List<Staff_Management> staff_Management { get; set; } = new();
        public List<Inpatient_Admission> inpatient_Admission { get; set; } = new();
        public List<Doctors> Doctor { get; set; } = new();

    }
}