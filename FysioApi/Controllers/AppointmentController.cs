using System.Threading.Tasks;
using ApplicationServices;
using Core;
using DomainServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FysioApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentController(IAppointmentRepository appointmentRepository,
            IDossierRepository dossierRepository, IAvailabilityRepository availabilityRepository)
        {
            _appointmentService =
                new AppointmentService(appointmentRepository, dossierRepository, availabilityRepository);
        }

        [HttpGet("{appointmentId}")]
        public async Task<ActionResult<Appointment>> GetAppointment(int appointmentId)
        {
            var result = await _appointmentService.GetAppointment(appointmentId);
            return result.Success ? result.Payload : StatusCode(StatusCodes.Status404NotFound, result.Message);
        }

        [HttpPost]
        public async Task<ActionResult<Appointment>> ClaimAppointment([FromBody] Appointment appointment)
        {
            try
            {
                var result = await _appointmentService.ClaimAvailableAppointment(appointment);
                appointment.Session = null;
                appointment.EfEmployee = null;
                appointment.Intake = null;
                appointment.Patient = null;
                if (result.Success) return StatusCode(StatusCodes.Status200OK, appointment);

                return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "There was an error claiming appointment");
            }
        }

        [HttpDelete("{intakeId}")]
        public async Task<ActionResult<Appointment>> DeleteAppointment(int intakeId)
        {
            try
            {
                var result = await _appointmentService.CancelAppointment(intakeId);
                if (result.Success) return StatusCode(StatusCodes.Status200OK);

                return StatusCode(StatusCodes.Status404NotFound, result.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "There was an error canceling appointment");
            }
        }
    }
}