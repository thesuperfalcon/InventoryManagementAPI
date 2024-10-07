using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace InventoryManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfilepicController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProfilepicController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public List<string> GetPicUrls()
        {

            List<string> picUrls = new List<string>();

            string imagesFolderPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");

            if (Directory.Exists(imagesFolderPath))
            {
                var imageFiles = Directory.GetFiles(imagesFolderPath);

                foreach (var imagePath in imageFiles)
                {
                    var fileName = Path.GetFileName(imagePath);
                    string url = "https://localhost:44353/images/" + fileName;

                    picUrls.Add(url);
                }
                
            }
            List<string> sortedList = picUrls.OrderBy(url => ExtractNumberFromFile(url)).ToList();
            return sortedList;
        }

        private int ExtractNumberFromFile(string url)
        {
            //Använder Regex för att sortera filerna i nummerordning

            string fileName = url.Substring(url.LastIndexOf('/') + 1);
            Match match = Regex.Match(fileName, @"\d+");

            return match.Success ? int.Parse(match.Value) : 0;
        }
    }
}
