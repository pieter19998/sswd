using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core;
using GraphQL;
using GraphQL.Client.Abstractions;

namespace Dashboard.Services
{
    public class AvailabilityGraphqlResponses
    {
        public List<Availability> availability { get; set; }
    }

    public class AvailabilityByEmployeeGraphqlResponses
    {
        public List<Availability> AvailabilityByEmployee { get; set; }
    }

    public class AvailabilityGraphqlResponse
    {
        public Availability Availability { get; set; }
    }

    public class AvailabilityApiService : BaseApiService
    {
        public AvailabilityApiService(HttpClient httpClient, IGraphQLClient client) : base(httpClient, client)
        {
        }

        public async Task<IEnumerable<Availability>> GetAvailabilities()
        {
            var query = new GraphQLRequest
            {
                Query =
                    "query{availability{ availabilityId availableFrom availableTo employeeId }}"
            };
            var response = await _client.SendQueryAsync<AvailabilityGraphqlResponses>(query);
            return response.Data.availability;
        }

        public async Task<ICollection<Availability>> GetAvailabilitiesByEmployee(int id)
        {
            var query = new GraphQLRequest
            {
                Query = "query{availabilityByEmployee(id : " + id +
                        " ){ availabilityId employeeId availableFrom availableTo}}"
            };
            var response = await _client.SendQueryAsync<AvailabilityByEmployeeGraphqlResponses>(query);
            return response.Data.AvailabilityByEmployee;
        }

        public async Task<Availability> GetAvailability(int id)
        {
            var query = new GraphQLRequest
            {
                Query = "query{availability(id : " + id + " ){ availabilityId employeeId availableFrom availableTo}}"
            };
            var response = await _client.SendQueryAsync<AvailabilityGraphqlResponse>(query);
            return response.Data.Availability;
        }

        public async Task<IResult<Availability>> AddAvailability(Availability availability)
        {
            return await SendHttpRequest("api/availability", availability, HttpAction.POST);
        }

        public async Task<IResult<Availability>> PutAvailability(Availability availability)
        {
            return await SendHttpRequest("api/availability", availability, HttpAction.PUT);
        }

        public async Task<IResult<Availability>> DeleteAvailability(Availability availability)
        {
            return await SendHttpRequest("api/availability/" + availability.AvailabilityId, availability,
                HttpAction.DELETE);
        }

        public class AvailabilityGraphqlResponse
        {
            public Availability Availability { get; set; }
        }
    }
}