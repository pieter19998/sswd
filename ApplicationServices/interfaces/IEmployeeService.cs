using System.Threading.Tasks;
using Core;

namespace ApplicationServices
{
    public interface IEmployeeService
    {
        Task<IResult<Employee>> AddEmployee(Employee employee, User user);
        Task<IResult<Employee>> GetEmployee(int employeeId);
    }
}