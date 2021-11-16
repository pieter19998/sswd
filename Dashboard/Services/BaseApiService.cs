using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Core;
using GraphQL;
using GraphQL.Client.Abstractions;
using Newtonsoft.Json;

namespace Dashboard.Services
{
    public enum HttpAction
    {
        GET,
        POST,
        PATCH,
        PUT,
        DELETE
    }

    public abstract class BaseApiService
    {
        protected BaseApiService(HttpClient httpClient, IGraphQLClient client)
        {
            _httpClient = httpClient;
            _client = client;
        }

        protected IGraphQLClient _client { get; set; }
        protected HttpClient _httpClient { get; set; }

        //send graphQl queries
        public async Task<IResult<T>> SendGraphQlQuery<T>(string query)
        {
            IResult<T> result = new Result<T>();
            var request = new GraphQLRequest
            {
                Query = query
            };
            try
            {
                var response = await _client.SendQueryAsync<T>(request);
                result.Payload = response.Data;
            }
            catch (Exception e)
            {
                result.Message = "something went wrong when requesting data, please contact the administrator";
                result.Success = false;
            }

            return result;
        }

        //send http request to api
        public async Task<IResult<T>> SendHttpRequest<T>(string url, T body, HttpAction httpAction)
        {
            IResult<T> result = new Result<T>();
            var data = JsonConvert.SerializeObject(body);
            var stringContent = new StringContent(data, Encoding.UTF8, "application/json");

            HttpResponseMessage response = null;
            switch (httpAction)
            {
                case HttpAction.GET:
                    response = await _httpClient.GetAsync(url);
                    break;
                case HttpAction.PATCH:
                    response = await _httpClient.PatchAsync(url, stringContent);
                    break;
                case HttpAction.POST:
                    response = await _httpClient.PostAsync(url, stringContent);
                    break;
                case HttpAction.PUT:
                    response = await _httpClient.PutAsync(url, stringContent);
                    break;
                case HttpAction.DELETE:
                    response = await _httpClient.DeleteAsync(url);
                    break;
                default: throw new NotSupportedException("Enum type not supported");
            }

            result.Success = response.IsSuccessStatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();

            try
            {
                result.Payload = JsonConvert.DeserializeObject<T>(responseBody);
            }
            catch
            {
                result.Success = false;
                result.Message = response.Content.ReadAsStringAsync().Result;
            }

            return result;
        }
    }
}