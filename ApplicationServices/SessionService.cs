using System;
using System.Threading.Tasks;
using Core;
using DomainServices;

namespace ApplicationServices
{
    public class SessionService : ISessionService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IDossierRepository _dossierRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IStamApiService _stamApiService;
        private readonly IUserRepository _userRepository;

        public SessionService(ISessionRepository sessionRepository, IAppointmentRepository appointmentRepository,
            IDossierRepository dossierRepository,
            IUserRepository userRepository, IStamApiService stamApiService)
        {
            _sessionRepository = sessionRepository;
            _appointmentRepository = appointmentRepository;
            _userRepository = userRepository;
            _dossierRepository = dossierRepository;
            _stamApiService = stamApiService;
        }

        public async Task<IResult<Session>> AddSession(Session session)
        {
            // var result = IsValid(session);
            var result = new Result<Session>();
            if (await CheckIfUserIsRegistered(session.PatientId))
            {
                result.Message = "user is not yet registered";
                result.Success = false;
                return result;
            }

            if (await CheckIfTreatmentIsOver(session.PatientId))
            {
                result.Message = "User Treatment is over";
                result.Success = false;
                return result;
            }

            try
            {
                await _sessionRepository.AddSession(session);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result.Message = "Can't add session";
                result.Success = false;
                return result;
            }

            try
            {
                var dossier = await _dossierRepository.GetDossierByPatientId(session.PatientId);
                await _dossierRepository.AddSession(session, dossier);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result.Message = e.Message;
                result.Success = false;
                await _sessionRepository.DeleteSession(session.SessionId);
            }

            return result;
        }

        public async Task<IResult<Session>> GetSession(int sessionId)
        {
            IResult<Session> result = new Result<Session>();
            result.Payload = await _sessionRepository.GetSession(sessionId);
            if (result.Payload == null)
            {
                result.Message = ErrorMessages.IdNotFound;
                result.Success = false;
            }

            return result;
        }

        public async Task<IResult<Session>> UpdateSession(Session session, int sessionId)
        {
            var result = IsValid(session);
            if (!result.Success) return result;
            if (GetSession(session.SessionId).Result.Success)
                try
                {
                    session.SessionId = sessionId;
                    await _sessionRepository.UpdateSession(session);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    result.Message = e.Message;
                    result.Success = false;
                }

            return result;
        }

        public async Task<IResult<Session>> DeleteSession(int sessionId)
        {
            IResult<Session> result = new Result<Session>();
            try
            {
                await _sessionRepository.DeleteSession(sessionId);
            }
            catch (Exception e)
            {
                result.Message = e.Message;
                result.Success = false;
            }

            return result;
        }

        public async Task<IResult<Session>> CancelSession(int sessionId)
        {
            var result = await GetSession(sessionId);
            if (result.Success)
                try
                {
                    result.Payload.Active = false;
                    await _sessionRepository.UpdateSession(result.Payload);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    result.Message = e.Message;
                    result.Success = false;
                }

            return result;
        }

        private async Task<bool> CheckIfUserIsRegistered(int id)
        {
            return await _userRepository.GetUser(id) == null;
        }

        private async Task<bool> CheckIfTreatmentIsOver(int patientId)
        {
            var dossier = await _dossierRepository.GetDossierByPatientId(patientId);


            return dossier.DismissalDay < DateTime.Now;
        }

        private IResult<Session> IsValid(Session session)
        {
            IResult<Session> result = new Result<Session>();
            if (string.IsNullOrWhiteSpace(session.Type)) result.Message += ErrorMessages.TextError;
            if (session.SessionDate.Day < DateTime.Today.Day) result.Message += ErrorMessages.DateError;
            if (session.DossierId == 0) result.Message += ErrorMessages.DossierError;
            if (NeedsNote(session).Result) result.Message += ErrorMessages.NoteError;
            if (result.Message.Length > 1) result.Success = false;
            return result;
        }

        private async Task<bool> NeedsNote(Session session)
        {
            var operation = await _stamApiService.GetOperation(session.Type);
            if (operation == null) return false;
            if (operation.Additional.Contains("nee") || operation.Additional.Contains("Nee")) return false;
            return session.Notices == null || session.Notices.Count < 1;
        }
    }
}