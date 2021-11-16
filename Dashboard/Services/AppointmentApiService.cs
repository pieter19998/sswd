using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core;
using Dashboard.Models;
using GraphQL;
using GraphQL.Client.Abstractions;

namespace Dashboard.Services
{
    public class AppointmentGraphqlResponse
    {
        public Appointment Appointment { get; set; }
    }

    public class AppointmentGraphqlResponses
    {
        public List<Appointment> Appointments { get; set; }
    }

    public class AppointmentsByPatientGraphqlResponses
    {
        public List<Appointment> AppointmentsByPatient { get; set; }
    }

    public class AppointmentListGraphqlResponses
    {
        public List<AppointmentList> AppointmentsByEmployee { get; set; }
    }

    public class AppointmentByEmployeeGraphqlResponses
    {
        public List<Appointment> AppointmentsByEmployee { get; set; }
    }

    public class AppointmentApiService : BaseApiService
    {
        public AppointmentApiService(HttpClient httpClient, IGraphQLClient client) : base(httpClient, client)
        {
        }

        public async Task<ICollection<Appointment>> GetAppointments()
        {
            var query = new GraphQLRequest
            {
                Query =
                    "query{appointments{ appointmentId startTime endTime appointmentType cancelled intake{intakeId intakeSuperVisor} session{ sessionId sessionDate} patientId employeeId}}"
            };
            var response = await _client.SendQueryAsync<AppointmentGraphqlResponses>(query);
            return response.Data.Appointments;
        }

        public async Task<ICollection<AppointmentList>> GetAppointmentListsByEmployee(int id)
        {
            var query = new GraphQLRequest
            {
                Query =
                    "query{appointmentsByEmployee(id: " + id + "){startTime endTime appointmentType cancelled}}"
            };
            var response = await _client.SendQueryAsync<AppointmentListGraphqlResponses>(query);
            return response.Data.AppointmentsByEmployee;
        }

        public async Task<ICollection<Appointment>> GetAppointmentsByEmployee(int id)
        {
            var query = new GraphQLRequest
            {
                Query =
                    "query{appointmentsByEmployee(id: " + id + " ) " +
                    "{startTime endTime cancelled intakeId sessionId appointmentType patient{ firstname lastname email} " +
                    "efEmployee{ firstname lastname } intake{ intakeId email } session{ sessionId}}}"
            };
            var response = await _client.SendQueryAsync<AppointmentByEmployeeGraphqlResponses>(query);
            return response.Data.AppointmentsByEmployee;
        }

        public async Task<ICollection<Appointment>> GetAppointmentsByPatient(int id)
        {
            var query = new GraphQLRequest
            {
                Query =
                    "query{appointmentsByPatient(id: " + id + ")" +
                    "{startTime endTime cancelled appointmentType intakeId sessionId patient{ firstname lastname email} " +
                    "efEmployee{ firstname lastname } intake{ intakeId email } session{ sessionId}}}"
            };
            var response = await _client.SendQueryAsync<AppointmentsByPatientGraphqlResponses>(query);
            return response.Data.AppointmentsByPatient;
        }

        public async Task<Appointment> GetAppointment(int id)
        {
            var query = new GraphQLRequest
            {
                Query =
                    "query{appointment(id : " + id +
                    " ){appointmentId startTime endTime appointmentType cancelled intakeId sessionId patientId employeeId}}"
            };
            var response = await _client.SendQueryAsync<AppointmentGraphqlResponse>(query);
            return response.Data.Appointment;
        }

        public async Task<IResult<Appointment>> ClaimAppointment(Appointment appointment)
        {
            return await SendHttpRequest("api/Appointment", appointment, HttpAction.POST);
        }
    }
}