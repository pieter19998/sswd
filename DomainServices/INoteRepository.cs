using System.Collections.Generic;
using System.Threading.Tasks;
using Core;

namespace DomainServices
{
    public interface INoteRepository
    {
        Task<ICollection<Notes>> GetNotices();
        Task<Notes> GetNotice(int noticeId);
        Task<Notes> GetNoticeBySessionId(int sessionId);
        Task<Notes> GetNoticeByDossierId(int dossierId);
        Task AddNotice(Notes notice);
    }
}