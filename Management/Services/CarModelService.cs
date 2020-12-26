using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Management.Data;
using Management.Models;
using System.ComponentModel;

namespace Management.Services
{

    public class CarModelService
    {

        private readonly OBDContext _context;

        public CarModelService(OBDContext context)
        {
            _context = context;
        }

        public CarModel GetCarModel(int id)
        {

            var carModel = _context.CarModel.Find(id);

            if (carModel == null)
            {
                return null;
            }

            return carModel;

        }

        public async Task<IEnumerable<CarModel>> GetCarModels(string value, int numberOfResults)
        {
            return await _context.CarModel.Where(brand => brand.Name.StartsWith(value)).Take(numberOfResults).OrderBy(brand => brand.Name).ToListAsync();
        }

    }
}
