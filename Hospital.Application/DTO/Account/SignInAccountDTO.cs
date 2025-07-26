
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Hospital.Application
{
    public class SignInAccountDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        //[Required]
        [RegularExpression("Male|Female", ErrorMessage = "Gender must be either Male or Female")]
        public string? Gender { get; set; }
        //[Required]
        public string? Address { get; set; }
        //[Required]  
        [DataType(DataType.Upload)]
        [Display(Name = "Upload Image")]
        //[NotMapped]
        public IFormFile? Image { get; set; }

        //public string? ImagePath { get; set; }
        public int? Age { get; set; }
        //public IFormFile? Image { get; set; }


    }
}
