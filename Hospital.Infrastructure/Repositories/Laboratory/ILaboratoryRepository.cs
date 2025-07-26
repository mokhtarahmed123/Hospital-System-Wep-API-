using HospitalAPI.Hospital.Domain.Models;

namespace HospitalAPI.Hospital.Infrastructure
{
    public interface ILaboratoryRepository
    {
        Task<List<Laboratory>> GetAllAsync();
        Task<Laboratory?> GetByIdAsync(int id);
        Task AddAsync(Laboratory laboratory);
        Task UpdateAsync(int id, Laboratory laboratory);
        Task DeleteAsync(int id);
        Task<bool> UpdateStatusAsync(int id, string newStatus);
        Task<List<Doctors>> SearchDoctorsAsync(string? specialization, string? name);
        Task<List<Laboratory>> SearchLabTestsAsync(string? testType, string? status);

    }
}
