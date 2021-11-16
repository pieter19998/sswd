using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core;
using GraphQL;
using GraphQL.Client.Abstractions;

namespace Dashboard.Services
{
    public class DossierGraphqlResponse
    {
        public Dossier Dossier { get; set; }
    }

    public class DossierGraphqlResponses
    {
        public List<Dossier> Dossiers { get; set; }
    }

    public class DossierByPatientGraphqlResponses
    {
        public Dossier DossierByPatient { get; set; }
    }


    public class DossierApiService : BaseApiService
    {
        public DossierApiService(HttpClient httpClient, IGraphQLClient client) : base(httpClient, client)
        {
        }

        public async Task<List<Dossier>> GetDossiers()
        {
            var query = new GraphQLRequest
            {
                Query =
                    "query{ dossiers{ dossierId age description diagnoseCode diagnoseDescription headPractitionerId" +
                    " applicationDay dismissalDay active treatmentPlanId patientId patient{firstname lastname address dayOfBirth" +
                    " personalNumber dayOfBirth email studentNumber patientNumber} headPractitioner{firstname lastname employeeId}" +
                    " sessions{ sessionId sessionDate type sessionEmployee{firstname lastname}}}}"
            };
            var response = await _client.SendQueryAsync<DossierGraphqlResponses>(query);
            return response.Data.Dossiers;
        }

        public async Task<Dossier> GetDossierByPatient(int id)
        {
            var query = new GraphQLRequest
            {
                Query = "query{dossierByPatient(patientId: " + id + ")" +
                        "{dossierId age description diagnoseCode diagnoseDescription headPractitionerId applicationDay dismissalDay active" +
                        " treatmentPlanId patientId treatmentPlan{sessionsPerWeek sessionDuration} patient{firstname lastname address dayOfBirth personalNumber dayOfBirth " +
                        "email photo studentNumber patientNumber} notices{text author visible date} headPractitioner{firstname lastname employeeId}" +
                        " sessions{sessionId roomType sessionDate sessionEmployee{lastname}}}}"
            };
            var response = await _client.SendQueryAsync<DossierByPatientGraphqlResponses>(query);
            return response.Data.DossierByPatient;
        }

        public async Task<Dossier> GetDossier(int id)
        {
            var query = new GraphQLRequest
            {
                Query = "query{dossier(id: " + id + ")" +
                        "{dossierId age description diagnoseCode diagnoseDescription headPractitionerId applicationDay dismissalDay active" +
                        " treatmentPlanId patientId treatmentPlan{sessionsPerWeek sessionDuration} patient{firstname lastname address dayOfBirth personalNumber dayOfBirth " +
                        "email photo studentNumber patientNumber} notices{text author visible date} headPractitioner{firstname lastname employeeId}" +
                        " sessions{sessionId roomType sessionDate sessionEmployee{lastname}}}}"
            };
            var response = await _client.SendQueryAsync<DossierGraphqlResponse>(query);
            return response.Data.Dossier;
        }

        public async Task<Dossier> GetDossierWithAllData(int id)
        {
            var query = new GraphQLRequest
            {
                Query = "query{dossier(id: " + id + ")" +
                        "{dossierId age description diagnoseCode diagnoseDescription headPractitionerId applicationDay dismissalDay active" +
                        " treatmentPlanId patientId treatmentPlan{sessionsPerWeek sessionDuration} patient{firstname lastname address dayOfBirth personalNumber dayOfBirth " +
                        "email photo studentNumber patientNumber} notices{text author visible date} headPractitioner{firstname lastname employeeId} sessions{sessionId roomType sessionDate sessionEmployee{lastname}}}}"
            };
            var response = await _client.SendQueryAsync<DossierGraphqlResponse>(query);
            return response.Data.Dossier;
        }

        public async Task<IResult<Dossier>> AddDossier(Dossier dossier)
        {
            return await SendHttpRequest("api/Dossier", dossier, HttpAction.POST);
        }
    }
}