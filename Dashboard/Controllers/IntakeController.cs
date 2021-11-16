using System.Linq;
using System.Threading.Tasks;
using Core;
using Dashboard.Models;
using Dashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dashboard
{
    public class IntakeController : Controller
    {
        private readonly AppointmentApiService _appointmentApiService;
        private readonly EmployeeApiService _employeeServiceApi;
        private readonly IntakeApiService _intakeApiService;

        public IntakeController(AppointmentApiService appointmentApiService, IntakeApiService intakeApiService,
            EmployeeApiService employeeApiService)
        {
            _appointmentApiService = appointmentApiService;
            _intakeApiService = intakeApiService;
            _employeeServiceApi = employeeApiService;
        }

        // GET: Intake/Create
        public IActionResult Create()
        {
            var employeeList = _employeeServiceApi.GetEmployees().Result;
            employeeList.ForEach(x => x.Firstname = x.Firstname + " " + x.Lastname);
            ViewData["EmployeeId"] = new SelectList(employeeList, "EmployeeId", "Firstname");
            return View();
        }

        // POST: Intake/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Day,Time,EmployeeId,Email")] AppointmentIntake intakeModel)
        {
            if (ModelState.IsValid)
            {
                var intake = new Intake
                {
                    Email = intakeModel.Email,
                    Date = intakeModel.Day,
                    IntakeById = intakeModel.EmployeeId
                };
                var result = await _intakeApiService.AddIntake(intake);

                if (result.Success)
                {
                    var appointment = new Appointment
                    {
                        StartTime = intakeModel.Day.Add(intakeModel.Time),
                        EmployeeId = intakeModel.EmployeeId,
                        AppointmentType = AppointmentType.INTAKE,
                        IntakeId = result.Payload.IntakeId
                    };
                    var resultAppointment = await _appointmentApiService.ClaimAppointment(appointment);
                    ModelState.AddModelError("", resultAppointment.Message);

                    if (resultAppointment.Success) return RedirectToAction("Index", "Home");
                    var delete = await _intakeApiService.DeleteIntake(result.Payload);
                }
                ModelState.AddModelError("", result.Message);
            }

            var employeeList = _employeeServiceApi.GetEmployees().Result;
            employeeList.ForEach(x => x.Firstname = x.Firstname + " " + x.Lastname);
            ViewData["EmployeeId"] = new SelectList(employeeList, "Firstname", "Firstname");
            ModelState.AddModelError("", "Invalid Intake Appointment");
            return View(intakeModel);
        }

        // GET: Intake/Edit/5
        [Authorize(Roles = "PHYSIO_THERAPIST,STUDENT_EMPLOYEE")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var intake = await _intakeApiService.GetIntake(id.Value);
            if (intake == null) return NotFound();

            var employeeList = _employeeServiceApi.GetEmployees().Result;
            employeeList.ForEach(x => x.Firstname = x.Firstname + " " + x.Lastname);
            ViewData["EmployeeName"] = new SelectList(employeeList.Where(x => x.Role == Role.PHYSIO_THERAPIST),
                "Firstname", "Firstname", new SelectListItem {Text = "None", Value = null});
            ViewData["EmployeeId"] = new SelectList(employeeList, "EmployeeId", "Firstname", intake.IntakeById);
            return View(intake);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IntakeId,Email,IntakeById,IntakeSuperVisor,Date")]
            Intake intake)
        {
            if (id != intake.IntakeId) return NotFound();

            if (ModelState.IsValid)
            {
                var result = await _intakeApiService.EditIntake(intake);
                if (result.Success)
                {
                    TempData["email"] = intake.Email;
                    return RedirectToAction("Create", "Patient");
                }

                ModelState.AddModelError("", result.Message);
            }

            var employeeList = _employeeServiceApi.GetEmployees().Result;
            employeeList.ForEach(x => x.Firstname = x.Firstname + " " + x.Lastname);
            ViewData["EmployeeName"] = new SelectList(employeeList.Where(x => x.Role == Role.PHYSIO_THERAPIST),
                "Firstname", "Firstname", new SelectListItem {Text = "None", Value = null});
            ViewData["EmployeeId"] = new SelectList(employeeList, "EmployeeId", "Firstname", intake.IntakeById);

            return View(intake);
        }
    }
}