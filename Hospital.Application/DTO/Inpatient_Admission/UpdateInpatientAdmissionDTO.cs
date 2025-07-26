namespace HospitalAPI.Hospital.Application.DTO.Inpatient_Admission
{
    public class UpdateInpatientAdmissionDTO
    {
        //public int RoomId { get; set; }
        public int RoomNumber { get; set; }
        public DateTime? DischargeDate { get; set; }
        public string DepartmentName { get; set; }
        //public int DoctorId { get; set; }
        public string DoctorEmail { get; set; }

    }
}
