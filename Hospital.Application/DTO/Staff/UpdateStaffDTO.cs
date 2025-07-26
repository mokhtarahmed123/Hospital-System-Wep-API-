using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Hospital.Application.DTO.Staff
{
    public class UpdateStaffDTO
    {
        public string Name { get; set; }

        //public string RoleID { get; set; }
        public string RoleName { get; set; }
        public string DepartmentName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime HireDate { get; set; }

        //public int DepartmentID { get; set; }
        public int Salary { get; set; }
    }
}
