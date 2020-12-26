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

    public class ConfigurationService : ControllerBase
    {

        private readonly OBDContext _context;

        public ConfigurationService(OBDContext context)
        {
            _context = context;
        }

        public async Task<ActionResult<Configuration>> GetConfig()
        {

            var config = await _context.Configuration.FirstOrDefaultAsync();

            if (config == null)
            {
                return NotFound();
            }

            return config;

        }

        public async Task<IActionResult> PutConfig(Configuration config)
        {

            _context.Entry(config).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return NoContent();

        }

    }
}
