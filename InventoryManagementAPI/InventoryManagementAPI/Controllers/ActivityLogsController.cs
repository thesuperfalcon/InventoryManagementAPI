using InventoryManagementAPI.Data;
using InventoryManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ActivityLogsController : Controller
	{
		private readonly InventoryManagementAPIContext _context;

		public ActivityLogsController(InventoryManagementAPIContext context)
		{
			_context = context;
		}

		[HttpPost]
		//public async Task<IActionResult> Post([FromBody] Models.ActivityLog activityLog)
		//{
		//	_context.ActivityLog.Add(activityLog);
		//	await _context.SaveChangesAsync();
		//	return Ok();
		//}
		public async Task<IActionResult> Post([FromBody] DTO.ActivityLogDTO log)
		{
			var logResponse = new ActivityLog
			{
				Id = log.Id,
				UserId = log.UserId,
				Action = (ActionType?)log.Action,
				ItemType = (ItemType?)log.ItemType,
				TypeId = log.TypeId,
				TimeStamp = log.TimeStamp,
				Notes = log.Notes
			};
			_context.ActivityLog.Add(logResponse);
			await _context.SaveChangesAsync();
			return Ok();
		}

		[HttpPut]
		public async Task<IActionResult> Put([FromBody] Models.ActivityLog activityLog)
		{
			_context.Update(activityLog);
			await _context.SaveChangesAsync();
			return Ok();
		}

		[HttpGet]
		public async Task<List<Models.ActivityLog>> GetActivityLogs()
		{
			List<Models.ActivityLog> activityLog = await _context.ActivityLog.ToListAsync();
			return activityLog;
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteActivityLog(int id)
		{
			var activityLog = await _context.ActivityLog.FindAsync(id);
			if (activityLog == null)
			{
				return NotFound();
			}

			_context.ActivityLog.Remove(activityLog);
			await _context.SaveChangesAsync();
			return Ok();
		}
	}
}
