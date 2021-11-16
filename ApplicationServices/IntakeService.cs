using System;
using System.Threading.Tasks;
using Core;
using DomainServices;

namespace ApplicationServices
{
    public class IntakeService : IIntakeService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IIntakeRepository _intakeRepository;

        public IntakeService(IIntakeRepository intakeRepository, IAppointmentRepository appointmentRepository)
        {
            _intakeRepository = intakeRepository;
            _appointmentRepository = appointmentRepository;
        }

        public async Task<IResult<Intake>> AddIntake(Intake intake)
        {
            var result = IsValid(intake);
            if (!result.Success) return result;
            try
            {
                await _intakeRepository.AddIntake(intake);
            }
            catch
            {
                result.Message = "Can't add intake, make sure the email is not already used.";
                result.Success = false;
                return result;
            }

            return result;
        }

        public async Task<IResult<Intake>> UpdateIntake(Intake intake, int id)
        {
            var result = IsValid(intake);
            if (!result.Success) return result;
            try
            {
                await _intakeRepository.UpdateIntake(intake, id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result.Success = false;
                result.Message = "cant update intake";
            }

            return result;
        }

        public async Task<IResult<Intake>> GetIntake(int intakeId)
        {
            IResult<Intake> result = new Result<Intake>();
            result.Payload = await _intakeRepository.GetIntake(intakeId);
            if (result.Payload == null)
            {
                result.Message = ErrorMessages.IdNotFound;
                result.Success = false;
            }

            return result;
        }

        public async Task<IResult<Intake>> DeleteIntake(int intakeId)
        {
            IResult<Intake> result = new Result<Intake>();
            try
            {
                await _intakeRepository.DeleteIntake(intakeId);
            }
            catch
            {
                result.Message = "cant delete intake with id: " + intakeId;
                result.Success = false;
            }

            return result;
        }

        private IResult<Intake> IsValid(Intake intake)
        {
            IResult<Intake> result = new Result<Intake>();
            if (string.IsNullOrWhiteSpace(intake.Email)) result.Message += ErrorMessages.EmailError;
            if (result.Message.Length > 1) result.Success = false;
            return result;
        }
    }
}