using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using DomainServices;
using Microsoft.EntityFrameworkCore;

namespace EF_Datastore
{
    public class IntakeRepository : IIntakeRepository
    {
        private readonly PracticeDbContext _context;

        public IntakeRepository(PracticeDbContext context)
        {
            _context = context;
        }

        public async Task<Intake> GetIntake(int intakeId)
        {
            return await _context.Intakes.FindAsync(intakeId);
        }

        public async Task<Intake> GetIntake(string email)
        {
            return await _context.Intakes.SingleOrDefaultAsync(x => x.Email == email);
        }

        public async Task<ICollection<Intake>> GetIntakes()
        {
            return await _context.Intakes.ToListAsync();
        }

        public async Task AddIntake(Intake intake)
        {
            await _context.AddAsync(intake);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateIntake(Intake intake, int id)
        {
            var intakeExist = await GetIntake(id);
            if (intakeExist != null)
            {
                intakeExist.Date = intake.Date;
                intakeExist.Email = intake.Email;
                intakeExist.IntakeById = intake.IntakeById;
                intakeExist.IntakeSuperVisor = intake.IntakeSuperVisor;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteIntake(int id)
        {
            var intake = await GetIntake(id);
            _context.Intakes.Remove(intake);
            await _context.SaveChangesAsync();
        }
    }
}