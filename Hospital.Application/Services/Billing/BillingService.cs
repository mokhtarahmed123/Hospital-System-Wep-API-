using HospitalAPI.Hospital.Application.DTO.BillingDTO;
using HospitalAPI.Hospital.Domain.Models;

//using HospitalAPI.Hospital.Domain.Models;
using HospitalAPI.Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Hospital.Application
{
    public class BillingService : IBillingService
    {
        private readonly HospitalContex contex;

        public BillingService(HospitalContex contex)
        {
            this.contex = contex;
        }
        public async Task<bool> BillExistsAsync(int id)
        {
            var Bill = await contex.Billing.FindAsync(id);
            return Bill != null;
        }

        public async Task<CreateBillDTO> CreateBillAsync(CreateBillDTO Bill)
        {
            var Patient = await contex.Patients.FirstOrDefaultAsync(a=>a.Email == Bill.PatientEmail);
            if (Patient == null) return null;
            var Accountant = await contex.Accountants.FirstOrDefaultAsync(a=>a.Email == Bill.ACCOUNTATEmail);
            if (Accountant == null) return null;
            Billing billing = new Billing()
            {
                PatientId = Patient.ID,
                AccountantId = Accountant.Id,
                TotalAmount = (int)Bill.TotalAmount,
                Status = Bill.Status,
                DateIssued = Bill.BillDate
            };
            await contex.Billing.AddAsync(billing);
            await contex.SaveChangesAsync();
            return Bill;

        }

        public async Task<bool> DeleteBillAsync(int id)
        {
            var Bill = await contex.Billing.FindAsync(id);
            if (Bill == null) { return false; }
            contex.Billing.Remove(Bill);
            await contex.SaveChangesAsync();
            return true;
        }

        public async Task<BillExportDTO?> ExportBillAsPdfAsync(int billId)
        {
            var bill = await contex.Billing
            .Include(b => b.Patient)
              .Include(b => b.Accountant)
           .FirstOrDefaultAsync(b => b.Id == billId);
            if (bill == null) return null;
            var exportDto = new BillExportDTO
            {
                PatientName = bill.Patient.Name,
                TotalAmount = bill.TotalAmount,
                Status = bill.Status,
                BillDate = (DateTime)bill.DateIssued
            };
            return exportDto;
        }

        public async Task<List<BillDetailsDTO>> Filter(BillFilterDTO billFilter)
        {
            var query = contex.Billing
                .Include(d => d.Patient)
                .Include(d => d.Accountant)
                .AsQueryable();

            if (!string.IsNullOrEmpty(billFilter.Status))
            {
                query = query.Where(d => d.Status.Contains(billFilter.Status));
            }

            if (billFilter.PatientId.HasValue)
            {
                query = query.Where(d => d.PatientId == billFilter.PatientId.Value);
            }

            if (billFilter.AccountentId.HasValue)
            {
                query = query.Where(d => d.AccountantId == billFilter.AccountentId.Value);
            }

            var result = await query.Select(d => new BillDetailsDTO
            {
                BillId = d.Id,
                Status = d.Status,
                TotalAmount = d.TotalAmount,
                BillDate = (DateTime)d.DateIssued,
                PatientID = d.PatientId,
                AccountentID = d.AccountantId,
                PatientName = d.Patient.Name,
            }).ToListAsync();

            return result;
        }

        public async Task<List<BillDetailsDTO>> GetAll()
        {
            return await contex.Billing
             .Include(b => b.Patient)
             .Include(b => b.Accountant)
             .Select(b => new BillDetailsDTO
             {
                 BillId = b.Id,
                 PatientName = b.Patient.Name,
                 AccountentID = b.AccountantId,
                 TotalAmount = b.TotalAmount,
                 Status = b.Status,
                 BillDate = (DateTime)b.DateIssued,
                 PatientID = b.PatientId,
             }).ToListAsync();
        }

        public async Task<CreateBillDTO> GetBillById(int id)
        {
            var Bill = await contex.Billing.Include(a => a.Accountant).Include(a => a.Patient).FirstOrDefaultAsync(a => a.Id == id);
            if (Bill == null) return null;
            CreateBillDTO DTO = new CreateBillDTO()
            {
                ACCOUNTATEmail = Bill.Accountant.Email,
                Status = Bill.Status,
                TotalAmount = Bill.TotalAmount,
                PatientEmail = Bill.Patient.Email,
                BillDate = (DateTime)Bill.DateIssued

            };
            return DTO;
        }

        public async Task<List<BillDetailsDTO>> GetBillsByPatientIdAsync(string PatientEmail)
        {
            var exists = await contex.Patients.AnyAsync(p => p.Email == PatientEmail);
            if (!exists)
                return new List<BillDetailsDTO>();

            var bills = await contex.Billing
                .Include(b => b.Accountant)
                .Include(b => b.Patient)
                .Where(b => b.Patient.Email == PatientEmail)
                .Select(b => new BillDetailsDTO
                {
                    BillId = b.Id,
                    Status = b.Status,
                    TotalAmount = b.TotalAmount,
                    BillDate = (DateTime)b.DateIssued,
                    PatientID = b.PatientId,
                    AccountentID = b.AccountantId,
                    PatientName = b.Patient.Name,
                })
                .ToListAsync();

            return bills;
        }

        public async Task<CreateBillDTO> UpdateBillAsync(CreateBillDTO Bill, int id)
        {
            var Patient = await contex.Patients.FirstOrDefaultAsync(a => a.Email == Bill.PatientEmail);
            if (Patient == null) return null;
            var Accountant = await contex.Accountants.FirstOrDefaultAsync(a => a.Email == Bill.ACCOUNTATEmail);
            if (Accountant == null) return null;

            var OldBill = await contex.Billing
                .Include(a => a.Patient)
                .Include(a => a.Accountant)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (OldBill == null) return null;

            OldBill.PatientId = Patient.ID;
            OldBill.AccountantId = Accountant.Id;
            OldBill.TotalAmount = (int)Bill.TotalAmount;
            OldBill.Status = Bill.Status;
            OldBill.DateIssued = Bill.BillDate;

            // حفظ التعديلات
            await contex.SaveChangesAsync();

            return Bill;
        }
    }
}
