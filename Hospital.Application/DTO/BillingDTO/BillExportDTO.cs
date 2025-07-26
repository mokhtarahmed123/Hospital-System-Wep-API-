namespace HospitalAPI.Hospital.Application.DTO.BillingDTO
{
    public class BillExportDTO
    {
        public string PatientName { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime BillDate { get; set; }
        public string Status { get; set; }
    }
}
