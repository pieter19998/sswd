using System;
using System.Linq;
using System.Threading.Tasks;
using Core;
using DomainServices;

namespace ApplicationServices
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IAvailabilityRepository _availabilityRepository;
        private readonly IDossierRepository _dossierRepository;

        public AppointmentService(IAppointmentRepository appointmentRepository, IDossierRepository dossierRepository,
            IAvailabilityRepository availabilityRepository)
        {
            _appointmentRepository = appointmentRepository;
            _dossierRepository = dossierRepository;
            _availabilityRepository = availabilityRepository;
        }

        public async Task<IResult<Appointment>> GetAppointment(int appointmentId)
        {
            IResult<Appointment> result = new Result<Appointment>();
            result.Payload = await _appointmentRepository.GetAppointment(appointmentId);
            if (result.Payload == null)
            {
                result.Message = ErrorMessages.IdNotFound;
                result.Success = false;
            }

            return result;
        }

        //for user to claim appointment
        public async Task<IResult<Appointment>> ClaimAvailableAppointment(Appointment appointment)
        {
            var result = IsValid(appointment);


            //make sure appointment is of one type.
            if (appointment.AppointmentType == AppointmentType.INTAKE)
            {
                appointment.SessionId = null;
                appointment.PatientId = null;
                appointment.EndTime = appointment.StartTime.AddHours(1);
            }
            else
            {
                appointment.IntakeId = null;
                //check sessions this week
                if (await CheckAppointmentsPerWeek(appointment))
                {
                    result.Message = ErrorMessages.SessionPerWeek;
                    result.Success = false;
                    return result;
                }

                //set end time based on session time
                SetEndTime(appointment);
            }

            //check if headPractitioner is available
            if (!await HeadPractitionerAvailable(appointment))
            {
                result.Message = "head practitioner is not available at this timeslot";
                result.Success = false;
                return result;
            }

            //update appointment
            try
            {
                await _appointmentRepository.UpdateAppointment(appointment);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                result.Message = e.Message;
                result.Success = false;
            }

            return result;
        }

        public async Task<IResult<Appointment>> CancelAppointment(int appointmentId)
        {
            var result = await GetAppointment(appointmentId);
            if (result.Success)
            {
                //check if appointment is cancellable
                if (!AppointmentIsCancelable(result.Payload))
                {
                    result.Message = "appointment can't be cancelled less than 24 hours before appointment";
                    result.Success = false;
                    return result;
                }

                try
                {
                    await _appointmentRepository.CancelAppointment(appointmentId);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    result.Message = e.Message;
                    result.Success = false;
                }
            }

            return result;
        }

        private async Task<bool> CheckAppointmentsPerWeek(Appointment appointment)
        {
            if (appointment.AppointmentType == AppointmentType.INTAKE) return false;
            //get sessionPerWeek
            var dossier = await _dossierRepository.GetDossierByPatientId(appointment.PatientId.GetValueOrDefault());
            var sessionsPerWeek = dossier?.TreatmentPlan?.SessionsPerWeek;
            //get all appointment of this week
            var appointments =
                await _appointmentRepository.GetAppointmentsByPatientThisWeek(appointment.PatientId
                    .GetValueOrDefault());
            return appointments.Count >= sessionsPerWeek;
        }

        //set endTime based on session duration from treatmentPlan
        private async void SetEndTime(Appointment appointment)
        {
            var dossier = await _dossierRepository.GetDossierByPatientId(appointment.PatientId.GetValueOrDefault());

            if (dossier != null)
            {
                var endTime = appointment.StartTime + TimeSpan.FromMinutes(dossier.TreatmentPlan.SessionDuration);
                appointment.EndTime = endTime;
            }
        }

        private async Task<bool> HeadPractitionerAvailable(Appointment appointment)
        {
            if (appointment.StartTime > appointment.EndTime) return false;
            //check if there is a appointment at this time with this employee
            var appointments = await _appointmentRepository.GetAppointmentsByEmployee(appointment.EmployeeId);
            if (appointments == null) return false;
            if (appointments.Any(a => appointment.StartTime >= a.StartTime && appointment.StartTime <= a.EndTime))
                return false;
            //check if employee is available at this time from availability
            var available = await _availabilityRepository.GetAvailabilityEmployee(appointment.EmployeeId);
            if (available.Count == 0) return false;
            return available.Any(a =>
                appointment.StartTime >= a.AvailableFrom && appointment.EndTime <= a.AvailableTo);
        }

        //check if appointment is cancelable
        private bool AppointmentIsCancelable(Appointment appointment)
        {
            var time = appointment.StartTime - DateTime.Now;
            return time.TotalHours > 24;
        }

        private IResult<Appointment> IsValid(Appointment appointment)
        {
            IResult<Appointment> result = new Result<Appointment>();
            if (appointment.EmployeeId <= 0) result.Message += ErrorMessages.EmployeeError;
            return result;
        }
    }
}