using System.Collections.Generic;
using System.Threading.Tasks;
using Core;

namespace DomainServices
{
    public interface IIntakeRepository
    {
        Task<Intake> GetIntake(int intakeId);
        Task<Intake> GetIntake(string email);
        Task<ICollection<Intake>> GetIntakes();
        Task AddIntake(Intake intake);
        Task UpdateIntake(Intake intake, int id);
        Task DeleteIntake(int id);
    }
}