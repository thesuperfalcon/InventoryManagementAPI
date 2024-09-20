using InventoryManagementAPI.DAL;
using InventoryManagementAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagementAPI.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class StatisticsController : Controller
	{
		private readonly InventoryManagementAPIContext _context;

		public StatisticsController(InventoryManagementAPIContext context)
		{
			_context = context;
		}
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] Models.Statistic statistic)
		{
			_context.Statistics.Add(statistic);
			await _context.SaveChangesAsync();
			return Ok();
		}

		[HttpPut]
		public async Task<IActionResult> Put([FromBody] Models.Statistic statistic)
		{
			_context.Update(statistic);
			await _context.SaveChangesAsync();
			return Ok();
		}

		[HttpGet]
		public async Task<IActionResult> GetStatistics()
		{
			var statistics = await StatisticData.GetStatisticsAsync();

			return Ok(statistics);
        }
        //[HttpGet("{id}")]
        //public async Task<List<Models.Statistic>> GetStatisticsById(int id)
        //{
        //    //var statistic = await StatisticData.GetStatisticsAsync(id);

        //    //return statistic;
        //}

        [HttpDelete("{id}")]
		public async Task<IActionResult> DeleteStatistic(int id)
		{
			var statistic = await _context.Statistics.FindAsync(id);
			if (statistic == null)
			{
				return NotFound();
			}

			_context.Statistics.Remove(statistic);
			await _context.SaveChangesAsync();
			return Ok();
		}
	}
}
