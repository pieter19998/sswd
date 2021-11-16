using System.Threading.Tasks;
using ApplicationServices;
using Core;
using DomainServices;
using FysioApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FysioApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientRepository patientRepository)
        {
            _patientService =
                new PatientService(patientRepository);
        }

        [AllowAnonymous]
        [HttpGet("{patientId}")]
        public async Task<ActionResult<Patient>> GetPatientEmployee(int patientId)
        {
            var result = await _patientService.GetPatient(patientId);

            return result.Success ? result.Payload : StatusCode(StatusCodes.Status404NotFound, result.Message);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<Patient>> PostPatient(Patient patient)
        {
            try
            {
                var result = await _patientService.AddPatient(patient);
                if (result.Success) return StatusCode(StatusCodes.Status201Created, patient);

                return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "There was an error adding patient");
            }
        }

        [AllowAnonymous]
        [HttpPatch("{patientId}")]
        public async Task<ActionResult<Patient>> PatchPatient(int patientId, [FromBody] AddressType address)
        {
            try
            {
                var result = await _patientService.UpdatePatientAddress(patientId, address.Address);
                if (result.Success) return StatusCode(StatusCodes.Status200OK);

                return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "There was an error patching patient");
            }
        }

        [AllowAnonymous]
        [HttpPut("{patientId}")]
        public async Task<ActionResult<Patient>> PutPatient(int patientId, [FromBody] Patient patient)
        {
            try
            {
                var result = await _patientService.UpdatePatient(patient, patientId);
                if (result.Success) return StatusCode(StatusCodes.Status200OK);

                return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "There was an error patching patient");
            }
        }
    }
}