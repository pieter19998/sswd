using System.Threading.Tasks;
using ApplicationServices;
using Core;
using DomainServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FysioApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserRepository userRepository, IPatientRepository patientRepository,
            IIntakeRepository intakeRepository, UserManager<User> userManager)
        {
            _userService = new UserService(userRepository, patientRepository, intakeRepository, userManager);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<User>> RegisterUser(User user)
        {
            var result = await _userService.RegisterUser(user);
            if (result.Success) return StatusCode(StatusCodes.Status201Created, user);
            return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<User>> LoginUser(User user)
        {
            var result = await _userService.LoginUser(user);
            if (result.Success) return StatusCode(StatusCodes.Status201Created, user);
            return StatusCode(StatusCodes.Status500InternalServerError, result.Message);
        }
    }
}