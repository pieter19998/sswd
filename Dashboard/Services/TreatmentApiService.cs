using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core;
using GraphQL;
using GraphQL.Client.Abstractions;

namespace Dashboard.Services
{
    public class TreatmentPlanGraphqlResponse
    {
        public TreatmentPlan Treatment { get; set; }
    }

    public class TreatmentPlanGraphqlResponses
    {
        public List<TreatmentPlan> TreatmentPlans { get; set; }
    }

    public class TreatmentApiService : BaseApiService
    {
        private readonly IGraphQLClient _client;
        private readonly HttpClient _httpClient;

        public TreatmentApiService(HttpClient httpClient, IGraphQLClient client) : base(httpClient, client)
        {
        }

        public async Task<IEnumerable<TreatmentPlan>> GetTreatmentPlans()
        {
            var query = new GraphQLRequest
            {
                Query =
                    "query{ treatmentPlans{ treatmentPlanId sessionsPerWeek sessionDuration diagnoseCode diagnoseDescription noteId}}"
            };
            var response = await _client.SendQueryAsync<TreatmentPlanGraphqlResponses>(query);
            return response.Data.TreatmentPlans;
        }

        public async Task<TreatmentPlan> GetTreatmentPlan(int id)
        {
            var query = new GraphQLRequest
            {
                Query =
                    "query{ treatmentPlan(id: " + id +
                    "){ treatmentPlanId sessionsPerWeek sessionDuration diagnoseCode diagnoseDescription noteId}}"
            };
            var response = await _client.SendQueryAsync<TreatmentPlanGraphqlResponse>(query);
            return response.Data.Treatment;
        }

        public async Task<IResult<TreatmentPlan>> AddTreatmentPlan(TreatmentPlan treatment, int dossierId)
        {
            return await SendHttpRequest("api/TreatmentPlan/" + dossierId, treatment, HttpAction.POST);
        }
    }
}