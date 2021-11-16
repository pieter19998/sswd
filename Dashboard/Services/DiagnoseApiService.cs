using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core.stam;
using GraphQL;
using GraphQL.Client.Abstractions;

namespace Dashboard.Services
{
    public class DiagnoseGraphqlResponses
    {
        public List<Diagnose> Diagnoses { get; set; }
    }

    public class DiagnoseGraphqlResponse
    {
        public Diagnose Diagnose { get; set; }
    }

    public class DiagnoseApiService : BaseApiService
    {
        public DiagnoseApiService(HttpClient httpClient, IGraphQLClient client) : base(httpClient, client)
        {
        }

        public async Task<Diagnose> GetDiagnose(int code)
        {
            var query = new GraphQLRequest
            {
                Query = "query{ diagnoses(code : " + code + "){ code pathology bodyLocation}}"
            };
            var response = await _client.SendQueryAsync<DiagnoseGraphqlResponse>(query);
            return response.Data.Diagnose;
        }

        public async Task<ICollection<Diagnose>> GetDiagnoses()
        {
            var query = new GraphQLRequest
            {
                Query = "query{ diagnoses{ code pathology bodyLocation}}"
            };
            var response = await _client.SendQueryAsync<DiagnoseGraphqlResponses>(query);
            return response.Data.Diagnoses;
        }
    }
}