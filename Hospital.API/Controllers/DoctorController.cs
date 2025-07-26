using HospitalAPI.Hospital.Application;
using HospitalAPI.Hospital.Application.DTO.DoctorDTo;
using HospitalAPI.Hospital.Domain;
using HospitalAPI.Hospital.Domain.Models;
using HospitalAPI.Hospital.Infrastructure.Data;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HospitalAPI.Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]

    public class DoctorController : ControllerBase
    {
        private readonly IDoctorsService doctorsService;
        private readonly HospitalContex hospitalContex;
        private readonly UserManager<UserApplication> userManager;
        private readonly RoleManager<RoleApplication> roleManager;

        public DoctorController(IDoctorsService DoctorsService,
            HospitalContex hospitalContex,
            UserManager<UserApplication> userManager
            ,RoleManager<RoleApplication> roleManager)
        {
            doctorsService = DoctorsService;
            this.hospitalContex = hospitalContex;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateDoctor([FromForm] CreateDoctorDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var createdDoctor = await doctorsService.CreateDoctorAsync(dto);
                return Created();
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred: " + ex.Message);
            }
        }


        [HttpDelete("DeleteDoctor")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteDoctorAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("Email is required.");

            var doctor = await hospitalContex.Doctors.FirstOrDefaultAsync(d => d.Email == email);
            if (doctor == null)
                return NotFound("Doctor not found.");

            var isDeleted = await doctorsService.DeleteDoctorAsync(email);
            if (!isDeleted)
                return BadRequest("Failed to delete doctor.");

            return Ok("Doctor deleted successfully.");
        }
        [HttpGet("GetProfile")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetProfileAsync(string? Email)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized();

            string targetEmail = User.IsInRole("Admin") && !string.IsNullOrEmpty(Email) ? Email : userEmail;

            var dto = await doctorsService.GetProfileAsync(targetEmail);
            if (dto == null)
                return NotFound($"Doctor with email {targetEmail} not found.");

            return Ok(dto);
        }


        [HttpGet("GetAppointments")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetAppointmentsAsync(string? Email)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
                return Unauthorized();

            string targetEmail = User.IsInRole("Admin") && !string.IsNullOrEmpty(Email) ? Email : userEmail;

            var appointments = await doctorsService.GetAppointmentsAsync(targetEmail);

            if (appointments == null || appointments.Count == 0)
                return NotFound("No appointments found");

            return Ok(appointments);
        }


        [HttpPost("RequestLab")]
        [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> RequestLabAsync([FromForm] LabTestRequestDTO laboratoryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var doctorEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(doctorEmail))
                return Unauthorized("Doctor email not found in token.");

            laboratoryDTO.DoctorEmail = doctorEmail;

            var labRequest = await doctorsService.RequestLabAsync(laboratoryDTO);

            if (labRequest == null)
            {
                return BadRequest("This lab request already exists or failed to create.");
            }

            return Ok("Lab Request Created Successfully");
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateDoctor")]
        public async Task<IActionResult> UpdateDoctorAsync([FromBody] CreateDoctorDTO updateDto, string? Email)
        {
            if (string.IsNullOrEmpty(Email))
                return BadRequest("Email is required");

            var doctorExists = await hospitalContex.Doctors.FirstOrDefaultAsync(d => d.Email == Email);
            if (doctorExists == null)
                return NotFound("Doctor not found");

            var updated = await doctorsService.UpdateDoctorAsync(updateDto, Email);
            if (updated == null)
                return BadRequest("Update failed");

            var user = await userManager.FindByEmailAsync(Email);
            if (user != null)
            {
                user.UserName = updateDto.Name;
                user.PhoneNumber = updateDto.Phone;
                user.Email = updateDto.Email;

                var result = await userManager.UpdateAsync(user);
                if (!result.Succeeded)
                    return BadRequest("Failed to update Identity user.");
            }

            return Ok(updated);
        }

        [HttpGet("AllDepartment")]
        [Authorize(Roles = "Admin,Doctor")]
        public async Task<IActionResult> GetAllDepartments()
        {
            var depts = await hospitalContex.Departments
                .Select(d => new { d.Id, d.Name })
                .ToListAsync();

            return Ok(depts);
        }


    }
}
