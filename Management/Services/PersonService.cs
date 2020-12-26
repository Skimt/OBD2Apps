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

    public class PersonService : ControllerBase
    {
        private readonly OBDContext _context;

        public PersonService(OBDContext context)
        {
            _context = context;
        }


        public async Task<ActionResult<IEnumerable<Person>>> GetPeople()
        {
            
            return await _context.Person
                .OrderByDescending(person => person.GivenName)
                .ThenBy(person => person.SurName)
                .ToListAsync();

        }

        public async Task<ActionResult<Person>> GetPerson(int id)
        {

            Person person = await _context.Person.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            return person;

        }

        public async Task<IEnumerable<Person>> GetPeople(string value, int numberOfResults)
        {
            return await _context.Person.Where(person => person.SurName.StartsWith(value) || person.GivenName.StartsWith(value))
                .Take(numberOfResults)
                .OrderBy(person => person.GivenName)
                .ThenBy(person => person.SurName)
                .ToListAsync();
        }

        public async Task<IActionResult> PutPerson(int id, Person person)
        {
            if (id != person.PersonId)
            {
                return BadRequest();
            }
            _context.Entry(person).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonModelExists(id))
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

        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            _context.Person.Add(person);
            await _context.SaveChangesAsync();
            return null;
        }

        public async Task<ActionResult<Person>> DeletePerson(int id)
        {
            var person = await _context.Person.FindAsync(id);
            if (person == null)
            {
                return CreatedAtAction("GetPerson", new { id = person.PersonId }, person);
            }
            _context.Person.Remove(person);
            await _context.SaveChangesAsync();
            return person;
        }

        private bool PersonModelExists(int id)
        {
            return _context.Person.Any(e => e.PersonId == id);
        }

    }
}
