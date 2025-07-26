using System.ComponentModel.DataAnnotations;

namespace HospitalAPI.Hospital.Application.DTO.AppointmentDTO
{
    public class EditAppointment
    {

        public string Status { get; set; }
        public string DoctorEmail { get; set; }
        public DateTime AppointmentDate { get; set; }


    }
}
