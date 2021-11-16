using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core.stam;
using GraphQL;
using GraphQL.Client.Abstractions;

namespace Dashboard.Services
{
    public class OperationGraphqlResponse
    {
        public Operation Operation { get; set; }
    }

    public class OperationsGraphqlResponse
    {
        public List<Operation> Operations { get; set; }
    }

    public class OperationApiService : BaseApiService
    {
        public OperationApiService(HttpClient httpClient, IGraphQLClient client) : base(httpClient, client)
        {
        }

        public async Task<List<Operation>> GetOperations()
        {
            var query = new GraphQLRequest
            {
                Query = "query{operations{value description additional}}"
            };
            var response = await _client.SendQueryAsync<OperationsGraphqlResponse>(query);
            return response.Data.Operations;
        }

        public async Task<Operation> GetOperation(string value)
        {
            var query = new GraphQLRequest
            {
                Query = "query{operation(value : \"" + value + "\"){value description additional }}"
            };
            var response = await _client.SendQueryAsync<OperationGraphqlResponse>(query);
            return response.Data.Operation;
        }
    }
}