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
    public class DossierController : ControllerBase
    {
        private readonly IDossierServices _dossierServices;

        public DossierController(IDossierRepository dossierRepository, IPatientRepository patientRepository,
            IOperationRepository operationRepository)
        {
            _dossierServices = new DossierService(dossierRepository, patientRepository, operationRepository);
        }

        [HttpGet("{dossierId}")]
        public async Task<ActionResult<Dossier>> GetDossier(int dossierId)
        {
            var result = await _dossierServices.GetDossier(dossierId);
            return result.Success ? result.Payload : StatusCode(StatusCodes.Status404NotFound, result.Message);
        }

        [HttpGet("patient/{dossierId}")]
        public async Task<ActionResult<Dossier>> GetDossierPatient(int dossierId)
        {
            var result = await _dossierServices.GetDossierPatient(dossierId);
            return result.Success ? result.Payload : StatusCode(StatusCodes.Status404NotFound, result.Message);
        }

        [HttpPost]
        public async Task<ActionResult<Dossier>> PostDossier(Dossier dossier)
        {
            try
            {
                var result = await _dossierServices.AddDossier(dossier);
                dossier.HeadPractitioner = null;
                dossier.Patient = null;
                if (result.Success) return StatusCode(StatusCodes.Status201Created, dossier);

                return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "There was an error adding dossier");
            }
        }

        [HttpPut("{dossierId}")]
        public async Task<ActionResult<Dossier>> Update(int dossierId, [FromBody] Dossier dossier)
        {
            try
            {
                await _dossierServices.UpdateDossier(dossier, dossierId);
                return Ok(dossier);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "There was an error patching dossier");
            }
        }
    }
}