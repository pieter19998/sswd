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
    public class IntakeController : ControllerBase
    {
        private readonly IIntakeService _intakeService;

        public IntakeController(IIntakeRepository intakeRepository, IAppointmentRepository appointmentRepository)
        {
            _intakeService = new IntakeService(intakeRepository, appointmentRepository);
        }

        [HttpGet("{intakeId}")]
        public async Task<ActionResult<Intake>> GetIntake(int intakeId)
        {
            var result = await _intakeService.GetIntake(intakeId);
            return result.Success ? result.Payload : StatusCode(StatusCodes.Status404NotFound, result.Message);
        }

        [HttpPost]
        public async Task<ActionResult<Intake>> PostIntake([FromBody] Intake intake)
        {
            try
            {
                var result = await _intakeService.AddIntake(intake);
                if (result.Success)
                {
                    intake.Appointment = null;
                    return StatusCode(StatusCodes.Status201Created, intake);
                }

                return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "There was an error adding intake");
            }
        }

        [HttpPut("{intakeId}")]
        public async Task<ActionResult<Intake>> PutIntake(int intakeId, [FromBody] Intake intake)
        {
            try
            {
                var result = await _intakeService.UpdateIntake(intake, intakeId);
                if (result.Success)
                {
                    intake.Appointment = null;
                    return StatusCode(StatusCodes.Status201Created, intake);
                }

                return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "There was an error adding intake");
            }
        }

        [HttpDelete("{intakeId}")]
        public async Task<ActionResult<Intake>> DeleteIntake(int intakeId)
        {
            try
            {
                var result = await _intakeService.DeleteIntake(intakeId);
                if (result.Success) return StatusCode(StatusCodes.Status200OK);

                return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "There was an error removing intake");
            }
        }
    }
}