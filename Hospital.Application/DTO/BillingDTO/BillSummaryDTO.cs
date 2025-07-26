namespace HospitalAPI.Hospital.Application.DTO.BillingDTO
{
    public class BillSummaryDTO
    {
        public int BillId { get; set; }
        public string PatientName { get; set; }
        public string PatientID { get; set; }
        public DateTime BillDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
