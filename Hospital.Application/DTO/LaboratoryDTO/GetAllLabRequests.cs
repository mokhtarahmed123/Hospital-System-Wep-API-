namespace HospitalAPI.Hospital.Application.DTO.LaboratoryDTO
{
    public class GetAllLabRequests
    {
        public string TestType { get; set; }
        public string Status { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public DateTime RequestDate { get; set; }

    }
}
