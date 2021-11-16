using System.Threading.Tasks;
using Core;

namespace ApplicationServices
{
    public interface ITreatmentPlanService
    {
        Task<IResult<TreatmentPlan>> AddTreatmentPlan(TreatmentPlan treatmentPlan, int dossierId);
        Task<IResult<TreatmentPlan>> GetTreatmentPlan(int dossierId);
    }
}