using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core;
using Dashboard.Models;
using GraphQL;
using GraphQL.Client.Abstractions;

namespace Dashboard.Services
{
    public class PatientGraphqlResponse
    {
        public Patient Patient { get; set; }
    }

    public class PatientGraphqlResponses
    {
        public List<Patient> Patients { get; set; }
    }

    public class PatientByEmailGraphqlResponse
    {
        public Patient patientByEmail { get; set; }
    }

    public class PatientApiService : BaseApiService
    {
        public PatientApiService(HttpClient httpClient, IGraphQLClient client) : base(httpClient, client)
        {
        }

        public async Task<List<Patient>> GetPatients()
        {
            var query = new GraphQLRequest
            {
                Query =
                    "query{ patients{ patientId firstname lastname email active address sex patientNumber photo dayOfBirth personalNumber studentNumber role type}}"
            };
            var response = await _client.SendQueryAsync<PatientGraphqlResponses>(query);
            return response.Data.Patients;
        }

        public async Task<Patient> GetPatient(int id)
        {
            var query = new GraphQLRequest
            {
                Query =
                    "query{ patient(id: " + id +
                    "){ patientId firstname lastname email active address sex patientNumber photo dayOfBirth personalNumber studentNumber role type}}"
            };
            var response = await _client.SendQueryAsync<PatientGraphqlResponse>(query);
            return response.Data.Patient;
        }

        public async Task<Patient> GetPatientByEmail(string email)
        {
            var query = new GraphQLRequest
            {
                Query =
                    "query{ patientByEmail(email:\"" + email +
                    "\"){ patientId firstname lastname email active address sex patientNumber photo dayOfBirth personalNumber studentNumber role type}}"
            };
            var response = await _client.SendQueryAsync<PatientByEmailGraphqlResponse>(query);
            return response.Data.patientByEmail;
        }

        public async Task<IResult<Patient>> AddPatient(Patient patient)
        {
            return await SendHttpRequest("api/Patient", patient, HttpAction.POST);
        }

        public async Task<IResult<UpdateAddress>> UpdateAddress(UpdateAddress address)
        {
            return await SendHttpRequest("api/Patient/" + address.PatientId, address, HttpAction.PATCH);
        }

        public async Task<IResult<Patient>> UpdatePatient(Patient patient)
        {
            return await SendHttpRequest("api/Patient/" + patient.PatientId, patient, HttpAction.PUT);
        }
    }
}