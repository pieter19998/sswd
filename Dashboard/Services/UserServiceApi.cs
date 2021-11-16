using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Core;
using Dashboard.Models;
using GraphQL;
using GraphQL.Client.Abstractions;

namespace Dashboard.Services
{
    public class UserGraphqlResponse
    {
        public User User { get; set; }
    }

    public class UserGraphqlResponses
    {
        public List<User> Users { get; set; }
    }

    public class UserServiceApi
    {
        private readonly IGraphQLClient _client;
        private readonly HttpClient _httpClient;

        public UserServiceApi(HttpClient httpClient, IGraphQLClient client)
        {
            _httpClient = httpClient;
            _client = client;
        }

        public async Task<User> GetUser(int userId)
        {
            var query = new GraphQLRequest
            {
                Query = "query{ user(id : " + userId + "){email phoneNumber userId userName id}}"
            };
            var response = await _client.SendQueryAsync<UserGraphqlResponse>(query);
            return response.Data.User;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var query = new GraphQLRequest
            {
                Query = "query{ userByEmail(email: \"" + email + "\"){email phoneNumber userId userName id}}"
            };

            var response = await _client.SendQueryAsync<UserGraphqlResponse>(query).ConfigureAwait(false);
            return response.Data.User;
        }

        public async Task<ICollection<User>> GetUsers()
        {
            var query = new GraphQLRequest
            {
                Query = "query{ users{email phoneNumber userId userName id}}"
            };
            var response = await _client.SendQueryAsync<UserGraphqlResponses>(query);
            return response.Data.Users;
        }

        public async Task<HttpResponseMessage> RegisterUser(RegisterModel user)
        {
            // HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
            //     "api/User", user);
            // response.EnsureSuccessStatusCode();

            var response = await _httpClient.PostAsJsonAsync(
                "api/User", user);
            return response;
        }
    }
}