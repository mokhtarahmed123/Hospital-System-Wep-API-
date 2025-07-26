using HospitalAPI.Hospital.Application.DTO.RoomDTO;
using HospitalAPI.Hospital.Application.Services.Room;
using HospitalAPI.Hospital.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]

    public class RoomController : ControllerBase
    {
        private readonly IRoomService room;
        private readonly HospitalContex hospitalContex;

        public RoomController(IRoomService room, HospitalContex contex)
        {
            this.room = room;
            hospitalContex = contex;
        }
        [Authorize(Roles = "Admin")]

        [HttpPost("CreateRoom")]
        public async Task<IActionResult> CreateRoomAsync([FromBody] CreateRoomDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var ROOM = await room.CreateRoomAsync(dto);
            return Ok(ROOM);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteRoom")]
        public async Task<IActionResult> DeleteRoom(int RoomNumber)
        {
            var isfound = await hospitalContex.Rooms.FirstOrDefaultAsync(i=>i.RoomNumber == RoomNumber);
            if (isfound == null) return BadRequest("Room Not Found");
            var Room = await room.DeleteRoomAsync(RoomNumber);
            if (Room == false) return BadRequest($"{Room} Not Deleted");
            return NoContent();
        }
        [Authorize(Roles = "Admin,Doctor")]

        [HttpGet("GetAllRooms")]
        public async Task<IActionResult> GetAllRoomsAsync()
        {
            var AllRooms = await room.GetAllRoomsAsync();
            if (AllRooms == null || AllRooms.Count == 0) return NotFound("No rooms found");
            return Ok(AllRooms);

        }
        [Authorize(Roles = "Admin,Doctor")]
        [HttpGet("GetAllRoomsAvailable")]
        public async Task<IActionResult> GetAllRoomsAvailable()
        {
            var AllRooms = await room.GetAvailableRoomsAsync();
            if (AllRooms is null) return BadRequest(ModelState);
            return Ok(AllRooms);

        }
        [Authorize(Roles = "Admin,Doctor")]

        [HttpGet("GetRoomByNumber")]
        public async Task<IActionResult> GetRoomByIdAsync(int Number)
        {
            var isfound = await hospitalContex.Rooms.FirstOrDefaultAsync(i=>i.RoomNumber == Number);
            if (isfound == null) return BadRequest("Room Not Found");
            var Room = await room.GetRoomByNumberAsync(Number);
            if (Room == null) return BadRequest($"{Room} Not Found");
            return Ok(Room);
        }
        [Authorize(Roles = "Admin")]

        [HttpPut("UpdateRoom")]
        public async Task<IActionResult> UpdateRoomAsync([FromQuery]int number, [FromBody] UpdateRoomDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var Result = await room.UpdateRoomAsync(number, dto);
            if (Result == null) return NotFound("Room not found or update failed");
            return Ok(Result);

        }
        [Authorize(Roles = "Admin,Doctor")]
        [HttpPost("FilterRooms")]
        public async Task<IActionResult> FilterRooms([FromForm] RoomFilterDTO filter)
        {
            var Rooms = await room.FilterRoomsAsync(filter);
            if (Rooms == null) return BadRequest(ModelState);
            return Ok(Rooms);
        }

    }
}
