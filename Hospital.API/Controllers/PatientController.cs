using HospitalAPI.Exceptions;
using HospitalAPI.Hospital.Application;
using HospitalAPI.Hospital.Application.DTO.PatientDTO;
using HospitalAPI.Hospital.Domain;
using HospitalAPI.Hospital.Domain.Models;
using HospitalAPI.Hospital.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HospitalAPI.Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    [Authorize]
    public class PatientController : ControllerBase

    {
        private readonly IPatientsService _patientsService;
        private readonly HospitalContex hospitalContex;
        private readonly UserManager<UserApplication> userManager;
        private readonly RoleManager<RoleApplication> roleManager;

        public PatientController(IPatientsService patientsService,
            HospitalContex hospitalContex,UserManager<UserApplication> userManager,RoleManager<RoleApplication> roleManager)
        {
            _patientsService = patientsService;
            this.hospitalContex = hospitalContex;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet("GetPatientByEmail")]
        public async Task<IActionResult> GetPatientByEmail( string? Email )
        {
            var userEmail =  User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null) return Unauthorized();

            var emailToUse = !string.IsNullOrEmpty(Email) ? Email : user.Email;
            var patient = await _patientsService.GetProfileAsync(emailToUse);
            if (patient == null)
            {
                throw new NotFoundException("User Not Found");
            }
            return Ok(patient);
        }
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(string? Email)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var currentUser = await userManager.FindByEmailAsync(userEmail);
            if (currentUser == null) return Unauthorized();

            var emailToUse = !string.IsNullOrEmpty(Email) ? Email : currentUser.Email;

            var patient = await hospitalContex.Patients
                .FirstOrDefaultAsync(p => p.Email == emailToUse);
            if (patient == null)
                throw new NotFoundException("Patient not found.");

            await _patientsService.DeletePatientAsync(emailToUse);

            var userToDelete = await userManager.FindByEmailAsync(emailToUse);
            if (userToDelete != null)
            {
                var result = await userManager.DeleteAsync(userToDelete);
                if (!result.Succeeded)
                    return BadRequest("Failed to delete user account.");
            }

            return Ok("Deleted successfully.");
        }
        [HttpPost("CreateAppointment")]
        public async Task<IActionResult> CreateAppointment([FromBody] AppointmentCreateDTO appointmentDto)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null) return Unauthorized();

            if (!ModelState.IsValid)
            {
                throw new  BadRequestException($"{ModelState}");
            }

            if (user.Email != appointmentDto.PatientEmail)
            {
                return Forbid("You can only create appointments for your own account.");
            }

            try
            {
                await _patientsService.CreateAppointmentAsync(appointmentDto);
                return StatusCode(201, "Created Successfully");
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ConflictException ex)
            {
                return Conflict(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                // fallback for unexpected errors
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }

        [HttpGet("GetAppointments")]
        public async Task<IActionResult> GetAppointmentsAsync()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null) return Unauthorized();
            var appointments = await _patientsService.GetAppointmentsAsync(user.Email);
            if (appointments == null || appointments.Count == 0)
                throw new NotFoundException ("No appointments found");
            return Ok(appointments);
        }
        [HttpGet("GetBillingHistory")]
        public async Task<IActionResult> GetBillingHistoryAsync()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null) return Unauthorized();

            var AllBilling = await _patientsService.GetBillingHistoryAsync(user.Email);
            if (AllBilling == null || AllBilling.Count == 0)
                return NotFound("No Billing found");
            return Ok(AllBilling);
        }
        [HttpGet("GetLabResults")]
        public async Task<IActionResult> GetLabResultsAsync()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null) return Unauthorized();
            var AllLabsResult = await _patientsService.GetLabResultsAsync(user.Email);
            if (AllLabsResult == null || AllLabsResult.Count == 0)
                return NotFound("No LabResults found");

            return Ok(AllLabsResult);
        }
        [Authorize(Roles ="Patient")]
        [HttpPut("UpdatePatient")]
        public async Task<IActionResult> UpdatePatientAsync([FromForm] PatientCreateDTO patient)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            var user = await userManager.FindByEmailAsync(userEmail);
            if (user == null)
                return Unauthorized();

            var existingPatient = await hospitalContex.Patients
                .FirstOrDefaultAsync(p => p.Email == userEmail);

            if (existingPatient == null)
                throw new NotFoundException("Patient not found for the logged-in user.");

            await _patientsService.UpdatePatientAsync(patient, userEmail);

            user.PhoneNumber = patient.Phone;
            user.UserName = patient.Name;
            user.Email = patient.Email;

            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest("Failed to update user data.");

            return Ok("Patient profile updated successfully.");
        }

        [HttpGet("Search")]
        public async Task<IActionResult> SearchDoctors([FromQuery] string? name, [FromQuery] string? specialization)
        {
            var results = await _patientsService.SearchDoctorsAsync(name, specialization);
            return Ok(results);
        }
        [Authorize(Roles = "Admin,Doctor")]
        [HttpPut("UpdateAppoiment/{AppoinmentID:int}")]
        public async Task<IActionResult> UpdateApp(int AppoinmentID, [FromBody] UpdateAppoinmentDTO appointmentDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
            var currentUser = await userManager.FindByEmailAsync(currentUserEmail);
            if (currentUser == null) return Unauthorized();

            if (User.IsInRole("Doctor"))
            {
                var doctor = await hospitalContex.Doctors.FirstOrDefaultAsync(d => d.Email == currentUser.Email);
                if (doctor == null)
                    return Forbid("Doctor account not found.");

                if (doctor.Email != appointmentDto.EmailDoctor)
                    return Forbid("You can only modify your own appointments.");
            }

            try
            {
                var updatedAppointment = await _patientsService.UpdateAppoinment(appointmentDto, AppoinmentID);
                return Ok(updatedAppointment);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (ConflictException ex)
            {
                return Conflict(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }



    }
}






