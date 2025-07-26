using HospitalAPI.Hospital.Application.DTO.AccountantDTO;

namespace HospitalAPI.Hospital.Application.Services.Accountant
{
    public interface IAccountantService
    {
        Task<CreateAccountantdto> CreateAccountanAsync(CreateAccountantdto dto);
        Task<CreateAccountantdto> UpdateAccountanAsync(CreateAccountantdto Accountan, string Email);
        Task<bool> DeleteAccountanAsync(string Email);
        Task<CreateAccountantdto> GetProfileAsync(string Email);
        Task<List<GetAccountantDTO>> GetAllBilling(string Email);

    }
}
