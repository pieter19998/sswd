using System.Collections.Generic;
using System.Threading.Tasks;
using Core.stam;

namespace DomainServices
{
    public interface IOperationRepository
    {
        Task<Operation> GetOperation(string value);
        Task<ICollection<Operation>> GetOperations();
    }
}