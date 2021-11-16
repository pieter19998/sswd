using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core.stam;
using DomainServices;
using Newtonsoft.Json;

namespace EF_Datastore
{
    public class ApiOperationRepository : IOperationRepository
    {
        public ApiOperationRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public HttpClient _httpClient { get; set; }

        public async Task<ICollection<Operation>> GetOperations()
        {
            var response = await _httpClient.GetAsync("/api/operations");
            var responseBody = await response.Content.ReadAsStringAsync();
            try
            {
                return JsonConvert.DeserializeObject<ICollection<Operation>>(responseBody);
            }
            catch
            {
                return new List<Operation>();
            }
        }

        public async Task<Operation> GetOperation(string id)
        {
            var response = await _httpClient.GetAsync("/api/operations/" + id);
            var responseBody = await response.Content.ReadAsStringAsync();
            try
            {
                return JsonConvert.DeserializeObject<Operation>(responseBody);
            }
            catch
            {
                return null;
            }
        }
    }
}