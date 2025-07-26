using HospitalAPI.Hospital.Domain.Models;
using HospitalAPI.Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Hospital.Infrastructure.Repositories.Room
{
    public class RoomRepository : IRoomRepository
    {
        private readonly HospitalContex hospitalContex;

        public RoomRepository(HospitalContex hospitalContex)
        {
            this.hospitalContex = hospitalContex;
        }
        public async Task AddAsync(Rooms room)
        {
            hospitalContex.Rooms.Add(room);
            await Save();
        }

        public async Task DeleteAsync(int id)
        {
            Rooms rooms = await hospitalContex.Rooms.FindAsync(id);
            if (rooms is not null)
            {
                hospitalContex.Rooms.Remove(rooms);
                await Save();

            }
        }

        public async Task<List<Rooms>> GetAllRooms()
        {
            return await hospitalContex.Rooms.
                Include(a => a.Department).
                Include(a => a.inpatient_Admissions).
                ToListAsync();

        }

        public async Task<List<Rooms>> GetAvailableRoomsAsync()
        {
            return await hospitalContex.Rooms.Where(a => a.IsAvailable == true).ToListAsync();

        }

        public async Task<List<Rooms>> GetByDepartmentIdAsync(int departmentId)
        {
            return await hospitalContex.Rooms.
                Include(a => a.Department).Where(d => d.DepartmentId == departmentId)
                .ToListAsync();
        }

        public async Task<Rooms?> GetByRoomNumberAsync(int roomNumber)
        {
            return await hospitalContex.Rooms.
     Include(a => a.Department).FirstOrDefaultAsync(n => n.RoomNumber == roomNumber);
        }

        public async Task<Rooms?> GetRoomByID(int ID)
        {
            return await hospitalContex.Rooms.
                 Include(a => a.Department).FirstOrDefaultAsync(n => n.Id == ID);
        }

        public async Task<int> GetTotalCapacityAsync()
        {
            int totalCapacity = await hospitalContex.Rooms.SumAsync(r => r.Capacity);
            return totalCapacity;
        }

        public async Task<bool> SetAvailabilityAsync(int roomId, bool isAvailable)
        {
            Rooms room = await hospitalContex.Rooms.FindAsync(roomId);
            if (room is not null)
            {
                room.IsAvailable = isAvailable;
                await Save();
                return true;
            }
            return false;
        }

        public async Task UpdateAsync(int id, Rooms room)
        {
            Rooms rooms = await hospitalContex.Rooms.FindAsync(id);
            if (rooms is not null)
            {
                rooms.RoomNumber = room.RoomNumber;
                rooms.inpatient_Admissions = room.inpatient_Admissions;
                rooms.IsAvailable = room.IsAvailable;
                rooms.Capacity = room.Capacity;
                rooms.DepartmentId = room.DepartmentId;
                await Save();

            }
        }
        public async Task Save()
        {
            await hospitalContex.SaveChangesAsync();
        }
    }
}
