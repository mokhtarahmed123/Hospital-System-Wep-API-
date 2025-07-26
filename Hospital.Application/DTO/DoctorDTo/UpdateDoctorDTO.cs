using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Hospital.Application.DTO.DoctorDTo
{
    public class UpdateDoctorDTO
    {
        [Required]
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "Name must be less than 50 characters")]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters")]
        public string? Name { get; set; }

        public string? Specialization { get; set; }

        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string? Phone { get; set; }

        public string? Gender { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        public int? Experience { get; set; }

        public int? WorkingHours { get; set; }

        public bool? IsAvailable { get; set; }

        [DataType(DataType.Date)]
        public DateTime? JoiningDate { get; set; }
    }
}
