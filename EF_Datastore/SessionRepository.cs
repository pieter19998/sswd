using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using DomainServices;
using Microsoft.EntityFrameworkCore;

namespace EF_Datastore
{
    public class SessionRepository : ISessionRepository
    {
        private readonly PracticeDbContext _context;

        public SessionRepository(PracticeDbContext context)
        {
            _context = context;
        }

        public async Task<Session> GetSession(int sessionId)
        {
            return await _context.Sessions.Include(x => x.Patient)
                .Include(x => x.Notices)
                .Include(x => x.SessionEmployee)
                .Include(x => x.Dossier)
                .SingleOrDefaultAsync(s => s.SessionId == sessionId && s.Active);
        }

        public async Task<ICollection<Session>> GetSessions()
        {
            return await _context.Sessions.Where(s => s.Active).ToListAsync();
        }

        public async Task AddSession(Session session)
        {
            await _context.Sessions.AddAsync(session);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateSession(Session session)
        {
            var sessionExists = await GetSession(session.SessionId);
            sessionExists.Type = session.Type;
            sessionExists.RoomType = session.RoomType;
            sessionExists.PatientId = session.PatientId;
            sessionExists.SessionEmployeeId = session.SessionEmployeeId;
            sessionExists.SessionDate = session.SessionDate;
            sessionExists.DossierId = session.DossierId;
            sessionExists.Notices = session.Notices;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSession(int sessionId)
        {
            var session = await GetSession(sessionId);
            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();
        }

        public async Task PatchNoteId(Notes note, int sessionId)
        {
            var session = await GetSession(sessionId);
            session.Notices.Add(note);
            await _context.SaveChangesAsync();
        }
    }
}