using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Hospital.Domain.Models
{
    public class Patients
    {
        [Key]
        public int ID { get; set; }
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
        public string Gander { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Please upload an image")]
        [DataType(DataType.Upload)]
        [Display(Name = "Upload Image")]
        [NotMapped]
        public IFormFile Image { get; set; }

        public string? ImagePath { get; set; }

        [Required(ErrorMessage = "Please enter Your  Age")]
        public int Age { get; set; }
        //[Required(ErrorMessage = "Please enter Your  Password")]
        //[DataType(DataType.Password)]
        //public string Password { get; set; }

        [ForeignKey("ApplicationUserId")]

        public string? ApplicationUserId { get; set; }

        public UserApplication ApplicationUser { get; set; }

        public List<Billing>? billings { get; set; } = new();
        public List<Inpatient_Admission>? Inpatients { get; set; } = new();
        public List<Appointments>? appointments { get; set; } = new();
        public List<Laboratory>? laboratory { get; set; } = new();

    }
}
