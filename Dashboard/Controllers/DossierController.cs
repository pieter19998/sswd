using System;
using System.Linq;
using System.Threading.Tasks;
using Core;
using Dashboard.Models;
using Dashboard.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Dashboard
{
    [Authorize]
    public class DossierController : Controller
    {
        private readonly DiagnoseApiService _diagnoseApiService;
        private readonly DossierApiService _dossierApiService;
        private readonly EmployeeApiService _employeeApiService;
        private readonly NoteApiService _noteApiService;
        private readonly OperationApiService _operationApiService;
        private readonly UserManager<User> _userManager;

        public DossierController(DossierApiService dossierApiService, DiagnoseApiService diagnoseApiService,
            OperationApiService operationApiService, EmployeeApiService employeeApiService,
            NoteApiService noteApiService, UserManager<User> userManager)
        {
            _dossierApiService = dossierApiService;
            _diagnoseApiService = diagnoseApiService;
            _operationApiService = operationApiService;
            _employeeApiService = employeeApiService;
            _noteApiService = noteApiService;
            _userManager = userManager;
        }

        // GET: Dossier
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            return User.IsInRole("PATIENT")
                ? View(_dossierApiService.GetDossiers().Result.Where(x => x.PatientId == user.UserId))
                : View(await _dossierApiService.GetDossiers());
        }

        // GET: Dossier/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            Dossier dossier;
            if (User.IsInRole("PATIENT"))
                dossier = await _dossierApiService.GetDossierByPatient(_userManager.GetUserAsync(User).Result.UserId);
            else
                dossier = await _dossierApiService.GetDossierWithAllData(id.Value);

            if (dossier == null) return NotFound();
            return View(dossier);
        }

        // GET: Dossier/Create
        [Authorize(Roles = "PHYSIO_THERAPIST,STUDENT_EMPLOYEE")]
        public IActionResult Create(int id)
        {
            if (TempData.ContainsKey("type")) ViewBag.type = TempData["type"];

            var user = _userManager.GetUserAsync(User).Result;
            var employeeList = _employeeApiService.GetEmployees().Result;
            employeeList.ForEach(x => x.Firstname = x.Firstname + " " + x.Lastname);
            ViewData["HeadPractitioner"] = new SelectList(employeeList, "EmployeeId", "Firstname", user.UserId);
            TempData["patientId"] = id;
            ViewData["BodyLocation"] =
                new SelectList(
                    _diagnoseApiService.GetDiagnoses().Result.GroupBy(x => x.BodyLocation).Select(y => y.First()),
                    "BodyLocation", "BodyLocation");
            return View();
        }

        // POST: Dossier/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "PHYSIO_THERAPIST,STUDENT_EMPLOYEE")]
        public async Task<IActionResult> Create(
            [Bind(
                "Code,BodyLocation,Pathology,Description,DiagnoseDescription,PatientType,HeadPractitioner,PatientId,DismissalDay")]
            DossierModel dossierModel)
        {
            if (ModelState.IsValid)
            {
                var dossier = new Dossier
                {
                    DiagnoseCode = dossierModel.Code.ToString(),
                    HeadPractitionerId = dossierModel.HeadPractitioner,
                    PatientId = dossierModel.PatientId,
                    Description = dossierModel.Description,
                    DiagnoseDescription = dossierModel.DiagnoseDescription,
                    DismissalDay = dossierModel.DismissalDay
                };
                var result = await _dossierApiService.AddDossier(dossier);
                if (result.Success)
                    return RedirectToAction("Create", "TreatmentPlan", new {id = result.Payload.DossierId});

                ModelState.AddModelError("", result.Message);
            }

            var employeeList = _employeeApiService.GetEmployees().Result;
            employeeList.ForEach(x => x.Firstname = x.Firstname + " " + x.Lastname);
            ViewData["HeadPractitioner"] = new SelectList(employeeList, "EmployeeId", "Firstname");
            TempData["patientId"] = dossierModel.PatientId;
            ViewData["BodyLocation"] = new SelectList(
                _diagnoseApiService.GetDiagnoses().Result.GroupBy(x => x.BodyLocation).Select(y => y.First()),
                "BodyLocation", "BodyLocation");
            ModelState.AddModelError("", "invalid Dossier");
            return View(dossierModel);
        }

        // GET: Dossier/Edit/5
        [Authorize(Roles = "PHYSIO_THERAPIST,STUDENT_EMPLOYEE")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var dossier = await _dossierApiService.GetDossier(id.Value);
            if (dossier == null) return NotFound();

            var employeeList = _employeeApiService.GetEmployees().Result;
            employeeList.ForEach(x => x.Firstname = x.Firstname + " " + x.Lastname);
            ViewData["HeadPractitioner"] =
                new SelectList(employeeList, "EmployeeId", "Firstname", dossier.HeadPractitionerId);
            ViewData["BodyLocation"] =
                new SelectList(
                    _diagnoseApiService.GetDiagnoses().Result.GroupBy(x => x.BodyLocation).Select(y => y.First()),
                    "BodyLocation", "BodyLocation");
            var dossierModel = new DossierEdit
            {
                DossierId = dossier.DossierId,
                PatientId = dossier.PatientId,
                TreatmentPlanId = dossier.TreatmentPlanId,
                Age = dossier.Age,
                Active = dossier.Active,
                Description = dossier.Description,
                DiagnoseDescription = dossier.DiagnoseDescription,
                DiagnoseCode = dossier.DiagnoseCode,
                ApplicationDay = dossier.ApplicationDay,
                DismissalDay = dossier.DismissalDay,
                HeadPractitionerId = dossier.HeadPractitionerId,
                Code = int.Parse(dossier.DiagnoseCode)
            };
            return View(dossierModel);
        }

        // POST: Dossier/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // [HttpPost]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> Edit(int id,
        //     [Bind(
        //         "DossierId,Age,Description,DiagnoseCode,DiagnoseDescription,PatientType,HeadPractitionerId,ApplicationDay,DismissalDay,Active,TreatmentPlanId,PatientId")]
        //     Dossier dossier)
        // {
        //     if (id != dossier.DossierId)
        //     {
        //         return NotFound();
        //     }
        //
        //     if (ModelState.IsValid)
        //     {
        //         try
        //         {
        //             _context.Update(dossier);
        //             await _context.SaveChangesAsync();
        //         }
        //         catch (DbUpdateConcurrencyException)
        //         {
        //             if (!DossierExists(dossier.DossierId))
        //             {
        //                 return NotFound();
        //             }
        //             else
        //             {
        //                 throw;
        //             }
        //         }
        //
        //         return RedirectToAction(nameof(Index));
        //     }
        //
        //     ViewData["HeadPractitionerId"] =
        //         new SelectList(_context.Employees, "EmployeeId", "Email", dossier.HeadPractitionerId);
        //     ViewData["PatientId"] = new SelectList(_context.Patients, "PatientId", "Address", dossier.PatientId);
        //     ViewData["TreatmentPlanId"] = new SelectList(_context.TreatmentPlans, "TreatmentPlanId", "TreatmentPlanId",
        //         dossier.TreatmentPlanId);
        //     return View(dossier);
        // }
        [Authorize(Roles = "PHYSIO_THERAPIST,STUDENT_EMPLOYEE")]
        [HttpPost]
        public async Task<JsonResult> GetPathology(string bodyLocation)
        {
            var data = await _diagnoseApiService.GetDiagnoses();
            return Json(data.Where(x => x.BodyLocation == bodyLocation));
        }

        [Authorize(Roles = "PHYSIO_THERAPIST,STUDENT_EMPLOYEE")]
        [HttpPost]
        public async Task<JsonResult> GetDescription(string code)
        {
            var data = await _operationApiService.GetOperation(code);
            if (data == null) return Json("No description available");

            return Json(data.Description);
        }

        [Authorize(Roles = "PHYSIO_THERAPIST,STUDENT_EMPLOYEE")]
        public IActionResult AddNote(int id)
        {
            ViewData["id"] = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddNote([Bind("Text,DossierId,Visible")] NoteModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var note = new Notes
                {
                    Author = user.UserName,
                    Text = model.Text,
                    NoteType = NoteType.DOSSIER,
                    Visible = model.Visible,
                    Date = DateTime.Now
                };

                var result = await _noteApiService.AddNote(note, model.DossierId);
                if (result.Success) return RedirectToAction("Details", "Dossier", new {id = model.DossierId});

                ModelState.AddModelError("", result.Message);
            }

            ModelState.AddModelError("", "Invalid note");

            return View();
        }
    }
}