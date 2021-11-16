using System.Threading.Tasks;
using Core;
using Dashboard.Models;
using Dashboard.Services;
using EF_Datastore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dashboard
{
    [Authorize]
    public class AvailabilityController : Controller
    {
        private readonly AvailabilityApiService _availabilityApiService;
        private readonly EmployeeApiService _employeeApiService;
        private readonly UserManager<User> _userManager;

        public AvailabilityController(UserManager<User> userManager,
            AvailabilityApiService availabilityApiService, EmployeeApiService employeeApiService)
        {
            _userManager = userManager;
            _availabilityApiService = availabilityApiService;
            _employeeApiService = employeeApiService;
        }

        // GET: Availability
        [Authorize(Roles = "PHYSIO_THERAPIST,STUDENT_EMPLOYEE")]
        public async Task<IActionResult> Index()
        {
            return View(await _availabilityApiService.GetAvailabilitiesByEmployee(
                _userManager.GetUserAsync(User).Result.UserId));
        }


        // GET: Availability/Create
        public IActionResult Create()
        {
            var employeeList = _employeeApiService.GetEmployees().Result;
            employeeList.ForEach(x => x.Firstname = x.Firstname + " " + x.Lastname);
            ViewData["EmployeeId"] =
                new SelectList(employeeList, "EmployeeId", "Firstname",
                    _userManager.GetUserAsync(User).Result.UserId);
            return View();
        }

        // POST: Availability/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Day,AvailableFrom,AvailableTo,EmployeeId")]
            AvailabilityModel availabilityModel)
        {
            if (ModelState.IsValid)
            {
                var availability = new Availability
                {
                    AvailableFrom = availabilityModel.Day.Add(availabilityModel.AvailableFrom),
                    AvailableTo = availabilityModel.Day.Add(availabilityModel.AvailableTo),
                    EmployeeId = availabilityModel.EmployeeId
                };
                var result = await _availabilityApiService.AddAvailability(availability);
                if (result.Success) return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", result.Message);
            }

            ModelState.AddModelError("", "Invalid Availability");
            var employeeList = _employeeApiService.GetEmployees().Result;
            employeeList.ForEach(x => x.Firstname = x.Firstname + " " + x.Lastname);
            ViewData["EmployeeId"] =
                new SelectList(employeeList, "EmployeeId", "Firstname", availabilityModel.EmployeeId);
            return View(availabilityModel);
        }

        // GET: Availability/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var availability = await _availabilityApiService.GetAvailability(id.Value);
            if (availability == null) return NotFound();

            var employeeList = _employeeApiService.GetEmployees().Result;
            employeeList.ForEach(x => x.Firstname = x.Firstname + " " + x.Lastname);
            ViewData["EmployeeId"] = new SelectList(employeeList, "EmployeeId", "Firstname", availability.EmployeeId);
            return View(availability);
        }

        // POST: Availability/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("AvailabilityId,AvailableFrom,AvailableTo,EmployeeId")]
            Availability availability)
        {
            if (id != availability.AvailabilityId) return NotFound();

            if (ModelState.IsValid)
            {
                var result = await _availabilityApiService.PutAvailability(availability);
                if (result.Success) return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", result.Message);
            }

            ModelState.AddModelError("", "Invalid Availability");
            var employeeList = _employeeApiService.GetEmployees().Result;
            employeeList.ForEach(x => x.Firstname = x.Firstname + " " + x.Lastname);
            ViewData["EmployeeId"] = new SelectList(employeeList, "EmployeeId", "Firstname", availability.EmployeeId);
            return View(availability);
        }

        // GET: Availability/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var availability = await _availabilityApiService.GetAvailability(id.Value);
            if (availability == null) return NotFound();

            return View(availability);
        }

        // POST: Availability/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var availability = await _availabilityApiService.GetAvailability(id);
            if (availability == null) return NotFound();
            var result = await _availabilityApiService.DeleteAvailability(availability);
            if (result.Success) return RedirectToAction(nameof(Index));

            ModelState.AddModelError("", result.Message);
            return View(availability);
        }
    }
}