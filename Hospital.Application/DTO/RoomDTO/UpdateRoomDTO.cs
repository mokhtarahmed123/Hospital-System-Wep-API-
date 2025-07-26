namespace HospitalAPI.Hospital.Application.DTO.RoomDTO
{
    public class UpdateRoomDTO
    {
        //public int RoomNumber { get; set; }
        public bool IsAvailable { get; set; }
        public int Capacity { get; set; }
        public int DepartmentId { get; set; }


    }
}
