using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Hospital.Application.DTO.Account
{
    public class LoginDTO
    {
        public string UserName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        public string RoleName { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
