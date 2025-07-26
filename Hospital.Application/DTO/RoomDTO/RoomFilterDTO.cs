namespace HospitalAPI.Hospital.Application.DTO.RoomDTO
{
    public class RoomFilterDTO
    {
        public int? DepartmentId { get; set; }
        public bool? IsAvailable { get; set; }
    }
}
