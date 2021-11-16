using System.Threading.Tasks;
using Core;

namespace ApplicationServices
{
    public interface IPatientService
    {
        Task<IResult<Patient>> AddPatient(Patient patient);
        Task<IResult<Patient>> UpdatePatientAddress(int patientId, string address);
        Task<IResult<Patient>> UpdatePatient(Patient patient, int patientId);
        Task<IResult<Patient>> GetPatient(int patientId);
    }
}