using System;
using System.Threading.Tasks;
using Core;
using DomainServices;
using Microsoft.AspNetCore.Identity;

namespace ApplicationServices
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, IUserRepository userRepository,
            UserManager<User> userManager)
        {
            _employeeRepository = employeeRepository;
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task<IResult<Employee>> AddEmployee(Employee employee, User user)
        {
            var result = IsValid(employee);
            var userResult = IsValid(user);
            if (!result.Success) return result;
            if (!userResult.Success) return userResult;
            try
            {
                await _employeeRepository.RegisterEmployee(employee);

                user.UserId = employee.EmployeeId;

                var passwordHasher = new PasswordHasher<User>();
                user.PasswordHash = passwordHasher.HashPassword(user, user.PasswordHash);
                await _userRepository.RegisterUser(user);

                IdentityResult roleCheck;
                if (employee.Role == Role.PHYSIO_THERAPIST)
                    roleCheck = await _userManager.AddToRoleAsync(user, "PHYSIO_THERAPIST");
                else
                    roleCheck = await _userManager.AddToRoleAsync(user, "STUDENT_EMPLOYEE");

                if (!roleCheck.Succeeded)
                {
                    result.Message = "something went wrong when adding role";
                    result.Success = false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result.Message = "Something went wrong when registering employee";
                result.Success = false;
            }

            return result;
        }

        public async Task<IResult<Employee>> GetEmployee(int employeeId)
        {
            IResult<Employee> result = new Result<Employee>();
            var employee = await _employeeRepository.GetEmployee(employeeId);

            if (result.Success)
            {
                result.Payload = employee;
            }
            else
            {
                result.Message = "Employee with id: " + employeeId + " does not exist";
                result.Success = false;
            }

            return result;
        }

        private IResult<Employee> IsValid(Employee employee)
        {
            IResult<Employee> result = new Result<Employee>();
            if (string.IsNullOrWhiteSpace(employee.Firstname)) result.Message += ErrorMessages.FirstNameError;
            if (string.IsNullOrWhiteSpace(employee.Lastname)) result.Message += ErrorMessages.LastNameError;
            if (string.IsNullOrWhiteSpace(employee.Email)) result.Message += ErrorMessages.EmailError;
            if (result.Message.Length > 1) result.Success = false;
            return result;
        }

        private IResult<Employee> IsValid(User user)
        {
            IResult<Employee> result = new Result<Employee>();
            if (string.IsNullOrWhiteSpace(user.Email)) result.Message += ErrorMessages.EmailError;
            if (string.IsNullOrWhiteSpace(user.PasswordHash)) result.Message += "Password is required";
            if (result.Message.Length > 1) result.Success = false;
            return result;
        }
    }
}