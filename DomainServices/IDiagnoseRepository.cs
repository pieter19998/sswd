using System.Collections.Generic;
using System.Threading.Tasks;
using Core.stam;

namespace DomainServices
{
    public interface IDiagnoseRepository
    {
        Task<Diagnose> GetDiagnose(int code);
        Task<ICollection<Diagnose>> GetDiagnoses();
    }
}