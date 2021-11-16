using System.Collections.Generic;
using System.Threading.Tasks;
using Core;

namespace ApplicationServices
{
    public interface IAvailabilityService
    {
        Task<IResult<ICollection<Availability>>> GetAvailabilitiesByEmployee(int id);
        Task<IResult<ICollection<Availability>>> GetAvailabilities();
        Task<IResult<Availability>> GetAvailability(int id);
        Task<IResult<Availability>> AddAvailability(Availability availability);
        Task<IResult<Availability>> UpdateAvailability(Availability appointment);
        Task<IResult<Availability>> DeleteAvailability(int id);
    }
}