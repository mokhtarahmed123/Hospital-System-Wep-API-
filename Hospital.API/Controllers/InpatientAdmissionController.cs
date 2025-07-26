
using HospitalAPI.Hospital.Application.DTO.Inpatient_Admission;
using HospitalAPI.Hospital.Application.Services.Inpatient_Admission;
using HospitalAPI.Hospital.Infrastructure.Data;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Hospital.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class InpatientAdmissionController : ControllerBase
    {
        private readonly HospitalContex contex;
        private readonly IInpatientAdmissionService inpatient;

        public InpatientAdmissionController(HospitalContex contex, IInpatientAdmissionService inpatient)
        {
            this.contex = contex;
            this.inpatient = inpatient;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("CreateInpatientAdmission")]
        public async Task<IActionResult> CreateAdmissionAsync([FromBody] CreateInpatientAdmissionDTO dto)
        {
            if (dto == null) return BadRequest(ModelState);
            var ROOM = await inpatient.CreateAdmissionAsync(dto);
            return Ok(ROOM);
        }
        [Authorize(Roles = "Admin,Doctor")]
        [HttpGet("GetAllInpatientAdmission")]
        public async Task<IActionResult> GetAllRoomsAsync()
        {
            var AllRooms = await inpatient.GetAllAdmissionsAsync();
            if (AllRooms is null) return NotFound("No Admissions Found");
            return Ok(AllRooms);
        }
        [Authorize(Roles = "Admin,Doctor")]
        [HttpGet("GetAdmissionById/{id}")]

        public async Task<IActionResult> GetAdmissionByIdAsync(int id)
        {
            var room = await inpatient.GetAdmissionByIdAsync(id);
            if (room == null) return NotFound("Admission not found");
            return Ok(room);
        }
        [Authorize(Roles = "Admin")]

        [HttpPut("UpdateInpatientAdmission/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateInpatientAdmissionDTO dto)
        {
            var ISFOUND = await contex.InpatientAdmissions.FindAsync(id);
            if (ISFOUND == null) return NotFound("room Not Found");
            var Result = await inpatient.UpdateAdmissionAsync(id, dto);
            if (Result == null) return BadRequest(Result);
            return Ok(Result);

        }
        [Authorize(Roles = "Admin,Doctor")]

        [HttpGet("GetCurrentAdmissionsAsync")]
        public async Task<IActionResult> GetCurrentAdmissionsAsync()
        {
            var AllRooms = await inpatient.GetCurrentAdmissionsAsync();
            if (AllRooms is null) return BadRequest(ModelState);
            return Ok(AllRooms);

        }
        [Authorize(Roles = "Admin,Doctor")]

        [HttpGet("GetAdmissionsByPatientEmail")]
        public async Task<IActionResult> GetAdmissionsByPatientEmail(string EMail)
        {
            var iSFOUND = await contex.InpatientAdmissions.FirstOrDefaultAsync(a => a.Patient.Email == EMail);
            if (iSFOUND == null) return BadRequest("Not Found,TRY AGAIN");
            var Admissions = await inpatient.GetAdmissionsByPatientIdAsync(EMail);
            if (Admissions is null) return BadRequest(ModelState);
            return Ok(Admissions);
        }
        [Authorize(Roles = "Admin,Doctor")]

        [HttpPost("FilterAdmissions")]
        public async Task<IActionResult> FilterAdmissions([FromForm] InpatientAdmissionFilterDTO filter)
        {
            var result = await inpatient.FilterAdmissionsAsync(filter);
            if (result is null) return BadRequest(result);
            return Ok(result);

        }
    }
}
