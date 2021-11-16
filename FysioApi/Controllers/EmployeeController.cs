using System.Threading.Tasks;
using ApplicationServices;
using Core;
using DomainServices;
using FysioApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FysioApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeRepository employeeRepository, IUserRepository userRepository,
            UserManager<User> userManager)
        {
            _employeeService = new EmployeeService(employeeRepository, userRepository, userManager);
        }

        [HttpGet("{employeeId}")]
        public async Task<ActionResult<Employee>> GetEmployee(int employeeId)
        {
            var result = await _employeeService.GetEmployee(employeeId);
            return result.Success ? result.Payload : StatusCode(StatusCodes.Status404NotFound, result.Message);
        }

        [HttpPost]
        public async Task<ActionResult<Intake>> PostEmployee([FromBody] EmployeeRegister employeeRegister)
        {
            try
            {
                var employee = new Employee
                {
                    Firstname = employeeRegister.Firstname,
                    Lastname = employeeRegister.Lastname,
                    Email = employeeRegister.Email,
                    Role = employeeRegister.Role,
                    BigNumber = employeeRegister.BigNumber,
                    StudentNumber = employeeRegister.StudentNumber
                };
                var user = new User
                {
                    Email = employee.Email,
                    UserName = employee.Email,
                    PasswordHash = employeeRegister.Password
                };
                var result = await _employeeService.AddEmployee(employee, user);
                if (result.Success) return StatusCode(StatusCodes.Status201Created, employee);

                return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "There was an error registering employee");
            }
        }
    }
}