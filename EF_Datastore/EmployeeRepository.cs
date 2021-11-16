using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using DomainServices;
using Microsoft.EntityFrameworkCore;

namespace EF_Datastore
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly PracticeDbContext _context;

        public EmployeeRepository(PracticeDbContext context)
        {
            _context = context;
        }

        public Task<Employee> GetEmployee(int employeeId)
        {
            return _context.Employees.SingleOrDefaultAsync(x => x.EmployeeId == employeeId && x.Active);
        }

        public async Task<ICollection<Employee>> GetEmployees()
        {
            return await _context.Employees.Where(x => x.Active).ToListAsync();
        }

        public async Task RegisterEmployee(Employee employee)
        {
            await _context.AddAsync(employee);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEmployee(int employeeId)
        {
            _context.Employees.FindAsync(employeeId).Result.Active = false;
            await _context.SaveChangesAsync();
        }
    }
}