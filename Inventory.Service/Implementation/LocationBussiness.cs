using Inventory.Data.Entities;
using Inventory.Repository;
using Inventory.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory.Service.Implementation
{
    public class LocationBussiness : ILocationBussiness
    {
        readonly private IRepository<Location, int> _LocationRepository;
        public LocationBussiness(IRepository<Location, int> LocationRepository)
        {
            _LocationRepository = LocationRepository;
        }
        public async Task<Location> AddNewLocation(Location _Location)
        {
            _LocationRepository.Add(_Location);
            int added = await _LocationRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(_Location);
            else
                return await Task.FromResult<Location>(null);
        }

        public IQueryable<Location> GetAllLocation()
        {
            var LocationList = _LocationRepository.GetAll(true);
            return LocationList;
        }

        public IQueryable<Location> GetActiveLocations()
        {
            var LocationList = _LocationRepository.GetAll();
            return LocationList;
        }


        public bool checkDeactivation(int LocationId)
        {
            var checkConnections = _LocationRepository.GetAll()
                .Where(x => x.Id == LocationId && x.Invoice.Any()).Count();
            if (checkConnections > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<bool> Activate(int LocationId,bool ActivationType)
        {
            if (ActivationType)
                _LocationRepository.Activate(new Location() { Id = LocationId });
            else
                _LocationRepository.DeActivate(new Location() { Id = LocationId });

            var added = await _LocationRepository.SaveChanges();

            if (added > 0)
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);
        }

        public async Task<bool> UpdateLocation(Location _Location)
        {
            _LocationRepository.PartialUpdate(_Location,l=>l.Name);
            int added = await _LocationRepository.SaveChanges();
            if (added > 0)
                return await Task.FromResult(true);
            else
                return await Task.FromResult(false);

        }

        public Location ViewLocation(int LocationId)
        {
            var LocationEntity = _LocationRepository.Get(LocationId);
            return LocationEntity;
        }
    }
}

