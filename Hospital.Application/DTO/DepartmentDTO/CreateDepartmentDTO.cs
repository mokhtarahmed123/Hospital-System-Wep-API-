using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Hospital.Application.DTO.DepartmentDTO
{
    public class CreateDepartmentDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

    }
}
