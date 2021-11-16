using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Dashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dashboard
{
    public class AppointmentController : Controller
    {
        private readonly AppointmentApiService _appointmentServiceApi;
        private readonly AvailabilityApiService _availabilityApiService;
        private readonly DossierApiService _dossierApiService;
        private readonly EmployeeApiService _employeeServiceApi;
        private readonly SessionApiService _sessionApiService;
        private readonly UserManager<User> _userManager;

        public AppointmentController(AppointmentApiService appointmentServiceApi, EmployeeApiService employeeServiceApi,
            SessionApiService sessionApiService, AvailabilityApiService availabilityApiService,
            DossierApiService dossierApiService, UserManager<User> userManager)
        {
            _appointmentServiceApi = appointmentServiceApi;
            _employeeServiceApi = employeeServiceApi;
            _employeeServiceApi = employeeServiceApi;
            _sessionApiService = sessionApiService;
            _availabilityApiService = availabilityApiService;
            _dossierApiService = dossierApiService;
            _userManager = userManager;
        }


        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            // string roleName = _userManager.GetRolesAsync(user).Result.FirstOrDefault();
            if (User.IsInRole("PATIENT"))
            {
                var data = _appointmentServiceApi.GetAppointmentsByPatient(user.UserId).Result
                    ?.OrderBy(x => x.StartTime);
                return View(data);
            }

            return View(_appointmentServiceApi.GetAppointmentsByEmployee(user.UserId).Result
                ?.OrderBy(x => x.StartTime));
        }

        // GET: CreateAppointment/Create
        public IActionResult Create()
        {
            var employeeList = _employeeServiceApi.GetEmployees().Result;
            employeeList.ForEach(x => x.Firstname = x.Firstname + " " + x.Lastname);
            ViewData["EmployeeId"] = new SelectList(employeeList, "EmployeeId", "Firstname");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("AppointmentId,StartTime,EndTime,AppointmentType,Cancelled,IntakeId,SessionId,PatientId,EmployeeId")]
            Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                var result = await _appointmentServiceApi.ClaimAppointment(appointment);
                if (result.Success)
                {
                    if (appointment.AppointmentType == AppointmentType.INTAKE)
                        return RedirectToAction("Create", "Intake", new {id = result.Payload.AppointmentId});

                    var user = await _userManager.GetUserAsync(User);
                    if (user == null)
                    {
                        ModelState.AddModelError("", "Only a logged in patient can make a session appointment.");
                        return View();
                    }

                    var dossier = await _dossierApiService.GetDossierByPatient(user.UserId);
                    if (dossier == null)
                    {
                        ModelState.AddModelError("", "Dossier not found, Contact administration");
                        return View();
                    }

                    var session = new Session
                    {
                        SessionDate = appointment.StartTime,
                        PatientId = user.UserId,
                        DossierId = dossier.DossierId
                    };
                    var result2 = await _sessionApiService.AddSession(session);
                    if (result2.Success) return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", result.Message);
            }

            ModelState.AddModelError("", "invalid appointment data");

            var employeeList = _employeeServiceApi.GetEmployees().Result;
            employeeList.ForEach(x => x.Firstname = x.Firstname + " " + x.Lastname);
            ViewData["EmployeeId"] =
                new SelectList(employeeList, "EmployeeId", "Firstname");

            return View(appointment);
        }

        [HttpPost]
        public JsonResult GetDetails(int id)
        {
            var data = _availabilityApiService.GetAvailabilitiesByEmployee(id).Result
                .Where(x => x.AvailableFrom.ToLocalTime() > DateTime.Now.ToLocalTime());
            return Json(data);
        }

        [HttpPost]
        public JsonResult GetAvailability(DateTime date, int id)
        {
            var data = _availabilityApiService.GetAvailabilitiesByEmployee(id).Result
                .Where(x => x.AvailableFrom.Date == date.Date);
            return Json(data);
        }

        [HttpPost]
        public JsonResult GetDays(int employeeId)
        {
            var availabilities = new List<Availability>();
            var data = _availabilityApiService.GetAvailabilitiesByEmployee(employeeId).Result;
            foreach (var availability in data)
            {
                availabilities.Add(availability);
            }
            
            return Json(availabilities.GroupBy(x => x.AvailableFrom.Date).Select(y => y.First()));
        }


        [HttpPost]
        public JsonResult GetTimes(int employeeId, DateTime date)
        {
            var availableTimes = new List<DateTime>();
            //get availability by day
            var day = _availabilityApiService.GetAvailabilitiesByEmployee(employeeId).Result
                .Where(x => x.AvailableFrom.ToLocalTime().Day == date.ToLocalTime().Day);

            //get all appointments of employee on that day
            var appointments = _appointmentServiceApi.GetAppointmentListsByEmployee(employeeId).Result
                ?.Where(x => x.StartTime.ToLocalTime().Day == date.ToLocalTime().Day);

            foreach (var partOfDay in day)
            {
                var previousAppointment = partOfDay.AvailableFrom;

                if (appointments == null)
                    while (previousAppointment.AddHours(1) <= partOfDay.AvailableTo)
                    {
                        availableTimes.Add(previousAppointment);
                        previousAppointment = previousAppointment.AddHours(1);
                    }
                else
                    foreach (var appointment in appointments)
                        if (appointment.StartTime - previousAppointment > TimeSpan.FromHours(1) &&
                            previousAppointment.AddHours(1) < partOfDay.AvailableTo)
                        {
                            availableTimes.Add(previousAppointment);
                            previousAppointment = previousAppointment.AddHours(1);
                        }
            }

            return Json(availableTimes);
        }

        public IActionResult Delete(int id)
        {
            return NotFound();
        }

        [HttpPost]
        public JsonResult GetClaimedTimes(int employeeId, DateTime date)
        {
            var data = _appointmentServiceApi.GetAppointmentListsByEmployee(employeeId).Result
                .Where(x => x.StartTime.Date == date.Date);
            return Json(data);
        }
    }
}