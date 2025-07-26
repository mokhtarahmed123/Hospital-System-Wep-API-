
using HospitalAPI.Hospital.Application.DTO.BillingDTO;


namespace HospitalAPI.Hospital.Application
{
    public interface IBillingService
    {
        Task<CreateBillDTO> CreateBillAsync(CreateBillDTO Bill);
        Task<CreateBillDTO> UpdateBillAsync(CreateBillDTO Bill, int id);
        Task<bool> DeleteBillAsync(int id);
        Task<CreateBillDTO> GetBillById(int id);
        Task<List<BillDetailsDTO>> GetAll();
        Task<List<BillDetailsDTO>> Filter(BillFilterDTO billFilter);
        Task<List<BillDetailsDTO>> GetBillsByPatientIdAsync(string PatientEmail);
        Task<BillExportDTO> ExportBillAsPdfAsync(int billId);
        Task<bool> BillExistsAsync(int id);
        //Task<PaginatedResult<BillDetailsDTO>> GetPagedBillsAsync(int pageNumber, int pageSize);

    }
}
