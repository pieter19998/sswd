using System.Collections.Generic;
using System.Threading.Tasks;
using Core.stam;
using DomainServices;
using GraphQL;
using GraphQL.Client.Abstractions;

namespace EF_Datastore
{
    public class ApiDiagnoseRepository : IDiagnoseRepository
    {
        private readonly IGraphQLClient _client;

        public ApiDiagnoseRepository(IGraphQLClient client)
        {
            _client = client;
        }

        public async Task<Diagnose> GetDiagnose(int code)
        {
            var query = new GraphQLRequest
            {
                Query = @"
               query{
                 diagnoses(code :  $Code){
                        code
                        pathology
                        bodyLocation
                    }
                }",
                Variables = new {Code = code}
            };
            var response = await _client.SendQueryAsync<Diagnose>(query);
            return response.Data;
        }

        public async Task<ICollection<Diagnose>> GetDiagnoses()
        {
            var query = new GraphQLRequest
            {
                Query = @"
               query{
                 diagnoses{
                        code
                        pathology
                        bodyLocation
                    }
                }"
            };
            var response = await _client.SendQueryAsync<ICollection<Diagnose>>(query);
            return response.Data;
        }
    }
}