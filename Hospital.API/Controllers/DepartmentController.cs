using HospitalAPI.Hospital.Application;
using HospitalAPI.Hospital.Application.DTO.DepartmentDTO;

using HospitalAPI.Hospital.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]

    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentService departmentService;
        private readonly HospitalContex hospitalContex;


        public DepartmentController(IDepartmentService DepartmentService, HospitalContex hospitalContex)
        {
            this.departmentService = DepartmentService;
            this.hospitalContex = hospitalContex;

        }

        [Authorize(Roles = "Admin")]
        [HttpPost("Create")]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentDTO dto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }
            var result = await departmentService.CreateDepartmentAsync(dto);
            if (result == null)
                return Conflict("Department name already exists.");

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteDepartment(string Name)
        {
            var Department = await hospitalContex.Departments.FirstOrDefaultAsync(i=>i.Name == Name);
            {
                if (Department == null)
                {
                    return BadRequest(ModelState);
                }
                await departmentService.DeleteDepartmentAsync(Name);
                return StatusCode(201, "Deleted SUCCCESS");
            }


        }
        [Authorize(Roles = "Admin,Doctor")]

        [HttpGet("GetByID/{id}")]
        public async Task<IActionResult> GetDeptById(int id)
        {
            var Dept = await departmentService.GetDepartmentAsync(id);
            if (Dept == null)
            {
                return NotFound();
            }
            return Ok(Dept);
        }
        [Authorize(Roles = "Admin")]

        [HttpGet("GetCountOfDoctors")]
        public async Task<IActionResult> GetDepartmentsWithDoctorsCountAsync(string Name)
        {
            var Dept = await departmentService.GetDepartmentsWithDoctorsCountAsync(Name);
            if (Dept == null)
            {
                return NotFound();
            }
            return Ok(Dept);
        }
        [Authorize(Roles = "Admin,Doctor")]
        [HttpGet("GetDepartmentsWithDoctorsNamest")]
        public async Task<IActionResult> GetDepartmentsWithDoctorsNamestAsync(string Name)
        {
            var Dept = await departmentService.GetDepartmentsWithDoctorsNamestAsync(Name);
            if (Dept == null)
            {
                return NotFound();
            }
            return Ok(Dept);
        }
        [Authorize(Roles = "Admin")]


        [HttpPut("Update")]
        public async Task<IActionResult> UpdateDepartmentAsync(CreateDepartmentDTO Department, string Name)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var Dept = await departmentService.UpdateDepartmentAsync(Department, Name);
            if (Dept == null)
            {
                return NotFound("Department not found or name already exists.");
            }
            return Ok(Dept);
        }
        [Authorize(Roles = "Admin,Doctor")]
        [HttpGet("AllDepartment")]
        public async Task<IActionResult> GetAllDepartments()
        {
            var depts = await hospitalContex.Departments
                .Select(d => new { d.Id, d.Name })
                .ToListAsync();

            return Ok(depts);
        }

    }


}