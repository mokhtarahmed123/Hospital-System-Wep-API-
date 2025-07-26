namespace HospitalAPI.Hospital.Application.DTO.PatientDTO
{
    public class SearchDoctorsDTO
    {
        public int DoctorID { get; set; }
        public string Name { get; set; }
        public string Specialization { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Gender { get; set; }
        public string? ImagePath { get; set; }
        public int? YearsOfExperience { get; set; }
        public bool? AvailabilityStatus { get; set; }

    }
}
