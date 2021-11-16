using System.Collections.Generic;
using System.Threading.Tasks;
using Core.stam;
using DomainServices;
using Microsoft.EntityFrameworkCore;

namespace EF_Datastore
{
    public class DiagnoseRepository : IDiagnoseRepository
    {
        private readonly StamDbContext _context;

        public DiagnoseRepository(StamDbContext context)
        {
            _context = context;
        }

        public async Task<Diagnose> GetDiagnose(int code)
        {
            return await _context.Diagnoses.SingleOrDefaultAsync(x => x.Code == code);
        }

        public async Task<ICollection<Diagnose>> GetDiagnoses()
        {
            return await _context.Diagnoses.ToListAsync();
        }
    }
}