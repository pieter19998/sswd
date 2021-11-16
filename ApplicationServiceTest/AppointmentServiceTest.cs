using System;
using System.Collections.Generic;
using ApplicationServices;
using Core;
using DomainServices;
using Moq;
using Xunit;

namespace ApplicationServiceTest
{
    public class AppointmentServiceTest
    {
        [Fact]
        public void Appointment_can_be_made_by_patient_if_all_data_is_valid()
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
                Patient = patient,
                PatientId = patient.PatientId,
                TreatmentPlan = treatmentPlan,
                TreatmentPlanId = 1
            };
            var availableTimes = new List<Availability>();
            availableTimes.Add(new Availability
            {
                AvailabilityId = 1,
                AvailableFrom = DateTime.Now.AddHours(-1),
                AvailableTo = DateTime.Now.AddHours(1),
                EmployeeId = employee.EmployeeId,
                Employee = employee
            });

            var appointment = new Appointment
            {
                AppointmentId = 1,
                StartTime = DateTime.Now,
                AppointmentType = AppointmentType.SESSION,
                PatientId = patient.PatientId,
                EmployeeId = employee.EmployeeId,
                SessionId = 1
            };
            IList<Appointment> appointments = new List<Appointment>();
            //Arrange
            var dossierRepo = new Mock<IDossierRepository>();
            dossierRepo.Setup(x => x.GetDossierByPatientId(patient.PatientId)).ReturnsAsync(dossier);
            var appointmentRepo = new Mock<IAppointmentRepository>();
            appointmentRepo.Setup(x => x.GetAppointmentsByPatientThisWeek(patient.PatientId))
                .ReturnsAsync(appointments);
            appointmentRepo.Setup(x => x.GetAppointmentsByEmployee(employee.EmployeeId)).ReturnsAsync(appointments);
            appointmentRepo.Setup(x => x.GetAppointment(1)).ReturnsAsync(appointment);

            var availableRepo = new Mock<IAvailabilityRepository>();
            availableRepo.Setup(x => x.GetAvailabilityEmployee(1)).ReturnsAsync(availableTimes);

            var appointmentService =
                new AppointmentService(appointmentRepo.Object, dossierRepo.Object, availableRepo.Object);
            //Act
            var result = appointmentService.ClaimAvailableAppointment(appointment);
            //Assert
            Assert.True(result.Result.Success);
        }

        [Fact]
        public void Appointment_cant_be_made_by_patient_if_session_amount_is_exceeded()
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
                Patient = patient,
                PatientId = patient.PatientId,
                TreatmentPlan = treatmentPlan,
                TreatmentPlanId = 1
            };
            var availableTimes = new List<Availability>();
            for (var i = 0; i < 10; i++)
                availableTimes.Add(new Availability
                {
                    AvailabilityId = i,
                    AvailableFrom = DateTime.Now.AddHours(i),
                    AvailableTo = DateTime.Now.AddHours(i + 1),
                    EmployeeId = employee.EmployeeId,
                    Employee = employee
                });

            var appointment = new Appointment
            {
                AppointmentId = 1,
                StartTime = DateTime.Now.AddHours(2),
                AppointmentType = AppointmentType.SESSION,
                PatientId = patient.PatientId,
                EmployeeId = employee.EmployeeId
            };
            var appointment2 = new Appointment
            {
                AppointmentId = 2,
                StartTime = DateTime.Now.AddHours(4),
                AppointmentType = AppointmentType.SESSION,
                PatientId = patient.PatientId,
                EmployeeId = employee.EmployeeId
            };

            IList<Appointment> appointments = new List<Appointment>();
            appointments.Add(appointment2);

            //Arrange
            var dossierRepo = new Mock<IDossierRepository>();
            dossierRepo.Setup(x => x.GetDossierByPatientId(patient.PatientId)).ReturnsAsync(dossier);
            var appointmentRepo = new Mock<IAppointmentRepository>();
            appointmentRepo.Setup(x => x.GetAppointmentsByPatientThisWeek(patient.PatientId))
                .ReturnsAsync(appointments);
            appointmentRepo.Setup(x => x.GetAppointmentsByEmployee(employee.EmployeeId)).ReturnsAsync(appointments);
            appointmentRepo.Setup(x => x.GetAppointment(1)).ReturnsAsync(appointment);

            var availableRepo = new Mock<IAvailabilityRepository>();
            availableRepo.Setup(x => x.GetAvailabilityEmployee(1)).ReturnsAsync(availableTimes);

            var appointmentService =
                new AppointmentService(appointmentRepo.Object, dossierRepo.Object, availableRepo.Object);
            //Act
            var result = appointmentService.ClaimAvailableAppointment(appointment);
            //Assert
            Assert.False(result.Result.Success);
        }

        [Fact]
        public void Appointment_cant_be_made_by_patient_if_headpractitioner_availibility_is_not_available()
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
                Patient = patient,
                PatientId = patient.PatientId,
                TreatmentPlan = treatmentPlan,
                TreatmentPlanId = 1
            };
            var availableTimes = new List<Availability>();
            availableTimes.Add(new Availability
            {
                AvailabilityId = 1,
                AvailableFrom = DateTime.Now,
                AvailableTo = DateTime.Now.AddHours(1),
                EmployeeId = employee.EmployeeId,
                Employee = employee
            });
            var appointment = new Appointment
            {
                AppointmentId = 1,
                StartTime = DateTime.Now.AddDays(10),
                AppointmentType = AppointmentType.SESSION,
                PatientId = patient.PatientId,
                EmployeeId = employee.EmployeeId
            };
            IList<Appointment> appointments = new List<Appointment>();
            //Arrange
            var dossierRepo = new Mock<IDossierRepository>();
            dossierRepo.Setup(x => x.GetDossierByPatientId(patient.PatientId)).ReturnsAsync(dossier);
            var appointmentRepo = new Mock<IAppointmentRepository>();
            appointmentRepo.Setup(x => x.GetAppointmentsByPatientThisWeek(patient.PatientId))
                .ReturnsAsync(appointments);
            appointmentRepo.Setup(x => x.GetAppointmentsByEmployee(employee.EmployeeId)).ReturnsAsync(appointments);
            appointmentRepo.Setup(x => x.GetAppointment(1)).ReturnsAsync(appointment);

            var availableRepo = new Mock<IAvailabilityRepository>();
            availableRepo.Setup(x => x.GetAvailabilityEmployee(1)).ReturnsAsync(availableTimes);

            var appointmentService =
                new AppointmentService(appointmentRepo.Object, dossierRepo.Object, availableRepo.Object);
            //Act
            var result = appointmentService.ClaimAvailableAppointment(appointment);
            //Assert
            Assert.False(result.Result.Success);
        }


        [Fact]
        public void Appointment_can_be_cancelled()
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
                Patient = patient,
                PatientId = patient.PatientId,
                TreatmentPlan = treatmentPlan,
                TreatmentPlanId = 1
            };
            var availableTimes = new List<Availability>();
            availableTimes.Add(new Availability
            {
                AvailabilityId = 1,
                AvailableFrom = DateTime.Now.AddHours(-1),
                AvailableTo = DateTime.Now.AddDays(7),
                EmployeeId = employee.EmployeeId,
                Employee = employee
            });

            var appointment = new Appointment
            {
                AppointmentId = 1,
                StartTime = DateTime.Now.AddDays(2),
                AppointmentType = AppointmentType.SESSION,
                PatientId = patient.PatientId,
                EmployeeId = employee.EmployeeId,
                SessionId = 1
            };
            IList<Appointment> appointments = new List<Appointment>();
            appointments.Add(appointment);
            //Arrange
            var dossierRepo = new Mock<IDossierRepository>();
            dossierRepo.Setup(x => x.GetDossierByPatientId(patient.PatientId)).ReturnsAsync(dossier);
            var appointmentRepo = new Mock<IAppointmentRepository>();
            appointmentRepo.Setup(x => x.GetAppointmentsByPatientThisWeek(patient.PatientId))
                .ReturnsAsync(appointments);
            appointmentRepo.Setup(x => x.GetAppointmentsByEmployee(employee.EmployeeId)).ReturnsAsync(appointments);
            appointmentRepo.Setup(x => x.GetAppointment(1)).ReturnsAsync(appointment);

            var availableRepo = new Mock<IAvailabilityRepository>();
            availableRepo.Setup(x => x.GetAvailabilityEmployee(1)).ReturnsAsync(availableTimes);

            var appointmentService =
                new AppointmentService(appointmentRepo.Object, dossierRepo.Object, availableRepo.Object);
            //Act
            var result = appointmentService.CancelAppointment(appointment.AppointmentId);
            //Assert
            Assert.True(result.Result.Success);
        }

        [Fact]
        public void Appointment_cant_be_cancelled()
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
                Patient = patient,
                PatientId = patient.PatientId,
                TreatmentPlan = treatmentPlan,
                TreatmentPlanId = 1
            };
            var availableTimes = new List<Availability>();
            availableTimes.Add(new Availability
            {
                AvailabilityId = 1,
                AvailableFrom = DateTime.Now.AddHours(-1),
                AvailableTo = DateTime.Now.AddDays(7),
                EmployeeId = employee.EmployeeId,
                Employee = employee
            });

            var appointment = new Appointment
            {
                AppointmentId = 1,
                StartTime = DateTime.Now.AddHours(2),
                AppointmentType = AppointmentType.SESSION,
                PatientId = patient.PatientId,
                EmployeeId = employee.EmployeeId,
                SessionId = 1
            };
            IList<Appointment> appointments = new List<Appointment>();
            appointments.Add(appointment);
            //Arrange
            var dossierRepo = new Mock<IDossierRepository>();
            dossierRepo.Setup(x => x.GetDossierByPatientId(patient.PatientId)).ReturnsAsync(dossier);
            var appointmentRepo = new Mock<IAppointmentRepository>();
            appointmentRepo.Setup(x => x.GetAppointmentsByPatientThisWeek(patient.PatientId))
                .ReturnsAsync(appointments);
            appointmentRepo.Setup(x => x.GetAppointmentsByEmployee(employee.EmployeeId)).ReturnsAsync(appointments);
            appointmentRepo.Setup(x => x.GetAppointment(1)).ReturnsAsync(appointment);

            var availableRepo = new Mock<IAvailabilityRepository>();
            availableRepo.Setup(x => x.GetAvailabilityEmployee(1)).ReturnsAsync(availableTimes);

            var appointmentService =
                new AppointmentService(appointmentRepo.Object, dossierRepo.Object, availableRepo.Object);
            //Act
            var result = appointmentService.CancelAppointment(appointment.AppointmentId);
            //Assert
            Assert.False(result.Result.Success);
        }
    }
}