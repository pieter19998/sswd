using System.Threading.Tasks;
using Core;

namespace ApplicationServices
{
    public interface INoteService
    {
        Task<IResult<Notes>> AddNote(Notes note, int id);
    }
}