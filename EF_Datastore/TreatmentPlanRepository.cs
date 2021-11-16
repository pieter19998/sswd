using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using DomainServices;
using Microsoft.EntityFrameworkCore;

namespace EF_Datastore
{
    public class TreatmentPlanRepository : ITreatmentPlanRepository
    {
        private readonly PracticeDbContext _context;

        public TreatmentPlanRepository(PracticeDbContext context)
        {
            _context = context;
        }

        public async Task<TreatmentPlan> GetTreatmentPlan(int treatmentId)
        {
            return await _context.TreatmentPlans.SingleOrDefaultAsync(t => t.TreatmentPlanId == treatmentId);
        }

        public async Task<ICollection<TreatmentPlan>> GetTreatmentPlans()
        {
            return await _context.TreatmentPlans.Where(s => s.Active).Include(x => x.Dossier).ToListAsync();
        }

        public async Task AddTreatmentPlan(TreatmentPlan treatmentPlan)
        {
            await _context.AddAsync(treatmentPlan);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTreatmentPlan(TreatmentPlan treatmentPlan)
        {
            _context.TreatmentPlans.Update(treatmentPlan);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTreatmentPlan(int treatmentId)
        {
            _context.TreatmentPlans.FindAsync(treatmentId).Result.Active = false;
            await _context.SaveChangesAsync();
        }
    }
}