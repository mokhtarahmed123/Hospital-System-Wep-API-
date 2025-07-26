namespace HospitalAPI.Hospital.Application.DTO.AppointmentDTO
{
    public class GetAllAppointmentsDTO
    {
        public int PatientID { get; set; }
        public string PatientName { get; set; }
        public int DoctorID { get; set; }
        public string DoctorName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; }
        public string specialization { get; set; }




    }
}
