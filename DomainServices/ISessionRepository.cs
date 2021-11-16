using System.Collections.Generic;
using System.Threading.Tasks;
using Core;

namespace DomainServices
{
    public interface ISessionRepository
    {
        Task<Session> GetSession(int sessionId);
        Task<ICollection<Session>> GetSessions();
        Task AddSession(Session session);
        Task UpdateSession(Session session);
        Task DeleteSession(int sessionId);
        Task PatchNoteId(Notes note, int sessionId);
    }
}