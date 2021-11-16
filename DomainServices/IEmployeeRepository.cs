using System.Collections.Generic;
using System.Threading.Tasks;
using Core;

namespace DomainServices
{
    public interface IEmployeeRepository
    {
        Task<Employee> GetEmployee(int employeeId);
        Task<ICollection<Employee>> GetEmployees();
        Task RegisterEmployee(Employee employee);
        Task DeleteEmployee(int employeeId);
    }
}