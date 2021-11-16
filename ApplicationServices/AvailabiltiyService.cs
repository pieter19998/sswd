using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using DomainServices;

namespace ApplicationServices
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly IAvailabilityRepository _availabilityRepository;

        public AvailabilityService(IAvailabilityRepository availabilityRepository)
        {
            _availabilityRepository = availabilityRepository;
        }

        public async Task<IResult<ICollection<Availability>>> GetAvailabilitiesByEmployee(int id)
        {
            IResult<ICollection<Availability>> result = new Result<ICollection<Availability>>();
            result.Payload = await _availabilityRepository.GetAvailabilityEmployee(id);
            if (result.Payload == null)
            {
                result.Message = ErrorMessages.IdNotFound;
                result.Success = false;
            }

            return result;
        }

        public async Task<IResult<ICollection<Availability>>> GetAvailabilities()
        {
            IResult<ICollection<Availability>> result = new Result<ICollection<Availability>>();
            result.Payload = await _availabilityRepository.GetAvailabilities();
            if (result.Payload == null)
            {
                result.Message = "something went wrong fetching data from the database";
                result.Success = false;
            }

            return result;
        }

        public async Task<IResult<Availability>> GetAvailability(int id)
        {
            IResult<Availability> result = new Result<Availability>();
            result.Payload = await _availabilityRepository.GetAvailability(id);
            if (result.Payload == null)
            {
                result.Message = "availability with id " + id + " not found.";
                result.Success = false;
            }

            return result;
        }

        public async Task<IResult<Availability>> AddAvailability(Availability availability)
        {
            var result = IsValid(availability);
            if (!result.Success) return result;
            try
            {
                await _availabilityRepository.AddAvailability(availability);
            }
            catch
            {
                result.Success = false;
                result.Message = "something went wrong when inserting availability";
            }

            return result;
        }

        public async Task<IResult<Availability>> UpdateAvailability(Availability availability)
        {
            var result = IsValid(availability);
            if (!result.Success) return result;
            if (await _availabilityRepository.GetAvailability(availability.AvailabilityId) == null)
            {
                result.Success = false;
                result.Message = "availability with id " + availability.AvailabilityId + " not found.";
            }
            else
            {
                try
                {
                    await _availabilityRepository.UpdateAvailability(availability);
                }
                catch
                {
                    result.Success = false;
                    result.Message = "something went wrong updating availability";
                }
            }

            return result;
        }

        public async Task<IResult<Availability>> DeleteAvailability(int id)
        {
            IResult<Availability> result = new Result<Availability>();
            try
            {
                await _availabilityRepository.DeleteAvailability(id);
            }
            catch
            {
                result.Success = false;
                result.Message = "availability id not found";
            }

            return result;
        }

        private IResult<Availability> IsValid(Availability availability)
        {
            IResult<Availability> result = new Result<Availability>();
            if (availability.AvailableFrom < DateTime.Now) result.Message = " invalid availableFrom";
            if (availability.AvailableTo < DateTime.Now) result.Message = " invalid availableTo";
            if (result.Message.Length > 1) result.Success = false;
            return result;
        }
    }
}