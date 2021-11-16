using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using DomainServices;
using EF_Datastore;
using Microsoft.EntityFrameworkCore;

namespace IF_Datastore
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PracticeDbContext _context;

        public PatientRepository(PracticeDbContext context)
        {
            _context = context;
        }


        public async Task<Patient> GetPatient(int patientId)
        {
            return await _context.Patients.SingleOrDefaultAsync(x => x.PatientId == patientId);
        }

        public async Task<Patient> GetPatientByEmail(string email)
        {
            return await _context.Patients.SingleOrDefaultAsync(x => x.Email == email);
        }

        public async Task UpdatePatient(Patient patient, int patientId)
        {
            var patientExist = await GetPatient(patientId);
            patientExist.Firstname = patient.Firstname;
            patientExist.Lastname = patient.Lastname;
            patientExist.Email = patient.Email;
            patientExist.Address = patient.Address;
            patientExist.Sex = patient.Sex;
            patientExist.PatientNumber = patient.PatientNumber;
            patientExist.Photo = patient.Photo;
            patientExist.DayOfBirth = patient.DayOfBirth;
            patientExist.PersonalNumber = patient.PersonalNumber;
            patientExist.StudentNumber = patient.StudentNumber;
            patientExist.Type = patient.Type;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAddress(string address, int patientId)
        {
            var patient = await GetPatient(patientId);
            if (patient != null)
            {
                patient.Address = address;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ICollection<Patient>> GetPatients()
        {
            return await _context.Patients.Where(p => p.Active).ToListAsync();
        }

        public async Task RegisterPatient(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
            await _context.SaveChangesAsync();
        }


        public async Task DeletePatient(int patientId)
        {
            _context.Patients.FindAsync(patientId).Result.Active = false;
            await _context.SaveChangesAsync();
        }
    }
}