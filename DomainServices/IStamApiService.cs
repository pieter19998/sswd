using System.Collections.Generic;
using System.Threading.Tasks;
using Core.stam;

namespace DomainServices
{
    public interface IStamApiService
    {
        public Task<ICollection<Operation>> GetOperations();
        Task<Operation> GetOperation(string id);
        Task<Diagnose> GetDiagnose(int code);
        Task<ICollection<Diagnose>> GetDiagnoses();
    }
}