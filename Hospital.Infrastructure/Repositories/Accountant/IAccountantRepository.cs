using HospitalAPI.Hospital.Domain.Models;

namespace HospitalAPI.Hospital.Infrastructure
{
    public interface IAccountantRepository
    {
        Task<List<Accountant>> GetAll();
        Task<Accountant?> GetProfile(int AccountantId);
        Task Add(Accountant accountant);
        Task Update(Accountant accountant, int accountantId);
        Task Delete(int id);
        Task<List<Billing>> GetBillingsByAccountantId(int accountantId);
        //Task<List<Billing>> GetBillingsByDateRange(int accountantId, DateTime start, DateTime end);
        Task<List<Billing>> GetPendingBillings(int accountantId);

        Task<bool> ConfirmBillingPayment(int billingId);
        Task<decimal> GetTotalCollectedAmount(int accountantId);


    }
}
