using HospitalAPI.Hospital.Domain.Models;
using System.Text.Json.Serialization;

namespace HospitalAPI.Hospital.Application.DTO.AccountantDTO
{
    public class GetAccountantDTO
    {
        public int Id { get; set; }
        //public int PatientId { get; set; }
        public int TotalAmount { get; set; }
        public DateTime BillingDate { get; set; }
        public string Status { get; set; }

        public string Name { get; set; }
        public string PatientName { get; set; }
        public string Certification { get; set; }
        [JsonIgnore]
        public List<Billing>? billings { get; set; }

    }
}
