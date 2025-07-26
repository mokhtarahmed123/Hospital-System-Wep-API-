using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace HospitalAPI.Hospital.Domain.Models
{
    public class Rooms
    {
        public int Id { get; set; }
        [Required]
        [Range(1, 1000)]

        public int RoomNumber { get; set; }
        [Required]
        [Range(1, 10)]

        public int Capacity { get; set; }
        [Required]
        public bool IsAvailable { get; set; }
        [Required]
        [ForeignKey("Department")]
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }
        public List<Inpatient_Admission> inpatient_Admissions { get; set; } = new();
    }
}
