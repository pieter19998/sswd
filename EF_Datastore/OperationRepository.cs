using System.Collections.Generic;
using System.Threading.Tasks;
using Core.stam;
using DomainServices;
using Microsoft.EntityFrameworkCore;

namespace EF_Datastore
{
    public class OperationRepository : IOperationRepository
    {
        private readonly StamDbContext _context;

        public OperationRepository(StamDbContext context)
        {
            _context = context;
        }

        public async Task<Operation> GetOperation(string value)
        {
            return await _context.Operations.SingleOrDefaultAsync(x => x.Value == value);
        }

        public async Task<ICollection<Operation>> GetOperations()
        {
            return await _context.Operations.ToListAsync();
        }
    }
}