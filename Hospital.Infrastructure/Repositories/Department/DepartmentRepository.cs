using HospitalAPI.Hospital.Domain.Models;
using HospitalAPI.Hospital.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;



namespace HospitalAPI.Hospital.Infrastructure
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly HospitalContex hospitalContex;

        public DepartmentRepository(HospitalContex hospitalContex)
        {
            this.hospitalContex = hospitalContex;
        }
        public async Task Add(Department department)
        {
            hospitalContex.Departments.Add(department);
            await Save();
        }

        public async Task Delete(int id)
        {
            Department department = await hospitalContex.Departments.FindAsync(id);
            if (department == null)
            {
                throw new Exception("Not Found");
            }
            hospitalContex.Departments.Remove(department);
            await Save();

        }

        public async Task<Department> departmentByAsync(int Id)
        {
            return await hospitalContex.Departments.
                Include(a => a.rooms).
                Include(a => a.staff_Management)
                .Include(a => a.Doctor).Include(i => i.inpatient_Admission).FirstOrDefaultAsync(a => a.Id == Id);

        }

        public async Task<bool> DepartmentExistsAsync(string name)
        {
            Department department = await hospitalContex.Departments.FirstOrDefaultAsync(a => a.Name == name);
            if (department == null)
            {
                return false;
            }
            return true;
        }

        public async Task<List<Department>> GetAllDepartment()
        {
            return await hospitalContex.Departments.
                  Include(a => a.Doctor).
                  Include(A => A.rooms).
                  Include(A => A.staff_Management)
                  .ToListAsync();
        }

        public async Task<List<Department>> SearchDepartmentsAsync(string? name)
        {
            return await hospitalContex.Departments.
       Include(a => a.Doctor).
       Include(A => A.rooms).
       Include(A => A.staff_Management).Where(i => string.IsNullOrEmpty(name) || i.Name.ToLower().Contains(name.ToLower()))

       .ToListAsync();
        }

        public async Task Update(Department department, int DepartmentId)
        {
            var dept = await hospitalContex.Departments.FindAsync(DepartmentId);
            if (dept == null) { return; }
            dept.Name = department.Name;
            await Save();

        }
        public async Task Save()
        {
            await hospitalContex.SaveChangesAsync();
        }
    }
}
