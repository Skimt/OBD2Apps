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

    public class SessionService : ControllerBase
    {
        private readonly OBDContext _context;

        public SessionService(OBDContext context)
        {
            _context = context;
        }

        public async Task<Session> GetLastSession()
        {

            return await _context.Session.FromSqlRaw(@"
                SELECT TOP 1 Session.SessionId, Session.CarId
                FROM Session
                JOIN LogEntry ON Session.SessionId = LogEntry.SessionId 
                ORDER BY Session.SessionId DESC
            ")
            .Select(session => new Session
            {
                SessionId = session.SessionId,
                CarId = session.CarId
            })
            .SingleAsync();

        }

        public async Task<ActionResult<IEnumerable<Session>>> GetSessions(int carId = 0)
        {
            if (carId == 0)
            {
                return await _context.Session
                    .OrderByDescending(session => session.SessionId)
                    .ToListAsync();
            }
            else
            {
                return await _context.Session
                    .Where(session => session.CarId == carId)
                    .OrderByDescending(session => session.SessionId)
                    .ToListAsync();
            }
        }

        public async Task<ActionResult<Session>> DeleteSession(int id)
        {
            Session session = await _context.Session.FindAsync(id);
            if (session == null)
            {
                return NotFound();
            }

            _context.Session.Remove(session);
            await _context.SaveChangesAsync();

            return session;
        }

    }
}
