using HospitalAPI.Hospital.Domain.Models;
using HospitalAPI.Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Hospital.Infrastructure
{
    public class LaboratoryRepository : ILaboratoryRepository
    {
        private readonly HospitalContex hospitalContex;

        public LaboratoryRepository(HospitalContex hospitalContex)
        {
            this.hospitalContex = hospitalContex;
        }
        public async Task AddAsync(Laboratory laboratory)
        {
            hospitalContex.Laboratory.Add(laboratory);
            await Save();
        }

        public async Task DeleteAsync(int id)
        {
            Laboratory lab = await hospitalContex.Laboratory.FindAsync(id);
            if (lab != null)
            {
                hospitalContex.Laboratory.Remove(lab);
                await Save();
            }

        }

        public async Task<List<Laboratory>> GetAllAsync()
        {
            return await hospitalContex.Laboratory.
                Include(a => a.doctor).
                Include(a => a.Patient).
                ToListAsync();
        }

        public async Task<Laboratory?> GetByIdAsync(int id)
        {
            return await hospitalContex.Laboratory.Include(i => i.Patient).Include(i => i.doctor).FirstOrDefaultAsync(i => i.Id == id);
        }

        public Task<List<Doctors>> SearchDoctorsAsync(string? specialization, string? name)
        {
            //    return hospitalContex.Doctors.
            //     Where(d =>
            //(string.IsNullOrEmpty(name) || d.Name.Contains(name)) &&
            //    (string.IsNullOrEmpty(specialization) || d.specialization.Contains(specialization))
            //    ).ToListAsync();
            return hospitalContex.Doctors
        .Where(d =>
            (string.IsNullOrEmpty(name) || EF.Functions.Like(d.Name, $"%{name}%")) &&
            (string.IsNullOrEmpty(specialization) || EF.Functions.Like(d.specialization, $"%{specialization}%"))
        ).ToListAsync();
        }

        public Task<List<Laboratory>> SearchLabTestsAsync(string? testType, string? status)
        {
            //        return hospitalContex.Laboratory.
            //Where(d =>
            //    (string.IsNullOrEmpty(testType) || d.TestType.Contains(testType)) &&
            //        (string.IsNullOrEmpty(status) || d.Status.Contains(status))
            //        ).ToListAsync();
            return hospitalContex.Laboratory
            .Where(d =>
                (string.IsNullOrEmpty(testType) || EF.Functions.Like(d.TestType, $"%{testType}%")) &&
                (string.IsNullOrEmpty(status) || EF.Functions.Like(d.Status, $"%{status}%"))
            ).ToListAsync();
        }

        public async Task UpdateAsync(int id, Laboratory laboratory)
        {
            Laboratory lab = await hospitalContex.Laboratory.FindAsync(id);
            if (lab is not null)
            {
                lab.ResultFilePath = laboratory.ResultFilePath;
                lab.ResultFile = laboratory.ResultFile;
                lab.Status = laboratory.Status;
                lab.DoctorId = laboratory.DoctorId;
                lab.PatientId = laboratory.PatientId;
                lab.TestType = laboratory.TestType;
                await Save();
            }

        }

        public async Task<bool> UpdateStatusAsync(int id, string newStatus)
        {
            Laboratory lab = await hospitalContex.Laboratory.FindAsync(id);
            if (lab is not null)
            {
                lab.Status = newStatus;
                await Save();
                return true;

            }
            return false;

        }
        public async Task Save()
        {
            await hospitalContex.SaveChangesAsync();
        }

    }
}
