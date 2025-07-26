using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Hospital.Domain.Models
{
    public class RevokedToken
    {
        [Key]
        public int id { get; set; }
        public string JWT { get; set; }
    }
}
