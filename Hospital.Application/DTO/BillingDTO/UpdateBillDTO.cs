namespace HospitalAPI.Hospital.Application.DTO.BillingDTO
{
    public class UpdateBillDTO
    {
        public DateTime? BillDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public int? PatientID { get; set; }
        public int? AccountentID { get; set; }
        public string? Status { get; set; }

    }
}
