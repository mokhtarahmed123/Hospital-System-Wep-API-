using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Hospital.Domain.Models
{
    public class Accountant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Certification { get; set; }
        [Required]
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


        [ForeignKey("Role")]
        public string? RoleID { get; set; }
        public RoleApplication Role { get; set; }
        [ForeignKey("UserApplication")]
        public string? UserId { get; set; }
        public UserApplication? UserApplication { get; set; }
        public List<Billing>? Billing { get; set; } = new();
    }
}
