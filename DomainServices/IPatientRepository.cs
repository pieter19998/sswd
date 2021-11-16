using System.Collections.Generic;
using System.Threading.Tasks;
using Core;

namespace DomainServices
{
    public interface IPatientRepository
    {
        Task<Patient> GetPatient(int patientId);
        Task<Patient> GetPatientByEmail(string email);
        Task UpdatePatient(Patient patient, int patientId);
        Task UpdateAddress(string address, int patientId);
        Task<ICollection<Patient>> GetPatients();
        Task RegisterPatient(Patient patient);
        Task DeletePatient(int patientId);
    }
}