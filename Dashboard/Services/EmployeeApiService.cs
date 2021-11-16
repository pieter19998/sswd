using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Core;
using Dashboard.Models;
using GraphQL;
using GraphQL.Client.Abstractions;

namespace Dashboard.Services
{
    public class EmployeeGraphqlResponse
    {
        // public EmployeeGraphqlResponse(Employee employee) => Employee = employee;
        public Employee Employee { get; set; }
    }

    public class EmployeeGraphqlResponses
    {
        public List<Employee> Employees { get; set; }
    }


    public class EmployeeApiService : BaseApiService
    {
        public EmployeeApiService(HttpClient httpClient, IGraphQLClient client) : base(httpClient, client)
        {
        }

        public async Task<List<Employee>> GetEmployees()
        {
            var query = new GraphQLRequest
            {
                Query = "query{ employees{bigNumber email employeeId firstname lastname role studentNumber}}"
            };
            var response = await _client.SendQueryAsync<EmployeeGraphqlResponses>(query).ConfigureAwait(false);

            return response.Data.Employees;
        }

        public async Task<Employee> GetEmployee(int id)
        {
            var query = new GraphQLRequest
            {
                // Query = "query{employee(id: " + id +
                //         "){employeeId firstname lastname email role studentNumber}}"
                Query = "query{ employee(id: " + id +
                        "){ active appointment{appointmentId} bigNumber email employeeId firstname headPractitioner{headPractitionerId} intake{intakeId} lastname role sessions{ sessionId } studentNumber}}"
            };
            var response = await _client.SendQueryAsync<EmployeeGraphqlResponse>(query).ConfigureAwait(false);
            return response.Data.Employee;
        }

        public async Task<IResult<EmployeeModel>> AddEmployee(EmployeeModel employee)
        {
            return await SendHttpRequest("api/Employee", employee, HttpAction.POST);
        }
    }
}