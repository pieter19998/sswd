using System;
using System.Threading.Tasks;
using Core;
using DomainServices;
using Microsoft.AspNetCore.Identity;

namespace ApplicationServices
{
    public class UserService : IUserService
    {
        private readonly IIntakeRepository _intakeRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly UserManager<User> _userManager;
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository, IPatientRepository patientRepository,
            IIntakeRepository intakeRepository, UserManager<User> userManager)
        {
            _patientRepository = patientRepository;
            _intakeRepository = intakeRepository;
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task<IResult<User>> RegisterUser(User user)
        {
            IResult<User> result = new Result<User>();
            //check email
            var email = await _intakeRepository.GetIntake(user.Email);
            if (email == null || email.Email != user.Email)
            {
                result.Message = "Email not found. Make sure you have been to a intake before registring";
                result.Success = false;
                return result;
            }

            try
            {
                var patient = _patientRepository.GetPatientByEmail(user.Email).Result.PatientId;
                user.UserId = patient;
            }
            catch
            {
                result.Message = "Email not found. Make sure you have been to a intake before registring";
                result.Success = false;
                return result;
            }

            if (user.PasswordHash == null)
            {
                result.Message = "passwordHash is required (password will be hashed on the server)";
                result.Success = false;
                return result;
            }

            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, user.PasswordHash);
            try
            {
                await _userManager.CreateAsync(user);
            }
            catch (Exception e)
            {
                result.Message = "User with this email already exists";
                result.Success = false;
                return result;
            }

            var roleCheck = await _userManager.AddToRoleAsync(user, "PATIENT");
            if (!roleCheck.Succeeded)
            {
                result.Message = "something went wrong when adding role";
                result.Success = false;
            }

            return result;
        }

        public Task<IResult<User>> LoginUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}