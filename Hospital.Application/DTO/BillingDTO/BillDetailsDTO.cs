namespace HospitalAPI.Hospital.Application.DTO.BillingDTO
{
    public class BillDetailsDTO
    {
        public int BillId { get; set; }
        public int PatientID { get; set; }
        public string Status { get; set; }
        public string PatientName { get; set; }
        public int? AccountentID { get; set; }
        public DateTime BillDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
