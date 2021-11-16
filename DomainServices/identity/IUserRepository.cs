using System.Collections.Generic;
using System.Threading.Tasks;
using Core;

namespace DomainServices.identity
{
    public interface IUserRepository
    {
        Task<User> GetUser(int userId);
        Task<ICollection<User>> GetUser();
        Task RegisterUser(User userId);
        Task DeleteUser(int userId);
    }
}