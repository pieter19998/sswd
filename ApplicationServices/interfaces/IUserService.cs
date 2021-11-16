using System.Threading.Tasks;
using Core;

namespace ApplicationServices
{
    public interface IUserService
    {
        Task<IResult<User>> RegisterUser(User user);
        Task<IResult<User>> LoginUser(User user);
    }
}