using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;
using DomainServices;
using Microsoft.EntityFrameworkCore;

namespace IF_Datastore
{
    public class UserRepository : IUserRepository
    {
        private readonly AppIdentityDbContext _context;

        public UserRepository(AppIdentityDbContext context)
        {
            _context = context;
        }

        public async Task<User> GetUser(int userId)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.UserId == userId && x.Active);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(x => x.Email == email && x.Active);
        }

        public async Task<ICollection<User>> GetUsers()
        {
            return await _context.Users.Where(x => x.Active).ToListAsync();
        }

        public async Task RegisterUser(User user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteUser(int userId)
        {
            _context.Users.FindAsync(userId).Result.Active = false;
            await _context.SaveChangesAsync();
        }
    }
}