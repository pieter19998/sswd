using System.Threading.Tasks;
using Core;

namespace ApplicationServices
{
    public interface IIntakeService
    {
        Task<IResult<Intake>> AddIntake(Intake intake);
        Task<IResult<Intake>> UpdateIntake(Intake intake, int id);
        Task<IResult<Intake>> GetIntake(int intakeId);
        Task<IResult<Intake>> DeleteIntake(int intakeId);
    }
}