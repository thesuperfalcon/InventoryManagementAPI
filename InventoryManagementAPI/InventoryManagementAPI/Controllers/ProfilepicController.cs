using Microsoft.AspNetCore.Mvc;

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
            return picUrls;
        }
    }
}
