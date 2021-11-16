using System.Threading.Tasks;
using Core;
using Dashboard.Models;
using Dashboard.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard
{
    public class PatientController : Controller
    {
        private readonly PatientApiService _patientApiService;

        public PatientController(PatientApiService patientApiService)
        {
            _patientApiService = patientApiService;
        }

        // GET: Patient/Create
        public IActionResult Create()
        {
            if (TempData.ContainsKey("email")) ViewBag.email = TempData["email"] as string;

            return View();
        }


        // POST: Patient/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind(
                "FirstName,LastName,Email,Address,Sex,PatientNumber,DayOfBirth,PersonalNumber,StudentNumber,Role,Type")]
            PatientModel patientModel)
        {
            if (patientModel.Type == PatientType.EMPLOYEE && patientModel.PersonalNumber == null)
            {
                ModelState.AddModelError("", "PersonalNumber is required");
                return View(patientModel);
            }

            if (patientModel.Type == PatientType.STUDENT && patientModel.StudentNumber == null)
            {
                ModelState.AddModelError("", "StudentNumber is required");
                return View(patientModel);
            }

            if (ModelState.IsValid)
            {
                var patient = new Patient
                {
                    Firstname = patientModel.FirstName,
                    Lastname = patientModel.LastName,
                    Email = patientModel.Email,
                    Sex = patientModel.Sex,
                    Address = patientModel.Address,
                    DayOfBirth = patientModel.DayOfBirth,
                    Type = patientModel.Type,
                    PersonalNumber = patientModel.PersonalNumber,
                    StudentNumber = patientModel.StudentNumber
                };
                var result = await _patientApiService.AddPatient(patient);
                if (result.Success)
                {
                    TempData["type"] = patient.Type;
                    return RedirectToAction("Create", "Dossier", new {id = result.Payload.PatientId});
                }

                ModelState.AddModelError("", result.Message);
            }

            return View(patientModel);
        }

        // GET: Patient/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var patient = await _patientApiService.GetPatient(id.Value);
            if (patient == null) return NotFound();
            return View(patient);
        }

        // POST: Patient/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind(
                "PatientId,IdentityPatientId,Firstname,Lastname,Email,Active,Address,Sex,PatientNumber,Photo,DayOfBirth,PersonalNumber,StudentNumber,Role,Type")]
            Patient patientModel)
        {
            if (id != patientModel.PatientId) return NotFound();

            if (patientModel.Type == PatientType.EMPLOYEE && patientModel.PersonalNumber == null)
            {
                ModelState.AddModelError("", "PersonalNumber is required");
                return View(patientModel);
            }

            if (patientModel.Type == PatientType.STUDENT && patientModel.StudentNumber == null)
            {
                ModelState.AddModelError("", "StudentNumber is required");
                return View(patientModel);
            }

            if (ModelState.IsValid)
            {
                var result = await _patientApiService.UpdatePatient(patientModel);
                if (result.Success) return RedirectToAction("Index", "Home");

                ModelState.AddModelError("", result.Message);
            }

            return View(patientModel);
        }


        public async Task<IActionResult> UpdateAddress(int? id)
        {
            if (id == null) return NotFound();
            var patient = await _patientApiService.GetPatient(id.Value);
            if (patient == null) return NotFound();
            var updateAddress = new UpdateAddress
            {
                PatientId = patient.PatientId,
                Address = patient.Address
            };
            return View(updateAddress);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAddress([Bind("Address,PatientId")] UpdateAddress updateAddress)
        {
            if (ModelState.IsValid)
            {
                var result = await _patientApiService.UpdateAddress(updateAddress);
                if (result.Success)
                    return RedirectToAction("Index", "Home");
                ModelState.AddModelError("", result.Message);
                ModelState.AddModelError("", "invalid address");
            }

            return View(updateAddress);
        }
    }
}