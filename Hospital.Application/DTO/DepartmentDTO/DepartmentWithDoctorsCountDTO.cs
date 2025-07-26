
using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Hospital.Application.DTO.DepartmentDTO
{
    public class DepartmentWithDoctorsCountDTO
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Doctors { get; set; }
    }
}
