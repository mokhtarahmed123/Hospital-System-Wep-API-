namespace HospitalAPI.Hospital.Application.DTO.RoomDTO
{
    public class GetAllRoomsDTO
    {
        public int RoomNumber { get; set; }
        public bool IsAvailable { get; set; }
        public int Capacity { get; set; }
        public string DepartmentName { get; set; }

    }
}
