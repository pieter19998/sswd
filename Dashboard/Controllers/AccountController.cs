using System.Threading.Tasks;
using Core;
using Dashboard.Models;
using Dashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserServiceApi _userManager;

        public AccountController(UserServiceApi userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginModel {ReturnUrl = returnUrl});
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                        return Redirect(model.ReturnUrl);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login ");
            }

            return View(model);
        }

        public async Task<IActionResult> Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Email, UserNam, PasswordHash,PhoneNumber")]
            RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                registerModel.UserName = registerModel.Email;
                var result = await _userManager.RegisterUser(registerModel);
                if (result.IsSuccessStatusCode) return Redirect(registerModel?.ReturnUrl ?? "/Home");

                var receiveStream = result.Content.ReadAsStringAsync().Result;

                ModelState.AddModelError("", receiveStream);
            }

            ModelState.AddModelError("", "invalid register credentials");
            return View();
        }


        public async Task<IActionResult> AccesDenied(string returnUrl)
        {
            return View();
        }

        public async Task<RedirectResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }
    }
}