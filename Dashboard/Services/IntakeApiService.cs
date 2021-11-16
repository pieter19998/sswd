using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core;
using GraphQL;
using GraphQL.Client.Abstractions;

namespace Dashboard.Services
{
    public class IntakeGraphqlResponse
    {
        public Intake Intake { get; set; }
    }

    public class IntakesGraphqlResponses
    {
        public List<Intake> Intakes { get; set; }
    }

    public class IntakeApiService : BaseApiService
    {
        public IntakeApiService(HttpClient httpClient, IGraphQLClient client) : base(httpClient, client)
        {
        }

        public async Task<IEnumerable<Intake>> GetIntakes()
        {
            var query = new GraphQLRequest
            {
                Query = "query{intakes{intakeId email intakeById intakeSuperVisor date}}"
            };
            var response = await _client.SendQueryAsync<IntakesGraphqlResponses>(query);
            return response.Data.Intakes;
        }

        public async Task<Intake> GetIntake(int id)
        {
            var query = new GraphQLRequest
            {
                Query = "query{intake(id: " + id + "){intakeId email intakeById intakeSuperVisor date}}"
            };
            var response = await _client.SendQueryAsync<IntakeGraphqlResponse>(query);
            return response.Data.Intake;
        }

        public async Task<IResult<Intake>> AddIntake(Intake intake)
        {
            return await SendHttpRequest("api/intake", intake, HttpAction.POST);
        }

        public async Task<IResult<Intake>> EditIntake(Intake intake)
        {
            return await SendHttpRequest("api/intake/" + intake.IntakeId, intake, HttpAction.PUT);
        }

        public async Task<IResult<Intake>> DeleteIntake(Intake intake)
        {
            return await SendHttpRequest("api/intake/" + intake.IntakeId, intake, HttpAction.DELETE);
        }
    }
}