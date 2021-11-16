using System.Threading.Tasks;
using Core;

namespace ApplicationServices
{
    public interface IDossierServices
    {
        Task<IResult<Dossier>> AddDossier(Dossier dossier);
        Task<IResult<Dossier>> UpdateDossier(Dossier dossier, int id);
        Task<IResult<Dossier>> GetDossier(int dossierId);
        Task<IResult<Dossier>> GetDossierPatient(int dossierId);
    }
}