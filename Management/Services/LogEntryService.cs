using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Management.Data;
using Management.Models;

namespace Management.Services
{
	public class LogEntryService : ControllerBase
	{

		private readonly OBDContext _context;

		public LogEntryService(OBDContext context)
		{
			_context = context;
		}

		public async Task<LogEntryViewModel> GetLogEntryView(int currentPage, int numRows, string pid = "")
		{

			IQueryable<LogEntry> query;

			currentPage -= 1;
			currentPage = currentPage < 1 ? 0 : currentPage;

			if (string.IsNullOrEmpty(pid))
			{
				query = _context.LogEntry;
			}
			else
			{
				query = _context.LogEntry.Where(c => c.Pid == pid);
			}

			int count = await query.CountAsync();
			query = query.Skip(currentPage * numRows).Take(numRows);

			return await Task.FromResult(new LogEntryViewModel
			{
				LogEntries = await query
					.OrderByDescending(logEntry => logEntry.LogEntryId)
					.ToListAsync(),
				RowCount = count
			});

		}

		public async Task<ActionResult<IEnumerable<LogEntry>>> GetLogEntries(int sessionId, string pid)
		{
			return await _context.LogEntry
				.Where(logEntry => logEntry.SessionId == sessionId && logEntry.Pid == pid)
				.ToListAsync();
		}

		public async Task<ActionResult<LogEntry>> DeleteLogEntry(int id)
		{
			LogEntry logEntry = await _context.LogEntry.FindAsync(id);
			if (logEntry == null)
			{
				return NotFound();
			}

			_context.LogEntry.Remove(logEntry);
			await _context.SaveChangesAsync();

			return logEntry;
		}


		public async Task<ActionResult<LogEntry[]>> DeleteLogEntries(int[] id)
		{
			var logEntries = await _context.LogEntry.Where(logEntry => id.Contains(logEntry.LogEntryId)).ToArrayAsync();
			if (logEntries == null)
			{
				return NotFound();
			}
			_context.LogEntry.RemoveRange(logEntries);
			await _context.SaveChangesAsync();
			return logEntries;
		}

		public async Task<IEnumerable<LogEntry>> GetLogEntries()
		{
			return await _context.LogEntry
				.ToListAsync();
		}

		public async Task<IEnumerable<LogEntry>> GetLogEntries(string pid)
		{
			return await _context.LogEntry
				.Where(c => c.Pid == pid)
				.ToListAsync();
		}

		public async Task<IEnumerable<LogEntry>> GetLogEntries(int fromIndex, int numRows)
		{
			return await _context.LogEntry
				.Skip(fromIndex)
				.Take(numRows)
				.ToListAsync();
		}

		public async Task<IEnumerable<LogEntry>> GetLogEntries(string pid, int fromIndex, int numRows)
		{

			IEnumerable<LogEntry> result;

			if (pid == "")
			{
				result = await _context.LogEntry
					.Skip(fromIndex)
					.Take(numRows)
					.ToListAsync();
			}
			else
			{
				result = await _context.LogEntry
					.Where(c => c.Pid == pid)
					.Skip(fromIndex)
					.Take(numRows)
					.ToListAsync();
			}

			return result;

		}

		public async Task<IEnumerable<string>> GetPidsUnique()
		{
			return await _context.LogEntry.Select(entry => entry.Pid).Distinct().ToListAsync();
		}

	}
}
