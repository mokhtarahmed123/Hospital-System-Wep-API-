using Microsoft.AspNetCore.Identity;
using System.Numerics;

namespace HospitalAPI.Hospital.Domain.Models
{
    public class UserApplication : IdentityUser
    {
        public Doctors? Doctor { get; set; }  // Navigation only

        public Patients? Patient { get; set; } // Navigation only
        public Staff_Management? staff_Management { get; set; } // Navigation only
        public Accountant? accountant { get; set; }


    }
}
