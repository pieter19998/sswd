using System.Threading.Tasks;
using Core;
using Dashboard.Models;
using Dashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard
{
    [Authorize(Roles = "PHYSIO_THERAPIST,STUDENT_EMPLOYEE")]
    public class EmployeeController : Controller
    {
        private readonly EmployeeApiService _employeeApiService;

        public EmployeeController(EmployeeApiService employeeApiService)
        {
            _employeeApiService = employeeApiService;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            return View(await _employeeApiService.GetEmployees());
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var employee = await _employeeApiService.GetEmployee(id.Value);
            if (employee == null) return NotFound();

            return View(employee);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Firstname,Lastname,Email,Role,BigNumber,StudentNumber,Password")]
            EmployeeModel employee)
        {
            if (ModelState.IsValid)
            {
                if (employee.Role == Role.PHYSIO_THERAPIST && employee.BigNumber == null)
                {
                    ModelState.AddModelError("", "Physiotherapist requires a BigNumber");
                    return View(employee);
                }

                if (employee.Role == Role.STUDENT_EMPLOYEE && employee.StudentNumber == null)
                {
                    ModelState.AddModelError("", "Student requires a StudentNumber");
                    return View(employee);
                }

                var result = await _employeeApiService.AddEmployee(employee);
                if (result.Success) return RedirectToAction(nameof(Index));

                ModelState.AddModelError("", result.Message);
                ModelState.AddModelError("", "Invalid Employee");
            }

            return View(employee);
        }

        private async Task<bool> EmployeeExists(int id)
        {
            return await _employeeApiService.GetEmployee(id) != null;
        }
    }
}