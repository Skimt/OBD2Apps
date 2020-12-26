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

    public class CarBrandService
    {

        private readonly OBDContext _context;

        public CarBrandService(OBDContext context)
        {
            _context = context;
        }

        public CarBrand GetCarBrand(int id)
        {

            var carModel = _context.CarBrand.Find(id);

            if (carModel == null)
            {
                return null;
            }

            return carModel;

        }

        public async Task<IEnumerable<CarBrand>> GetCarBrands(string value, int numberOfResults)
        {
            return await _context.CarBrand.Where(brand => brand.Name.StartsWith(value)).Take(numberOfResults).OrderBy(brand => brand.Name).ToListAsync();
        }

    }
}
