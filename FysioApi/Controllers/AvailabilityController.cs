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
    public class AvailabilityController : ControllerBase
    {
        private readonly IAvailabilityService _availabilityService;

        public AvailabilityController(IAppointmentRepository appointmentRepository,
            IDossierRepository dossierRepository, IAvailabilityRepository availabilityRepository)
        {
            _availabilityService = new AvailabilityService(availabilityRepository);
        }

        [HttpGet("/{availabilityId}")]
        public async Task<ActionResult<Availability>> GetAvailabilityById(int availabilityId)
        {
            var result = await _availabilityService.GetAvailability(availabilityId);
            return result.Success ? result.Payload : StatusCode(StatusCodes.Status404NotFound, result.Message);
        }

        [HttpPost]
        public async Task<ActionResult<Appointment>> PostAvailability([FromBody] Availability availability)
        {
            try
            {
                var result = await _availabilityService.AddAvailability(availability);
                if (result.Success) return StatusCode(StatusCodes.Status201Created, availability);

                return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "There was an error adding availability");
            }
        }

        [HttpPut("{availabilityId}")]
        public async Task<ActionResult<Appointment>> PutAvailability(int availabilityId,
            [FromBody] Availability availability)
        {
            try
            {
                var result = await _availabilityService.AddAvailability(availability);
                if (result.Success) return StatusCode(StatusCodes.Status200OK, availability);

                return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "There was an error edeting availability");
            }
        }

        [HttpDelete("{availabilityId}")]
        public async Task<ActionResult<Appointment>> DeleteAvailability(int availabilityId)
        {
            try
            {
                var result = await _availabilityService.DeleteAvailability(availabilityId);
                if (result.Success) return StatusCode(StatusCodes.Status200OK);

                return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "There was an error removing availability");
            }
        }
    }
}