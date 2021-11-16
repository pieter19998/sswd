using System;
using System.Threading.Tasks;
using Core;
using DomainServices;

namespace ApplicationServices
{
    public class DossierService : IDossierServices
    {
        private readonly IDossierRepository _dossierRepository;
        private readonly IOperationRepository _operationRepository;
        private readonly IPatientRepository _patientRepository;

        public DossierService(IDossierRepository dossierRepository, IPatientRepository patientRepository,
            IOperationRepository operationRepository)
        {
            _dossierRepository = dossierRepository;
            _patientRepository = patientRepository;
            _operationRepository = operationRepository;
        }

        public async Task<IResult<Dossier>> AddDossier(Dossier dossier)
        {
            var result = IsValid(dossier);
            try
            {
                //set patient age inside dossier
                var patient = await _patientRepository.GetPatient(dossier.PatientId);
                if (patient == null)
                {
                    result.Message = ErrorMessages.PatientError;
                    result.Success = false;
                    return result;
                }

                dossier.Age = (uint) (DateTime.Today.Year - patient.DayOfBirth.Year);
                //get diagnose description
                if (string.IsNullOrEmpty(dossier.DiagnoseDescription))
                {
                    var operation = await _operationRepository.GetOperation(dossier.DiagnoseCode);
                    if (operation == null)
                    {
                        result.Message = "operation with this code not found";
                        result.Success = false;
                        return result;
                    }

                    dossier.DiagnoseDescription = operation.Description;
                }

                dossier.ApplicationDay = DateTime.Now;
                await _dossierRepository.AddDossier(dossier);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result.Message = e.Message;
                result.Success = false;
            }

            return result;
        }

        public async Task<IResult<Dossier>> UpdateDossier(Dossier dossier, int id)
        {
            var result = IsValid(dossier);
            try
            {
                //set patient age inside dossier
                var patient = await _patientRepository.GetPatient(dossier.PatientId);
                if (patient == null)
                {
                    result.Message = ErrorMessages.PatientError;
                    result.Success = false;
                    return result;
                }

                //get diagnosedescription
                if (string.IsNullOrEmpty(dossier.DiagnoseDescription))
                {
                    var operation = await _operationRepository.GetOperation(dossier.DiagnoseCode);
                    if (operation == null)
                    {
                        result.Message = "operation with this code not found";
                        result.Success = false;
                        return result;
                    }

                    dossier.DiagnoseDescription = operation.Description;
                }

                dossier.ApplicationDay = DateTime.Now;
                await _dossierRepository.UpdateDossier(dossier, id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result.Message = e.Message;
                result.Success = false;
            }

            return result;
        }

        public async Task<IResult<Dossier>> GetDossier(int dossierId)
        {
            IResult<Dossier> result = new Result<Dossier>();
            result.Payload = await _dossierRepository.GetDossier(dossierId);
            if (result.Payload == null)
            {
                result.Message = ErrorMessages.IdNotFound;
                result.Success = false;
            }

            return result;
        }

        public async Task<IResult<Dossier>> GetDossierPatient(int dossierId)
        {
            IResult<Dossier> result = new Result<Dossier>();
            result.Payload = await _dossierRepository.GetDossierPatient(dossierId);
            if (result.Payload == null)
            {
                result.Message = ErrorMessages.IdNotFound;
                result.Success = false;
            }

            return result;
        }

        private IResult<Dossier> IsValid(Dossier dossier)
        {
            IResult<Dossier> result = new Result<Dossier>();
            if (string.IsNullOrWhiteSpace(dossier.Description)) result.Message += ErrorMessages.DescriptionError;
            if (string.IsNullOrWhiteSpace(dossier.DiagnoseCode)) result.Message += ErrorMessages.DiagnoseCodeError;
            if (string.IsNullOrWhiteSpace(dossier.DiagnoseDescription)) result.Message += ErrorMessages.DiagnoseDescriptionError;
            if (result.Message.Length > 1) result.Success = false;
            return result;
        }
    }
}