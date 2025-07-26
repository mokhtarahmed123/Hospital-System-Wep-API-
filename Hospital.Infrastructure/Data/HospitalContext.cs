using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HospitalAPI.Hospital.Domain.Models;
namespace HospitalAPI.Hospital.Infrastructure.Data
{
    public class HospitalContex : IdentityDbContext<UserApplication>
    {
        public DbSet<Patients> Patients { get; set; }
        public DbSet<Doctors> Doctors { get; set; }
        public DbSet<Appointments> Appointments { get; set; }
        public DbSet<Laboratory> Laboratory { get; set; }
        public DbSet<Billing> Billing { get; set; }
        public DbSet<Inpatient_Admission> InpatientAdmissions { get; set; }
        public DbSet<Rooms> Rooms { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Staff_Management> Staff_Managements { get; set; }
        public DbSet<Accountant> Accountants { get; set; }

        public DbSet<RevokedToken> revokedTokens { get; set; }


        public HospitalContex()
        {

        }
        public HospitalContex(DbContextOptions<HospitalContex> options) : base(options)
        {
        }
    }
}
