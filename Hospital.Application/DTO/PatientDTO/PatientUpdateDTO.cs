namespace HospitalAPI.Hospital.Application.DTO.PatientDTO
{
    public class PatientUpdateDTO
    {
        public int PatientID { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public int? Age { get; set; }
        public IFormFile? Image { get; set; }
    }
}
