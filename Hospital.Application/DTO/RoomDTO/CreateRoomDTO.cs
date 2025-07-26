namespace HospitalAPI.Hospital.Application.DTO.RoomDTO
{
    public class CreateRoomDTO
    {
        public int RoomNumber { get; set; }
        public bool IsAvailable { get; set; }
        public int Capacity { get; set; }
        public int DepartmentId { get; set; }
    }
}
