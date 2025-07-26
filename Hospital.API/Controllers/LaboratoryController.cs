using HospitalAPI.Hospital.Application;
using HospitalAPI.Hospital.Application.DTO.LaboratoryDTO;
using HospitalAPI.Hospital.Application.Services.Laboratory;
using HospitalAPI.Hospital.Infrastructure.Data;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class LaboratoryController : ControllerBase
    {
        private readonly ILaboratoryService laboratoryService;
        private readonly HospitalContex contex;

        public LaboratoryController(ILaboratoryService laboratoryService, HospitalContex contex)
        {
            this.laboratoryService = laboratoryService;
            this.contex = contex;
        }
        [HttpGet("GetAllLabRequests")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetAllLabRequests()
        {
            var Result = await laboratoryService.GetAllLabRequests();
            if (Result == null) return BadRequest(ModelState);
            return Ok(Result);
        }
        [Authorize(Roles = "Admin,Doctor")]
        [HttpGet("GetByID/{id:int}")]
        public async Task<IActionResult> GetByID(int id)
        {
            var ISFOUND = await contex.Laboratory.FindAsync(id);
            if (ISFOUND == null) return NotFound("Labo request Not Found");
            var Result = await laboratoryService.GetByID(id);
            if (Result == null) return BadRequest(Result);
            return Ok(Result);
        }
        [Authorize(Roles = "Admin,Doctor")]
        [HttpPost("GetByDoctorEmail")]
        public async Task<IActionResult> GetByDoctorEmail([FromBody]string DoctorEmail)
        {
            var ISFOUND = await contex.Doctors.FirstOrDefaultAsync(i=>i.Email == DoctorEmail);
            if (ISFOUND == null) return NotFound("Doctor Not Found");
            var Result = await laboratoryService.GetByDoctorIdAsync(DoctorEmail);
            if (Result == null) return BadRequest(Result);
            return Ok(Result);
        }
        [HttpPost("GetByStatus")]
        public async Task<IActionResult> GetByStatusAsync([FromBody]string Status)
        {
            var ISFOUND = await contex.Laboratory.FirstOrDefaultAsync(i => i.Status == Status);
            if (ISFOUND == null) return NotFound("status Not Found");
            var Result = await laboratoryService.GetByStatusAsync(Status);
            if (Result == null) return BadRequest(Result);
            return Ok(Result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("GetBytestType")]
        public async Task<IActionResult> GetBytestTypeAsync([FromBody]string testType)
        {
            var ISFOUND = await contex.Laboratory.FirstOrDefaultAsync(i => i.TestType == testType);
            if (ISFOUND == null) return NotFound("Test  Not Found");
            var Result = await laboratoryService.GetByTestTypeAsync(testType);
            if (Result == null) return BadRequest(Result);
            return Ok(Result);
        }
        [HttpPost("GetByPatientEmail")]
        public async Task<IActionResult> GetByPatientId(string PatientEmail)
        {
            var ISFOUND = await contex.Patients.FirstOrDefaultAsync(i=>i.Email == PatientEmail);
            if (ISFOUND == null) return NotFound("Patient Not Found");
            var Result = await laboratoryService.GetByPatientIdAsync(PatientEmail   );
            if (Result == null) return BadRequest(Result);
            return Ok(Result);

        }
        [Authorize(Roles = "Admin")]
        [HttpPost("ConfirmLabRequest/{id:int}")]
        public async Task<IActionResult> ConfirmLabRequest(int id, [FromBody] string Status)
        {
            var ISFOUND = await contex.Laboratory.FindAsync(id);
            if (ISFOUND == null) return NotFound("Lab request not found");
            var Result = await laboratoryService.Confirm(id, Status);
            if (Result == false) return BadRequest("Failed to confirm lab request");
            return Ok("Lab request confirmed successfully");

        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ISFOUND = await contex.Laboratory.FindAsync(id);
            if (ISFOUND == null) return NotFound("Lab Record  Not Found");
            var Result = await laboratoryService.Delete(id);
            if (Result == false) return BadRequest(Result);
            return Ok(Result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateLabRequest/{id:int}")]
        public async Task<IActionResult> LabRequestAsync(int id, [FromBody] UpdateLabDTO update)
        {
            var AccountExists = await contex.Laboratory.FindAsync(id);
            if (AccountExists == null)
                return NotFound("Lab Request  not found");

            var updated = await laboratoryService.Update(id, update);
            if (updated == false)
                return BadRequest("Update failed");

            return Ok(updated);
        }


    }
}
