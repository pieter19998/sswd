using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using DomainServices;
using Microsoft.EntityFrameworkCore;

namespace EF_Datastore
{
    public class DossierRepository : IDossierRepository
    {
        private readonly PracticeDbContext _context;

        public DossierRepository(PracticeDbContext context)
        {
            _context = context;
        }

        public async Task AddDossier(Dossier dossier)
        {
            await _context.AddAsync(dossier);
            await _context.SaveChangesAsync();
        }

        public async Task AddTreatmentPlan(TreatmentPlan treatment, Dossier dossier)
        {
            dossier.TreatmentPlan = treatment;
            await _context.SaveChangesAsync();
        }

        public async Task AddSession(Session session, Dossier dossier)
        {
            if (dossier.Sessions != null) dossier.Sessions.Add(session);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDossier(Dossier dossier, int id)
        {
            var dossierExist = await GetDossier(id);
            dossierExist.Age = dossier.Age;
            dossierExist.Description = dossier.Description;
            dossierExist.ApplicationDay = dossier.ApplicationDay;
            dossierExist.DiagnoseCode = dossier.DiagnoseCode;
            dossierExist.DiagnoseDescription = dossier.DiagnoseDescription;
            dossierExist.DismissalDay = dossier.DismissalDay;
            dossierExist.PatientId = dossier.PatientId;
            dossierExist.HeadPractitionerId = dossier.HeadPractitionerId;
            dossierExist.TreatmentPlanId = dossier.TreatmentPlanId;
            await _context.SaveChangesAsync();
        }

        public async Task<Dossier> GetDossier(int dossierId)
        {
            return await _context.Dossiers.Include(d => d.Notices)
                .Include(t => t.TreatmentPlan)
                .Include(p => p.Patient)
                .Include(x => x.HeadPractitioner)
                .Include(x => x.Sessions).ThenInclude<Dossier, Session, Employee>(y => y.SessionEmployee)
                .FirstOrDefaultAsync(d => d.DossierId == dossierId && d.Active);
        }

        //get dossier with notices visible for patient 
        public async Task<Dossier> GetDossierPatient(int dossierId)
        {
            return await _context.Dossiers.Include(d => d.Notices.Where(n => n.Visible)).Include(t => t.TreatmentPlan)
                .Include(p => p.Patient).Include(x => x.HeadPractitioner)
                .FirstOrDefaultAsync(d => d.DossierId == dossierId && d.Active);
        }

        //get dossier by efPatientId
        public async Task<Dossier> GetDossierByPatientId(int patientId)
        {
            return await _context.Dossiers.Include(d => d.Notices.Where(n => n.Visible)).Include(t => t.TreatmentPlan)
                .Include(x => x.Patient)
                .Include(x => x.HeadPractitioner)
                .Include(x => x.Sessions).ThenInclude<Dossier, Session, Employee>(y => y.SessionEmployee)
                .FirstOrDefaultAsync(d => d.PatientId == patientId && d.Active);
        }

        public async Task<ICollection<Dossier>> GetDossiers()
        {
            return await _context.Dossiers.Include(x => x.TreatmentPlan).Include(x => x.Sessions)
                .Include(x => x.Notices)
                .Include(x => x.Patient)
                .Include(x => x.Sessions)
                .Where(d => d.Active).ToListAsync();
        }

        public async Task<ICollection<Dossier>> GetDossiersWithTreatmentPlan()
        {
            return await _context.Dossiers.Include(d => d.TreatmentPlan).Where(d => d.Active).ToListAsync();
        }

        public async Task PatchNoteId(Notes note, int dossierId)
        {
            var dossier = await GetDossier(dossierId);
            dossier.Notices.Add(note);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDossier(int dossierId)
        {
            _context.Dossiers.FindAsync(dossierId).Result.Active = false;
            await _context.SaveChangesAsync();
        }
    }
}