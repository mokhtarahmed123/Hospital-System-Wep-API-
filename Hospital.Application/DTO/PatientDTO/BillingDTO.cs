using HospitalAPI.Hospital.Domain.Models;

namespace HospitalAPI.Hospital.Application.DTO.PatientDTO
{
    public class Billingdto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal? TotalPaid { get; set; }
        public string? Status { get; set; }
        //public DateTime? BillingDate { get; set; }
        public List<Billing>? Billings { get; set; } = new();

    }
}
