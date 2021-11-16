using System.Net.Http;
using System.Threading.Tasks;
using Core;
using GraphQL.Client.Abstractions;

namespace Dashboard.Services
{
    public class NoteApiService : BaseApiService
    {
        public NoteApiService(HttpClient httpClient, IGraphQLClient client) : base(httpClient, client)
        {
        }

        public async Task<IResult<Notes>> AddNote(Notes notes, int id)
        {
            return await SendHttpRequest("api/Notice/" + id, notes, HttpAction.POST);
        }
    }
}