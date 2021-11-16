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
    public class NoticeController : ControllerBase
    {
        private readonly INoteService _noteService;

        public NoticeController(INoteRepository noteRepository, IDossierRepository dossierRepository,
            ISessionRepository sessionRepository)
        {
            _noteService = new NoteService(noteRepository, sessionRepository, dossierRepository);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<Notes>> PostNoticeSession(Notes notice, int id)
        {
            try
            {
                await _noteService.AddNote(notice, id);
                return StatusCode(StatusCodes.Status201Created);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "There was an error adding note");
            }
        }
    }
}