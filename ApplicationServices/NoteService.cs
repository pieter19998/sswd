using System;
using System.Threading.Tasks;
using Core;
using DomainServices;

namespace ApplicationServices
{
    public class NoteService : INoteService
    {
        private readonly IDossierRepository _dossierRepository;
        private readonly INoteRepository _noteRepository;
        private readonly ISessionRepository _sessionRepository;

        public NoteService(INoteRepository noteRepository, ISessionRepository sessionRepository,
            IDossierRepository dossierRepository)
        {
            _noteRepository = noteRepository;
            _sessionRepository = sessionRepository;
            _dossierRepository = dossierRepository;
        }

        public async Task<IResult<Notes>> AddNote(Notes note, int id)
        {
            var result = IsValid(note);
            if (!result.Success) return result;
            try
            {
                if (note.NoteType == NoteType.DOSSIER)
                {
                    var dossier = await _dossierRepository.GetDossier(id);
                    if (dossier == null)
                    {
                        result.Message = ErrorMessages.IdNotFound;
                        result.Success = false;
                        return result;
                    }

                    await _noteRepository.AddNotice(note);
                    await _dossierRepository.PatchNoteId(note, id);
                }
                else
                {
                    var session = await _sessionRepository.GetSession(id);
                    if (session == null)
                    {
                        result.Message = ErrorMessages.IdNotFound;
                        result.Success = false;
                        return result;
                    }

                    await _noteRepository.AddNotice(note);
                    await _sessionRepository.PatchNoteId(note, id);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result.Message = e.Message;
                result.Success = false;
            }

            return result;
        }

        public async Task<IResult<Notes>> AddNoteSession(Notes note, int sessionId)
        {
            var result = IsValid(note);
            if (!result.Success) return result;


            try
            {
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result.Message = e.Message;
                result.Success = false;
            }

            return result;
        }

        private IResult<Notes> IsValid(Notes note)
        {
            IResult<Notes> result = new Result<Notes>();
            if (string.IsNullOrWhiteSpace(note.Author)) result.Message += ErrorMessages.TextError;
            if (string.IsNullOrWhiteSpace(note.Text)) result.Message += ErrorMessages.AuthorError;
            if (result.Message.Length > 1) result.Success = false;
            return result;
        }
    }
}