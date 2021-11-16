using System.Collections.Generic;
using System.Threading.Tasks;
using Core;

namespace DomainServices
{
    public interface IAppointmentRepository
    {
        Task<Appointment> GetAppointment(int appointmentId);
        Task<ICollection<Appointment>> GetAppointments();
        Task<ICollection<Appointment>> GetAppointmentsByPatient(int patientId);
        Task<ICollection<Appointment>> GetAppointmentsByPatientThisWeek(int patientId);
        Task<ICollection<Appointment>> GetAppointmentsByEmployee(int employeeId);
        Task CreateAppointment(Appointment appointment);
        Task UpdateAppointment(Appointment appointment);
        Task CancelAppointment(int appointmentId);
    }
}