
using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Hospital.Application
{
    public class CreateAccountantdto
    {
        [Required]
        [MaxLength(50, ErrorMessage = "Name Must be less Than 50 ")]
        [MinLength(3, ErrorMessage = "Name Must be Greater Than 3 ")]
        [Display(Name = " Patient Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please select gender")]
        [Display(Name = "Gender")]

        public string Gender { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        public string? Certification { get; set; }
        public string RoleName { get; set; }
        //public int? StaffId { get; set; }
        //public string? UserId { get; set; }
        //public List<Billing>?billings { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
