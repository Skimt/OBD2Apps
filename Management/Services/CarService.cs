using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Management.Data;
using Management.Models;

namespace Management.Services
{

    public class CarService : ControllerBase
    {

        private readonly OBDContext _context;

        public CarService(OBDContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<IEnumerable<Car>>> GetCars()
        {

            return await _context.Car
                .Include(car => car.CarBrand)
                .Include(car => car.CarModel)
                .Include(car => car.Person)
                .OrderByDescending(car => car.CarId)
                .ToListAsync();

        }

        public async Task<ActionResult<Car>> GetCar(int id)
        {

            var car = await _context.Car
                .Include(car => car.CarBrand)
                .Include(car => car.CarModel)
                .Include(car => car.Person)
                .FirstOrDefaultAsync(car => car.CarId == id);

            if (car == null)
            {
                return NotFound();
            }

            return car;

        }

        public async Task<IActionResult> PutCar(int id, Car car)
        {
            if (id != car.CarId)
            {
                return BadRequest();
            }

            _context.Entry(car).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        public async Task<IActionResult> PostCar(Car car)
        {
            _context.Car.Add(car);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetCar", new { id = car.CarId }, car);
        }

        public async Task<ActionResult<Car>> DeleteCar(int id)
        {
            var car = await _context.Car.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            _context.Car.Remove(car);
            await _context.SaveChangesAsync();

            return car;
        }

        private bool CarExists(int id)
        {
            return _context.Car.Any(e => e.CarId == id);
        }

    }
}
