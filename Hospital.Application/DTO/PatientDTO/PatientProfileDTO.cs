using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using HospitalAPI.Hospital.Domain.Models;

namespace HospitalAPI.Hospital.Application.DTO.PatientDTO
{
    public class PatientProfileDTO
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int Age { get; set; }
        public string ImagePath { get; set; }

        public decimal? TotalPaid { get; set; }
        public DateTime? BillingDate { get; set; }
        public string? BillingStatus { get; set; }

        public DateTime? AppointmentDate { get; set; }
        public string? AppointmentStatus { get; set; }

        public string? LabStatus { get; set; }
        public string? LabResult { get; set; }
        public DateTime? LabDate { get; set; }
        public string? LabTestType { get; set; }

        public List<Appointments>? Appointments { get; set; } = new();
        public List<Laboratory>? LabResults { get; set; } = new();
        public List<Billing>? Billings { get; set; } = new();

    }
}
