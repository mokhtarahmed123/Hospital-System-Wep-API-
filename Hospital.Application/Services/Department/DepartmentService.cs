using HospitalAPI.Hospital.Application.DTO.DepartmentDTO;
using HospitalAPI.Hospital.Domain.Models;
using HospitalAPI.Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Hospital.Application
{
    public class DepartmentService : IDepartmentService
    {
        private readonly HospitalContex contex;
        public DepartmentService(HospitalContex contex)
        {
            this.contex = contex;
        }
        public async Task<CreateDepartmentDTO> CreateDepartmentAsync(CreateDepartmentDTO dto)
        {
            if (dto == null) { return null; }

            bool isExist = await contex.Departments
                .AnyAsync(d => d.Name.ToLower() == dto.Name.ToLower());

            if (isExist)
                return null;

            var department = new Department
            {
                Name = dto.Name
            };

            await contex.Departments.AddAsync(department);
            await contex.SaveChangesAsync();
            return dto;
        }

        public async Task<bool> DeleteDepartmentAsync(string Name)
        {
            var Dept = await contex.Departments.FirstOrDefaultAsync(i=>i.Name == Name);
            if (Dept == null) { return false; }
            contex.Departments.Remove(Dept);
            await contex.SaveChangesAsync();
            return true;
        }
        public async Task<CreateDepartmentDTO> GetDepartmentAsync(int id)
        {
            var Dept = await contex.Departments.FindAsync(id);
            if (Dept == null) return null;
            CreateDepartmentDTO dto = new CreateDepartmentDTO
            {
                Name = Dept.Name,
            };
            return dto;

        }

        public async Task<DepartmentWithDoctorsCountDTO> GetDepartmentsWithDoctorsCountAsync(string Name)
        {
            {
                var Dept = await contex.Departments.FirstOrDefaultAsync(i=>i.Name == Name);
                int totalDoctors = await contex.Departments
                    .SelectMany(d => d.Doctor)
                    .CountAsync(i => i.Name == Name); if (Dept == null) return null;
                DepartmentWithDoctorsCountDTO dto = new DepartmentWithDoctorsCountDTO
                {
                    //Id = id,
                    Name = Dept.Name,
                    Doctors = totalDoctors
                };
                return dto;
            }

        }
        public async Task<DepartmentWithDoctorsNamesDTO> GetDepartmentsWithDoctorsNamestAsync(string Name)
        {
            var Dept = await contex.Departments.Include(a => a.Doctor).FirstOrDefaultAsync(i => i.Name == Name);

            DepartmentWithDoctorsNamesDTO dto = new DepartmentWithDoctorsNamesDTO
            {
                //Id = id,
                Name = Dept.Name,
                DoctorNames = Dept.Doctor.Select(a => a.Name).ToList()
            };
            return dto;
        }

        public async Task<CreateDepartmentDTO> UpdateDepartmentAsync(CreateDepartmentDTO Department, string NewName)
        {
            var Dept = await contex.Departments.FirstOrDefaultAsync(i => i.Name.ToLower() == Department.Name.ToLower());
            if (Dept == null) return null;

            Dept.Name = NewName;
            await contex.SaveChangesAsync();

            return new CreateDepartmentDTO { Name = NewName };
        }

    }
}
