using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core;
using GraphQL;
using GraphQL.Client.Abstractions;

namespace Dashboard.Services
{
    public class SessionGraphqlResponse
    {
        public Session Session { get; set; }
    }

    public class SessionsGraphqlResponses
    {
        public List<Session> Sessions { get; set; }
    }

    public class SessionApiService : BaseApiService
    {
        public SessionApiService(HttpClient httpClient, IGraphQLClient client) : base(httpClient, client)
        {
        }

        public async Task<IEnumerable<Session>> GetSessions()
        {
            var query = new GraphQLRequest
            {
                Query =
                    "query{ sessions{ sessionId type roomType patientId sessionEmployeeId sessionDate active dossierId}}"
            };
            var response = await _client.SendQueryAsync<SessionsGraphqlResponses>(query);
            return response.Data.Sessions;
        }

        public async Task<Session> GetSession(int id)
        {
            var query = new GraphQLRequest
            {
                Query = "query{ session(id: " + id +
                        "){ sessionId type roomType patientId sessionEmployeeId sessionDate active dossierId notices{text author date}" +
                        " sessionEmployee{firstname lastname} patient{firstname lastname}}}"
            };
            var response = await _client.SendQueryAsync<SessionGraphqlResponse>(query);
            return response.Data.Session;
        }

        public async Task<IResult<Session>> AddSession(Session session)
        {
            return await SendHttpRequest("api/Session", session, HttpAction.POST);
        }

        public async Task<IResult<Session>> UpdateSession(Session session)
        {
            return await SendHttpRequest("api/Session/" + session.SessionId, session, HttpAction.PUT);
        }

        public async Task<IResult<Session>> DeleteSession(Session session)
        {
            return await SendHttpRequest("api/Session/" + session.SessionId, session, HttpAction.DELETE);
        }
    }
}