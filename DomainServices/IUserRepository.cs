using System.Collections.Generic;
using System.Threading.Tasks;
using Core;

namespace DomainServices
{
    public interface IUserRepository
    {
        Task<User> GetUser(int userId);
        Task<User> GetUserByEmail(string email);
        Task<ICollection<User>> GetUsers();
        Task RegisterUser(User user);
        Task DeleteUser(int userId);
    }
}