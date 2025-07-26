using HospitalAPI.Hospital.Application.DTO.AppointmentDTO;
using HospitalAPI.Hospital.Application.Services.Appointment;
using HospitalAPI.Hospital.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class AppointmentController : ControllerBase
    {

        private readonly IAppointmentServices AppointmentService;
        private readonly HospitalContex hospitalContex;


        public AppointmentController(IAppointmentServices Appointment, HospitalContex hospitalContex)
        {
            AppointmentService = Appointment;
            this.hospitalContex = hospitalContex;
        }


        [Authorize(Roles = "Admin,Accountant")]
        [HttpGet("GetAllAppointments")]
        public async Task<IActionResult> GetAllAppointments()
        {
            var Result = await AppointmentService.GetAllAppointmentsAsync();
            if (Result == null) return BadRequest(ModelState);
            return Ok(Result);

        }
        [Authorize(Roles = "Admin,Accountant")]


        [HttpPost("GetAppointmentById/{id}")]
        public async Task<IActionResult> GetAppointmentById(int id)
        {
            var ISFOUND = await hospitalContex.Appointments.FindAsync(id);
            if (ISFOUND == null) return NotFound("Appointment Not Found");

            var Result = await AppointmentService.GetAppointmentByIdAsync(id);
            if (Result == null) return BadRequest(Result);

            return Ok(Result);
        }
        [Authorize(Roles = "Admin,Doctor,Accountant,Patient")]
        [HttpGet("GetAppointmentsByPatientId")]
        public async Task<IActionResult> GetAppointmentsByPatientId(string EmailOfPatient)
        {
            var ISFOUND = await hospitalContex.Patients.FirstOrDefaultAsync(i=>i.Email == EmailOfPatient);
            if (ISFOUND == null) return NotFound("Patient Not Found");
            var Result = await AppointmentService.GetAppointmentsByPatientIdAsync(EmailOfPatient);
            if (Result == null) return BadRequest(Result);
            return Ok(Result);

        }
        [Authorize(Roles = "Admin,Doctor")]
        [HttpPost("ConfirmAppointment/{id}")]
        public async Task<IActionResult> ConfirmAppointment(int id, [FromBody] ConfirmAppointment confirm)
        {
            var ISFOUND = await hospitalContex.Appointments.FindAsync(id);
            if (ISFOUND == null) return NotFound("User Not Found");
            var Result = await AppointmentService.ConfirmAppointment(id, confirm);
            if (Result == false) return BadRequest(Result);
            return Ok(Result);

        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ISFOUND = await hospitalContex.Appointments.FindAsync(id);
            if (ISFOUND == null) return NotFound("User Not Found");
            var Result = await AppointmentService.Delete(id);
            if (Result == false) return BadRequest(Result);
            return Ok(Result);
        }
        [HttpPost("SearchDoctorsAsync")]

        public async Task<IActionResult> SearchDoctorsAsync([FromForm] FilterAppointment filter)
        {
            var Result = await AppointmentService.SearchDoctorsAsync(filter);
            if (Result == null) return BadRequest(Result);
            return Ok(Result);

        }
        [Authorize(Roles = "Admin,Doctor")]
        [HttpPut("EditAppointment/{id}")]
        public async Task<IActionResult> EditAppointment(int id, [FromBody] EditAppointment edit)
        {
            var ISFOUND = await hospitalContex.Appointments.FindAsync(id);
            if (ISFOUND == null) return NotFound("User Not Found");
            var Result = await AppointmentService.EditAppointment(id, edit);
            if (Result == null) return BadRequest(Result);
            return Ok(Result);

        }
    }
}

