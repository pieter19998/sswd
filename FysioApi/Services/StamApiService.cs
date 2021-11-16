using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core.stam;
using DomainServices;
using GraphQL;
using GraphQL.Client.Abstractions;

namespace FysioApi.Services
{
    public class DiagnoseGraphqlResponses
    {
        public List<Diagnose> Diagnoses { get; set; }
    }

    public class DiagnoseGraphqlResponse
    {
        public Diagnose Diagnose { get; set; }
    }

    public class OperationGraphqlResponse
    {
        public Operation Operation { get; set; }
    }

    public class OperationsGraphqlResponses
    {
        public List<Operation> Operations { get; set; }
    }


    public class StamApiService : IStamApiService
    {
        public StamApiService(HttpClient httpClient, IGraphQLClient client)
        {
            _httpClient = httpClient;
            _client = client;
        }

        public IGraphQLClient _client { get; set; }
        public HttpClient _httpClient { get; set; }

        public async Task<ICollection<Operation>> GetOperations()
        {
            var query = new GraphQLRequest
            {
                Query = "query{operations{value description additional}}"
            };
            var response = await _client.SendQueryAsync<OperationsGraphqlResponses>(query);
            return response.Data.Operations;
        }

        public async Task<Operation> GetOperation(string value)
        {
            var query = new GraphQLRequest
            {
                Query = "query{ operation(value : \"" + value + "\"){value description additional }}"
            };
            var response = await _client.SendQueryAsync<OperationGraphqlResponse>(query);
            return response.Data.Operation;
        }

        public async Task<Diagnose> GetDiagnose(int code)
        {
            var query = new GraphQLRequest
            {
                Query = "query{ diagnose(code : " + code + "){ code pathology bodyLocation}}"
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