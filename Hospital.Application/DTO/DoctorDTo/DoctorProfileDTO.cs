using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Hospital.Application.DTO.DoctorDTo
{
    public class DoctorProfileDTO
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Experience { get; set; }
        public int WorkingHours { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime? JoiningDate { get; set; }
    }
}
