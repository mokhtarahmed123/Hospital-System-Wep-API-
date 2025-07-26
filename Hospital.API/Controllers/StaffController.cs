using HospitalAPI.Hospital.Application.DTO.Staff;
using HospitalAPI.Hospital.Application.Services.Staff;
using HospitalAPI.Hospital.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HospitalAPI.Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  

    [Authorize(Roles = "Admin")]

    public class StaffController : ControllerBase
    {
        private readonly HospitalContex context;
        private readonly IStaffService staff;

        public StaffController(HospitalContex context, IStaffService staff)
        {
            this.context = context;
            this.staff = staff;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateStaff([FromBody] CreateStaffDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await staff.CreateStaffAsync(dto);
            return Ok(result);
        }
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await staff.GetAllStaffAsync();
            return Ok(result);
        }
        [HttpGet("GetByEmail")]
        public async Task<IActionResult> GetByEmail([FromBody]string email)
        {
            var result = await staff.GetStaffByIdAsync(email);
            if (result == null) return NotFound("Staff not found");
            return Ok(result);
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromQuery]string email, [FromBody] UpdateStaffDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await staff.UpdateStaffAsync(email, dto);
            if (!updated) return NotFound("Staff not found");
            return Ok("Staff updated successfully");
        }

        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete([FromBody]string email)
        {
            var deleted = await staff.DeleteStaffAsync(email);
            if (!deleted) return NotFound("Staff not found");
            return Ok("Staff deleted successfully");
        }
        [HttpGet("count")]
        public async Task<IActionResult> GetCount()
        {
            var count = await staff.GetStaffCountAsync();
            return Ok(new { total = count });

        }
        [HttpPost("filter")]
        public async Task<IActionResult> Filter([FromForm] StaffFilterDTO filter)
        {
            var result = await staff.FilterStaffAsync(filter);
            return Ok(result);
        }
    }
}
