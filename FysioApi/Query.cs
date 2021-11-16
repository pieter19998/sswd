using System.Linq;
using System.Threading.Tasks;
using Core;
using Core.stam;
using DomainServices;
using FysioApi.Services;
using HotChocolate;
using HotChocolate.Data;

namespace FysioApi
{
    public class Query
    {
        [UseProjection]
        [UseFiltering]
        public async Task<TreatmentPlan> GetTreatmentPlan([Service] ITreatmentPlanRepository treatment, int id)
        {
            return treatment.GetTreatmentPlan(id).Result;
        }

        public async Task<IQueryable<TreatmentPlan>> GetTreatmentPlans([Service] ITreatmentPlanRepository treatment)
        {
            return treatment.GetTreatmentPlans().Result.AsQueryable();
        }

        [UseProjection]
        [UseFiltering]
        public async Task<Appointment> GetAppointment([Service] IAppointmentRepository appointment, int id)
        {
            return appointment.GetAppointment(id).Result;
        }

        public async Task<IQueryable<Appointment>> GetAppointments([Service] IAppointmentRepository appointment)
        {
            return appointment.GetAppointments().Result.AsQueryable();
        }

        public async Task<IQueryable<Appointment>> GetAppointmentsByEmployee(
            [Service] IAppointmentRepository appointment, int id)
        {
            return appointment.GetAppointmentsByEmployee(id).Result.AsQueryable();
        }

        public async Task<IQueryable<Appointment>> GetAppointmentsByPatient(
            [Service] IAppointmentRepository appointment, int id)
        {
            return appointment.GetAppointmentsByPatient(id).Result.AsQueryable();
        }

        [UseProjection]
        [UseFiltering]
        public async Task<Dossier> GetDossier([Service] IDossierRepository dossier, int id)
        {
            return dossier.GetDossier(id).Result;
        }

        public async Task<IQueryable<Dossier>> GetDossiers([Service] IDossierRepository dossier)
        {
            return dossier.GetDossiers().Result.AsQueryable();
        }

        [UseProjection]
        [UseFiltering]
        public async Task<Dossier> GetDossierByPatient([Service] IDossierRepository dossier, int patientId)
        {
            return dossier.GetDossierByPatientId(patientId).Result;
        }

        [UseProjection]
        [UseFiltering]
        public async Task<Intake> GetIntake([Service] IIntakeRepository intake, int id)
        {
            return intake.GetIntake(id).Result;
        }

        public async Task<IQueryable<Intake>> GetIntakes([Service] IIntakeRepository intake)
        {
            return intake.GetIntakes().Result.AsQueryable();
        }

        public async Task<IQueryable<Notes>> GetNotices([Service] INoteRepository note)
        {
            return note.GetNotices().Result.AsQueryable();
        }

        [UseProjection]
        [UseFiltering]
        public async Task<Patient> GetPatient([Service] IPatientRepository patient, int id)
        {
            return patient.GetPatient(id).Result;
        }

        [UseProjection]
        [UseFiltering]
        public async Task<Patient> GetPatientByEmail([Service] IPatientRepository patient, string email)
        {
            return patient.GetPatientByEmail(email).Result;
        }

        public async Task<IQueryable<Patient>> GetPatients([Service] IPatientRepository patient)
        {
            return patient.GetPatients().Result.AsQueryable();
        }

        [UseProjection]
        [UseFiltering]
        public async Task<Session> GetSession([Service] ISessionRepository session, int id)
        {
            return await session.GetSession(id);
        }

        public async Task<IQueryable<Session>> GetSessions([Service] ISessionRepository session)
        {
            return session.GetSessions().Result.AsQueryable();
        }

        [UseProjection]
        [UseFiltering]
        public async Task<User> GetUser([Service] IUserRepository userRepository, int id)
        {
            return userRepository.GetUser(id).Result;
        }

        [UseProjection]
        [UseFiltering]
        public async Task<User> GetUserByEmail([Service] IUserRepository userRepository, string email)
        {
            return userRepository.GetUserByEmail(email).Result;
        }

        public async Task<IQueryable<User>> GetUsers([Service] IUserRepository user)
        {
            return user.GetUsers().Result.AsQueryable();
        }

        [UseProjection]
        [UseFiltering]
        public async Task<Employee> GetEmployee([Service] IEmployeeRepository employee, int id)
        {
            return employee.GetEmployee(id).Result;
        }

        public async Task<IQueryable<Employee>> GetEmployees([Service] IEmployeeRepository employee)
        {
            return employee.GetEmployees().Result.AsQueryable();
        }

        public async Task<IQueryable<Availability>> GetAvailabilities([Service] IAvailabilityRepository availability)
        {
            return availability.GetAvailabilities().Result.AsQueryable();
        }

        [UseProjection]
        [UseFiltering]
        public async Task<IQueryable<Availability>> GetAvailabilityByEmployee(
            [Service] IAvailabilityRepository availability, int id)
        {
            return availability.GetAvailabilityEmployee(id).Result.AsQueryable();
        }


        [UseProjection]
        [UseFiltering]
        public async Task<Availability> GetAvailability([Service] IAvailabilityRepository availabilityRepository,
            int id)
        {
            return availabilityRepository.GetAvailability(id).Result;
        }

        [UseProjection]
        [UseFiltering]
        public async Task<IQueryable<Operation>> GetOperations([Service] StamApiService operationRepository)
        {
            return operationRepository.GetOperations().Result.AsQueryable();
        }

        [UseProjection]
        [UseFiltering]
        public async Task<Operation> GetOperation([Service] StamApiService operationRepository, string value)
        {
            return operationRepository.GetOperation(value).Result;
        }

        [UseProjection]
        [UseFiltering]
        public async Task<IQueryable<Diagnose>> GetDiagnoses([Service] StamApiService diagnoseRepository)
        {
            return diagnoseRepository.GetDiagnoses().Result.AsQueryable();
        }

        [UseProjection]
        [UseFiltering]
        public async Task<Diagnose> GetDiagnose([Service] StamApiService diagnoseRepository, int id)
        {
            return diagnoseRepository.GetDiagnose(id).Result;
        }
    }
}