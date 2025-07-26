using HospitalAPI.Hospital.Application.DTO.RoomDTO;
using HospitalAPI.Hospital.Domain.Models;
using HospitalAPI.Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Hospital.Application.Services.Room
{
    public class RoomService : IRoomService
    {
        private readonly HospitalContex hospitalContex;

        public RoomService(HospitalContex context)
        {
            hospitalContex = context;

        }
        //public async Task<bool> AssignPatientToRoomAsync(AssignPatientToRoomDTO dto)
        //{
        //    if (dto == null) return false ;
        //    Inpatient_Admission inpatient_ = new Inpatient_Admission
        //    {
        //        AdmissionDate = dto.AdmissionDate,
        //        DepartmentId = dto.DepartmentID,
        //        DischargeDate = dto.DischargeDate,
        //        PatientID = dto.PatientID,
        //        RoomId = dto.RoomId,
        //    };
        //    await hospitalContex.InpatientAdmissions.AddAsync(inpatient_);
        //    await hospitalContex.SaveChangesAsync();
        //    return true;

        //}

        public async Task<CreateRoomDTO?> CreateRoomAsync(CreateRoomDTO dto)
        {
            if (dto == null) return dto;
            Rooms rooms = new Rooms()
            {
                RoomNumber = dto.RoomNumber,
                DepartmentId = dto.DepartmentId,
                Capacity = dto.Capacity,
                IsAvailable = dto.IsAvailable,
            };
            await hospitalContex.Rooms.AddAsync(rooms);
            await hospitalContex.SaveChangesAsync();
            return dto;
        }

        public async Task<bool> DeleteRoomAsync(int Number)
        {
            var isfound = await hospitalContex.Rooms.FirstOrDefaultAsync(i=>i.RoomNumber == Number);
            if (isfound == null) return false;
            hospitalContex.Remove(isfound);
            await hospitalContex.SaveChangesAsync();
            return true;
        }

        public async Task<List<GetAllRoomsDTO>> FilterRoomsAsync(RoomFilterDTO filter)
        {
            var query = hospitalContex.Rooms.AsQueryable();

            if (filter.DepartmentId.HasValue)
            {
                query = query.Where(r => r.DepartmentId == filter.DepartmentId.Value);
            }

            if (filter.IsAvailable.HasValue)
            {
                query = query.Where(r => r.IsAvailable == filter.IsAvailable.Value);
            }

            return await query.Select(r => new GetAllRoomsDTO
            {
                RoomNumber = r.RoomNumber,
                Capacity = r.Capacity,
                IsAvailable = r.IsAvailable,
                DepartmentName = r.Department.Name
            }).ToListAsync();


        }

        public async Task<List<GetAllRoomsDTO>> GetAllRoomsAsync()
        {
            return await hospitalContex.Rooms.Include(a => a.Department).Include(A => A.inpatient_Admissions).
                Select(a => new GetAllRoomsDTO
                {
                    Capacity = a.Capacity,
                    IsAvailable = a.IsAvailable,
                    DepartmentName = a.Department.Name,
                    RoomNumber = a.RoomNumber
                }).ToListAsync();
        }

        public async Task<List<GetAllRoomsDTO>> GetAvailableRoomsAsync()
        {
            return await hospitalContex.Rooms.
                Include(a => a.Department).
                Include(A => A.inpatient_Admissions)
                .Where(A => A.IsAvailable == true).
            Select(a => new GetAllRoomsDTO
            {
                Capacity = a.Capacity,
                IsAvailable = a.IsAvailable,
                DepartmentName = a.Department.Name,
                RoomNumber = a.RoomNumber
            }).ToListAsync();
        }

        public async Task<GetRoomDTO> GetRoomByNumberAsync(int Number)
        {
            var isfound = await hospitalContex.Rooms.Include(r => r.Department).FirstOrDefaultAsync(i => i.RoomNumber == Number);
            if (isfound == null) return null;
            return new GetRoomDTO
            {
                Capacity = isfound.Capacity,
                IsAvailable = isfound.IsAvailable,
                DepartmentName = isfound.Department.Name,
                RoomNumber = isfound.RoomNumber

            };

        }



        public async Task<UpdateRoomDTO> UpdateRoomAsync(int Nmuber, UpdateRoomDTO dto)
        {
            var isfound = await hospitalContex.Rooms.Include(a=>a.Department).FirstOrDefaultAsync(i=>i.RoomNumber == Nmuber);
            if (isfound is null) return null;
            isfound.Capacity = dto.Capacity;
            isfound.IsAvailable = dto.IsAvailable;
            isfound.DepartmentId = dto.DepartmentId;
            await hospitalContex.SaveChangesAsync();
            return dto;

        }
    }
}
