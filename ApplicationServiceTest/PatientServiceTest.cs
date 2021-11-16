using System;
using System.Collections.Generic;
using ApplicationServices;
using Core;
using DomainServices;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace ApplicationServiceTest
{
    public class PatientServiceTest
    {
        public static Mock<UserManager<TUser>> MockUserManager<TUser>(List<TUser> ls) where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            var mgr = new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<TUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<TUser>());

            mgr.Setup(x => x.DeleteAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.CreateAsync(It.IsAny<TUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success)
                .Callback<TUser, string>((x, y) => ls.Add(x));
            mgr.Setup(x => x.UpdateAsync(It.IsAny<TUser>())).ReturnsAsync(IdentityResult.Success);
            mgr.Setup(x => x.AddToRoleAsync(It.IsAny<TUser>(), "PATIENT")).ReturnsAsync(IdentityResult.Success);

            return mgr;
        }

        [Fact]
        public void Should_Return_One_Avans_Employee_Patient()
        {
            //Arrange
            var patient = new Patient
            {
                PatientId = 1,
                Firstname = "Jan",
                Lastname = "Steen",
                Role = Role.PATIENT,
                Address = "Lovensdijkstraat 61 - 63, 4818 AJ Breda",
                Type = PatientType.STUDENT,
                StudentNumber = "1",
                Sex = Sex.MALE,
                Photo = "-",
                DayOfBirth = new DateTime(1990, 08, 12),
                PersonalNumber = "1234"
            };
            var patientMock = new Mock<IPatientRepository>();
            var intakeMock = new Mock<IIntakeRepository>();
            var userMock = new Mock<IUserRepository>();
            var dossierMock = new Mock<IDossierRepository>();
            var patientService = new PatientService(patientMock.Object);
            patientMock.Setup(x => x.GetPatient(1)).ReturnsAsync(patient);

            //Act
            var result = patientService.GetPatient(1);

            //Assert
            Assert.NotNull(result.Result.Payload);
            Assert.True(result.Result.Success);
        }

        [Fact]
        public void Should_Return_Zero_Avans_Employee_Patient()
        {
            //Arrange
            var patient = new Patient
            {
                PatientId = 1,
                Firstname = "Jan",
                Lastname = "Steen",
                Role = Role.PATIENT,
                Address = "Lovensdijkstraat 61 - 63, 4818 AJ Breda",
                Type = PatientType.STUDENT,
                StudentNumber = "1",
                Sex = Sex.MALE,
                Photo = "-",
                DayOfBirth = new DateTime(1990, 08, 12),
                PersonalNumber = "1234"
            };
            var patientMock = new Mock<IPatientRepository>();
            var intakeMock = new Mock<IIntakeRepository>();
            var userMock = new Mock<IUserRepository>();
            var dossierMock = new Mock<IDossierRepository>();
            var patientService = new PatientService(patientMock.Object);
            patientMock.Setup(x => x.GetPatient(1)).ReturnsAsync(patient);


            //Act
            var result = patientService.GetPatient(99);

            //Assert
            Assert.Null(result.Result.Payload);
            Assert.False(result.Result.Success);
        }

        [Fact]
        public void Should_Create_One_Avans_Employee_Patient_Valid_Birthday()
        {
            var intake = new Intake
            {
                IntakeId = 1,
                Email = "test@mail.com",
                IntakeById = 1,
                IntakeSuperVisor = "Jan",
                Date = DateTime.Now,
                Appointment = null
            };
            //Arrange
            var patient = new Patient
            {
                PatientId = 1,
                Firstname = "Jan",
                Lastname = "Steen",
                Role = Role.PATIENT,
                Address = "Lovensdijkstraat 61 - 63, 4818 AJ Breda",
                Type = PatientType.STUDENT,
                StudentNumber = "1",
                Sex = Sex.MALE,
                Photo = "-",
                DayOfBirth = new DateTime(1990, 08, 12),
                PersonalNumber = "1234"
            };
            var patientMock = new Mock<IPatientRepository>();
            var userMock = new Mock<IUserRepository>();
            var intakeMock = new Mock<IIntakeRepository>();
            var dossierMock = new Mock<IDossierRepository>();
            intakeMock.Setup(x => x.GetIntake(patient.Email)).ReturnsAsync(intake);
            var patientService = new PatientService(patientMock.Object);
            //Act
            var result = patientService.AddPatient(patient);
            //Assert
            Assert.True(result.Result.Success);
        }

        [Fact]
        public void Should_Create_One_Avans_Employee_Patient_InValid_Birthday()
        {
            var intake = new Intake
            {
                IntakeId = 1,
                Email = "test@mail.com",
                IntakeById = 1,
                IntakeSuperVisor = "Jan",
                Date = DateTime.Now,
                Appointment = null
            };
            //Arrange
            var patient = new Patient
            {
                PatientId = 1,
                Firstname = "Jan",
                Lastname = "Steen",
                Role = Role.PATIENT,
                Address = "Lovensdijkstraat 61 - 63, 4818 AJ Breda",
                Type = PatientType.STUDENT,
                StudentNumber = "1",
                Sex = Sex.MALE,
                Photo = "-",
                DayOfBirth = new DateTime(2021, 08, 12),
                PersonalNumber = "1234"
            };
            var user = new User
            {
                UserId = 1,
                PasswordHash = "231321",
                Email = "test@mail.com"
            };
            var patientMock = new Mock<IPatientRepository>();
            var userMock = new Mock<IUserRepository>();
            var intakeMock = new Mock<IIntakeRepository>();
            var userManagerMock = MockUserManager<User>(null).Object;
            intakeMock.Setup(x => x.GetIntake(user.Email)).ReturnsAsync(intake);
            patientMock.Setup(x => x.GetPatientByEmail(patient.Email)).ReturnsAsync(patient);
            var userService = new UserService(userMock.Object, patientMock.Object, intakeMock.Object, userManagerMock);
            var patientService = new PatientService(patientMock.Object);
            //Act
            patientService.AddPatient(patient);
            var result = userService.RegisterUser(user);
            //Assert
            Assert.False(result.Result.Success);
        }

        [Fact]
        public void Patient_email_should_be_in_intake_invalid()
        {
            var intake = new Intake
            {
                IntakeId = 1,
                Email = "test@mail123.com",
                IntakeById = 1,
                IntakeSuperVisor = "Jan",
                Date = DateTime.Now,
                Appointment = null
            };
            //Arrange
            var patient = new Patient
            {
                PatientId = 1,
                Firstname = "Jan",
                Lastname = "Steen",
                Email = "test@mail.com",
                Address = "Lovensdijkstraat 61 - 63, 4818 AJ Breda",
                Type = PatientType.STUDENT,
                StudentNumber = "1",
                Sex = Sex.MALE,
                Photo = "-",
                DayOfBirth = new DateTime(1990, 08, 12),
                PersonalNumber = "1234"
            };
            var user = new User
            {
                UserId = 1,
                PasswordHash = "231321",
                Email = "test@mail.com"
            };
            var patientMock = new Mock<IPatientRepository>();
            var userMock = new Mock<IUserRepository>();
            var intakeMock = new Mock<IIntakeRepository>();
            var userManagerMock = MockUserManager<User>(null).Object;
            intakeMock.Setup(x => x.GetIntake(user.Email)).ReturnsAsync(intake);
            patientMock.Setup(x => x.GetPatientByEmail(patient.Email)).ReturnsAsync(patient);
            var userService = new UserService(userMock.Object, patientMock.Object, intakeMock.Object, userManagerMock);
            var patientService = new PatientService(patientMock.Object);
            //Act
            var result = userService.RegisterUser(user);

            //Assert
            Assert.False(result.Result.Success);
        }

        [Fact]
        public void Patient_email_should_be_in_intake_valid()
        {
            var intake = new Intake
            {
                IntakeId = 1,
                Email = "test@mail.com",
                IntakeById = 1,
                IntakeSuperVisor = "Jan",
                Date = DateTime.Now,
                Appointment = null
            };
            var user = new User
            {
                UserId = 1,
                PasswordHash = "231321",
                Email = "test@mail.com"
            };
            //Arrange
            var patient = new Patient
            {
                Firstname = "Jan",
                Lastname = "Steen",
                Email = "test@mail.com",
                Address = "Lovensdijkstraat 61 - 63, 4818 AJ Breda",
                Sex = Sex.MALE,
                Type = PatientType.STUDENT,
                StudentNumber = "1",
                Photo = "-",
                DayOfBirth = new DateTime(1990, 08, 12),
                PersonalNumber = "1234"
            };


            var patientMock = new Mock<IPatientRepository>();
            var userMock = new Mock<IUserRepository>();
            var intakeMock = new Mock<IIntakeRepository>();
            var userManagerMock = MockUserManager<User>(null).Object;
            intakeMock.Setup(x => x.GetIntake(intake.Email)).ReturnsAsync(intake);
            patientMock.Setup(x => x.GetPatientByEmail(intake.Email)).ReturnsAsync(patient);
            var userService = new UserService(userMock.Object, patientMock.Object, intakeMock.Object, userManagerMock);
            var patientService = new PatientService(patientMock.Object);
            //Act
            patientService.AddPatient(patient);
            var result = userService.RegisterUser(user);
            //Assert
            Assert.True(result.Result.Success);
        }

        [Fact]
        public void Patient_type_student_requires_student_number()
        {
            //Arrange
            var patient = new Patient
            {
                Firstname = "Jan",
                Lastname = "Steen",
                Email = "test@mail.com",
                Address = "Lovensdijkstraat 61 - 63, 4818 AJ Breda",
                Sex = Sex.MALE,
                Type = PatientType.STUDENT,
                Photo = "-",
                DayOfBirth = new DateTime(1990, 08, 12),
                PersonalNumber = "1234"
            };

            var patientMock = new Mock<IPatientRepository>();
            var userMock = new Mock<IUserRepository>();
            var intakeMock = new Mock<IIntakeRepository>();
            var dossierMock = new Mock<IDossierRepository>();
            var patientService = new PatientService(patientMock.Object);
            //Act
            var result = patientService.AddPatient(patient);
            //Assert
            Assert.False(result.Result.Success);
        }

        public void Patient_type_employee_requires_employee_number()
        {
            //Arrange
            var patient = new Patient
            {
                Firstname = "Jan",
                Lastname = "Steen",
                Email = "test@mail.com",
                Address = "Lovensdijkstraat 61 - 63, 4818 AJ Breda",
                Sex = Sex.MALE,
                Type = PatientType.EMPLOYEE,
                Photo = "-",
                DayOfBirth = new DateTime(1990, 08, 12),
                PersonalNumber = "1234"
            };

            var patientMock = new Mock<IPatientRepository>();
            var userMock = new Mock<IUserRepository>();
            var intakeMock = new Mock<IIntakeRepository>();
            var dossierMock = new Mock<IDossierRepository>();
            var patientService = new PatientService(patientMock.Object);
            //Act
            var result = patientService.AddPatient(patient);
            //Assert
            Assert.False(result.Result.Success);
        }
    }
}