using System;
using System.Linq;
using System.Threading.Tasks;
using Core;
using DomainServices;

namespace ApplicationServices
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        public async Task<IResult<Patient>> AddPatient(Patient patient)
        {
            var result = IsValid(patient);
            //check age
            if (!result.Success) return result;
            //check if numbers
            if (patient.Type == PatientType.EMPLOYEE)
            {
                patient.StudentNumber = null;
                if (string.IsNullOrWhiteSpace(patient.PersonalNumber))
                {
                    result.Message = ErrorMessages.PersonalNumberError;
                    result.Success = false;
                    return result;
                }
            }
            else
            {
                patient.PersonalNumber = null;
                if (string.IsNullOrWhiteSpace(patient.StudentNumber))
                {
                    result.Message = ErrorMessages.StudentNumberError;
                    result.Success = false;
                    return result;
                }
            }
            
            if (DateTime.Today.Year - patient.DayOfBirth.Year < 16)
            {
                result.Message = "patient must be at least 16 years old";
                result.Success = false;
                return result;
            }
            try
            {
                patient.Role = Role.PATIENT;
                patient.PatientNumber = GeneratePatientNumber();
                await _patientRepository.RegisterPatient(patient);
            }
            catch (Exception e)
            {
                result.Message = "Duplicate email. Patient with this Email already exists";
                result.Success = false;
                return result;
            }

            return result;
        }

        public async Task<IResult<Patient>> UpdatePatientAddress(int patientId, string address)
        {
            var result = new Result<Patient>();
            try
            {
                await _patientRepository.UpdateAddress(address, patientId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result.Message = e.Message;
                result.Success = false;
            }

            return result;
        }

        public async Task<IResult<Patient>> UpdatePatient(Patient patient, int patientId)
        {
            var result = IsValid(patient);
            try
            {
                await _patientRepository.UpdatePatient(patient, patientId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result.Message = e.Message;
                result.Success = false;
                throw;
            }

            return result;
        }

        public async Task<IResult<Patient>> GetPatient(int patientId)
        {
            IResult<Patient> result = new Result<Patient>();
            result.Payload = await _patientRepository.GetPatient(patientId);
            if (result.Payload == null)
            {
                result.Message = ErrorMessages.IdNotFound;
                result.Success = false;
            }

            return result;
        }

        private string GeneratePatientNumber()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private bool CheckAge(Patient patient)
        {
            return DateTime.Today.Year - patient.DayOfBirth.Year >= 16 &&
                   DateTime.Today > patient.DayOfBirth.Date;
        }

        private IResult<Patient> IsValid(Patient patient)
        {
            IResult<Patient> result = new Result<Patient>();
            if (string.IsNullOrWhiteSpace(patient.Firstname)) result.Message += ErrorMessages.FirstNameError;
            if (string.IsNullOrWhiteSpace(patient.Lastname)) result.Message += ErrorMessages.LastNameError;
            if (!CheckAge(patient)) result.Message += ErrorMessages.DayOfBirthError;
            if (string.IsNullOrWhiteSpace(patient.Address)) result.Message += ErrorMessages.AddressError;
            return result;
        }
    }
}