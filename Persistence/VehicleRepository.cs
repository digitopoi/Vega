using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using vega.Core;
using vega.Core.Models;

namespace vega.Persistence
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly VegaDbContext _context;
        public VehicleRepository (VegaDbContext context)
        {
            this._context = context;

        }

        public async Task<Vehicle> GetVehicle (int id, bool includeRelated = true)
        {
            if (!includeRelated)
            {
                return await _context.Vehicles.FindAsync();
            }

            return await _context.Vehicles
                            .Include (v => v.Features)
                                .ThenInclude (vf => vf.Feature)
                            .Include (v => v.Model)
                                .ThenInclude (m => m.Make)
                            .SingleOrDefaultAsync (v => v.Id == id);
        }

        public void Add(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
        }

        public void Remove(Vehicle vehicle)
        {
            _context.Remove(vehicle);
        }
    }
}