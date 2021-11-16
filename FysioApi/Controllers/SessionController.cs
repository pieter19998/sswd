using System.Threading.Tasks;
using ApplicationServices;
using Core;
using DomainServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FysioApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionRepository sessionRepository, IAppointmentRepository appointmentRepository,
            IDossierRepository dossierRepository, IUserRepository userRepository,
            IStamApiService stamApiService)
        {
            _sessionService = new SessionService(sessionRepository, appointmentRepository, dossierRepository,
                userRepository, stamApiService);
        }

        [HttpGet("{sessionId}")]
        public async Task<ActionResult<Session>> GetSession(int sessionId)
        {
            var result = await _sessionService.GetSession(sessionId);
            return result.Success ? result.Payload : StatusCode(StatusCodes.Status404NotFound, result.Message);
        }

        [HttpPost]
        public async Task<ActionResult<Session>> PostSession([FromBody] Session session)
        {
            try
            {
                var result = await _sessionService.AddSession(session);
                session.Appointment = null;
                session.SessionEmployee = null;
                session.Patient = null;
                session.Notices = null;
                session.Dossier = null;
                if (result.Success) return StatusCode(StatusCodes.Status201Created, session);

                return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "There was an error adding session");
            }
        }

        [HttpPut("{sessionId}")]
        public async Task<ActionResult<Session>> EditSession(int sessionId, [FromBody] Session session)
        {
            try
            {
                var result = await _sessionService.UpdateSession(session, sessionId);
                session.Appointment = null;
                session.SessionEmployee = null;
                session.Patient = null;
                session.Notices = null;
                session.Dossier = null;
                if (result.Success) return StatusCode(StatusCodes.Status201Created, session);
                return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "There was an error adding session");
            }
        }

        [HttpDelete("{sessionId}")]
        public async Task<ActionResult<Session>> DeleteSession(int sessionId)
        {
            try
            {
                var result = await _sessionService.DeleteSession(sessionId);
                if (result.Success) return StatusCode(StatusCodes.Status200OK);

                return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "There was an error removing session");
            }
        }
    }
}