using System.Collections.Generic;
using System.Threading.Tasks;
using Core;

namespace DomainServices
{
    public interface IDossierRepository
    {
        Task<Dossier> GetDossier(int dossierId);
        Task<ICollection<Dossier>> GetDossiers();
        Task<ICollection<Dossier>> GetDossiersWithTreatmentPlan();
        Task<Dossier> GetDossierPatient(int dossierId);
        Task<Dossier> GetDossierByPatientId(int patientId);
        Task AddDossier(Dossier dossier);
        Task UpdateDossier(Dossier dossier, int id);
        Task AddTreatmentPlan(TreatmentPlan treatment, Dossier dossier);
        Task AddSession(Session session, Dossier dossier);
        Task PatchNoteId(Notes note, int noteId);
    }
}