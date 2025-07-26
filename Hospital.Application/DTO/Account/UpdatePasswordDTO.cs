namespace HospitalAPI.Hospital.Application.DTO.Account
{
    public class UpdatePasswordDTO
    {
        //public string Email {  get; set; }// Danger , If Any One Has a Email 
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
