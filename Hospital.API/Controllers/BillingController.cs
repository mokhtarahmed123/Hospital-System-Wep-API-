using HospitalAPI.Hospital.Application;
using HospitalAPI.Hospital.Application.DTO.BillingDTO;
using HospitalAPI.Hospital.Domain.Models;

//using HospitalAPI.Hospital.Application.Services.Billing;
using HospitalAPI.Hospital.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Hospital.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class BillingController : ControllerBase
    {
        private readonly IBillingService billingService;
        private readonly HospitalContex hospitalContex;
        private readonly UserManager<UserApplication> userManager;
        private readonly RoleManager<RoleApplication> roleManager;

        public BillingController(IBillingService _bllingService, 
            HospitalContex hospitalContex,
            UserManager<UserApplication> userManager,
            RoleManager<RoleApplication> roleManager)
        {
            billingService = _bllingService;
            this.hospitalContex = hospitalContex;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }


        [Authorize(Roles = "Admin,Accountant")]
        [HttpPost("CreateBill")]
        public async Task<IActionResult> CreateBill([FromForm] CreateBillDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }
            var result = await billingService.CreateBillAsync(dto);
            if (result == null)
                return Conflict("Bill  already exists.");

            return Ok(result);

        }
        [Authorize(Roles = "Admin,Accountant")]
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteBill(int id)
        {
            var Billing = await hospitalContex.Billing.FindAsync(id);
            {
                if (Billing == null)
                {
                    return BadRequest(ModelState);
                }
                await billingService.DeleteBillAsync(id);
                return NoContent();
            }
        }
        [Authorize(Roles = "Admin,Accountant")]
        [HttpGet("GetByID/{id}")]
        public async Task<IActionResult> GetBillById(int id)
        {
            var Billing = await hospitalContex.Billing.FindAsync(id);
            {
                if (Billing == null)
                {
                    return NotFound($"{id} NOT FOUND");
                }
                var Bill = await billingService.GetBillById(id);
                return Ok(Bill);
            }
        }
        [Authorize(Roles = "Admin,Accountant")]
        [HttpGet("GetAllBilling")]
        public async Task<IActionResult> GetAllBilling()
        {

            var bills = await billingService.GetAll();
            return Ok(bills);
        }
        [HttpPost("Filter")]
        public async Task<IActionResult> Filter([FromForm] BillFilterDTO billFilter)
        {
            if (billFilter == null)
                return BadRequest("Invalid filter data");
            var results = await billingService.Filter(billFilter);
            return Ok(results);
        }
        [Authorize(Roles = "Admin,Accountant")]
        [HttpGet("GetBillsByPatientId")]
        public async Task<IActionResult> GetBillsByPatientId(string Email)
        {
            var PID = await hospitalContex.Billing.Include(a=>a.Patient).FirstOrDefaultAsync(a => a.Patient.Email == Email);
            if (PID == null) return NotFound("Patient Not Found");

            var Details = await billingService.GetBillsByPatientIdAsync(Email);
            return Ok(Details);

        }
        [Authorize(Roles = "Admin,Accountant")]

        [HttpGet("ExportBillAsPdf/{billId}")]
        public async Task<IActionResult> ExportBillAsPdf(int billId)
        {
            var Bill = await hospitalContex.Billing.FindAsync(billId);
            if (Bill == null) return NotFound("Not Found");
            var dATA = await billingService.BillExistsAsync(billId);
            return Ok(dATA);
        }
        [Authorize(Roles = "Admin,Accountant")]
        [HttpPut("UpdateData/{id}")]
        public async Task<IActionResult> UpdateBillAsync([FromBody] CreateBillDTO Bill, int id)
        {
            var ISFound = await hospitalContex.Billing.FindAsync(id);
            if (ISFound == null) return NotFound("Not Found");
            if (Bill == null) return BadRequest("Please Fill Al l Fildes");
            var SendData = await billingService.UpdateBillAsync(Bill, id);
            return Ok(SendData);
        }
    }
}
