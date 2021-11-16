using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using DomainServices;
using Microsoft.EntityFrameworkCore;

namespace EF_Datastore
{
    public class AvailabilityRepository : IAvailabilityRepository
    {
        private readonly PracticeDbContext _context;

        public AvailabilityRepository(PracticeDbContext context)
        {
            _context = context;
        }

        public async Task<ICollection<Availability>> GetAvailabilityEmployee(int employeeId)
        {
            return await _context.Availabilities.Where(x => x.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<ICollection<Availability>> GetAvailabilities()
        {
            return await _context.Availabilities.Where(x => DateTime.Now < x.AvailableTo).ToListAsync();
        }

        public async Task<Availability> GetAvailability(int id)
        {
            return await _context.Availabilities.Where(x => x.AvailabilityId == id).FirstOrDefaultAsync();
        }


        public async Task AddAvailability(Availability availability)
        {
            _context.Availabilities.Add(availability);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAvailability(Availability availability)
        {
            _context.Availabilities.Update(availability);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAvailability(int id)
        {
            _context.Availabilities.Remove(await _context.Availabilities.FindAsync(id));
            await _context.SaveChangesAsync();
        }
    }
}