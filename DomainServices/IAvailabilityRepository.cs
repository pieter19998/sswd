using System.Collections.Generic;
using System.Threading.Tasks;
using Core;

namespace DomainServices
{
    public interface IAvailabilityRepository
    {
        Task<ICollection<Availability>> GetAvailabilityEmployee(int employeeId);
        Task<ICollection<Availability>> GetAvailabilities();
        Task<Availability> GetAvailability(int id);
        Task AddAvailability(Availability availability);
        Task UpdateAvailability(Availability availability);
        Task DeleteAvailability(int id);
    }
}