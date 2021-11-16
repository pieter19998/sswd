using System;
using System.Collections.Generic;
using ApplicationServices;
using Core;
using Core.stam;
using DomainServices;
using Moq;
using Xunit;

namespace ApplicationServiceTest
{
    public class SessionServiceTest
    {
        [Fact]
        public void Session_is_valid()
        {
            var employee = new Employee
            {
                EmployeeId = 1,
                Firstname = "Ja",
                Lastname = " Steen",
                Email = "Jansteen@gmail.com",
                Role = Role.PHYSIO_THERAPIST,
                BigNumber = "123"
            };
            var user = new User
            {
                Id = "1",
                UserId = 2
            };
            var patient = new Patient
            {
                PatientId = 2,
                Firstname = "Willem",
                Lastname = "Bakker",
                PatientNumber = "12",
                Email = "Wim123@gmail.com",
                Role = Role.PATIENT,
                Type = PatientType.STUDENT
            };
            var treatmentPlan = new TreatmentPlan
            {
                TreatmentPlanId = 1,
                SessionsPerWeek = 1,
                SessionDuration = 30.0
            };
            var dossier = new Dossier
            {
                DossierId = 1,
                Age = 30,
                Description = "-",
                DiagnoseCode = "-",
                DiagnoseDescription = "-",
                HeadPractitioner = employee,
                HeadPractitionerId = employee.EmployeeId,
                ApplicationDay = DateTime.Now,
                DismissalDay = DateTime.Now.AddDays(10),
                Patient = patient,
                PatientId = patient.PatientId,
                TreatmentPlan = treatmentPlan,
                TreatmentPlanId = 1
            };

            var appointment = new Appointment
            {
                AppointmentId = 1,
                StartTime = DateTime.Now,
                AppointmentType = AppointmentType.SESSION,
                PatientId = patient.PatientId,
                EmployeeId = employee.EmployeeId,
                SessionId = 1
            };
            var operation = new Operation
            {
                Value = "1004",
                Description =
                    "Individuele zitting reguliere fysiotherapie met toeslag voor behandeling op de werkplek (eenmalig)",
                Additional = "Nee"
            };
            var session = new Session
            {
                SessionDate = DateTime.Now,
                Type = "1004",
                RoomType = RoomType.PRACTICE_ROOM,
                Patient = patient,
                PatientId = patient.PatientId,
                SessionEmployee = employee,
                SessionEmployeeId = employee.EmployeeId,
                Dossier = dossier,
                DossierId = dossier.DossierId,
                Appointment = appointment
            };

            var sessionRepo = new Mock<ISessionRepository>();
            var appointmentRepo = new Mock<IAppointmentRepository>();
            var userRepo = new Mock<IUserRepository>();
            var dossierRepo = new Mock<IDossierRepository>();
            var operationMock = new Mock<IStamApiService>();
            dossierRepo.Setup(x => x.GetDossierByPatientId(patient.PatientId)).ReturnsAsync(dossier);
            operationMock.Setup(x => x.GetOperation("1004")).ReturnsAsync(operation);
            appointmentRepo.Setup(x => x.GetAppointment(appointment.AppointmentId)).ReturnsAsync(appointment);
            userRepo.Setup(x => x.GetUser(session.PatientId)).ReturnsAsync(user);
            var service = new SessionService(sessionRepo.Object, appointmentRepo.Object, dossierRepo.Object,
                userRepo.Object, operationMock.Object);
            //Act
            var result = service.AddSession(session);
            //Assert
            Assert.True(result.Result.Success);
        }

        [Fact]
        public void Treatment_is_over()
        {
            var employee = new Employee
            {
                EmployeeId = 1,
                Firstname = "Ja",
                Lastname = " Steen",
                Email = "Jansteen@gmail.com",
                Role = Role.PHYSIO_THERAPIST,
                BigNumber = "123"
            };
            var user = new User
            {
                Id = "1",
                UserId = 2
            };
            var patient = new Patient
            {
                PatientId = 2,
                Firstname = "Willem",
                Lastname = "Bakker",
                PatientNumber = "12",
                Email = "Wim123@gmail.com",
                Role = Role.PATIENT,
                Type = PatientType.STUDENT
            };
            var treatmentPlan = new TreatmentPlan
            {
                TreatmentPlanId = 1,
                SessionsPerWeek = 1,
                SessionDuration = 30.0
            };
            var dossier = new Dossier
            {
                DossierId = 1,
                Age = 30,
                Description = "-",
                DiagnoseCode = "-",
                DiagnoseDescription = "-",
                HeadPractitioner = employee,
                HeadPractitionerId = employee.EmployeeId,
                ApplicationDay = DateTime.Now,
                DismissalDay = DateTime.Now.AddDays(-10),
                Patient = patient,
                PatientId = patient.PatientId,
                TreatmentPlan = treatmentPlan,
                TreatmentPlanId = 1
            };

            var appointment = new Appointment
            {
                AppointmentId = 1,
                StartTime = DateTime.Now,
                AppointmentType = AppointmentType.SESSION,
                PatientId = patient.PatientId,
                EmployeeId = employee.EmployeeId,
                SessionId = 1
            };

            var session = new Session
            {
                SessionDate = DateTime.Now,
                Type = "1004",
                RoomType = RoomType.PRACTICE_ROOM,
                Patient = patient,
                PatientId = patient.PatientId,
                SessionEmployee = employee,
                SessionEmployeeId = employee.EmployeeId,
                Dossier = dossier,
                DossierId = dossier.DossierId,
                Appointment = appointment
            };

            var operation = new Operation
            {
                Value = "1004",
                Description =
                    "Individuele zitting reguliere fysiotherapie met toeslag voor behandeling op de werkplek (eenmalig)",
                Additional = "Nee"
            };

            var sessionRepo = new Mock<ISessionRepository>();
            var appointmentRepo = new Mock<IAppointmentRepository>();
            var userRepo = new Mock<IUserRepository>();
            var dossierRepo = new Mock<IDossierRepository>();
            var operationMock = new Mock<IStamApiService>();
            dossierRepo.Setup(x => x.GetDossierByPatientId(patient.PatientId)).ReturnsAsync(dossier);
            operationMock.Setup(x => x.GetOperation("1004")).ReturnsAsync(operation);
            appointmentRepo.Setup(x => x.GetAppointment(appointment.AppointmentId)).ReturnsAsync(appointment);
            userRepo.Setup(x => x.GetUser(session.PatientId)).ReturnsAsync(user);
            var service = new SessionService(sessionRepo.Object, appointmentRepo.Object, dossierRepo.Object,
                userRepo.Object, operationMock.Object);
            //Act
            var result = service.AddSession(session);
            //Assert
            Assert.False(result.Result.Success);
        }

        [Fact]
        public void User_is_not_yet_registered()
        {
            var employee = new Employee
            {
                EmployeeId = 1,
                Firstname = "Ja",
                Lastname = " Steen",
                Email = "Jansteen@gmail.com",
                Role = Role.PHYSIO_THERAPIST,
                BigNumber = "123"
            };
            var user = new User
            {
                Id = "1",
                UserId = 2
            };
            var patient = new Patient
            {
                PatientId = 2,
                Firstname = "Willem",
                Lastname = "Bakker",
                PatientNumber = "12",
                Email = "Wim123@gmail.com",
                Role = Role.PATIENT,
                Type = PatientType.STUDENT
            };
            var treatmentPlan = new TreatmentPlan
            {
                TreatmentPlanId = 1,
                SessionsPerWeek = 1,
                SessionDuration = 30.0
            };
            var dossier = new Dossier
            {
                DossierId = 1,
                Age = 30,
                Description = "-",
                DiagnoseCode = "-",
                DiagnoseDescription = "-",
                HeadPractitioner = employee,
                HeadPractitionerId = employee.EmployeeId,
                ApplicationDay = DateTime.Now,
                DismissalDay = DateTime.Now.AddDays(-10),
                Patient = patient,
                PatientId = patient.PatientId,
                TreatmentPlan = treatmentPlan,
                TreatmentPlanId = 1
            };

            var appointment = new Appointment
            {
                AppointmentId = 1,
                StartTime = DateTime.Now,
                AppointmentType = AppointmentType.SESSION,
                PatientId = patient.PatientId,
                EmployeeId = employee.EmployeeId,
                SessionId = 1
            };

            var session = new Session
            {
                SessionDate = DateTime.Now,
                Type = "1004",
                RoomType = RoomType.PRACTICE_ROOM,
                Patient = patient,
                PatientId = patient.PatientId,
                SessionEmployee = employee,
                SessionEmployeeId = employee.EmployeeId,
                Dossier = dossier,
                DossierId = dossier.DossierId,
                Appointment = appointment
            };
            var operation = new Operation
            {
                Value = "1004",
                Description =
                    "Individuele zitting reguliere fysiotherapie met toeslag voor behandeling op de werkplek (eenmalig)",
                Additional = "Nee"
            };

            var sessionRepo = new Mock<ISessionRepository>();
            var appointmentRepo = new Mock<IAppointmentRepository>();
            var userRepo = new Mock<IUserRepository>();
            var dossierRepo = new Mock<IDossierRepository>();
            var operationMock = new Mock<IStamApiService>();
            dossierRepo.Setup(x => x.GetDossierByPatientId(patient.PatientId)).ReturnsAsync(dossier);
            operationMock.Setup(x => x.GetOperation("1004")).ReturnsAsync(operation);
            appointmentRepo.Setup(x => x.GetAppointment(appointment.AppointmentId)).ReturnsAsync(appointment);
            var service = new SessionService(sessionRepo.Object, appointmentRepo.Object, dossierRepo.Object,
                userRepo.Object, operationMock.Object);
            //Act
            var result = service.AddSession(session);
            //Assert
            Assert.False(result.Result.Success);
        }

        [Fact]
        public void Session_needs_note_when_type_specified_failure()
        {
            var employee = new Employee
            {
                EmployeeId = 1,
                Firstname = "Ja",
                Lastname = " Steen",
                Email = "Jansteen@gmail.com",
                Role = Role.PHYSIO_THERAPIST,
                BigNumber = "123"
            };
            var user = new User
            {
                Id = "1",
                UserId = 2
            };
            var patient = new Patient
            {
                PatientId = 2,
                Firstname = "Willem",
                Lastname = "Bakker",
                PatientNumber = "12",
                Email = "Wim123@gmail.com",
                Role = Role.PATIENT,
                Type = PatientType.STUDENT
            };
            var treatmentPlan = new TreatmentPlan
            {
                TreatmentPlanId = 1,
                SessionsPerWeek = 1,
                SessionDuration = 30.0
            };
            var dossier = new Dossier
            {
                DossierId = 1,
                Age = 30,
                Description = "-",
                DiagnoseCode = "-",
                DiagnoseDescription = "-",
                HeadPractitioner = employee,
                HeadPractitionerId = employee.EmployeeId,
                ApplicationDay = DateTime.Now,
                DismissalDay = DateTime.Now.AddDays(10),
                Patient = patient,
                PatientId = patient.PatientId,
                TreatmentPlan = treatmentPlan,
                TreatmentPlanId = 1
            };

            var appointment = new Appointment
            {
                AppointmentId = 1,
                StartTime = DateTime.Now,
                AppointmentType = AppointmentType.SESSION,
                PatientId = patient.PatientId,
                EmployeeId = employee.EmployeeId,
                SessionId = 1
            };
            var operation = new Operation
            {
                Value = "1004",
                Description =
                    "Individuele zitting reguliere fysiotherapie met toeslag voor behandeling op de werkplek (eenmalig)",
                Additional = "Ja"
            };
            var session = new Session
            {
                SessionId = 100,
                SessionDate = DateTime.Now,
                Type = "1004",
                RoomType = RoomType.PRACTICE_ROOM,
                Patient = patient,
                PatientId = patient.PatientId,
                SessionEmployee = employee,
                SessionEmployeeId = employee.EmployeeId,
                Dossier = dossier,
                DossierId = dossier.DossierId,
                Appointment = appointment
            };

            var sessionRepo = new Mock<ISessionRepository>();
            var appointmentRepo = new Mock<IAppointmentRepository>();
            var userRepo = new Mock<IUserRepository>();
            var dossierRepo = new Mock<IDossierRepository>();
            var operationMock = new Mock<IStamApiService>();
            dossierRepo.Setup(x => x.GetDossierByPatientId(patient.PatientId)).ReturnsAsync(dossier);
            operationMock.Setup(x => x.GetOperation("1004")).ReturnsAsync(operation);
            appointmentRepo.Setup(x => x.GetAppointment(appointment.AppointmentId)).ReturnsAsync(appointment);
            userRepo.Setup(x => x.GetUser(session.PatientId)).ReturnsAsync(user);
            var service = new SessionService(sessionRepo.Object, appointmentRepo.Object, dossierRepo.Object,
                userRepo.Object, operationMock.Object);
            //Act
            var result1 = service.AddSession(session);
            var result2 = service.UpdateSession(session, 100);
            //Assert
            Assert.False(result2.Result.Success);
        }

        [Fact]
        public void Session_needs_note_when_type_specified_success()
        {
            var employee = new Employee
            {
                EmployeeId = 1,
                Firstname = "Ja",
                Lastname = " Steen",
                Email = "Jansteen@gmail.com",
                Role = Role.PHYSIO_THERAPIST,
                BigNumber = "123"
            };
            var user = new User
            {
                Id = "1",
                UserId = 2
            };
            var patient = new Patient
            {
                PatientId = 2,
                Firstname = "Willem",
                Lastname = "Bakker",
                PatientNumber = "12",
                Email = "Wim123@gmail.com",
                Role = Role.PATIENT,
                Type = PatientType.STUDENT
            };
            var treatmentPlan = new TreatmentPlan
            {
                TreatmentPlanId = 1,
                SessionsPerWeek = 1,
                SessionDuration = 30.0
            };
            var dossier = new Dossier
            {
                DossierId = 1,
                Age = 30,
                Description = "-",
                DiagnoseCode = "-",
                DiagnoseDescription = "-",
                HeadPractitioner = employee,
                HeadPractitionerId = employee.EmployeeId,
                ApplicationDay = DateTime.Now,
                DismissalDay = DateTime.Now.AddDays(10),
                Patient = patient,
                PatientId = patient.PatientId,
                TreatmentPlan = treatmentPlan,
                TreatmentPlanId = 1
            };

            var appointment = new Appointment
            {
                AppointmentId = 1,
                StartTime = DateTime.Now,
                AppointmentType = AppointmentType.SESSION,
                PatientId = patient.PatientId,
                EmployeeId = employee.EmployeeId,
                SessionId = 1
            };
            var operation = new Operation
            {
                Value = "1004",
                Description =
                    "Individuele zitting reguliere fysiotherapie met toeslag voor behandeling op de werkplek (eenmalig)",
                Additional = "Ja"
            };
            var note = new Notes
            {
                NoticeId = 1,
                Text = "test'",
                Date = DateTime.Now,
                Author = "jan"
            };
            var session = new Session
            {
                SessionDate = DateTime.Now,
                Type = "1004",
                RoomType = RoomType.PRACTICE_ROOM,
                Patient = patient,
                PatientId = patient.PatientId,
                SessionEmployee = employee,
                SessionEmployeeId = employee.EmployeeId,
                Dossier = dossier,
                DossierId = dossier.DossierId,
                Appointment = appointment,
                Notices = new List<Notes>()
            };
            session.Notices.Add(note);

            var sessionRepo = new Mock<ISessionRepository>();
            var appointmentRepo = new Mock<IAppointmentRepository>();
            var userRepo = new Mock<IUserRepository>();
            var dossierRepo = new Mock<IDossierRepository>();
            var operationMock = new Mock<IStamApiService>();
            dossierRepo.Setup(x => x.GetDossierByPatientId(patient.PatientId)).ReturnsAsync(dossier);
            operationMock.Setup(x => x.GetOperation("1004")).ReturnsAsync(operation);
            appointmentRepo.Setup(x => x.GetAppointment(appointment.AppointmentId)).ReturnsAsync(appointment);
            userRepo.Setup(x => x.GetUser(session.PatientId)).ReturnsAsync(user);
            var service = new SessionService(sessionRepo.Object, appointmentRepo.Object, dossierRepo.Object,
                userRepo.Object, operationMock.Object);
            //Act
            var result = service.AddSession(session);
            //Assert
            Assert.True(result.Result.Success);
        }

        [Fact]
        public void Session_does_not__need_note_when_type_specified()
        {
            var employee = new Employee
            {
                EmployeeId = 1,
                Firstname = "Ja",
                Lastname = " Steen",
                Email = "Jansteen@gmail.com",
                Role = Role.PHYSIO_THERAPIST,
                BigNumber = "123"
            };
            var user = new User
            {
                Id = "1",
                UserId = 2
            };
            var patient = new Patient
            {
                PatientId = 2,
                Firstname = "Willem",
                Lastname = "Bakker",
                PatientNumber = "12",
                Email = "Wim123@gmail.com",
                Role = Role.PATIENT,
                Type = PatientType.STUDENT
            };
            var treatmentPlan = new TreatmentPlan
            {
                TreatmentPlanId = 1,
                SessionsPerWeek = 1,
                SessionDuration = 30.0
            };
            var dossier = new Dossier
            {
                DossierId = 1,
                Age = 30,
                Description = "-",
                DiagnoseCode = "-",
                DiagnoseDescription = "-",
                HeadPractitioner = employee,
                HeadPractitionerId = employee.EmployeeId,
                ApplicationDay = DateTime.Now,
                DismissalDay = DateTime.Now.AddDays(10),
                Patient = patient,
                PatientId = patient.PatientId,
                TreatmentPlan = treatmentPlan,
                TreatmentPlanId = 1
            };

            var appointment = new Appointment
            {
                AppointmentId = 1,
                StartTime = DateTime.Now,
                AppointmentType = AppointmentType.SESSION,
                PatientId = patient.PatientId,
                EmployeeId = employee.EmployeeId,
                SessionId = 1
            };
            var operation = new Operation
            {
                Value = "1004",
                Description =
                    "Individuele zitting reguliere fysiotherapie met toeslag voor behandeling op de werkplek (eenmalig)",
                Additional = "nee"
            };
            var session = new Session
            {
                SessionDate = DateTime.Now,
                Type = "1004",
                RoomType = RoomType.PRACTICE_ROOM,
                Patient = patient,
                PatientId = patient.PatientId,
                SessionEmployee = employee,
                SessionEmployeeId = employee.EmployeeId,
                Dossier = dossier,
                DossierId = dossier.DossierId,
                Appointment = appointment
            };

            var sessionRepo = new Mock<ISessionRepository>();
            var appointmentRepo = new Mock<IAppointmentRepository>();
            var userRepo = new Mock<IUserRepository>();
            var dossierRepo = new Mock<IDossierRepository>();
            var operationMock = new Mock<IStamApiService>();
            dossierRepo.Setup(x => x.GetDossierByPatientId(patient.PatientId)).ReturnsAsync(dossier);
            operationMock.Setup(x => x.GetOperation("1004")).ReturnsAsync(operation);
            appointmentRepo.Setup(x => x.GetAppointment(appointment.AppointmentId)).ReturnsAsync(appointment);
            userRepo.Setup(x => x.GetUser(session.PatientId)).ReturnsAsync(user);
            var service = new SessionService(sessionRepo.Object, appointmentRepo.Object, dossierRepo.Object,
                userRepo.Object, operationMock.Object);
            //Act
            var result = service.AddSession(session);
            //Assert
            Assert.True(result.Result.Success);
        }
    }
}