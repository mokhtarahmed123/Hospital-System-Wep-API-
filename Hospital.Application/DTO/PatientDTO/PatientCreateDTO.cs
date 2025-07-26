using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Hospital.Application.DTO.PatientDTO
{
    public class PatientCreateDTO
    {

        [Required(ErrorMessage ="Name Is Required ")]
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

        [Required]
        public int Age { get; set; }

        [Required(ErrorMessage = "Please upload an image")]
        [DataType(DataType.Upload)]
        [Display(Name = "Upload Image")]
        [NotMapped]

        public IFormFile? Image { get; set; }
        public string? ImagePath { get; set; }
        //[Required]

        //public string? Password { get; set; }

    }
}
