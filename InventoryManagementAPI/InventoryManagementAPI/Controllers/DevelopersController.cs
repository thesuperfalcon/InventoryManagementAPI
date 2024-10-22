using InventoryManagementAPI.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Net.WebRequestMethods;

namespace InventoryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DevelopersController : Controller
    {
        private readonly InventoryManagementAPIContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public DevelopersController(InventoryManagementAPIContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<List<Models.Developer>> GetDevelopers()
        {
            string imagesFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "developers");
            var developers = await _context.Developers.ToListAsync();
            var imageFiles = new string[developers.Count];
            List<string> picUrls = new List<string>();
            if (Directory.Exists(imagesFolderPath))
            {
                 imageFiles = Directory.GetFiles(imagesFolderPath);
                foreach(var imagePath in imageFiles)
                {
                    var fileName = Path.GetFileName(imagePath);
                    string url = "https://localhost:44353/developers/" + fileName;
                    picUrls.Add(url);
                }
            }

           
            foreach (var developer in developers)
            {
                developer.ImageUrl = picUrls.Where(x => x.Contains(developer.ImageUrl)).FirstOrDefault();
            }
            return developers;
        }
    }
}
