using HospitalAPI.Hospital.Application.DTO.RoomDTO;

namespace HospitalAPI.Hospital.Application.Services.Room
{
    public interface IRoomService
    {
        Task<CreateRoomDTO> CreateRoomAsync(CreateRoomDTO dto);
        Task<UpdateRoomDTO> UpdateRoomAsync(int Number, UpdateRoomDTO dto);
        Task<bool> DeleteRoomAsync(int Number);
        Task<List<GetAllRoomsDTO>> GetAllRoomsAsync();
        Task<GetRoomDTO> GetRoomByNumberAsync(int Number);
        Task<List<GetAllRoomsDTO>> FilterRoomsAsync(RoomFilterDTO filter);
        
        Task<List<GetAllRoomsDTO>> GetAvailableRoomsAsync();

    }
}
