namespace HospitalAPI.Hospital.Application.DTO.Inpatient_Admission
{
    public class CreateInpatientAdmissionDTO
    {
        //public int PatientID { get; set; }
        //public int DoctorID { get; set; }
        //public int DepartmentID { get; set; }
        public string PatientEmail { get; set; }
        public int RoomNumber { get; set; }
        //public int RoomID { get; set; }
        public string DoctorEmail { get; set; }
        public string DepartmentName { get; set; }
        public DateTime AdmissionDate { get; set; }
        public DateTime? DischargeDate { get; set; }
    }
}
