using System.Collections.Generic;
using System.Threading.Tasks;
using Core;

namespace DomainServices
{
    public interface ITreatmentPlanRepository
    {
        Task<TreatmentPlan> GetTreatmentPlan(int treatmentId);
        Task<ICollection<TreatmentPlan>> GetTreatmentPlans();
        Task AddTreatmentPlan(TreatmentPlan treatmentPlan);
        Task UpdateTreatmentPlan(TreatmentPlan treatmentPlan);
        Task DeleteTreatmentPlan(int treatmentId);
    }
}