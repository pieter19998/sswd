using System;
using System.Threading.Tasks;
using Core;
using DomainServices;

namespace ApplicationServices
{
    public class TreatmentPlanService : ITreatmentPlanService
    {
        private readonly IDossierRepository _dossierRepository;
        private readonly ITreatmentPlanRepository _treatmentPlanRepository;

        public TreatmentPlanService(ITreatmentPlanRepository treatmentPlanRepository,
            IDossierRepository dossierRepository)
        {
            _treatmentPlanRepository = treatmentPlanRepository;
            _dossierRepository = dossierRepository;
        }

        public async Task<IResult<TreatmentPlan>> AddTreatmentPlan(TreatmentPlan treatmentPlan, int dossierId)
        {
            var result = IsValid(treatmentPlan);
            try
            {
                if (!result.Success) return result;
                await _treatmentPlanRepository.AddTreatmentPlan(treatmentPlan);
            }

            catch (Exception e)
            {
                Console.WriteLine(e);
                result.Message = e.Message;
                result.Success = false;
                return result;
            }

            try
            {
                var dossier = await _dossierRepository.GetDossier(dossierId);
                if (dossier.TreatmentPlanId is > 0)
                {
                    result.Message = "Dossier already has a treatmentplan";
                    result.Success = false;
                }
                else
                {
                    await _dossierRepository.AddTreatmentPlan(treatmentPlan, dossier);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result.Message = "Dossier id not found";
                result.Success = false;
            }

            return result;
        }

        public async Task<IResult<TreatmentPlan>> GetTreatmentPlan(int treatmentPlanId)
        {
            IResult<TreatmentPlan> result = new Result<TreatmentPlan>();
            result.Payload = await _treatmentPlanRepository.GetTreatmentPlan(treatmentPlanId);
            if (result.Payload == null)
            {
                result.Message = ErrorMessages.IdNotFound;
                result.Success = false;
            }

            return result;
        }


        public IResult<TreatmentPlan> IsValid(TreatmentPlan treatmentPlan)
        {
            IResult<TreatmentPlan> result = new Result<TreatmentPlan>();
            if (treatmentPlan.SessionDuration == 0) result.Message += ErrorMessages.SessionPerWeekError;
            if (treatmentPlan.SessionsPerWeek == 0) result.Message += ErrorMessages.SessionDurationError;
            if (result.Message.Length > 1) result.Success = false;
            return result;
        }
    }
}