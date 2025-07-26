using HospitalAPI.Hospital.Domain.Models;
using HospitalAPI.Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Hospital.Infrastructure
{
    public class BillingRepository : IBillingRepository
    {
        private readonly HospitalContex hospitalContex;

        public BillingRepository(HospitalContex hospitalContex)
        {
            this.hospitalContex = hospitalContex;
        }
        public async Task<bool> BillingExistsAsync(int billingId)
        {
            var Bill = await hospitalContex.Billing.FindAsync(billingId);
            if (Bill == null)
            {
                return false;
            }
            return true;
        }

        public async Task<Billing> CreateBilling(int patientId, Billing billing)
        {
            Patients Patient = await hospitalContex.Patients.FindAsync(patientId);
            if (Patient == null)
            {
                throw new Exception("Patient Not Found");
            }
            Billing billing1 = new Billing();
            billing1.DateIssued = DateTime.Now;
            billing1.Status = billing.Status;
            billing1.TotalAmount = billing.TotalAmount;
            billing1.PatientId = patientId;
            billing1.AccountantId = billing.AccountantId;
            hospitalContex.Billing.Add(billing1);
            await Save();
            return billing1;

        }

        public async Task DeleteAsync(int id)
        {
            var Billing = await hospitalContex.Billing.FindAsync(id);
            if (Billing == null)
            {
                throw new Exception("Not FOUND");
            }
            hospitalContex.Billing.Remove(Billing);
            await Save();

        }

        public async Task<List<Billing>> GetAllAsync()
        {
            return await hospitalContex.Billing.
                Include(a => a.Accountant).
                Include(a => a.Patient).
                ToListAsync();
        }

        public async Task<Billing?> GetByIdAsync(int id)
        {
            return await hospitalContex.Billing.
                Include(a => a.Accountant).
                Include(a => a.Patient).
                FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Billing>> GetByPatientIdAsync(int patientId)
        {
            return await hospitalContex.Billing.
             Include(a => a.Accountant).
             Include(a => a.Patient).
             Where(a => a.PatientId == patientId).ToListAsync();
        }

        public async Task<List<Billing>> GetByStatusAsync(string status)
        {
            return await hospitalContex.Billing.
            Include(a => a.Accountant).
            Include(a => a.Patient).
            Where(a => a.Status == status).ToListAsync();
        }

        public async Task<decimal> GetTotalAmountForPatientAsync(int patientId)
        {
            decimal totalCapacity = await hospitalContex.Billing.
            Where(a => a.PatientId == patientId && a.Status == "Completed").SumAsync(a => a.TotalAmount);
            return totalCapacity;
        }

        public async Task UpdateAsync(int id, Billing billing)
        {
            var Billing = await hospitalContex.Billing.Include(a => a.Patient).Include(a => a.Accountant).
                FirstOrDefaultAsync(I => I.Id == id);
            if (Billing == null)
            {
                throw new Exception("Not found");
            }
            Billing.Status = billing.Status;
            Billing.TotalAmount = billing.TotalAmount;
            Billing.AccountantId = billing.AccountantId;
            Billing.PatientId = billing.PatientId;
            await Save();

        }

        public async Task<bool> UpdateStatusAsync(int id, string newStatus)
        {
            var Billing = await hospitalContex.Billing.Include(a => a.Patient).Include(a => a.Accountant).
      FirstOrDefaultAsync(I => I.Id == id);
            if (Billing == null)
            {
                throw new Exception("Not found");

            }
            Billing.Status = newStatus;
            await Save();
            return true;
        }
        public async Task Save()
        {
            await hospitalContex.SaveChangesAsync();
        }
    }
}
