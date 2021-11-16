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
    public class TreatmentPlanController : ControllerBase
    {
        private readonly ITreatmentPlanService _treatmentPlanService;

        public TreatmentPlanController(ITreatmentPlanRepository treatmentPlanRepository,
            IDossierRepository dossierRepository)
        {
            _treatmentPlanService =
                new TreatmentPlanService(treatmentPlanRepository, dossierRepository);
        }

        [HttpGet("{treatmentPlanId}")]
        public async Task<ActionResult<TreatmentPlan>> GetTreatmentPlan(int treatmentPlanId)
        {
            var result = await _treatmentPlanService.GetTreatmentPlan(treatmentPlanId);
            return result.Success ? result.Payload : StatusCode(StatusCodes.Status404NotFound, result.Message);
        }

        [HttpPost("{dossierId}")]
        public async Task<ActionResult<TreatmentPlan>> PostTreatmentPlan(TreatmentPlan treatmentPlan, int dossierId)
        {
            try
            {
                var result = await _treatmentPlanService.AddTreatmentPlan(treatmentPlan, dossierId);
                if (result.Success)
                {
                    treatmentPlan.Dossier = null;
                    return StatusCode(StatusCodes.Status201Created, treatmentPlan);
                }

                return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "There was an error adding treatmentPlan");
            }
        }
    }
}