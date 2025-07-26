using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Hospital.Application.DTO.BillingDTO
{
    public class CreateBillDTO
    {
        //public int PatientId { get; set; }
        [DataType(DataType.EmailAddress)]
        public string PatientEmail { get; set; }
        public DateTime BillDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        [DataType(DataType.EmailAddress)]


        public string ACCOUNTATEmail { get; set; }

        public string Status { get; set; } = "Pending";
    }
}
