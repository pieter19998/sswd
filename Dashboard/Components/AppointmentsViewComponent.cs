using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Dashboard.Models;
using Dashboard.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Component
{
    public class AppointmentsViewComponent : ViewComponent
    {
        private readonly AppointmentApiService _appointmentServiceApi;
        private readonly UserManager<User> _userManager;

        public AppointmentsViewComponent(AppointmentApiService apiService, UserManager<User> userManager)
        {
            _appointmentServiceApi = apiService;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(Request.HttpContext.User);
            var appointments = await _appointmentServiceApi.GetAppointmentListsByEmployee(user.UserId);

            if (appointments == null) return View(new List<AppointmentList>());
            return View(appointments);
        }
    }
}