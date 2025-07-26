using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Hospital.Domain.Models
{
    public class Staff_Management
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }
        //[Required]

        public int? Salary { get; set; }
        //[Required]
        [DataType(DataType.Date)]
        public DateTime? DateHired { get; set; }

        //[Required]
        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }
        public Department? Department { get; set; }
        [ForeignKey("RoleApplication")]

        public string? RoleId { get; set; }
        public RoleApplication? RoleApplication { get; set; }

        public List<Doctors>? Doctors { get; set; }
        //public List<Accountant>? Accountant { get; set; }
        [ForeignKey("UserApplication")]
        public string? UserId { get; set; }
        public UserApplication? UserApplication { get; set; }
    }
}