using HospitalAPI.Hospital.Domain.Models;

namespace HospitalAPI.Hospital.Infrastructure
{
    public interface IBillingRepository

    {
        Task<List<Billing>> GetAllAsync();
        Task<Billing?> GetByIdAsync(int id);
        Task UpdateAsync(int id, Billing billing);
        Task DeleteAsync(int id);
        Task<bool> UpdateStatusAsync(int id, string newStatus);
        Task<List<Billing>> GetByPatientIdAsync(int patientId);
        Task<List<Billing>> GetByStatusAsync(string status);//Filter
        Task<decimal> GetTotalAmountForPatientAsync(int patientId);
        Task<Billing> CreateBilling(int patientId, Billing billing);
        Task<bool> BillingExistsAsync(int billingId);



    }
}
