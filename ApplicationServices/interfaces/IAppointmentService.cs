using System.Threading.Tasks;
using Core;

namespace ApplicationServices
{
    public interface IAppointmentService
    {
        Task<IResult<Appointment>> GetAppointment(int appointmentId);
        Task<IResult<Appointment>> ClaimAvailableAppointment(Appointment appointment);
        Task<IResult<Appointment>> CancelAppointment(int appointmentId);
    }
}