using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Hospital.Application.DTO.Staff
{
    public class CreateStaffDTO
    {
        public string Name { get; set; }

        public string RoleName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime HireDate { get; set; }

        //public int DepartmentID { get; set; }
        public string DepartmentName {  get; set;  }
        public int Salary { get; set; }
        [DataType(DataType.Password)]
        public string Password {  get; set; }
    }
}
