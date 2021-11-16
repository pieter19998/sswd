using System;
using System.Collections.Generic;
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
    public class SessionController : Controller
    {
        private readonly AppointmentApiService _appointmentApiService;
        private readonly DossierApiService _dossierApiService;
        private readonly EmployeeApiService _employeeApiService;
        private readonly NoteApiService _noteApiService;
        private readonly OperationApiService _operationApiService;
        private readonly PatientApiService _patientApiService;
        private readonly SessionApiService _sessionApiService;
        private readonly UserManager<User> _userManager;


        public SessionController(SessionApiService sessionApiService, EmployeeApiService employeeApiService,
            UserManager<User> userManager, DossierApiService dossierApiService,
            AppointmentApiService appointmentApiService, PatientApiService patientApiService,
            OperationApiService operationApiService, NoteApiService noteApiService)
        {
            _sessionApiService = sessionApiService;
            _employeeApiService = employeeApiService;
            _userManager = userManager;
            _dossierApiService = dossierApiService;
            _appointmentApiService = appointmentApiService;
            _patientApiService = patientApiService;
            _operationApiService = operationApiService;
            _noteApiService = noteApiService;
        }

        // GET: Session
        [Authorize]
        public async Task<IActionResult> Index()
        {
            ;
            return View(await _sessionApiService.GetSessions());
        }

        //GET: Session/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var session = await _sessionApiService.GetSession(id.Value);
            if (session == null) return NotFound();

            return View(session);
        }

        // // GET: Session/Create
        [Authorize]
        public IActionResult Create()
        {
            var user = _userManager.GetUserAsync(User).Result;
            if (User.IsInRole("PATIENT"))
            {
                var dossier = _dossierApiService.GetDossierByPatient(user.UserId).Result;
                if (dossier == null) return NotFound();

                var employeeList = _employeeApiService.GetEmployees().Result;
                employeeList.ForEach(x => x.Firstname = x.Firstname + " " + x.Lastname);
                ViewData["EmployeeId"] = new SelectList(employeeList, "EmployeeId", "Firstname",
                    dossier.HeadPractitionerId
                );

                var patientList = _patientApiService.GetPatients().Result;
                patientList.ForEach(x => x.Firstname = x.Firstname + " " + x.Lastname);
                ViewData["PatientId"] = new SelectList(patientList, "PatientId", "Firstname", user.UserId);
            }
            else
            {
                var employeeList = _employeeApiService.GetEmployees().Result;
                employeeList.ForEach(x => x.Firstname = x.Firstname + " " + x.Lastname);
                ViewData["EmployeeId"] = new SelectList(employeeList, "EmployeeId", "Firstname", user.UserId
                );

                var patientList = _patientApiService.GetPatients().Result;
                patientList.ForEach(x => x.Firstname = x.Firstname + " " + x.Lastname);
                ViewData["PatientId"] = new SelectList(patientList, "PatientId", "Firstname");
            }

            return View();
        }

        // POST: Session/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Day,Time,EmployeeId,PatientId")]
            AppointmentSession appointmentSession)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user == null && appointmentSession.PatientId == 0)
                {
                    ModelState.AddModelError("", "Only a logged in patient can make a session appointment.");
                    return View();
                }

                var patient = await _patientApiService.GetPatient(appointmentSession.PatientId);
                if (patient == null)
                {
                    ModelState.AddModelError("", "Patient not found, Contact administration");
                    return View();
                }

                var dossier = await _dossierApiService.GetDossierByPatient(patient.PatientId);
                if (dossier == null)
                {
                    ModelState.AddModelError("", "Dossier not found, Contact administration");
                    return View();
                }

                var session = new Session
                {
                    SessionDate = appointmentSession.Day,
                    PatientId = appointmentSession.PatientId,
                    DossierId = dossier.DossierId,
                    SessionEmployeeId = appointmentSession.EmployeeId,
                    RoomType = appointmentSession.RoomType
                };
                var result = await _sessionApiService.AddSession(session);
                if (result.Success)
                {
                    var appointment = new Appointment
                    {
                        StartTime = appointmentSession.Day.Add(appointmentSession.Time),
                        EmployeeId = appointmentSession.EmployeeId,
                        AppointmentType = AppointmentType.SESSION,
                        PatientId = appointmentSession.PatientId,
                        SessionId = result.Payload.SessionId
                    };
                    var resultAppointment = await _appointmentApiService.ClaimAppointment(appointment);
                    if (resultAppointment.Success) return RedirectToAction("Index", "Appointment");
                    var delete = await _sessionApiService.DeleteSession(result.Payload);
                }
                ModelState.AddModelError("", result.Message);
            }

            var employeeList = _employeeApiService.GetEmployees().Result;
            employeeList.ForEach(x => x.Firstname = x.Firstname + " " + x.Lastname);
            ViewData["EmployeeId"] = new SelectList(employeeList, "EmployeeId", "Firstname");


            return View(appointmentSession);
        }

        [HttpPost]
        public JsonResult GetSessionDuration(int patientId)
        {
            var data = _dossierApiService.GetDossierByPatient(patientId).Result.TreatmentPlan.SessionDuration;
            return Json(data);
        }

        // GET: Session/Edit/5
        [Authorize(Roles = "PHYSIO_THERAPIST,STUDENT_EMPLOYEE")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var session = await _sessionApiService.GetSession(id.Value);
            if (session == null) return NotFound();

            if (session.Type != null) return NotFound();

            var dossier = await _dossierApiService.GetDossier(session.DossierId);
            if (dossier == null) return NotFound();

            var employee = await _employeeApiService.GetEmployee(session.SessionEmployeeId);
            if (employee == null) return NotFound();

            var sessionEditModel = new SessionEditModel
            {
                SessionId = session.SessionId,
                Active = session.Active,
                Type = session.Type,
                RoomType = session.RoomType,
                PatientId = session.PatientId,
                SessionEmployeeId = session.SessionEmployeeId,
                SessionDate = session.SessionDate,
                DossierId = session.DossierId
            };

            var patientList = _patientApiService.GetPatients().Result;
            patientList.ForEach(x => x.Firstname = x.Firstname + " " + x.Lastname);
            var employeeList = _employeeApiService.GetEmployees().Result;
            employeeList.ForEach(x => x.Firstname = x.Firstname + " " + x.Lastname);
            ViewData["PatientId"] = new SelectList(patientList, "PatientId", "Firstname", session.PatientId);
            ViewData["SessionEmployeeId"] =
                new SelectList(employeeList, "EmployeeId", "Firstname", dossier.HeadPractitionerId);
            ViewData["TypeId"] =
                new SelectList(_operationApiService.GetOperations().Result, "Value", "Value", session.Type);
            return View(sessionEditModel);
        }

        // // POST: Session/Edit/5
        // // To protect from overposting attacks, enable the specific properties you want to bind to.
        // // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind(
                "SessionId,Type,RoomType,PatientId,SessionEmployeeId,SessionDate,Active,DossierId,Visible,Text,Additional")]
            SessionEditModel sessionModel)
        {
            if (id != sessionModel.SessionId) return NotFound();

            if (ModelState.IsValid)
            {
                var session = new Session
                {
                    SessionId = sessionModel.SessionId,
                    Active = sessionModel.Active,
                    Type = sessionModel.Type,
                    RoomType = sessionModel.RoomType,
                    PatientId = sessionModel.PatientId,
                    SessionEmployeeId = sessionModel.SessionEmployeeId,
                    SessionDate = sessionModel.SessionDate,
                    DossierId = sessionModel.DossierId,
                    Notices = new List<Notes>()
                };

                var user = await _userManager.GetUserAsync(User);
                var note = new Notes
                {
                    Author = user.UserName,
                    Text = sessionModel.Text,
                    NoteType = NoteType.DOSSIER,
                    Visible = sessionModel.Visible,
                    Date = DateTime.Now
                };

                session.Notices.Add(note);

                var result = await _sessionApiService.UpdateSession(session);
                if (result.Success) return RedirectToAction("Details", "Dossier", new {id = session.DossierId});
                ModelState.AddModelError("", result.Message);
            }

            //reload page
            var patientList = _patientApiService.GetPatients().Result;
            patientList.ForEach(x => x.Firstname = x.Firstname + " " + x.Lastname);
            var employeeList = _employeeApiService.GetEmployees().Result;
            employeeList.ForEach(x => x.Firstname = x.Firstname + " " + x.Lastname);
            // ViewData["DossierId"] = new SelectList(dossierList, "DossierId", "Patient.Firstname", session.DossierId);
            ViewData["PatientId"] = new SelectList(patientList, "PatientId", "Firstname", sessionModel.PatientId);
            ViewData["SessionEmployeeId"] =
                new SelectList(employeeList, "EmployeeId", "Firstname", sessionModel.SessionEmployeeId);
            ViewData["TypeId"] = new SelectList(_operationApiService.GetOperations().Result, "Value", "Value",
                sessionModel.Type);
            return View(sessionModel);
        }

        [HttpPost]
        public JsonResult GetOperation(string value)
        {
            var data = _operationApiService.GetOperation(value).Result;
            return Json(data);
        }
    }
}