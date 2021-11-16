using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using DomainServices;
using Microsoft.EntityFrameworkCore;

namespace EF_Datastore
{
    public class NoteRepository : INoteRepository
    {
        private readonly PracticeDbContext _context;

        public NoteRepository(PracticeDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Notes>> GetNotices()
        {
            return await _context.Notes.ToListAsync();
        }

        public async Task<Notes> GetNotice(int noticeId)
        {
            return await _context.Notes.FindAsync(noticeId);
        }

        public async Task<Notes> GetNoticeBySessionId(int sessionId)
        {
            throw new NotImplementedException();
        }

        public Task<Notes> GetNoticeByDossierId(int dossierId)
        {
            throw new NotImplementedException();
        }

        public async Task AddNotice(Notes notice)
        {
            await _context.SaveChangesAsync();
            await _context.Notes.AddAsync(notice);
        }
    }
}