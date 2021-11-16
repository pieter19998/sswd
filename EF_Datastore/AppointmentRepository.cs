using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using DomainServices;
using Microsoft.EntityFrameworkCore;

namespace EF_Datastore
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly PracticeDbContext _context;

        public AppointmentRepository(PracticeDbContext context)
        {
            _context = context;
        }

        public async Task<Appointment> GetAppointment(int appointmentId)
        {
            return await _context.Appointments.FindAsync(appointmentId);
        }

        public async Task<ICollection<Appointment>> GetAppointments()
        {
            return await _context.Appointments.Include(x => x.Patient).Include(x => x.EfEmployee).ToListAsync();
        }

        public async Task<ICollection<Appointment>> GetAppointmentsByPatient(int patientId)
        {
            return await _context.Appointments.Where(x => x.PatientId == patientId).Include(x => x.Patient)
                .Include(x => x.EfEmployee).Include(x => x.Intake).Include(x => x.Session)
                .ToListAsync();
        }

        public async Task<ICollection<Appointment>> GetAppointmentsByPatientThisWeek(int patientId)
        {
            return await _context.Appointments
                .Where(x => x.PatientId == patientId && DateTime.Today > DateTime.Today.AddDays(-7))
                .Include(x => x.Patient).ToListAsync();
        }

        public async Task<ICollection<Appointment>> GetAppointmentsByEmployee(int employeeId)
        {
            return await _context.Appointments.Where(x => x.EmployeeId == employeeId).OrderBy(x => x.StartTime)
                .Include(x => x.Patient).Include(x => x.EfEmployee).Include(x => x.Intake).Include(x => x.Session)
                .ToListAsync();
        }

        public async Task CreateAppointment(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAppointment(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
        }

        public async Task CancelAppointment(int appointmentId)
        {
            _context.Appointments.FindAsync(appointmentId).Result.Cancelled = true;
            await _context.SaveChangesAsync();
        }
    }
}