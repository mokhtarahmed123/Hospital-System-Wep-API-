using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HospitalAPI.Hospital.Domain.Models
{
    public class Doctors
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Name Must be less Than 50 ")]
        [MinLength(3, ErrorMessage = "Name Must be Greater Than 3 ")]
        [Display(Name = " Doctor Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "specialization is Required")]
        public string specialization { get; set; }// May Be Drop Down List 

        [Required(ErrorMessage = "Email is Required")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please select gender")]
        [Display(Name = "Gender")]
        public string Gander { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        [Required]

        public int Experience { get; set; }
        [Required]
        public int WorkingHours { get; set; }
        [Required]
        public bool IsAvailable { get; set; } //    Radio
        public DateTime? JoiningDate { get; set; }
        [ForeignKey("DepartmentID")]
        public int? DepartmentId { get; set; }
        [JsonIgnore]

        public Department DepartmentID { get; set; }
        //[ForeignKey("StaffMemberId")]
        //public int? StaffMemberID { get; set; }
        [ForeignKey("ApplicationUserId")]
        public string? ApplicationUserId { get; set; }

        public UserApplication ApplicationUser { get; set; }
        //[JsonIgnore]
        //public Staff_Management StaffMemberId { get; set; }
        public List<Laboratory>? laboratories { get; set; }
        public List<Appointments>? Appointments { get; set; }
        public List<Inpatient_Admission>? Inpatient_Admission { get; set; }


    }
}
