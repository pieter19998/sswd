using System.Threading.Tasks;
using Core;

namespace ApplicationServices
{
    public interface ISessionService
    {
        Task<IResult<Session>> AddSession(Session session);
        Task<IResult<Session>> GetSession(int sessionId);
        Task<IResult<Session>> UpdateSession(Session session, int sessionId);
        Task<IResult<Session>> DeleteSession(int sessionId);
        Task<IResult<Session>> CancelSession(int sessionId);
    }
}