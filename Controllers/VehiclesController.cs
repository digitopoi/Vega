using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vega.Controllers.Resources;
using vega.Models;
using vega.Persistence;

namespace vega.Controllers
{
    [Route ("/api/vehicles")]
    public class VehiclesController : Controller
    {
        private readonly IMapper _mapper;
        private readonly VegaDbContext _context;
        private readonly IVehicleRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public VehiclesController (IMapper mapper, VegaDbContext context, IVehicleRepository repository, IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
            this._repository = repository;
            this._context = context;
            this._mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateVehicle ([FromBody] SaveVehicleResource vehicleResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest ();
            }

            var vehicle = _mapper.Map<SaveVehicleResource, Vehicle> (vehicleResource);

            vehicle.LastUpdate = DateTime.Now;

            _repository.Add (vehicle);
            await _context.SaveChangesAsync ();

            vehicle = await _repository.GetVehicle (vehicle.Id);

            var result = _mapper.Map<Vehicle, VehicleResource> (vehicle);
            return Ok (result);
        }

        [HttpPut ("{id}")]
        public async Task<IActionResult> UpdateVehicle (int id, [FromBody] SaveVehicleResource vehicleResource)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest ();
            }

            var vehicle = await _repository.GetVehicle (id);

            if (vehicle == null)
            {
                return NotFound ();
            }

            _mapper.Map<SaveVehicleResource, Vehicle> (vehicleResource, vehicle);

            vehicle.LastUpdate = DateTime.Now;

            await _context.SaveChangesAsync ();

            var result = _mapper.Map<Vehicle, VehicleResource> (vehicle);
            return Ok (result);
        }

        [HttpDelete ("{id}")]
        public async Task<IActionResult> DeleteVehicle (int id)
        {
            var vehicle = await _repository.GetVehicle (id, includeRelated: false);

            if (vehicle == null)
            {
                return NotFound ();
            }

            _repository.Remove (vehicle);

            await _context.SaveChangesAsync ();

            return Ok (id);
        }

        [HttpGet ("{id}")]
        public async Task<IActionResult> GetVehicle (int id)
        {
            var vehicle = await _repository.GetVehicle (id);

            if (vehicle == null)
            {
                return NotFound ();
            }

            var vehicleResource = _mapper.Map<Vehicle, VehicleResource> (vehicle);

            return Ok (vehicleResource);
        }

    }
}