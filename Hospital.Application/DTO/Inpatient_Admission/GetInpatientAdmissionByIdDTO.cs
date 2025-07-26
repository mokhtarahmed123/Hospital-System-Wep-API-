namespace HospitalAPI.Hospital.Application.DTO.Inpatient_Admission
{
    public class GetInpatientAdmissionByIdDTO
    {
        //public int Id { get; set; }
        public string PatientName { get; set; }
        public string DoctorName { get; set; }
        public int RoomNumber { get; set; }
        public DateTime AdmissionDate { get; set; }
        public DateTime? DischargeDate { get; set; }
    }
}
