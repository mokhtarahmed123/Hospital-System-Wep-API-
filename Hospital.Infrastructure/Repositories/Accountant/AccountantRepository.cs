using HospitalAPI.Hospital.Domain.Models;
using HospitalAPI.Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using HospitalAPI.Hospital.Domain.Models;
namespace HospitalAPI.Hospital.Infrastructure
{
    public class AccountantRepository : IAccountantRepository
    {
        private readonly HospitalContex hospitalContex;

        public AccountantRepository(HospitalContex hospitalContex)
        {
            this.hospitalContex = hospitalContex;
        }
        public async Task Add(Accountant accountant)
        {
            hospitalContex.Accountants.Add(accountant);
            await Save();
        }

        public async Task<bool> ConfirmBillingPayment(int billingId)
        {
            Billing billing = await hospitalContex.Billing.FindAsync(billingId);
            if (billing is not null)
            {
                billing.Status = "Completed";
                await Save();
                return true;
            }
            return false;
        }

        public async Task<Billing> CreateBilling(int patientId, Billing billing)
        {
            var patient = await hospitalContex.Patients.FindAsync(patientId);
            if (patient is null)
                throw new Exception("Patient not found");

            billing.PatientId = patientId;
            billing.Status = "Pending";
            billing.TotalAmount = 0;
            hospitalContex.Billing.Add(billing);
            await Save();
            return billing;
        }

        public async Task Delete(int id)
        {
            Accountant accountant = await hospitalContex.Accountants.FindAsync(id);
            if (accountant is null)
                throw new Exception("Account Not Found");

            hospitalContex.Accountants.Remove(accountant);
            await Save();
        }

        public async Task<List<Accountant>> GetAll()
        {
            return await hospitalContex.Accountants
                //Include(a => a.Staff)
                .Include(a => a.Role.Name).ToListAsync();
        }

        public async Task<List<Billing>> GetBillingsByAccountantId(int accountantId)
        {
            return await hospitalContex.Billing
                .Include(b => b.Patient)
                .Where(b => b.AccountantId == accountantId)
                .ToListAsync();

        }


        public async Task<List<Billing>> GetPendingBillings(int accountantId)
        {
            return await hospitalContex.Billing
        .Include(b => b.Patient)
        .Where(b => b.AccountantId == accountantId && b.Status == "Pending")
        .ToListAsync();
        }

        public async Task<Accountant?> GetProfile(int AccountantId)
        {
            return await hospitalContex.Accountants
                .Include(a => a.Role.Name)
                //.Include(a => a.Staff)
                .FirstOrDefaultAsync(a => a.Id == AccountantId);
        }

        public async Task<decimal> GetTotalCollectedAmount(int accountantId)
        {
            decimal totalCapacity = await hospitalContex.Billing.
                Where(a => a.AccountantId == accountantId && a.Status == "Completed").SumAsync(a => a.TotalAmount);
            return totalCapacity;
        }

        public async Task Update(Accountant accountant, int accountantId)
        {
            Accountant accountant1 = await hospitalContex.Accountants.FindAsync(accountantId);
            if (accountant1 == null)
                throw new Exception("Accountant not Found");
            accountant1.Certification = accountant.Certification;
            //accountant1.StaffId = accountant.StaffId;
            //accountant1.UserId = accountant.UserId;
            await Save();



        }
        public async Task Save()
        {
            await hospitalContex.SaveChangesAsync();
        }
    }
}
