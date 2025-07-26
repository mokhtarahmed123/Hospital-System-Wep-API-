using HospitalAPI.Hospital.Domain.Models;

namespace HospitalAPI.Hospital.Infrastructure.Repositories.Room
{
    public interface IRoomRepository
    {
        Task<List<Rooms>> GetAllRooms();
        Task<Rooms?> GetRoomByID(int ID);
        Task<Rooms?> GetByRoomNumberAsync(int roomNumber);
        Task AddAsync(Rooms room);
        Task UpdateAsync(int id, Rooms room);
        Task DeleteAsync(int id);
        Task<List<Rooms>> GetAvailableRoomsAsync();
        Task<List<Rooms>> GetByDepartmentIdAsync(int departmentId);
        Task<bool> SetAvailabilityAsync(int roomId, bool isAvailable);
        Task<int> GetTotalCapacityAsync();



    }
}
