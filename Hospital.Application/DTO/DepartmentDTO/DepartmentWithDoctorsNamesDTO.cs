using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Hospital.Application.DTO.DepartmentDTO
{
    public class DepartmentWithDoctorsNamesDTO
    {
        [Key]

        public int Id { get; set; }
        public string Name { get; set; }

        public List<string> DoctorNames { get; set; }
    }
}
