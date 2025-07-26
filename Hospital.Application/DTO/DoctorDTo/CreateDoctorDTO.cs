
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Hospital.Application
{
    public class CreateDoctorDTO
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Name must be less than 50 characters")]
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters")]
        [Display(Name = "Doctor Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Specialization is required")]
        public string Specialization { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Gender is required")]
        public string Gender { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Experience is required")]
        public int Experience { get; set; }

        [Required(ErrorMessage = "Working hours are required")]
        public int WorkingHours { get; set; }

        [Required]
        public bool IsAvailable { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? JoiningDate { get; set; }

        public string DepartmentName {  get; set; }
        //public string PhoneNumber { get; internal set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
